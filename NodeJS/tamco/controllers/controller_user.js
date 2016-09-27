require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/user')();
require('../models/map')();
require('../models/token')();
require('../models/luckyWheelLog')();

const fs      	= require('fs');

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');
var db;
var viewCache = {};

exports.setup = function(callback) { callback(null); }
exports.init = function(database) {
	db = database;
	viewCache['register'] = fs.readFileSync(process.env.OPENSHIFT_REPO_DIR + 'views/register.html');
}

// Connect get all Player logged in
exports.getAllPVPConnects = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var pageIndex = request.query.page;
		var amountItem = request.query.amount;
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (pageIndex && amountItem && uName && tokenId) {
			var currentDate = new Date();
			var limit = parseInt (amountItem.toString());
			var skip = (parseInt (pageIndex.toString()) - 1) * limit;
			if (limit > 0 && skip >= 0) {
				token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
					user.findSkipAndLimitUser(db.getDatabase(), {'isLogin': true, 'active': true, 'power': {$gte:500}, 'userName': {'$ne':uName.toString()}}, skip, limit, function(users) {
						var userConnected = [];
						var userIds = [];
						for	(var i = 0; i < users.length; i++) {
							userConnected[i] = {
								id: users[i]['displayName'],
								user: users[i]['displayName'],
								avatar: users[i]['avatar'],
								gold: users[i]['gold']
							}
							userIds.push({
								owner: users[i]['_id']
							});
						}
						map.findMap(db.getDatabase(), {'$or': userIds}, function(maps){
							var mapIds = [];
							for	(var i = 0; i < maps.length; i++) {
								mapIds.push({
									id: maps[i].id,
									mapName: maps[i].name
								});
							}
							var connects = [{
								id: 'UserConnects',
								connects: userConnected,
								mapIds: mapIds
							}];
							response.write(resultResponse.createResult(1, connects));
							response.end();
						}, function (error) {
							response.end(error);
						}); 
					}, function(error) {
						response.end(error);
					});
				}, function(error) {
					response.end(error);
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

// Register user
exports.postRegister = function(request, response){
	var uName = request.body.uname;
	var uPass = request.body.upass;
	var uEmail = request.body.uemail;
	if (uName && uPass && uEmail) {
		if (checkLength (uName.toString(), 5, 18) && checkSpecialChar(uName.toString()) == false) {
			if (checkLength (uPass.toString(), 5, 18) && checkSpecialChar(uPass.toString()) == false) {
				if (checkLength (uEmail.toString(), 5, 18)) {
					user.registerUser(db.getDatabase(), uName, uPass, uEmail, function(complete) {
						response.write(resultResponse.createResult(1, complete));
						response.end();
					}, function(err) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Email must length greater 5 and lower 18 character."));
				}
			} else {
				response.end(errorResponse.createErrorCode(1, "Password must length greater 5 and lower 18 character."));
			}
		} else {
			response.end(errorResponse.createErrorCode(1, "User name must length greater 5 and lower 18 character."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Field not empty."));
	}
}

// Active user
exports.postActiveUser = function(request, response){
	var uName = request.body.uname;
	var dName = request.body.dname;
	var uSecretKey = request.body.secret_key;
	var tokenId = request.body.token;
	if (uName && dName && uSecretKey && tokenId) {
		if (checkLength(dName.toString(), 5, 18) && checkSpecialChar(dName.toString()) == false) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.activeUser(db.getDatabase(), uName.toString(), dName.toString(), uSecretKey.toString(), function(user) {
						if (user) {
							createFirstMapForUser (db.getDatabase(), "101", 'mName-' + dName, 'mDisplayImage-' + dName, uName.toString(), tokenId.toString(), function(comp) {
								var activeUser = [{
										id: user['userName'],
										active: true
									}];
								response.write(resultResponse.createResult(1, activeUser));
								response.end();
							}, function(error) {
								response.end(error);
							});
						} else {
							response.end(errorResponse.createErrorCode(1, "Active not complete !!"));
						}
					}, function(error) {
						response.end(error);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
			}, function(error) {
				response.end(error);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "User name must length greater 5 and lower 18 character."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Field not empty."));
	}
}

// Complete tutorial user
exports.postCompleteTutorialUser = function(request, response){
	var uName = request.body.uname;
	var tokenId = request.body.token;
	if (uName && tokenId) {
		var currentDate = new Date();
		token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
			var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
			if (verifyToken.verify == true) {
				user.completeTutorialUser(db.getDatabase(), uName.toString(), true, function(complete){
					response.write(resultResponse.createResult(1, complete));
					response.end();
				}, function(error) {
					response.end(error);
				});
			} else {
				response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
			}
		}, function(error) {
			response.end(error);
		});
	} else {
		response.end(errorResponse.createErrorCode(1, "Field not empty."));
	}
}

// Change Password
exports.postChangePassword = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var uOldPass = request.body.oldpass;
		var uNewPass = request.body.newpass;
		if (uName && uOldPass && uOldPass) {
			user.changePassword(db.getDatabase(), uName.toString(), uOldPass.toString(), uNewPass.toString(), function(complete) {
				complete[0]['id'] = 'changePassword-' + uName;
				response.write(resultResponse.createResult(1, complete));
				response.end();
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

// Login User
exports.postLogin = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var uPass = request.body.upass;
		var macAddress = request.body.macadd;
		if (uName && uPass && macAddress) {
			var currentDate = new Date();
			user.loginUser(db.getDatabase(), uName.toString(), uPass.toString(), macAddress.toString(), function(userResult) {
				token.createToken(db.getDatabase(), userResult['_id'], "{u:'" + userResult['userName'] + "',a:'" + user.userActions[0] + "',t:'" + currentDate +"'}", currentDate, 
				function(tokenResult) {
					var userResponse = [user.alreadyUserResponse(userResult, currentDate, tokenResult.token)];
					response.write(resultResponse.createResult(1, userResponse));
					response.end();
				}, function(err) {
					response.end(err);
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

// Logout User
exports.postLogout = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var uPass = request.body.upass;
		if (uName && uPass) {
			user.logoutUser(db.getDatabase(), uName.toString(), uPass.toString(), function(complete) {
				response.write(resultResponse.createResult(1, complete));
				response.end();
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

// Logout User
exports.viewRegister = function(request, response){
	response.setHeader('Content-Type', 'text/html');
	response.write(viewCache['register']);
	response.end();
}

// Create First Map For User
var createFirstMapForUser = function (database, sId, mName, mDisplayImage, uName, tokenId, complete, error) {
	var currentDate = new Date();
	user.findUser(database, {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
		var userId = users[0]['_id'];
		var objectUserId = new Mongo.ObjectId(userId.toString());
		map.findMap(database, {'owner': objectUserId}, function (items) {
			if (error) {
				error(errorResponse.createErrorCode(1, "Map already complete"));
			}
		}, function(findMapError) {
			sodier.findSodier(database, {'id': sId.toString()}, function(sodiers) {
				var newMercenary = mercenary.parseMercenary(sodiers[0]);
				var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[3] + "',t:'" + currentDate +"'}";
				var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
				var maxHealth = newMercenary.maxHealth;
				var maxDamage = newMercenary.damage;
				var maxDefend = newMercenary.defend;
				newMercenary.id = md5;
				newMercenary.owner = userId;
				newMercenary.hiredDate = currentDate;
				newMercenary.tokenAvailable = tokenId;
				mercenary.createMercenary (database, newMercenary, function(mers) {
					var mapValueToHash = "{u:'" + uName + "',a:'" + user.userActions[5] + "',t:'" + currentDate +"'}";
					var mapMd5 = crypto.createHash('md5').update(mapValueToHash).digest('hex');
					var mSlots = [{
							id: 0,
							gameType: mers[0]['id'],
							slotIds:[1, 1],
							active: true
						}];
					var goldAward = 200;
					var diamondAward = 10;
					map.createEmptyMap (database, mapMd5, mName, mDisplayImage, userId, mSlots, goldAward, diamondAward, function(createMap) {
						var valueLuckyWheelLogHashed = token.createHashId(uName, user[12]);
						var alreadyLog = luckyWheelLog.newAlreadyLog(userId, valueLuckyWheelLogHashed, currentDate);
						luckyWheelLog.createEmptyLuckyWheelLog(database, userId, uName, currentDate, function() {
							user.findAndUpdateUser(database, {'userName':uName, 'isLogin': true, 'active': true}, {'power': maxHealth + maxDamage + maxDefend}, complete, error);
						}, error);
					}, error);
				}, error);
			}, error);
		});
	}, error);
}

function checkLength(valueStr, min, max) {
	if (valueStr.length < min || valueStr.length > max)
		return false;
	return true;
}

function checkSpecialChar(valueStr) {
	 var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,./~`-=@!";
	 for(i = 0; i < specialChars.length; i++){
	   if(valueStr.indexOf(specialChars[i]) > -1){
		   return true;
		}
	 }
	 return false;
}