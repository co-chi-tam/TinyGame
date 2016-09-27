require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	this.tokenSecretKey = "Zt8G01Jf";
	// Token Manager
	this.token = {
		collectionName: "tokens",
		expireDay: 1,
		// Find Token
		findToken: function (database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, query, complete, error);
		},
		// Create token
		createToken: function(database, uId, valueToHash, currentDate, complete, error) {
			var alreadyToken = this.newAlreadyToken(uId, valueToHash, currentDate);
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, { 'ownerId': uId }, function(items) {
				simpleData.findAndUpdateData(database, { 'ownerId': uId }, {token: alreadyToken.token, createdTime: alreadyToken.createdTime, expireTime: alreadyToken.expireTime}, function() {
					if (complete) {
						complete(alreadyToken);
					}
				}, error);
			}, function(err) {
				simpleData.createData (database, alreadyToken, function() {
					if (complete) {
						complete(alreadyToken);
					}
				}, error);
			});
		}, 
		// Find good token
		goodToken: function(database, curToken, curTime, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, { 'token': curToken.toString() }, function(items) {
				var expireTime = new Date (Date.parse (items[0]['expireTime'].toISOString()));
				if (curTime.getTime() < expireTime.getTime()) {
					if (complete) {
						complete([{
							id: items[0]['token'],
							token: items[0]['token'],
							createdTime: items[0]['createdTime']
						}]);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(601, "Token no longer available !!! Please try LOGOUT and LOGIN..."));
					}
				}
			}, function(err) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Token not found !!! " + JSON.stringify(err)));
				}
			});
		},
		// New token
		newAlreadyToken: function(uId, valueToHash, currentDate){
			var expireDate = new Date();
			expireDate.setDate (currentDate.getDate() + this.expireDay);
			var md5 = crypto.createHash('md5').update(valueToHash + this.tokenSecretKey).digest('hex');
			var token = {
				ownerId: uId,
				token: md5,
				createdTime: currentDate,
				expireTime: expireDate
			};
			return token;
		},
		// Verify token 
		verifyToken: function(token, dName, actNames, cDate) {
			for(var i = 0; i < actNames.length; i++) {
				var valueToHash = "{u:'" + dName + "',a:'" + actNames[i] + "',t:'" + cDate +"'}";
				var md5 = crypto.createHash('md5').update(valueToHash + this.tokenSecretKey).digest('hex');
				if (token.toString() == md5.toString()) {
					return {
						action: i,
						verify: true
					};
				}
			}
			return {
					action: -1,
					verify: false
				};
		},
		createHashId: function(userName, userAction){
			var currentDate = new Date();
			var valueToHash = "{u:'" + userName + "',a:'" + userAction + "',t:'" + currentDate +"'}";
			var hashId = crypto.createHash('md5').update(valueToHash).digest('hex');
			return hashId;
		}
	}
}	





