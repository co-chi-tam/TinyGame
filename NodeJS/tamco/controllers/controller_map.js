require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/map')();
require('../models/simpleData')();
require('../models/user')();
require('../models/sodier')();
require('../models/mercenary')();
require('../models/worldMap')();
require('../models/token')();
require('../models/skill')();

var db;
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');

exports.setup = function(callback) { callback(null); }

exports.init = function(database) {
	db = database;
}

// Get Map base ID
exports.getMap = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mapId = request.query.mid;
		var tokenId = request.query.token;
		if (mapId && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				map.findMap(db.getDatabase(), {'id': mapId.toString()}, function(items) {
					var slots = items[0]['slots'];
					response.write(resultResponse.createResult(1, slots));
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

// Get Map Formation
exports.getMapFormation = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mapId = request.query.mid;
		var tokenId = request.query.token;
		if (mapId && tokenId) {
			var currentDate = new Date();
			var mapItems = null;
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				worldMap.findMap(db.getDatabase(), {'id': mapId.toString()}, function (items) {
					mapItems = items;
					delete mapItems[0]['_id'];
					delete mapItems[0]['owner'];
					var slots = items[0]['slots'];
					var sodierIds = [];
					for (var i = 0; i < slots.length; i++) {
						sodierIds.push({
							id: slots[i]['gameType'].toString()
						});
					}
					sodier.findSodier(db.getDatabase(), {$or: sodierIds}, function (sodiers) {
						var skillIds = [];
						for (var i = 0; i < sodiers.length; i++) {
							var skills = sodiers[i]['skillSlots'][0];
							skillIds.push({
								id: skills.toString()
							});
						}
						mapItems[0]['sodiers'] = sodiers;
						skill.findSkill(db.getDatabase(), {$or: skillIds}, function(skillResults) {
							for (var i = 0; i < skillResults.length; i++) {
								delete skillResults[i]['_id'];
							}
							mapItems[0]['skills'] = skillResults;
							response.write(resultResponse.createResult(1, mapItems));
							response.end();
						}, function(err) {
							response.end(err);
						});
					}, function(err) {
						response.end(err);
					});
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

// Get User Map Formation
exports.getUserMapFormation = function(request, response){
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
						map.findMap(db.getDatabase(), {'owner': objectUserId}, function (items) {
							mapItems = items;
							delete mapItems[0]['_id'];
							delete mapItems[0]['owner'];
							items[0]['id'] = 'map-' + uName;
							var slots = items[0]['slots'];
							var mercenaryIds = [];
							for (var i = 0; i < slots.length; i++) {
								if (slots[i].active == true) { 
									mercenaryIds.push({
										id: slots[i]['gameType'].toString()
									});
								}
							}
							mercenary.findMercenariesQuery(db.getDatabase(), {"owner":objectUserId, 'dismiss': false, $or: mercenaryIds}, function (sodiers) {
								var skillIds = [];
								for (var i = 0; i < sodiers.length; i++) {
									delete sodiers[i]['tokenAvailable'];
									delete sodiers[i]['hiredDate'];
									delete sodiers[i]['dismiss'];
									delete sodiers[i]['dismissDate'];
									var skills = sodiers[i]['skillSlots'][0];
									skillIds.push({
										id: skills.toString()
									});
								}
								mapItems[0]['sodiers'] = sodiers;
								skill.findSkill(db.getDatabase(), {$or: skillIds}, function(skillResults) {
									for (var i = 0; i < skillResults.length; i++) {
										delete skillResults[i]['_id'];
									}
									mapItems[0]['skills'] = skillResults;
									response.write(resultResponse.createResult(1, mapItems));
									response.end();
								}, function(err) {
									response.end(err);
								});
							}, function(err) {
								response.end(err);
							});
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

// Get PVP Map Formation
exports.getPVPMapFormation = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var mapId = request.query.mapid;
		var pvpName = request.query.pvpuname;
		var uName = request.query.uname;
		var tokenId = request.query.token;
		if (mapId && pvpName && uName && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'displayName':pvpName.toString(), 'active': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						map.findMap(db.getDatabase(), {'owner': objectUserId}, function (items) {
							mapItems = items;
							delete mapItems[0]['_id'];
							delete mapItems[0]['owner'];
							items[0]['id'] = 'map-' + pvpName;
							var slots = items[0]['slots'];
							var mercenaryIds = [];
							for (var i = 0; i < slots.length; i++) {
								if (slots[i].active == true) { 
									mercenaryIds.push({
										id: slots[i]['gameType'].toString()
									});
								}
							}
							mercenary.findMercenariesQuery(db.getDatabase(), {"owner":objectUserId, 'dismiss': false, $or: mercenaryIds}, function (sodiers) {
								var skillIds = [];
								for (var i = 0; i < sodiers.length; i++) {
									delete sodiers[i]['tokenAvailable'];
									delete sodiers[i]['hiredDate'];
									delete sodiers[i]['dismiss'];
									delete sodiers[i]['dismissDate'];
									var skills = sodiers[i]['skillSlots'][0];
									skillIds.push({
										id: skills.toString()
									});
								}
								mapItems[0]['sodiers'] = sodiers;
								skill.findSkill(db.getDatabase(), {$or: skillIds}, function(skillResults) {
									for (var i = 0; i < skillResults.length; i++) {
										delete skillResults[i]['_id'];
									}
									mapItems[0]['skills'] = skillResults;
									response.write(resultResponse.createResult(1, mapItems));
									response.end();
								}, function(err) {
									response.end(err);
								});
							}, function(err) {
								response.end(err);
							});
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

// Post User Map Formation
exports.postCreateUserMapFormation = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (uName && tokenId) {
			var mName = 'mName-' + uName;
			var mDisplayImage = 'mDisplayImage-' + uName;
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true}, function(users) {
						var userId = users[0]['_id'];
						var objectUserId = new Mongo.ObjectId(userId.toString());
						map.findMap(db.getDatabase(), {'owner': objectUserId}, function (items) {
							response.end(errorResponse.createErrorCode(1, "Map already created !!"));
						}, function (errorCreated) {
							var valueToHash = "{u:'" + uName + "',a:'" + user.userActions[5] + "',t:'" + currentDate +"'}";
							var md5 = crypto.createHash('md5').update(valueToHash).digest('hex');
							var mSlots = [];
							map.createEmptyMap (db.getDatabase(), md5, mName, mDisplayImage, userId, mSlots, function(items) {
								response.write(resultResponse.createResult(1, items));
								response.end();
							}, function(err) {
								response.end(err);
							});
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

// Post Update Map Formation
exports.postUpdateUserMapFormation = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.body.uname;
		var tokenId = request.body.token;
		if (uName && tokenId) {
			// "0,0000000000000000,-1,-1"
			var slot1 = request.body.slot1;
			var slot2 = request.body.slot2;
			var slot3 = request.body.slot3;
			var slot4 = request.body.slot4;
			var slot5 = request.body.slot5;
			var slot6 = request.body.slot6;
			var slot7 = request.body.slot7;
			var slot8 = request.body.slot8;
			var slot9 = request.body.slot9;
			if (slot1 && slot2 && slot3 && slot4 && slot5 && slot6 && slot7 && slot8 && slot9) {
				var mSlots = [];
				var mercenaryIds = [];
				var active = false;
				var slotValue1 = stringToSlotValue (0, slot1.toString());
				if (findDuplicated(slotValue1, mSlots) == false && slotValue1['active'] == true) {
					mSlots.push (slotValue1);
					mercenaryIds.push({
						id: slotValue1['gameType'].toString()
					});
					active = true;
				} 
				var slotValue2 = stringToSlotValue (1, slot2.toString());
				if (findDuplicated(slotValue2, mSlots) == false && slotValue2['active'] == true) {
					mSlots.push (slotValue2);
					mercenaryIds.push({
						id: slotValue2['gameType'].toString()
					});
					active = true;
				}
				var slotValue3 = stringToSlotValue (2, slot3.toString());
				if (findDuplicated(slotValue3, mSlots) == false && slotValue3['active'] == true) {
					mSlots.push (slotValue3);
					mercenaryIds.push({
						id: slotValue3['gameType'].toString()
					});
					active = true;
				}
				var slotValue4 = stringToSlotValue (3, slot4.toString());
				if (findDuplicated(slotValue4, mSlots) == false && slotValue4['active'] == true) {
					mSlots.push (slotValue4);
					mercenaryIds.push({
						id: slotValue4['gameType'].toString()
					});
					active = true;
				}
				var slotValue5 = stringToSlotValue (4, slot5.toString());
				if (findDuplicated(slotValue5, mSlots) == false && slotValue5['active'] == true) {
					mSlots.push (slotValue5);
					mercenaryIds.push({
						id: slotValue5['gameType'].toString()
					});
					active = true;
				}
				var slotValue6 = stringToSlotValue (5, slot6.toString());
				if (findDuplicated(slotValue6, mSlots) == false && slotValue6['active'] == true) {
					mSlots.push (slotValue6);
					mercenaryIds.push({
						id: slotValue6['gameType'].toString()
					});
					active = true;
				}
				var slotValue7 = stringToSlotValue (6, slot7.toString());
				if (findDuplicated(slotValue7, mSlots) == false && slotValue7['active'] == true) {
					mSlots.push (slotValue7);
					mercenaryIds.push({
						id: slotValue7['gameType'].toString()
					});
					active = true;
				}
				var slotValue8 = stringToSlotValue (7, slot8.toString());
				if (findDuplicated(slotValue8, mSlots) == false && slotValue8['active'] == true) {
					mSlots.push (slotValue8);
					mercenaryIds.push({
						id: slotValue8['gameType'].toString()
					});
					active = true;
				}
				var slotValue9 = stringToSlotValue (8, slot9.toString());
				if (findDuplicated(slotValue9, mSlots) == false && slotValue9['active'] == true) {
					mSlots.push (slotValue9);
					mercenaryIds.push({
						id: slotValue9['gameType'].toString()
					});
					active = true;
				}
				if (active == true) {
					var currentDate = new Date();
					token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
						var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
						if (verifyToken.verify == true) {
							user.findUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, function(users) {
								var userId = users[0]['_id'];
								map.updateMapFormation(db.getDatabase(), userId, mSlots, function(items) {
									mercenary.findMercenariesQuery(db.getDatabase(), {'owner': userId, 'dismiss': false,  $or: mercenaryIds}, function(mercenaries) {
										var maxHealth = 0;
										var maxDamage = 0;
										var maxDefend = 0;
										var power = 0;
										for (var i = 0; i < mercenaries.length; i++) {
											maxHealth += mercenaries[i]['maxHealth'];
											maxDamage += mercenaries[i]['damage'];
											maxDefend += mercenaries[i]['defend'];
										}
										power = maxHealth + maxDamage + maxDefend;
										user.findAndUpdateUser(db.getDatabase(), {'userName':uName, 'isLogin': true, 'active': true}, {'power': power}, function(compleUpdatePower) {
											items[0]['power'] = power;
											response.write(resultResponse.createResult(1, items));
											response.end();
										}, function (errorUpdatePower) {
											response.end(errorUpdatePower);
										});
									}, function (errorFindMercenary) {
										response.end(errorFindMercenary);
									});
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
					response.end(errorResponse.createErrorCode(1, "Map formation not active."));
				}
			} else {
				response.end(errorResponse.createErrorCode(1, "Field not empty."));
			}
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

var stringToSlotValue = function(index, strValue) {
	var arrayValue = strValue.split(",").map(function (val) {
	  return val.toString();
	});
	var slotValue = {
				id:index,
				gameType:arrayValue[1],
				slotIds:[arrayValue[2], arrayValue[3]],
				active: (arrayValue[0] == '0' ? false : true)
			}
	return slotValue;
}

var findDuplicated = function(slotValue, slotArray) {
	for (var i = 0; i < slotArray.length; i++) {
		if (slotValue['gameType'] == slotArray[i]['gameType']) 
		{
			return true;
		}
	}
	return false;
}
