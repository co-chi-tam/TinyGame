require('../utils/UtilJS')();
require('../models/errorResponse')();

const  WebSocket = require('ws');

var client;
var url = "ws://chat-tinygame.rhcloud.com:8000/socket.io/?EIO=4&transport=websocket";
var connected = false;

module.exports = function() {
	this.wss = {
		// WebSocket Manager
		getConnection: function() {
			return connected;
		},
		getSocketClient: function() {
			return client;
		},
		initializeWebSocketServer: function(openConnect, message, closeConnect, errorConnect) {
			client = new WebSocket(url, 'echo-protocol');
			client.onopen = function () {
				print('Socket connection opened properly');
				connected = true;
				if (openConnect) {
					openConnect();
				}
			};
			client.onmessage = function (evt) {
				print("Message received = " + evt.data);
				if (message) {
					message(evt.data);
				}
			};
			client.onerror = function (err) {
				print("ERROR " + err.message);
				connected = false;
				if (errorConnect) {
					errorConnect(JSON.stringify(err.message));
				}
				// Reconnect
				this.wss.initializeWebSocketServer();
			};
			client.onclose = function () {
				print("Connection closed...");
				connected = false;
				if (closeConnect) {
					closeConnect();
				}
				// Reconnect
				this.wss.initializeWebSocketServer();
			};
		}, 
		send: function(name, data, complete, error) {
			if (connected) {
				client.send("42[\"" + name + "\"," + data + "]");
				if (complete) {
					complete();
				}
			} else {
				// TODO
				print("CLIENT NOT CONNECT");
				if (error) {
					error(errorResponse.createErrorCode(1, "CLIENT NOT CONNECTED !!! "));
				}
			}
		}, closeConnect: function() {
			client.close();
		}
	}
}	





