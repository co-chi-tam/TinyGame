require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/simpleData')();
require('../models/battleLog')();
require('../models/token')();
require('../models/user')();
require('../models/map')();
require('../models/worldMap')();

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

// Get All Battle Log
exports.getAllBattleLog = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var pageIndex = request.query.page;
		var amountItem = request.query.amount;
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (pageIndex && amountItem && uName && tokenId) {
			var limit = parseInt (amountItem.toString());
			var skip = (parseInt (pageIndex.toString()) - 1) * limit;
			if (limit > 0 && skip >= 0) {
				var currentDate = new Date();
				token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
					var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
					if (verifyToken.verify == true) {
						user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
							battleLog.findSkipAndLimitBattleLog(db.getDatabase(), {'owner': uName}, skip, limit, function(logResults) {
								var logs = [{
									id: "BattleLog-" + uName,
									token: tokenId
								}];
								for	(var i = 0; i < logResults.length; i++) {
									delete logResults[i]['_id'];
									delete logResults[i]['createDate'];
									delete logResults[i]['active'];
									delete logResults[i]['target'];
								}
								logs[0]['logs'] = logResults;
								response.write(resultResponse.createResult(1, logs));
								response.end();
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
				response.end(errorResponse.createErrorCode(1, "Page not negative."));
			}
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

// Post battle log 
exports.postBattleLog = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mapId = request.body.mid;
		var battleResult = request.body.battle;
		var pvp = request.body.pvp;
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (pvp && uName && mapId && battleResult && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName.toString(), 'isLogin': true, 'active': true}, function(users) {
						var successBaltle = parseInt (battleResult.toString());
						var battleAlreadyLog = battleLog.newAlreadyBattleLog(users[0]['_id'], uName.toString(), mapId.toString(), successBaltle);
						if (parseInt (pvp.toString()) == 1) {
							map.findMap(db.getDatabase(), {'id': mapId.toString()}, function(mapResults) {
								if (successBaltle) {
									wss.send("all_client", "{\"result\": \"Player "+users[0]['displayName']+" have a victory in PVP !!\"}");
								}
								createBattleLog (db.getDatabase(), battleAlreadyLog, uName.toString(), successBaltle, currentDate, function (createLogResult) {
									response.write(resultResponse.createResult(1, createLogResult));
									response.end();
								}, function(err) {
									response.end(err);
								});
							}, function(err) {
								response.end(err);
							});
						} else {
							worldMap.findMap(db.getDatabase(), {'id': mapId.toString()}, function(mapResults) {
								createBattleLog (db.getDatabase(), battleAlreadyLog, uName.toString(), successBaltle, currentDate, function (createLogResult) {
									response.write(resultResponse.createResult(1, createLogResult));
									response.end();
								}, function(err) {
									response.end(err);
								});
							}, function(err) {
								response.end(err);
							});
						}
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
exports.postClaimAwardBattleLog = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var battleId = request.body.bid;
		var uName = request.body.uname;
		var pvp = request.body.pvp;
		var tokenId = request.body.token;
		if (uName && battleId && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					battleLog.findAndUpdateBattleLog (db.getDatabase(), {'id':battleId.toString(), 'owner': uName.toString(), 'active': false }, { 'active': true }, function (logResult) {
						if (logResult) {
							var mapId = logResult['target'];
							var successBaltle = parseInt (logResult['battleResult']);
							if (parseInt (pvp.toString()) == 1) {
								map.findMap(db.getDatabase(), {'id': mapId.toString()}, function(mapResults){
									ClaimAwardBattleLog(db.getDatabase(), uName.toString(), successBaltle, currentDate, mapResults, function (logs) {
										response.write(resultResponse.createResult(1, logs));
										response.end();
									}, function(err) {
										response.end(err);
									});
								}, function(err) {
									response.end(err);
								});
							} else {
								worldMap.findMap(db.getDatabase(), {'id': mapId.toString()}, function(mapResults){
									ClaimAwardBattleLog(db.getDatabase(), uName.toString(), successBaltle, currentDate, mapResults, function (logs) {
										response.write(resultResponse.createResult(1, logs));
										response.end();
									}, function(err) {
										response.end(err);
									});
								}, function(err) {
									response.end(err);
								});
							}
						} else {
							response.end(errorResponse.createErrorCode(1, "Can not claim award !!"));
						}
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

function createBattleLog(database, alreadyBattleLog, uName, successBaltle, currentDate, complete, error) {
	battleLog.createBattleLog (database, alreadyBattleLog, function (createResult) {
		var logs = [{
			id: "BattleLog-" + uName,
			awardCode: alreadyBattleLog.id,
			result: successBaltle.toString(),
			createDate: currentDate
		}];
		if (complete) {
			complete (logs);
		}
	}, error);
}

function ClaimAwardBattleLog(database, uName, successBaltle, currentDate, mapResults, complete, error) {
	var logs = [{
		id: "BattleLog-" + uName,
		result: successBaltle.toString(),
		goldAward: 0,
		createDate: currentDate
	}];
	if (successBaltle == 1) {
		var goldAward = parseInt (mapResults[0]['goldAward'].toString());
		user.findAndModifyUser(database, { 'userName': uName.toString(), 'isLogin': true, 'active': true }, {'$inc':{'gold': goldAward}}, function (userFound) {
			if (userFound) {
				logs[0]['goldAward'] = goldAward;
				if (complete) {
					complete(logs);
				}
			} else {
				if (error) {
					error();
				}
			}
		}, error);
	} else {
		if (error) {
			error();
		}
	}
}
