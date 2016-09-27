require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/user')();
require('../models/token')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');
var db;

exports.init = function(database) {
	db = database;
}

exports.setup = function(callback) { callback(null); }

// Get Map base ID
exports.getGoodLoginToken = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.query.uname;
		var macAddress = request.query.macadd;
		var tokenId = request.query.token;
		if (uName && macAddress && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				var verifyToken = token.verifyToken(tokenId.toString(), uName.toString(), user.userActions, alreadyToken[0].createdTime);
				if (verifyToken.verify == true) {
					user.findUser(db.getDatabase(), {'userName': uName/*, 'macAddress': macAddress.toString()*/}, function(users) {
						if (users) {
							var userResponse = [user.alreadyUserResponse(users[0], currentDate, alreadyToken[0].token)];
							var result = resultResponse.createResult(1, userResponse);
							response.end(result);
						} else {
							response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
						}
					}, function(err) {
						response.end(err);
					});
				} else {
					response.end(errorResponse.createErrorCode(1, "Token not verify complete"));
				}
			}, function(err) {
				response.end(err);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}