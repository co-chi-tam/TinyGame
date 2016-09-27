require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	this.battleLog = {
		collectionName: "battleLogs",
		createBattleLog: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		findBattleLog: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		},
		findAndUpdateBattleLog: function(database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData(database, query, item, complete, error);
		},
		findSkipAndLimitBattleLog: function(database, query, skip, limit, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findSkipAndLimitData (database, query, skip, limit, complete, error);
		},
		newAlreadyBattleLog: function (userId, userName, targetName, success) {
			var hashId = token.createHashId(userName + '-vs-' + targetName, user.userActions[11]);
			var logAlready = {
				id: hashId,
				createDate: new Date(),
				owner: userName,
				ownerId: userId,
				target: targetName,
				battleResult: success,
				active: !success
			};
			return logAlready;
		}
	}
}	





