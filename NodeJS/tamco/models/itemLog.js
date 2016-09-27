require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	this.itemLog = {
		collectionName: "itemLogs",
		createItemLog: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		findItemLog: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		},
		newAlreadyLog: function (userName, itemId, amount, success) {
			var hashId = token.createHashId(userName, user.userActions[7]);
			var logAlready = {
				id: hashId,
				itemId: itemId,
				amount: amount,
				createDate: new Date(),
				consumeSuccess: success
			};
			return logAlready;
		}
	}
}	





