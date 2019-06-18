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
    
    lookupPageUrl(config.itemid, logger);
};

var lookupPageUrl = function (id, logger) {
    var url = config.host + config.api + id + "/";

    console.log("about to call API " + url);

    got(url, { json: true }).then(response => {
        console.log(response.body);
        processPageUrl(response.body, logger);
    }).catch(error => {
        console.log(error);
    });
}

var processPageUrl = function (pageUrl, logger) {
    if (pageUrl.Success) {
        console.log("page url obtained " + pageUrl.Data);
        processCritical(pageUrl.Data, logger);
    } else {
        console.log("page url lookup failed " + pageUrl.Errors);
    }
};


var processCritical = function (pageUrl, logger) {

    var url = pageUrl;

    logger.debug('about to critical:' + url);

    console.log("about to run critical HTML generator " + url);

    var cleanedUpCcss = "";

    try {

        var criticalresult = critical.generate({
            src: url,
            minify: true,
            inline: false,
            extract: true,
            pathPrefix: '/',
            timeout: parseInt(config.timeout),
            width: parseInt(config.width),
            height: parseInt(config.height),
            penthouse: {
                url: url,
                cssString: ''
            },
        }).then(function (result) {
            console.log("critical node tool -> promise resolved");

            let cleanedUpCcss = new CleanCss({ compress: true }).minify(result).styles;

            logger.debug('html: ' + cleanedUpCcss);

            console.log("--------------------------------------------");
            console.log("Critical HTML was generated");
            console.log("--------------------------------------------");

            beingCriticalSave(cleanedUpCcss, logger);

        }).catch(function (err) {
            console.log("processing result rejected: " + err);
            logger.error('critical API issues: ' + err);
        });

    } catch (err) {
        console.log("rejected" + err);
        logger.error('general error:' + err);
    }

}


var beingCriticalSave = function (result, logger) {
    if (result) {
        console.log("--------------------------------------------");
        console.log("page speedy html retrieved " + result.substring(0, 100));
        console.log("--------------------------------------------");

        var res = switchFontPaths(result);

        logger.debug('fonts fixed: ' + res);

        updateCriticalField(res);

    } else {
        console.log("page speedy html retrieved but some other error -- " + result.Errors);
    }
};

var switchFontPaths = function replaceAll(input) {
    var output2 = input;
    config.fontmap.forEach(function (fontReplacement) {
        console.log("fontReplacement.find -- " + fontReplacement.find + " -> " + fontReplacement.replace);
        output2 = findReplace(output2, fontReplacement.find, fontReplacement.replace);
    });
    return output2;
};

var findReplace = function (str, find, replaceToken) {
    return str.replace(new RegExp(escapeRegExp(find), 'gi'), replaceToken);
};

var escapeRegExp = function(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
}

var updateCriticalField = function(html, logger) {
    var url = config.host + config.apisave;

    console.log("about to call API " + url);

    got.post(url, { json: true, body: { Result: html, Id: config.itemid } }).then(response => {
        console.log("Got response from " + url);
        saveCompletedEvent(response);
    }).catch(error => {
        console.log("ERROR SAVING TO SITECORE " + error);
        console.log(error.response.body);
    });
};

var saveCompletedEvent = function (result) {
    if (result.body.Data) {
        console.log("--------------------------------------------");
        console.log("page speedy html updated " + result.Success);
        console.log("--------------------------------------------");

    } else {
        console.log("ERROR SAVING TO SITECORE -- " + result.Errors);
    }
};



require('make-runnable');