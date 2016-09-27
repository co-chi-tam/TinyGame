require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// Item Manager
	this.item = {
		collectionName: "items",
		createItem: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		findItem: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		},
		newAlreadyItem: function () {
			var itemAlready = {
				id: '',
				name: '',
				modelPath: '',
				avatar: '',
				activeMethod: 'emptyItem',
				valuesMethod: [],
				amountPerConsume: 1,
				// owner: '',
				// active: false,
				// consume: false,
				// createdDate: '',
				// consumeDate: '',
				// tokenVerify: '',
			};
			return itemAlready;
		}
	}
}	





