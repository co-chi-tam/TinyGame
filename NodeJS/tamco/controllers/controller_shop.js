require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/simpleData')();
require('../models/sodier')();
require('../models/mercenary')();
require('../models/item')();
require('../models/shop')();
require('../models/inventory')();
require('../models/token')();
require('../models/purchase')();

var db;
var wss;
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');

exports.setup = function(callback) { callback(null); }

exports.init = function(database, websocket) {
	db = database;
	wss = websocket;
}

// Get All Item In Shop
exports.getAllItemInShop = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var tokenId = request.query.token;
		if (tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				shop.findItemInShop(db.getDatabase(), {}, function(items) {
					delete items[0]['_id'];
					var slots = items[0]['slots'];
					var itemIds = [];
					for (var i = 0; i < slots.length; i++) {
						itemIds.push({
							id: slots[i]['gameType'].toString()
						});
					}
					item.findItem(db.getDatabase(), {$or: itemIds}, function (itemResults) {
						for (var i = 0; i < itemResults.length; i++) {
							delete itemResults[i]['_id'];
							delete itemResults[i]['activeMethod'];
							delete itemResults[i]['valuesMethod'];
						}
						items[0]['items'] = itemResults;
						response.write(resultResponse.createResult(1, items));
						response.end();
					}, function(err) {
						response.end(err);
					});
				}, function (error) {
					response.end(error);
				});
			}, function(err) {
				response.end(err);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

// Post Buy Item By Gold
exports.postBuyItemByGoldInShop = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var itemId = request.body.iid;
		var amount = request.body.amount;
		var tokenId = request.body.token;
		if (uName && itemId && tokenId && amount) {
			var amountItem = parseInt (amount.toString());
			if (amountItem > 0) {
				var currentDate = new Date();
				token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
					var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
					if (verifyToken.verify == true) {
						shop.findItemInShop(db.getDatabase(), {'slots':{'$elemMatch':{'gameType': itemId.toString()}}}, function(itemShop) {
							var slots = itemShop[0]['slots'];
							var goldPercentPrice = Math.pow(2, 53) - 1;
							var goldPrice = Math.pow(2, 53) - 1;
							var itemGameType = '';
							var alreadyFindItem = false;
							for (var i = 0; i < slots.length; i++) {
								if (slots[i]['gameType'].toString() == itemId.toString()) {
									itemGameType = slots[i]['gameType'].toString();
									goldPercentPrice = parseInt (slots[i]['goldPercentPrice'].toString());
									alreadyFindItem = true;
									break;
								}
							}
							if (alreadyFindItem) {
								item.findItem(db.getDatabase(), {'id': itemId.toString()}, function (itemResults) {
									goldPrice = parseInt (itemResults[0]['goldPrice'].toString());
									var goldNeedPay = ((goldPrice / 100) * goldPercentPrice) * amount;
									var productName = itemResults[0]['name'].toString();
									user.findAndModifyUser(db.getDatabase(), {'userName':uName.toString(), 'isLogin': true, 'active': true, "gold": {"$gte":goldNeedPay}}, {"$inc": {"gold": -goldNeedPay}}, function(users) {
										if (users) {
											var md5 = token.createHashId (uName, user.userActions[8]);
											var currentPurchase = purchase.newAlreadyPurchase(uName.toString(), productName, amount, objectUserId, goldNeedPay, 0, users['gold'], users['diamond'], tokenId.toString());
											var newItem = inventory.newAlreadyItem(md5, users['_id'], amountItem, itemId.toString(), currentPurchase.id.toString());
											var userId = users['_id'];
											var objectUserId = new Mongo.ObjectId(userId.toString());
											var userResponse = [{
												id: uName.toString()+'-buy-'+productName,
												currentGold: parseInt (users['gold'].toString()) - goldNeedPay,
												currentDiamond: parseInt (users['diamond'].toString()),
												productName: productName,
												amount: amount,
												totalGold: goldNeedPay,
												totalDiamond: 0,
												token: tokenId
											}];
											purchase.createPurchase(db.getDatabase(), currentPurchase, function(purchaseResult) {
												inventory.findItemInInventory (db.getDatabase(), {'owner': objectUserId, 'gameType': itemGameType}, function(items) {
													inventory.updateItemInventory(db.getDatabase(), {'owner': objectUserId, 'gameType': itemGameType}, {'$inc': {'amount': amountItem}, '$set':{'purchaseId': currentPurchase.id.toString()}}, function(findItemResult) {
														if (findItemResult) {
															response.write(resultResponse.createResult(1, userResponse));
															response.end();
														} else {
															response.end(errorResponse.createErrorCode(1, "Update Item Amount Fail !!"));
														}
													}, function(err) {
														response.end(errorResponse.createErrorCode(1, "Update Item Amount Error !!"));
													});
												}, function (error) {
													inventory.createItemInInventory(db.getDatabase(), newItem, function(createItemResult) {
														response.write(resultResponse.createResult(1, userResponse));
														response.end();
													}, function(err) {
														response.end(errorResponse.createErrorCode(1, "Create Item Fail !!"));
													});
												});
											}, function(err) {
												response.end(errorResponse.createErrorCode(1, "Create Purchase Fail !!"));
											});
										} else {
											response.end(errorResponse.createErrorCode(1, "User not enough gold !"));
										}
									}, function(err) {
										response.end(errorResponse.createErrorCode(1, "User not enough gold !"));
									});
								}, function(err) {
									response.end(err);
								});
							} else {
								response.end(errorResponse.createErrorCode(1, "Item not found !!"));
							}
						}, function (error){
							response.end(error);
						});
					} else {
						response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
					}
				}, function(err) {
					response.end(err);
				});
			} else {
				response.end(errorResponse.createErrorCode(1, "Amount not less one."));
			}
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}
