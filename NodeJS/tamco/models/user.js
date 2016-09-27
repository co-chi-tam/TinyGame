require('../utils/UtilJS')();
require('../models/simpleData')();
require('../models/token')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;
var uuid = require('node-uuid');
var crypto = require('crypto');

module.exports = function() {
	// User manager
	this.user = {
		collectionName: "users",
		userActions: ['login', 'logout', 'change_pass', 'create_mer', 'dismiss_mer', 'create_map_formation', 'inventory', 'use_item', 'buy_gold_item', 'buy_diamond_item', 'get_purchase', 'battle_log', 'lucky_wheel_log'],
		userErrorCode: [100, 101],
		// Login
		loginUser: function(database, uName, uPass, uMacAddress, complete, error) {
			var currentDate = new Date();
			simpleData.collectionName = this.collectionName;
			simpleData.findData(database, {'userName': uName, 'password': uPass}, function(users) {
				if (users) {
					var userLogin = users[0]['isLogin'];
					simpleData.findAndUpdateData (database, {'userName': uName, 'password': uPass, 'isLogin': userLogin}, {'lastLogin': currentDate, 'isLogin': true, 'macAddress': uMacAddress}, function(user) {
						if (user) {
							if (complete) {
								complete (user);
							}
						} else {
							if (error) {
								error(errorResponse.createErrorCode(1, "User not existed !!!"));
							}	
						}
					}, function(errorFindAndUpdateData) {
						if (error) {
							error(errorResponse.createErrorCode(1, "Wrong username or password !!!"));
						}
					});
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "Wrong username or password !!!"));
					}
				}
			}, function(errorFindData) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Wrong username or password !!!"));
				}
			});
		},
		// Logout
		logoutUser: function(database, uName, uPass, complete, error) {
			var currentDate = new Date();
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData (database, {'userName': uName, 'password': uPass}, {'lastLogout': currentDate, 'isLogin': false}, function(item) {
				if (complete) {
					complete([{
						id: item['userName'],
						userName: item['userName'],
						logoutTime: currentDate
					}]);
				}
			}, error);
		},
		// Register User
		registerUser: function (database, uName, uPass, uEmail, complete, error) {
			var self = this;
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, {$or: [{'userName': uName}, {'email': uEmail}]}, function(users) {
				if (error) {
					error(errorResponse.createErrorCode(1, "User already register"));
				}
			}, function (err) {
				var secretKey = uuid.v4().toString();
				var newUser = self.newAlreadyUser(uName, uPass, uEmail, secretKey);
				simpleData.createData(database, newUser, function() {
					if (complete) {
						complete([{
							userName: uName,
							secretKey: secretKey
						}]);
					}
				}, function(errorCreate) {
					if (error) {
						error(errorCreate);
					}
				});
			});
		},
		// Change password
		changePassword: function(database, uName, uOldPass, uNewPass, complete, error) {
			var currentDate = new Date();
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData (database, {'userName': uName, 'password': uOldPass}, {'password': uNewPass, "changePasswordDate": currentDate}, function(user) {
				if (user) {
					if (complete) {
						complete([{
							userName: uName,
							changePasswordDate: new Date()
						}]);
					}
				} else {
					if (error) {
						error(errorResponse.createErrorCode(1, "User not existed !!!"));
					}	
				}
			}, function (err) {
				if (error) {
					error(err);
				}
			});
		},
		// Find User
		findUser: function(database, query, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, query, complete, error);
		},
		findSkipAndLimitUser: function(database, query, skip, limit, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findSkipAndLimitData (database, query, skip, limit, complete, error);
		},
		// Find And Update User 
		findAndUpdateUser: function(database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData (database, query, item, complete, error);
		},
		// Find And Modifier User 
		findAndModifyUser: function(database, query, item, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndModifyData (database, query, item, complete, error);
		},
		// Active User
		activeUser: function(database, uName, dName, uSecretKey, complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findData (database, {'displayName': dName}, function(users) {
				if (error) {
					error(errorResponse.createErrorCode(1, "Display name not available !!!"));
				}
			}, function (errorFindDisplay) {
				simpleData.findAndUpdateData (database, {'userName': uName, 'secretKey': uSecretKey, 'isLogin': true, 'active': false}, {'displayName': dName, 'active': true}, function(user) {
					if (user) {
						if (complete) {
							complete(user);
						}
					} else {
						if (errorFindUser) {
							error(errorResponse.createErrorCode(1, "User not existed !!!"));
						}	
					}
				}, error);
			});
		},
		// Complete Tutorial User
		completeTutorialUser: function(database, uName, uCompleteTutorial,complete, error) {
			simpleData.collectionName = this.collectionName;
			simpleData.findAndUpdateData (database, {'userName': uName, 'isLogin': true, 'active': true}, {'completeTutorial': uCompleteTutorial}, function(user) {
				if (user) {
					if (complete) {
						complete([{
							displayName: user['displayName'],
							completeTutorial: uCompleteTutorial
						}]);
					}
				} else {
					if (errorFindUser) {
						error(errorResponse.createErrorCode(1, "User not existed !!!"));
					}	
				}
			}, error);
		},
		// New User
		newAlreadyUser: function(uName, uPass, uEmail, secretKey) {
			var newUser = { 
				'userName': uName,
				'password': uPass, 
				'email': uEmail, 
				'displayName': "displayName" + secretKey, 
				'avatar': 'character_male',
				'gold': 5000,
				'diamond': 0,
				'active': false, 
				'completeTutorial': false,
				"isLogin": false,
				'power': 0,
				'secretKey': secretKey,
				'createdDate': new Date()
			};
			return newUser;
		},
		// User Already
		alreadyUserResponse: function(user, currentDate, token) {
			var userResponse = {
				id: user['userName'],
				userName: user['userName'],
				displayName: user['displayName'],
				avatar: user['avatar'],
				active: user['active'],
				completeTutorial: user['completeTutorial'],
				secretKey: user['secretKey'],
				gold: user['gold'],
				diamond: user['diamond'],
				power: user['power'],
				loginTime: currentDate,
				token: token
			};
			return userResponse;
		}
	}
}
