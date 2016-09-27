require('../utils/UtilJS')(); 
require('../models/errorResponse')();
require('../models/resultResponse')();

const fs = require('fs');
var db;
var wss;
var viewCache = {};

exports.setup = function(callback) { callback(null); }

exports.init = function(database, websocket) {
	db = database;
	wss = websocket;
}

exports.getChat = function(request, response){
	var verify = request.headers['verify'];
	if (verify && verify == "TinyGame") {
		var uName = request.query.uname;
		var uChat = request.query.chat;
		var tokenId = request.query.token;
		if (uChat) {
			wss.send("all_client", "{\"result\":\""+uChat+"\"}", function() {
				response.write(resultResponse.createResult(1, {id:1, result:1, chatting:uChat.toString(), serStatus: wss.getConnection().toString()}));
				response.end();	
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