require('../utils/UtilJS')();
require('../models/simpleData')();
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;

var crypto = require('crypto');

module.exports = function() {
	// Mercenary manager
	this.mercenary = {
		collectionName: "mercenaries",
		// Find All Mercenary
		findAllMercenary: function (database, uId, complete, error) {
			simpleData.collectionName = this.collectionName;
			var objectId = new Mongo.ObjectId(uId.toString());
			simpleData.findDataAndSort(database, { "owner": objectId, 'dismiss': false }, {"name":1, "level": 1}, function(items) {
				if (items.length > 0) {
					for(var i = 0; i < items.length; i++) {
						delete items[i]['_id'];
						delete items[i]['owner'];
					}
					if (complete) {
						complete(items);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "Not Found"));
					}
				}
			}, error);
		},
		// Find Mercenary
		findMercenary: function (database, merId, uId, complete, error) {
			simpleData.collectionName = this.collectionName;
			var objectId = new Mongo.ObjectId(uId.toString());
			simpleData.findData(database, { "owner": objectId, "id": merId.toString(), 'dismiss': false }, function(items) {
				if (items && items.length > 0) {
					for(var i = 0; i < items.length; i++) {
						delete items[i]['_id'];
						delete items[i]['owner'];
					}
					if (complete) {
						complete(items);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "Not Found"));
					}
				}
			}, error);
		},
		// Find Mercenary Query
		findMercenariesQuery: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, function(items) {
				if (items && items.length > 0) {
					for(var i = 0; i < items.length; i++) {
						delete items[i]['_id'];
						delete items[i]['owner'];
					}
					if (complete) {
						complete(items);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "Not Found"));
					}
				}
			}, error);
		},
		// Create Mercenary
		createMercenary: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, function(results) {
				if (complete) {
					complete(results['ops']);
				}
			}, error);
		},
		// Find And Update Mercenary
		findAndUpdateMercenary: function (database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData(database, query, item, complete, error);
		},
		// Find And Update Mercenary
		findAndModifyMercenary: function (database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndModifyData(database, query, item, complete, error);
		},
		// Find And Update Mercenary
		updateMercenary: function (database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.updateData(database, query, item, complete, error);
		},
		simpleMercanry: function(item) {
			var mer = {
				id : item['id'],
				name : item['name'],
				modelPath : item['modelPath'],
				avatar: item['avatar'],
				level : item['level'],
				gameType: item['gameType']
			};
			return mer;
		},
		// Parser Sodier to Mercenary
		parseMercenary: function(sodier) {
			var mercenary = {
				id : sodier['id'],
				name : sodier['name'],
				modelPath : sodier['modelPath'],
				avatar: sodier['avatar'],
				currentHealth : sodier['currentHealth'],
				maxHealth : sodier['maxHealth'],
				currentMana: sodier['currentMana'],
				maxMana: sodier['maxMana'],
				currentRage: sodier['currentRage'],
				maxRage: sodier['maxRage'],
				damage : sodier['damage'],
				defend : sodier['defend'],
				moveSpeed : sodier['moveSpeed'],
				attackSpeed : sodier['attackSpeed'],
				attackRange : sodier['attackRange'],
				skillSlots : sodier['skillSlots'],
				level : sodier['level'],
				levelData: sodier['levelData'],
				gameType: sodier['id'],
				dismiss: false,
				dismissDate: '',
				tokenAvailable: '',
				owner : '',
				hiredDate : new Date()
			}
			return mercenary;
		}
	}
}
