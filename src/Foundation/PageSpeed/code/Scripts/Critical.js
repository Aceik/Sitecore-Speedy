//node ./Scripts/Critical.js main
var critical = require('critical');
var log4js = require('log4js');
const CleanCss = require('clean-css');
const config = require('./Critical.json');
const got = require('got');

module.exports.main = function () {

    const log4js = require('log4js');
    log4js.configure({
        appenders: { criticallogger: { type: 'file', filename: 'criticallogger.log' } },
        categories: { default: { appenders: ['criticallogger'], level: 'trace' } }
    });
 
    const logger = log4js.getLogger('criticallogger');
    logger.debug('Got criticallogger logger - critical started.');

    logger.debug('timeout:' + config.timeout);
    logger.debug('width:' + config.width);
    logger.debug('height:' + config.height);

    var criticalCallback = function (html) {
        console.log("criticalCallback");
    };

    var pageUrlCallback = function (pageUrl) {
        console.log("pageUrlCallback");
        processCritical(pageUrl, logger, criticalCallback);
    };
    
    lookupPageUrl(config.homeid, pageUrlCallback);


};

var processCritical = function (pageUrl, logger, callback) {
    logger.debug('about to critical:' + pageUrl);

    var cleanedUpCcss = "";

    try {
        var criticalresult = critical.generate({
            src: pageUrl,
            minify: true,
            inline: false,
            extract: true,
            timeout: parseInt(config.timeout),
            width: parseInt(config.width),
            height: parseInt(config.height),
            penthouse: {
                url: url,
                cssString: ''
            },
        }).then(function (result) {
            console.log("promise resolved");

            let cleanedUpCcss = new CleanCss({ compress: true }).minify(result).styles;

            logger.debug('html:' + cleanedUpCcss);
            callback(cleanedUpCcss);

            console.log("---------------");
            console.log("Critical HTML was generated");
            console.log("---------------");

        }).catch(function (err) {
            console.log("promise rejected" + err);
            logger.error('critical API issues:' + err);
        });

        console.log("completed");
    } catch (err) {
        console.log("rejected" + err);
        logger.error('general error:' + err);
    }
  
    //return cleanedUpCcss;
}

var lookupPageUrl = function (id, callback) {
    var url = config.host + config.api + "url/" + id + "/";

    console.log("about to call API " + url);

    got(url, { json: true }).then(response => {
        console.log(response.body);
        callback(response.body);
        //console.log(response.body.explanation);
    }).catch(error => {
        console.log(error.response.body);
    });
}


require('make-runnable');