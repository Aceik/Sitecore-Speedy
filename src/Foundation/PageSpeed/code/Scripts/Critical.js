//node ./Scripts/Critical.js main "https://www.laminex.com.au/||30000||1800||1100"
var critical = require('critical');
var log4js = require('log4js');
const CleanCss = require('clean-css');

module.exports.main = function (data, callback) {

const log4js = require('log4js');
log4js.configure({
    appenders: { criticallogger: { type: 'file', filename: 'cheese.log' } },
  categories: { default: { appenders: ['criticallogger'], level: 'trace' } }
});
 
    const logger = log4js.getLogger('criticallogger');
    logger.debug('Got criticallogger logger - critical started.');

    var parts = data.split('||');
    var url = parts[0];
	logger.debug('url:' + url);

    var timeout = parts[1];
	logger.debug('timeout:' + timeout);
    var width = parts[2];
	logger.debug('width:' + width);
    var height = parts[3];
	logger.debug('height:' + height);
    
	logger.debug('about to critical:' + url);
	console.log('about to critical:' + url);
	
    var cleanedUpCcss = "";
		
	try {
		var criticalresult = critical.generate({
			src: url,
			minify: true,
			inline: false,
			extract: true,
			timeout: parseInt(timeout),
			width: parseInt(width),
			height: parseInt(height),
			penthouse: {
			  url: url,
			  cssString: ''
			},
		}).then(function (result) {
				console.log("promise resolved");			
				
				let cleanedUpCcss = new CleanCss({ compress: true }).minify(result).styles;
				
				logger.debug('html:' + cleanedUpCcss);
				console.log(cleanedUpCcss);
				callback(null, cleanedUpCcss);
				//return cleanedUpCcss;
			}).catch(function (err) {
				 console.log("promise rejected" + err);
                logger.error('critical API issues:' + err);
			}); 
				
			console.log("completed");
	  } catch (err) {
		console.log("rejected" + err);
        logger.error('general error:' + err);
	  }
    //callback(null, html);
};

// module.exports.init = function () {
    // console.log('hi');
// };

require('make-runnable');