require('../utils/UtilJS')();

module.exports = function() {
	this.resultResponse = {
		createResult: function(resCode, resContent) {
			return JSON.stringify ({
				resultCode: resCode,
				resultContent: resContent
			});
		}
	}
	
}