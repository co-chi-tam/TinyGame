require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// Inventory Manager
	this.inventory = {
		collectionName: "inventories",
		findItemInInventory: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		},
		findItemAndUpdateInventory: function(database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData(database, query, item, complete, error);
		},
		updateItemInventory: function(database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndModifyData(database, query, item, complete, error);
		},
		createItemInInventory: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		newAlreadyItem: function(itemId, ownerId, amount, gameType, purchaseId) {
			var alreadyItem = {
				id: itemId,
				owner: ownerId,
				createdDate: new Date(),
				amount: amount,
				gameType: gameType,
				purchaseId: purchaseId
			};
			return alreadyItem;
		}
	}
}	

/* {"id":"a4b97082b4ff0bccac023a76ed12c81f","owner":ObjectId('57b58937a739d580e9a323ce'),"createdDate":new Date,"amount":1,"gameType":"2001"} */





