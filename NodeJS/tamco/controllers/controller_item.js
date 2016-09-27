require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/simpleData')();
require('../models/sodier')();
require('../models/mercenary')();
require('../models/item')();
require('../models/token')();
require('../models/user')();

var db;
var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');

exports.setup = function(callback) { callback(null); }

exports.init = function(database) {
	db = database;
}

// Get All Item
exports.getAllItem = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var tokenId = request.query.token;
		if (tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				item.findItem(db.getDatabase(), {}, function(items){
					response.write(resultResponse.createResult(1, items));
					response.end();
				}, function (error) {
					response.end(error);
				});
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

// Get Item base ID
exports.getItem = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var itemId = request.query.iid;
		var tokenId = request.query.token;
		if (itemId && tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				item.findItem(db.getDatabase(), {"id": itemId.toString()}, function(items){
					response.write(resultResponse.createResult(1, items));
					response.end();
				}, function (error) {
					response.end(error);
				});
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
