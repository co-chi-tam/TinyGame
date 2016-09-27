require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;

module.exports = function() {
	// Sodier manager
	this.sodier = {
		collectionName: "sodiers",
		// Find Sodier
		findSodier: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, function(items) {
				if (items && items.length > 0) {
					for(var i = 0; i < items.length; i++) {
						delete items[i]['_id'];
					}
					if (complete) {
						complete(items);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "Not found !!!"));
					}
				}
			}, error);
		},
		// Create Sodier
		createSodier: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		// Update Sodier
		findAndUpdate: function (database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData(database, query, item, complete, error);
		},
		newAlreadySodier: function(){
			var newSodier = {
				id:"",
				name:"",
				modelPath:"",
				avatar: "none",
				currentHealth:0,
				maxHealth:0,
				currentMana: 0,
				maxMana: 0,
				currentRage: 0,
				maxRage: 0,
				damage:0,
				defend:0,
				moveSpeed:0,
				attackSpeed:0,
				attackRange:0,
				skillSlots:[0], 
				level: 0,
				gameType: ''
			};
			return newSodier;
		}
	}
}
