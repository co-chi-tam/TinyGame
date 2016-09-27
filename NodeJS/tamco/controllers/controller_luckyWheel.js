require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/simpleData')();
require('../models/token')();
require('../models/user')();
require('../models/item')();
require('../models/inventory')();
require('../models/token')();
require('../models/purchase')();
require('../models/luckyWheel')();
require('../models/luckyWheelLog')();
require('../models/sodier')();
require('../models/mercenary')();

var db;
var wss;
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');
var luckyWheelActions = {};

exports.setup = function(callback) { callback(null); }

exports.init = function(database, websocket) {
	db = database;
	wss = websocket;
	luckyWheelActions = {
		'emptyLuckyWheel': emptyLuckyWheel,
		'createRandomItem': createRandomItem,
		'createRandomMercenery': createRandomMercenery,
		'createItem': createItem
		};
}

// Get time Scroll
exports.getTimeScrollLuckWheel = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName.toString(), 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						luckyWheelLog.goodLuckyWheel(db.getDatabase(), userId, currentDate, function(goodLuckyLogs) {
							response.end(resultResponse.createResult(1, goodLuckyLogs));
						}, function(err) {
							response.end(err);
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

// Post Claim Award battle log 
exports.postScrollLuckWheel = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName.toString(), 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						luckyWheelLog.goodLuckyWheel(db.getDatabase(), userId, currentDate, function(goodLuckyLogs) {
							if (goodLuckyLogs[0].amount == 1) {
								luckyWheelLog.createLuckyWheelLog(db.getDatabase(), userId, users[0]['userName'], function(logResult) {
									luckyWheel.findLuckyWheel(db.getDatabase(), {}, function(items) {
										var random = getRandomInt(0, items.length - 1);
										var methodName = items[random]['activeMethod'].toString();
										var methodParam = items[random]['valuesMethod'];
										if (luckyWheelActions[methodName]) {
											luckyWheelActions[methodName](userId, users[0]['userName'], methodParam, tokenId, function(luckyWheelComplete){
												luckyWheelComplete[0]['nextTimes'] = goodLuckyLogs[0]['nextTimes'];
												if (luckyWheelComplete[0]['showAllPlayer']) {
													var notice = luckyWheelComplete[0]['notice'].toString();
													var amount = luckyWheelComplete[0]['amount'].toString();
													wss.send("all_client", "{\"result\": \"Player "+users[0]['displayName']+" won "+notice+" x"+amount+" in lucky wheel !!\"}");
												}
												response.end(resultResponse.createResult(1, luckyWheelComplete));
											}, function (error) {
												response.end(error);
											});
										} else {
											response.end(errorResponse.createErrorCode(1, "Can not find lucky wheel award."));
										}
									}, function (errorFind) {
										response.end(errorFind);
									} );
								}, function(err) {
									response.end(err);
								}); 
							} else {
								response.end(resultResponse.createResult(1, goodLuckyLogs));
							}
						}, function(err) {
							response.end(err);
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

function emptyLuckyWheel(userId, uName, methodParam, tokenId, complete, error) {
	if (complete) {
		complete([{
					id: "LuckyWheel",
					result: "Good luck next times.",
					showAllPlayer: false,
					notice: "Good luck next times.",
					amount: 0
				}]);
	}
}

function createRandomItem(userId, uName, methodParams, tokenId, complete, error) {
	var random = getRandomInt(0, methodParams.length - 1);
	var itemGameType = methodParams[random]['id'];
	var itemAmount = parseInt (methodParams[random]['amount'].toString());
	var md5 = token.createHashId (uName, user.userActions[8]);
	var newItem = inventory.newAlreadyItem(md5, userId, itemAmount, itemGameType, 'LW');
	inventory.findItemInInventory (db.getDatabase(), {'owner': userId, 'gameType': itemGameType}, function(items) {
		inventory.updateItemInventory(db.getDatabase(), {'owner': userId, 'gameType': itemGameType}, {'$inc': {'amount': itemAmount}}, function(findItemResult) {
			if (findItemResult) {
				item.findItem(db.getDatabase(), {'id': findItemResult['gameType'].toString()}, function(findItems){
					if (complete) {
						complete ([{
								id: "LuckyWheel",
								result: "You won " + itemAmount + " Item " + findItems[0]['name'],
								showAllPlayer: true,
								notice: findItems[0]['name'],
								amount: itemAmount
							}]);
					}
				}, function(err) {
					if (error) {
						error(errorResponse.createErrorCode(1, "Item Not Found !!"));
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Update Item Amount Fail !!"));
				}
			}
		}, function(err) {
			if (error) {
				error(errorResponse.createErrorCode(1, "Update Item Amount Error !!"));
			}
		});
	}, function (error) {
		inventory.createItemInInventory(db.getDatabase(), newItem, function(createItemResult) {
			item.findItem(db.getDatabase(), {'id': itemGameType.toString()}, function(findItems){
				if (complete) {
					complete ([{
							id: "LuckyWheel",
							result: "You won " + itemAmount + " Item " + findItems[0]['name'],
							showAllPlayer: true,
							notice: findItems[0]['name'],
							amount: itemAmount
						}]);
				}
			}, function(err) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Item Not Found !!"));
				}
			});
		}, function(err) {
			response.end(errorResponse.createErrorCode(1, "Create Item Fail !!"));
		});
	});
}

function createItem(userId, uName, methodParams, tokenId, complete, error) {
	var random = 0;
	var itemGameType = methodParams[random]['id'];
	var itemAmount = parseInt (methodParams[random]['amount'].toString());
	var md5 = token.createHashId (uName, user.userActions[8]);
	var newItem = inventory.newAlreadyItem(md5, userId, itemAmount, itemGameType, 'LW');
	inventory.findItemInInventory (db.getDatabase(), {'owner': userId, 'gameType': itemGameType}, function(items) {
		inventory.updateItemInventory(db.getDatabase(), {'owner': userId, 'gameType': itemGameType}, {'$inc': {'amount': itemAmount}}, function(findItemResult) {
			if (findItemResult) {
				item.findItem(db.getDatabase(), {'id': findItemResult['gameType'].toString()}, function(findItems){
					if (complete) {
						complete ([{
								id: "LuckyWheel",
								result: "You won " + itemAmount + " Item " + findItems[0]['name'],
								showAllPlayer: true,
								notice: findItems[0]['name'],
								amount: itemAmount
							}]);
					}
				}, function(err) {
					if (error) {
						error(errorResponse.createErrorCode(1, "Item Not Found !!"));
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Update Item Amount Fail !!"));
				}
			}
		}, function(err) {
			if (error) {
				error(errorResponse.createErrorCode(1, "Update Item Amount Error !!"));
			}
		});
	}, function (error) {
		inventory.createItemInInventory(db.getDatabase(), newItem, function(createItemResult) {
			item.findItem(db.getDatabase(), {'id': itemGameType.toString()}, function(findItems){
				if (complete) {
					complete ([{
							id: "LuckyWheel",
							result: "You won " + itemAmount + " Item " + findItems[0]['name'],
							showAllPlayer: true,
							notice: findItems[0]['name'],
							amount: itemAmount
						}]);
				}
			}, function(err) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Item Not Found !!"));
				}
			});
		}, function(err) {
			response.end(errorResponse.createErrorCode(1, "Create Item Fail !!"));
		});
	});
}

function createRandomMercenery(userId, uName, methodParams, tokenId, complete, error) {
	var random = getRandomInt(0, methodParams.length - 1);
	var merGameType = methodParams[random]['id'];
	var merLevel = parseInt (methodParams[random]['level'].toString());
	var currentDate = new Date();
	sodier.findSodier(db.getDatabase(), {'id': merGameType.toString()}, function(sodiers) {
		var newMercenary = mercenary.parseMercenary(sodiers[0]);
		var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + currentDate +"'}";
		var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
		newMercenary.id = md5;
		newMercenary.level = merLevel;
		newMercenary.owner = userId;
		newMercenary.hiredDate = currentDate;
		newMercenary.tokenAvailable = tokenId.toString();
		mercenary.createMercenary (db.getDatabase(), newMercenary, function(createComplete) {
			if (complete) {
				complete([{
					id: "LuckyWheel",
					result: "You have Sodier " + createComplete[0]['name'] + " Lv:" + merLevel,
					showAllPlayer: true,
					notice: createComplete[0]['name'] + " Lv:" + merLevel,
					amount: 1
				}]);
			}
		}, error);
	}, error);
}


