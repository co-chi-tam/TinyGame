require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;

module.exports = function() {
	// Maps manager
	this.worldMap = {
		collectionName: "worldMap",
		// Find map
		findMap: function (database, query, complete, error) {
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
		findSkipAndLimitMap: function(database, query, skip, limit, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findSkipAndLimitData (database, query, skip, limit, complete, error);
		}
	}
}
