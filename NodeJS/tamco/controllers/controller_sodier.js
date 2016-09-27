require('../utils/UtilJS')();
require('../models/errorResponse')();
require('../models/resultResponse')();
require('../models/sodier')();
require('../models/token')();
require('../models/user')();

var db;

var sodiers = {};

exports.setup = function(callback) { callback(null); }

exports.init = function(database) {
	db = database;
	sodiers = {};
}

// Get all sodier
exports.getAllSodiers = function (request, response) {
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var tokenId = request.query.token;
		if (tokenId) {
			var currentDate = new Date();
			token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
				sodier.findSodier(db.getDatabase(), {}, function(items) {
					response.write(resultResponse.createResult(1, items));
					response.end();
				}, function(error) {
					response.end(error);
				});
			}, function(error) {
				response.end(error);
			});
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}

// Get Sodier base ID
exports.getSodier = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var sodierId = request.query.sid;
		var tokenId = request.query.token;
		if (sodierId && tokenId) {
			if (sodiers[sodierId.toString()]) {
				response.write(sodiers[sodierId.toString()]);
				response.end();
			} else {
				var currentDate = new Date();
				token.goodToken(db.getDatabase(), tokenId.toString(), currentDate, function(alreadyToken) {
					sodier.findSodier(db.getDatabase(), {'id': sodierId.toString()}, function(items) {
						var result = resultResponse.createResult(1, items);
						sodiers[sodierId.toString()] = result;
						response.write(result);
						response.end();
					}, function(error) {
						response.end(error);
					});
				}, function(error) {
					response.end(error);
				});
			}
		} else {
			response.end(errorResponse.createErrorCode(1, "Field not empty."));
		}
	} else {
		response.end(errorResponse.createErrorCode(1, "Header not empty."));
	}
}