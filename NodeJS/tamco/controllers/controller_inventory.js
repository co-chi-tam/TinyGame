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
require('../models/user')();
require('../models/itemLog')();

var db;
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');
var itemActions = {};
var cache = {};

exports.setup = function(callback) { callback(null); }

exports.init = function(database) {
	db = database;
	itemActions = {
		'emptyItem': emptyItem,
		'createMercenary': createMercenary,
		'createRandomMercenary': createRandomMercenary,
		'addGold': addGold
		};
}

// Get All Item In Inventory
exports.getAllItemInInventory = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						inventory.findItemInInventory(db.getDatabase(), {'owner': objectUserId}, function(items){
							var itemIds = [];
							for (var i = 0; i < items.length; i++) {
								delete items[i]['_id'];
								delete items[i]['owner'];
								delete items[i]['createdDate'];
								itemIds.push({
									id: items[i]['gameType'].toString()
								});
							}
							var inventoryResult = [{
								id: 'inventory-' + uName,
								owner: uName,
								gold: users[0]['gold'],
								diamond: users[0]['diamond'],
								inventory: items
							}];
							item.findItem(db.getDatabase(), {$or: itemIds}, function (itemResults) {
								for (var i = 0; i < itemResults.length; i++) {
									delete itemResults[i]['_id'];
									delete itemResults[i]['activeMethod'];
									delete itemResults[i]['valuesMethod'];
									delete itemResults[i]['purchaseId'];
								}
								inventoryResult[0]['items'] = itemResults;
								response.write(resultResponse.createResult(1, inventoryResult));
								response.end();
							}, function(err) {
								response.end(err);
							});
						}, function(error) {
							response.end(error);
						});
					}, function(err) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
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

// Use Item
exports.postUseItemInInventory = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var itemId = request.body.iid;
		var amount = request.body.amount;
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (itemId && amount && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						inventory.findItemInInventory(db.getDatabase(), {'id': itemId.toString(), 'owner': objectUserId}, function(items){
							var currentItem = null;
							var amountItem = parseInt(items[0].amount);
							var amountUse = parseInt(amount.toString()); 
							if (amountItem > 0 && amountItem >= amountUse) {
								currentItem = items[0];
							}
							if (currentItem) {
								var gameTypeId = currentItem['gameType'];
								var inventoryItemdId = currentItem['id'];
								if (cache[gameTypeId]) {
									var itemResults = cache[gameTypeId];
									var amountPerConsume = parseInt(itemResults[0].amountPerConsume);
									if (amountItem >= amountPerConsume && amountUse >= amountPerConsume) {
										var methodName = itemResults[0]['activeMethod'].toString();
										var methodParam = itemResults[0]['valuesMethod'];
										if (itemActions[methodName]) {
											itemActions[methodName](userId, inventoryItemdId, methodParam, -amountPerConsume, tokenId, function(comp){
												response.end(resultResponse.createResult(1, comp));
											}, function (error) {
												response.end(error);
											});
										} else {
											response.end(errorResponse.createErrorCode(1, "Can not find item."));
										}
									} else {
										response.end(errorResponse.createErrorCode(1, "Can not use item. Item not enough to use."));
									}
								} else {
									item.findItem(db.getDatabase(), {'id': gameTypeId}, function (itemResults) {
										cache[gameTypeId] = itemResults;
										var amountPerConsume = parseInt(itemResults[0].amountPerConsume);
										if (amountItem >= amountPerConsume && amountUse >= amountPerConsume) {
											var methodName = itemResults[0]['activeMethod'].toString();
											var methodParam = itemResults[0]['valuesMethod'];
											if (itemActions[methodName]) {
												itemActions[methodName](userId, inventoryItemdId, methodParam, -amountPerConsume, tokenId, function(comp){
													response.end(resultResponse.createResult(1, comp));
												}, function (error) {
													response.end(error);
												});
											} else {
												response.end(errorResponse.createErrorCode(1, "Can not find item."));
											}
										} else {
											response.end(errorResponse.createErrorCode(1, "Can not use item. Item not enough to use."));
										}
									}, function(err) {
										response.end(err);
									});
								}
							} else {
								response.end(errorResponse.createErrorCode(1, "Can not use item. Item not in inventory."));
							}
						}, function(error) {
							response.end(error);
						});
					}, function(err) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
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

function emptyItem(userId, itemId, values, tokenId, complete, error) {
	updateAmountItem (userId, itemId, -1, function (com) {
		if (complete) {
			complete ("Use item complete ! ");
		}
	}, error);
}

function createMercenary(userId, itemId, values, amount, tokenId, complete, error) {
	user.findUser(db.getDatabase(), {'_id':userId, 'isLogin': true, 'active': true}, function(users) {
		var uName = users[0]["userName"];
		updateAmountItem(uName, userId, itemId, amount, function (com) {
			var sId = values[0];
			var currentDate = new Date();
			sodier.findSodier(db.getDatabase(), {'id': sId.toString()}, function(sodiers) {
				var newMercenary = mercenary.parseMercenary(sodiers[0]);
				var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + currentDate +"'}";
				var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
				newMercenary.id = md5;
				newMercenary.owner = userId;
				newMercenary.hiredDate = currentDate;
				newMercenary.tokenAvailable = tokenId.toString();
				mercenary.createMercenary (db.getDatabase(), newMercenary, function(createComplete) {
					if (complete) {
						complete([{
							id: "UseItem-" + itemId,
							result: "You have Sodier " + createComplete[0]['name'],
							amount: createComplete.length
						}]);
					}
				}, error);
			}, error);
		}, error);
	}, error);
}

function createRandomMercenary(userId, itemId, values, amount, tokenId, complete, error) {
	user.findUser(db.getDatabase(), {'_id':userId, 'isLogin': true, 'active': true}, function(users) {
		var uName = users[0]["userName"];
		updateAmountItem(uName, userId, itemId, amount, function (com) {
			var random = getRandomInt(0, values.length - 1);
			var sId = values[random];
			var currentDate = new Date();
			sodier.findSodier(db.getDatabase(), {'id': sId.toString()}, function(sodiers) {
				var newMercenary = mercenary.parseMercenary(sodiers[0]);
				var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + currentDate +"'}";
				var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
				newMercenary.id = md5;
				newMercenary.owner = userId;
				newMercenary.hiredDate = currentDate;
				newMercenary.tokenAvailable = tokenId.toString();
				mercenary.createMercenary (db.getDatabase(), newMercenary, function(createComplete) {
					if (complete) {
						complete([{
							id: "UseItem-" + itemId,
							result: "You have Sodier " + createComplete[0]['name'],
							amount: createComplete.length
						}]);
					}
				}, error);
			}, error);
		}, error);
	}, error);
}

function addGold(userId, itemId, values, amount, tokenId, complete, error) {
	user.findUser(db.getDatabase(), {'_id':userId, 'isLogin': true, 'active': true}, function(users) {
		var uName = users[0]["userName"];
		var goldUpdate = parseInt ( values[0].toString());
		updateAmountItem(uName, userId, itemId, amount, function (com) {
			user.findAndModifyUser(db.getDatabase(), {'userName':uName.toString(), 'isLogin': true, 'active': true}, {"$inc": {"gold": goldUpdate}}, function(users) {
				if (complete) {
					complete([{
						id: "UseItem-" + itemId,
						result: "You update gold " + goldUpdate,
						amount: 1
					}]);
				}
			}, function(err) {
				response.end(errorResponse.createErrorCode(1, "User not enough gold !"));
			});
		}, error);
	}, error);
}

function updateAmountItem(userName, userId, itemId, amount, complete, error) {
	var objectUserId = new Mongo.ObjectId(userId.toString());
	var itemLogAlready = itemLog.newAlreadyLog(userName, itemId, amount, true);
	inventory.updateItemInventory(db.getDatabase(), {'id': itemId.toString(), 'owner': objectUserId, "amount": {"$gte":0}}, {"$inc": {"amount": amount}}, function (itemResults) {
		itemLog.createItemLog(db.getDatabase(), itemLogAlready, function(completeCreate) {
			if (complete) {
				complete(itemResults);
			}
		}, error);
	}, error);
}


























