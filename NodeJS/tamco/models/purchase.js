require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();
require('../models/token')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// Maps manager
	this.purchase = {
		collectionName: "purchase",
		// Find map
		findPurchase: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		},
		createPurchase: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.createData(database, query, complete, error);
		},
		// New Already Map
		newAlreadyPurchase: function(userName, productName, amount, ownerId, goldPrice, diamondPrice, currentUserGold, currentUserDiamond, tokenId) {
			var hashId = token.createHashId(userName, user.userActions[10]);
			var newPurchase = {
				id: hashId,
				nameProduct: productName,
				amountProduct: amount,
				totalGold: goldPrice,
				totalDiamond: diamondPrice,
				currentUserGold: currentUserGold,
				currentUserGold: currentUserDiamond,
				owner: ownerId,
				createdDate: new Date(),
				tokenAccess: tokenId
			};
			return newPurchase;
		}
	}
}
