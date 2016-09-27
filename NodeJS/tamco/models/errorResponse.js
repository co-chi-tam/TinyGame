require('../utils/UtilJS')();

module.exports = function() {
	this.errorResponse = {
		createErrorCode: function(errCode, errContent) {
			return JSON.stringify ({
				errorCode: errCode,
				errorContent: errContent
			});
		}
	}
	
}