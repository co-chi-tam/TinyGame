require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();
require('../models/user')();
require('../models/token')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// Lucky Wheel Log Manager
	this.luckyWheelLog = {
		collectionName: "luckyWheelLogs",
		expireHours: 1,
		// Find Token
		findLuckyWheelLog: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, query, complete, error);
		},
		createEmptyLuckyWheelLog: function (database, uID, uName, currentDate, complete, error) {
			simpleData.collectionName = this.collectionName;
			var valueHashed = token.createHashId(uName, user[12]);
			var alreadyLog = this.newAlreadyLog(uID, valueHashed, currentDate);
			alreadyLog['expireTime'] = currentDate;
			simpleData.createData(database, alreadyLog, function() {
				if (complete) {
					complete (alreadyLog);
				}
			}, error);
		},
		createLuckyWheelLog: function (database, uID, uName, complete, error) {
			simpleData.collectionName = this.collectionName;
			var currentDate = new Date();
			var valueHashed = token.createHashId(uName, user[12]);
			var alreadyLog = this.newAlreadyLog(uID, valueHashed, currentDate);
			simpleData.findData (database, {'owner': uID}, function(findResults){
				simpleData.findAndUpdateData(database, {'owner': uID}, {'id': alreadyLog['id'], 'createdTime': alreadyLog['createdTime'], 'expireTime': alreadyLog['expireTime']}, function(updateLogResults) {
					if (updateLogResults) {
						if (complete) {
							complete (updateLogResults);
						}
					} else {
						if (error) {
							error(errorResponse.createErrorCode(1, "Lucky Wheel Not Updated !!! "));
						}
					}
				}, error);
			}, function(findError) {
				simpleData.createData(database, alreadyLog, function() {
					if (complete) {
						complete (alreadyLog);
					}
				}, error);
			});
		},
		// Find good token
		goodLuckyWheel: function(database, uID, curTime, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, { 'owner': uID }, function(items) {
				var expireTime = new Date (Date.parse (items[0]['expireTime'].toISOString()));
				var inTime = curTime.getTime() < expireTime.getTime() ? 0 : 1;
				if (complete) {
					complete([{
							id: "LuckyWheel",
							result: inTime == 0 ? "Please wait next times." : "Let play lucky wheel.",
							amount: inTime,
							nextTimes: (expireTime.getTime() - curTime.getTime())
						}]);
				}
			}, function(err) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Log not found !!! "));
				}
			});
		},
		// New token
		newAlreadyLog: function(uId, valueHashed, currentDate){
			var expireDate = new Date();
			expireDate.setHours (currentDate.getHours() + this.expireHours);
			var log = {
				id: valueHashed,
				owner: uId,
				createdTime: currentDate,
				expireTime: expireDate
			};
			return log;
		}
	}
}	





