require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/user')();
require('../models/sodier')();
require('../models/map')();
require('../models/mercenary')();
require('../models/token')();
require('../models/item')();

require('../models/errorResponse')();
require('../models/resultResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var crypto = require('crypto');

var connection_string = "mongodb://admin:KxWyMbza2bGI@" +
  process.env.OPENSHIFT_MONGODB_DB_HOST + ':' +
  process.env.OPENSHIFT_MONGODB_DB_PORT + '/tamco?authMode=scram-sha1';

var tinyGameDB;
var mongoDB;
var connectCount = 0;
var userActions = ['login', 'logout', 'change_pass', 'create_mer', 'dismiss_mer', 'create_map_formation'];
		
module.exports = function() {
	this.databaseName = "TinyGame";
	this.databaseAdmin = 'administrator';
	this.databasePassword = 'pro12a833';
	this.databaseHost = process.env.OPENSHIFT_MONGODB_DB_HOST;
	this.databasePort = process.env.OPENSHIFT_MONGODB_DB_PORT;
	this.db = {
		// Get current database
		getDatabase: function() {
			return tinyGameDB;
		},
		// Connect using MongoClient
		connectDB: function (complete, error) {
			MongoClient.connect(connection_string, function(err, database) {
				if (err) {
					if (error) {
						error (errorResponse.createErrorCode(1, err));
					}
				} else {
					connectCount++;
					mongoDB = database;
					tinyGameDB = database.db(this.databaseName);
					tinyGameDB.authenticate(this.databaseAdmin, this.databasePassword, function(authError, authResult){
						if (authError) {
							print(authError);
						} else {
							if (complete) {
								complete(tinyGameDB);
							}
						}
					});
				}
			});
		},
		// Close connection
		closeDB: function() {
			if (mongoDB){
				connectCount--;
				mongoDB.close();
			}
		},
		// Find Data
		findData: function(colName, query, complete, error) {
			simpleData.collectionName = colName;
			simpleData.findData(tinyGameDB, query, complete, error);
		},
		// Create Data
		createData: function (colName, query, complete, error) {
			simpleData.collectionName = colName;
			simpleData.createData (tinyGameDB, query, complete, error);
		},
		// Find And Update Data
		findAndUpdateData: function (colName, query, item, complete, error) {
			simpleData.collectionName = colName;
			simpleData.findAndUpdateData(tinyGameDB, query, item, complete, error);
		},
		// Delete Data
		deleteData: function (colName, query, complete, error) { 
			simpleData.collectionName = colName;
			simpleData.deleteData (tinyGameDB, query, complete, error);
		}
	}
}


	