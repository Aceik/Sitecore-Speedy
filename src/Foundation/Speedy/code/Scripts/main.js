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

    criticalLogger('about to critical:' + url);
    criticalLogger("about to run critical HTML generator " + url);

    try {

        // Please note that invalid SSL certificates will break the generation.

        critical.generate({
            src: 'http://sc10aceik.australiaeast.cloudapp.azure.com:500/?speedyByPass=true',
            base: '/',
            minify: true,
            rebase: asset => `${asset.absolutePath}`,
            width: parseInt(width),
            height: parseInt(height),
            penthouse: {
                timeout: parseInt(config.timeout)
            },
        }).then(function (result) {
            criticalLogger("critical node tool -> promise resolved", result);
            let cleanedUpCcss = result.css;
            criticalLogger('html: ' + cleanedUpCcss);

            criticalLogger("--------------------------------------------");
            criticalLogger("Critical HTML was generated");
            criticalLogger("--------------------------------------------");
            
            cb(cleanedUpCcss);

        }).catch(function (err) {
            criticalLogger("processing result rejected: " + err, false);
            cb(null);
        });

    } catch (err) {
        criticalLogger("rejected" + err);
        logger.error('general error:' + err);
        cb(null);
    }

}


var testFunction = function a(input) {

    criticalLogger("fontReplacement.find -- " + testFunction);

    return input;
};

var switchFontFaceNames = function replaceAll(input, fontFaceSwitch) {
    if(!fontFaceSwitch)
        return input;
    var output3 = input;
    fontFaceSwitch.forEach(function (fontFaceSwitch) {
        criticalLogger("fontFaceSwitch.find -- " + fontFaceSwitch.find + " -> " + fontFaceSwitch.replace);
        if(fontFaceSwitch.find && fontFaceSwitch.replace)
            output3 = findReplace(output3, fontFaceSwitch.find, fontFaceSwitch.replace);
    });
    return output3;
};

var removeDuplicates = function removeDups(input, removeDuplicates) {
    if(!fontFaceSwitch)
        return input;
    var output2 = input;
    removeDuplicates.forEach(function (fontReplacement) {
        criticalLogger("fontReplacement.find -- " + fontReplacement.find);
        if(fontReplacement.find && fontReplacement.replace)
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

var criticalLogger = function(message, extra) {

    if(extra) {
        console.log(message, extra);
        logger.debug(message);
        return;
    }

    console.log(message);
    logger.debug(message);
}

app.post('/', function(req, res){

    try{
        process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = 0

        criticalLogger('Got criticallogger logger - critical started.');
     
        criticalLogger('url:' + req.body.url);
        criticalLogger('width:' + req.body.width);
        criticalLogger('height:' + req.body.height);
        criticalLogger("Request ", req);

        req.body.url = findReplace(req.body.url, "habitat.speedy.dev", "habitat.speedy.dev:70");      

        processCritical(req.body.url, req.body.width, req.body.height, function(criticalCss){
            
            if(!criticalCss) {
                const notice = "Failure --- "+req.body.url+" --- Critical CSS from this URL is empty or failed to generate";
                criticalLogger(notice);
                res.send(notice);
                return;
            }

            // if(req.body.fontMap){
            //     criticalCss = switchFontPaths(criticalCss, req.body.fontMap);
            // }
     
            if(req.body.removeDuplicates){
                criticalCss = removeDuplicates(criticalCss, req.body.removeDuplicates);
            }
             
            if(req.body.fontFaceSwitch){
                criticalCss = switchFontFaceNames(criticalCss, req.body.fontFaceSwitch);
            }
     
            criticalLogger("Sending response back to Sitecore");
            res.send(criticalCss);
            criticalLogger("Completed");
        });
    }
    catch (err){
        criticalLogger("API Issue", err);
        res.send("API Issue");
    }
});

app.listen(config.nodePort);