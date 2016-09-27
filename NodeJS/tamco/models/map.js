require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;

module.exports = function() {
	// Maps manager
	this.map = {
		collectionName: "maps",
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
		},
		// Create Empty Map
		createEmptyMap: function (database, mId, mName, mDisplayName, mOwerId, mSlots, goldAward, diamondAward, complete, error) {
			var newMap = {  id: mId,
							name: mName,
							displayImage: mDisplayName,
							bg: "BattleBG_11",
							owner: mOwerId,
							slots: mSlots,
							goldAward: goldAward,
							diamondAward: diamondAward,
							itemAward: [] };
							/*{
								id:'',
								gameType:'',
								slotIds:[-1,-1],
								active: false
							}*/
			simpleData.collectionName = this.collectionName;
			simpleData.createData (database, newMap, function() {
				if (complete) {
					complete(newMap);
				}
			}, error);
		},
		// Update Map formation
		updateMapFormation: function (database, ownerId, mSlots, complete, error) {
			simpleData.collectionName = this.collectionName;
			var objectUserId = new Mongo.ObjectId(ownerId.toString());
			simpleData.findAndUpdateData (database, {'owner': objectUserId}, {'slots': mSlots}, function(map) {
				if (map) {
					if (complete) {
						delete map['_id'];
						map['id'] = map['name'];
						complete([map]);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "User not existed !!!"));
					}	
				}
			}, function (err) {
				if (error) {
					error(errorResponse.createErrorCode(1, err));
				}
			});
		},
		// New Already Map
		newAlreadyMap: function() {
			var newMap = {
				id: "",
				name: "",
				displayImage: "",
				bg: "BattleBG_11",
				owner: "",
				goldAward: 0,
				diamondAward: 0,
				itemAward: [],
				slots: [{}]
			};
			return newMap;
		}
	}
}
