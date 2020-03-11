var express = require("express");
var critical = require('critical');
var log4js = require('log4js');
const CleanCss = require('clean-css');
const config = require('./Critical.json');
const bodyParser = require('body-parser');

log4js.configure({
    appenders: { criticallogger: { type: 'file', filename: 'criticallogger.log' } },
    categories: { default: { appenders: ['criticallogger'], level: 'trace' } }
});

const logger = log4js.getLogger('criticallogger');

var app = express();
app.use(bodyParser.json());

var processCritical = function (pageUrl, width, height, cb) {

    var url = pageUrl;

    logger.debug('about to critical:' + url);
    logger.log("about to run critical HTML generator " + url);

    try {

        critical.generate({
            src: url,
            minify: true,
            inline: false,
            extract: true,
            pathPrefix: '/',
            timeout: parseInt(config.timeout),
            width: parseInt(width),
            height: parseInt(height),
            penthouse: {
                url: url,
                cssString: ''
            },
        }).then(function (result) {
            logger.log("critical node tool -> promise resolved");

            let cleanedUpCcss = new CleanCss({ compress: true }).minify(result).styles;

            logger.debug('html: ' + cleanedUpCcss);

            logger.log("--------------------------------------------");
            logger.log("Critical HTML was generated");
            logger.log("--------------------------------------------");
            
            cb(cleanedUpCcss);

        }).catch(function (err) {
            logger.log("processing result rejected: " + err);
            logger.error('critical API issues: ' + err);
            cb(null);
        });

    } catch (err) {
        logger.log("rejected" + err);
        logger.error('general error:' + err);
        cb(null);
    }

}

var switchFontPaths = function replaceAll(input, fontMaps) {
    var output2 = input;
    fontMaps.forEach(function (fontReplacement) {
        logger.log("fontReplacement.find -- " + fontReplacement.find + " -> " + fontReplacement.replace);
        output2 = findReplace(output2, fontReplacement.find, fontReplacement.replace);
    });
    return output2;
};

var switchFontFaceNames = function replaceAll(input, fontFaceSwitch) {
    var output3 = input;
    fontFaceSwitch.forEach(function (fontFaceSwitch) {
        logger.log("fontFaceSwitch.find -- " + fontFaceSwitch.find + " -> " + fontFaceSwitch.replace);
        output3 = findReplace(output3, fontFaceSwitch.find, fontFaceSwitch.replace);
    });
    return output3;
};

var removeDuplicates = function removeDups(input, removeDuplicates) {
    var output2 = input;
    removeDuplicates.forEach(function (fontReplacement) {
        logger.log("fontReplacement.find -- " + fontReplacement.find);
        output2 = findReplace(output2, fontReplacement.find, "") + fontReplacement.find;        
    });
    return output2;
};

var findReplace = function (str, find, replaceToken) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replaceToken);
};

var escapeRegExp = function(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
}

app.post('/', function(req, res){

    try{
        process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = 0

        logger.debug('Got criticallogger logger - critical started.');
     
        logger.debug('url:' + req.body.url);
        logger.debug('width:' + req.body.width);
        logger.debug('height:' + req.body.height);
     
        processCritical(req.body.url, req.body.width, req.body.height, function(criticalCss){
     
            if(req.body.fontMap){
                criticalCss = switchFontPaths(criticalCss, req.body.fontMap);
            }
     
            if(req.body.removeDuplicates){
                criticalCss = removeDuplicates(criticalCss, req.body.removeDuplicates);
            }
             
            if(req.body.fontFaceSwitch){
                criticalCss = switchFontFaceNames(criticalCss, req.body.fontFaceSwitch);
            }
     
            res.send(criticalCss);
        });
    }
    catch{
        res.send("API Issue");
    }
});

app.listen(config.nodePort);