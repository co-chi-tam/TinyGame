require('../utils/UtilJS')();
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/user')();
require('../models/sodier')();
require('../models/mercenary')();
require('../models/token')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');
var db;
var wss;

exports.setup = function(callback) { callback(null); }

exports.init = function(database, websocket) {
	db = database;
	wss = websocket;
}

// Calculate Level Rate
exports.getLevelRate = function (request, response) {
	var level = request.query.level;
	if (level) {
		var levelInt = parseInt(level.toString());
		var rate = calculatePercentRate(levelInt, 30);
		var random = getRandomArbitrary(1.0, 100.0);
		var result = (random <= rate ? "Level up" : "Fail");
		response.write(resultResponse.createResult(1, [{
			id: "Level rate",
			level: levelInt,
			rate: rate,
			result: result
		}]));
		response.end();
	} else {
		response.end(errorResponse.createErrorCode(1, "Field not empty."));
	}
}

// Get All Mercenary
exports.getAllMercenaries = function(request, response){
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
						mercenary.findAllMercenary(db.getDatabase(), userId, function(items) {
							if (items) {
								var mercenaries = [];
								for (var i = 0; i < items.length; i++) {
									mercenaries.push(mercenary.simpleMercanry(items[i]));
								}
								response.write(resultResponse.createResult(1, mercenaries));
								response.end();
							} else {
								response.end(errorResponse.createErrorCode(1, "None Mercenary !!"));
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

// Get Mercenary
exports.getMercenary = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var merId = request.query.merid;
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (merId && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						mercenary.findMercenary(db.getDatabase(), merId.toString(), userId, function(mer) {
							if (mer) {
								var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + mer[0].hiredDate +"'}";
								var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
								if (md5.toString() == merId.toString()) {
									response.write(resultResponse.createResult(1, mer));
									response.end();
								} else {
									response.end(errorResponse.createErrorCode(1, "None Mercenary !!"));
								}
							} else {
								response.end(errorResponse.createErrorCode(1, "Mercenary not verify complete"));
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

// Create Mercenary
exports.postCreateMercenary = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var sId = request.body.sid;
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (sId && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						sodier.findSodier(db.getDatabase(), {'id': sId.toString()}, function(sodiers) {
							var newMercenary = mercenary.parseMercenary(sodiers[0]);
							var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + currentDate +"'}";
							var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
							newMercenary.id = md5;
							newMercenary.owner = userId;
							newMercenary.hiredDate = currentDate;
							newMercenary.tokenAvailable = tokenId.toString();
							mercenary.createMercenary (db.getDatabase(), newMercenary, function(comp) {
								response.write(resultResponse.createResult(1, comp));
								response.end();
							}, function(err ) {
								response.end(err);
							});
						}, function(err ) {
							response.end(err);
						});
					}, function(err ) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
			}, function(err ) {
				response.end(err);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

// Post Dismiss Mercenary
exports.postDismissMercenary = function(request, response) {
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mercenaryId = request.body.merid;
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (mercenaryId && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						mercenary.findAndUpdateMercenary(db.getDatabase(), {'id': mercenaryId.toString(), 'owner': objectUserId}, {'dismiss': true, 'dismissDate': currentDate, 'tokenAvailable': tokenId.toString()}, function(mer) {
							if (mer) {
								response.write(resultResponse.createResult(1, [{
									id: mercenaryId.toString(),
									result: "Dismiss complete!!",
									token: tokenId.toString()
								}]));
								response.end();
							} else {
								response.end(errorResponse.createErrorCode(1, "Mercenary not verify complete"));
							}	
						}, function(err) {
							response.end(err);
						});
					}, function(err ) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
			}, function(err ) {
				response.end(err);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

// Post Level Up Mercenary
exports.postLevelUpMercenary = function(request, response) {
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mercenaryId = request.body.merid;
		var material1 = request.body.mat1;
		var material2 = request.body.mat2;
		var material3 = request.body.mat3;
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (mercenaryId && material1 && material2 && material3 && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var materialMers = [];
						materialMers.push({id: material1.toString()});
						materialMers.push({id: material2.toString()});
						materialMers.push({id: material3.toString()});
						mercenary.findMercenary(db.getDatabase(), mercenaryId.toString(), userId, function(mer) {
							if (mer) {
								var gameType = mer[0]['gameType'];
								var level = mer[0]['level'];
								var levelInt = parseInt(level.toString());
								mercenary.findMercenariesQuery(db.getDatabase(), {'owner': userId, 'dismiss': false, 'gameType': gameType, 'id':{'$ne': mercenaryId.toString()}, 'level': {'$gte': level}, $or: materialMers}, function(mercenaries) {
									if (mercenaries && mercenaries.length == 3) {
										mercenary.updateMercenary(db.getDatabase(),{ 'owner': userId, 'dismiss': false, $or: materialMers}, {'dismiss': true, 'dismissDate': currentDate, 'tokenAvailable': tokenId.toString()}, function() {
											var rate = calculatePercentRate(levelInt, 30);
											var random = getRandomArbitrary(1.0, 100.0);
											if (random <= rate) {
												mercenary.findAndModifyMercenary(db.getDatabase(), { 'owner': userId, 'dismiss': false, 'id': mercenaryId.toString() }, {'$inc': {'level': 1}, '$set': {'tokenAvailable': tokenId.toString()}}, function(mercenaryLevelUpResults) {
													if (mercenaryLevelUpResults) {
														var merName = mercenaryLevelUpResults['name'];
														var merLevel = parseInt (mercenaryLevelUpResults['level'].toString()) + 1;
														wss.send("all_client", "{\"result\": \"Player "+users[0]["displayName"]+" was level up "+merName+" to Lv:"+merLevel+" !!\"}");
														response.write(resultResponse.createResult(1, [{
															id: mercenaryId.toString(),
															result: 1,
															token: tokenId.toString()
														}]));
														response.end();
													} else {
														response.end(errorResponse.createErrorCode(1, "Level up not complete !"));
													}
												}, function (errorFind) {
													response.end(errorFind);
												});
											} else {
												response.write(resultResponse.createResult(1, [{
													id: mercenaryId.toString(),
													result: 0,
													token: tokenId.toString()
												}]));
												response.end();
											}
										}, function (errorUpdateMaterial) {
											response.end(errorUpdateMaterial);
										});
									} else {
										response.end(errorResponse.createErrorCode(1, "Material Mercenary not found or level not greater and equal!"));
									}
								}, function (errorFindMatertial) {
									response.end(errorFindMatertial);
								});
							} else {
								response.end(errorResponse.createErrorCode(1, "Mercenary found !!"));
							}
						}, function(err) {
							response.end(err);
						});
					}, function (errorFindUser) {
						response.end(errorFindUser);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
			}, function(err ) {
				response.end(err);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

function calculatePercentRate(levelTarget, percentDesc) {
	if (levelTarget >= 10 || levelTarget < 1) {
		return 0.0;
	}
	var max = 100.0;
	for(var i = 0; i < 10; i++) {
		var thirtyPercent = (max / 100.0) * percentDesc;
		var rate = max - thirtyPercent;
		max = rate;
		if (levelTarget - 1 == i) {
			return rate < 1.0 ? 1.0 : rate;
		}
	}
	return 0.0;
} 

