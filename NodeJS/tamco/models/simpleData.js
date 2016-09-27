require('../utils/UtilJS')();
require('../models/errorResponse')();

var Mongo = require('mongodb');
var MongoClient = Mongo.MongoClient;

module.exports = function() {
	// Simple
	this.simpleData = {
		collectionName: "simpleData",
		// Find And Sort Data
		findDataAndSort: function (database, query, sort, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.find(query, function(errQuery, cursor) {
					if (errQuery) {
						if (error) {
							error(errorResponse.createErrorCode(1, errQuery));
						}
					} else {
						cursor.sort(sort).toArray (function (errToArray, items){
							if (errToArray) {
								if (error) {
									error(errorResponse.createErrorCode(1, errToArray));
								}
							} else {
								if (items.length > 0) {
									if (complete) {
										complete(items);
									}
								} else {
									if (error) {
										error(errorResponse.createErrorCode(1, "Not found item !! "));
									}
								}
							}
						});
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Find Data
		findData: function (database, query, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.find(query, function(errQuery, cursor) {
					if (errQuery) {
						if (error) {
							error(errorResponse.createErrorCode(1, errQuery));
						}
					} else {
						cursor.toArray (function (errToArray, items){
							if (errToArray) {
								if (error) {
									error(errorResponse.createErrorCode(1, errToArray));
								}
							} else {
								if (items.length > 0) {
									if (complete) {
										complete(items);
									}
								} else {
									if (error) {
										error(errorResponse.createErrorCode(1, "Not found item !! "));
									}
								}
							}
						});
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Limit item
		findSkipAndLimitData: function (database, query, skip, limit, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.find(query).skip(skip).limit(limit).toArray (function (err, items) {
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (items) {
							if (items.length > 0) {
								if (complete) {
									complete(items);
								}
							} else {
								if (error) {
									error(errorResponse.createErrorCode(1, "Not found item !! "));
								}
							}
						} else {
							if (error) {
								error(errorResponse.createErrorCode(1, "Not found item !! "));
							}
						}
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Create Data
		createData: function (database, query, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.insert(query, function(err, docs){
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (complete) {
							complete(docs);
						}
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Update Data
		updateData: function (database, query, item, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.update(query, { $set:item }, { upsert: false, multi: true }, function (err, result) {
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (result) {
							if (complete) {
								complete(result);
							}
						} else {
							if (error) {
								error(errorResponse.createErrorCode(1, "Query return null !! "));
							}
						}
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Find And Update Data
		findAndUpdateData: function (database, query, item, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.findAndModify(query, [], { $set:item }, { upsert: false }, function (err, result) {
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (result.value) {
							if (complete) {
								complete(result.value);
							}
						} else {
							if (error) {
								error(errorResponse.createErrorCode(1, "Query return null !! " + JSON.stringify(query) + " // " + JSON.stringify(item)));
							}
						}
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Modify Data
		findAndModifyData: function (database, query, item, complete, error) {
			var col = database.collection(this.collectionName);
			if (col) {
				col.findAndModify(query, [],  item , { upsert: false }, function (err, result) {
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (result.value) {
							if (complete) {
								complete(result.value);
							}
						} else {
							if (error) {
								error(errorResponse.createErrorCode(1, "Query return null !! "));
							}
						}
					}
				});
			} else {
				if (error) {
					error(errorResponse.createErrorCode(1, "Not have collection to query item !! "));
				}
			}
		},
		// Delete Data
		deleteData: function (database, query, complete, error) { 
			var col = database.collection(this.collectionName);
			this.findData (query, function(items) {
				col.remove(items[0], function(err, docs){
					if (err) {
						if (error) {
							error(errorResponse.createErrorCode(1, err));
						}
					} else {
						if (complete) {
							complete(docs);
						}
					}
				});
			}, function(err) {
				if (error) {
					error(errorResponse.createErrorCode(1, err));
				}
			});
		}
	}
}
