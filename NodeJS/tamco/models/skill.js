require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// Skill Manager
	this.skill = {
		collectionName: "skills",
		findSkill: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, query, complete, error);
		}
	}
}	





