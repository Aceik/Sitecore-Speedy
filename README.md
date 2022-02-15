![Sitecore Speedy](https://aceiksolutions.files.wordpress.com/2019/06/speedylogo.png?w=200)

#Feb 2022 - Update Notice Below

- There won't be a release for 10.2 planned. Its 2022 and Sitecore 10.2 now supports Next.JS which is a far better option if your serious about page speed. Yes the headless world has it challanges and its a little bit of the bleeding edge / wild west, but worth considering if you have a new build. 
- If you want to get in touch, reach out on Sitecore slack. I'm not actively watching the issues tab in this repository. 
- Google has updated the Page Speed test. You will now see the real world data tests at the top, which is an amazing improvement for everyone. 
- On the other hand if your just running single one off tests, its also cranked up the algoritm the next notch it appears. So its a bit harder again to get into the green zone for single test runs. 
- One tip worth mentioning for those single tests before your get into production. Switch over to using GTMetrics (which in now also based on lighthouse) and choose a testing server closer to your hosting server. For those not using an Edge CDN  this will provide a more accurate 'single' run score. 




## Sitecore Speedy (SXA Version) 
<img src='https://img.shields.io/github/tag/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/issues/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/license/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/languages/code-size/Aceik/Sitecore-Speedy.svg' />

Use best practice page load techniques to achieve Outstanding Page Speed scores for your Sitecore pages. 

## What does it do ?

Speedy provides a Sitecore Layout and Asset provider that structures your HTML in accordance with Google's recommendations.  Google ranks your website with a score out of 100 and provides recommendations on how to achieve better scores.
Implementing Critical CSS and Deferred Javascript loading can be tricky. This module provides a framework and brings together the tools needed to automate the process. 

[Installation, Usage and results screencast ... click here](https://www.youtube.com/watch?v=8q4BTHYBsMI)

Results: 

<img src="https://aceiksolutions.files.wordpress.com/2019/07/results.png?w=768"/>

Before / After (Full Screenshots):

<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=200"/></a>
<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=200"/></a>

## What does Speedy solve in regard to Page Speed ?

In order to get great page speed scores there are several aspects you need to address. [Read more here.](https://aceik.com.au/2020/05/23/one-performance-blog-to-rule-them-all-combining-the-6-pillars-of-speed/)

This module addresses Critical CSS and Deferred asset loading, which is perhaps one of the hardest parts of Page Speed to get right.

<img src="https://aceiksolutions.files.wordpress.com/2019/07/critical_plus_defer.png?w=720"/>

## Is it easy to use ?

It is likely that a developer will be required to setup and tweak the settings before the first full deployment. 

## Installation prerequisites and notes

* <img src="https://img.shields.io/badge/requires-sitecore-blue.svg?style=flat-square" alt="requires sitecore">
* <img src="https://img.shields.io/badge/supports-sitecore%20v10.1-green.svg?style=flat-square" alt="supports sitecore 10.1">
* <img src="https://img.shields.io/badge/supports-sitecore%20v10-green.svg?style=flat-square" alt="supports sitecore 10">
* <img src="https://img.shields.io/badge/supports-sitecore%20v9.3-green.svg?style=flat-square" alt="supports sitecore 9.3">
* <img src="https://img.shields.io/badge/supports-sitecore%20v9.2-green.svg?style=flat-square" alt="supports sitecore 9.2">
* <img src="https://img.shields.io/badge/supports-sitecore%20v9.0.2-green.svg?style=flat-square" alt="supports sitecore 9.0.2">
* <img src="https://img.shields.io/badge/supports-sitecore%20v9.1-green.svg?style=flat-square" alt="supports sitecore 9.1">
* <img src="https://img.shields.io/badge/supports-helix-green.svg?style=flat-square" alt="requires Helix Foundation"/>

## Getting Started Steps
1) Installation - via Sitecore Package - Select Override when promted
2) Change the layout on the page you want to enable Speedy on
3) Enable Speedy 

### Sitecore Settings
* [Speedy Global Settings](https://github.com/Aceik/Sitecore-Speedy/wiki/06---Global-Settings)
* [Speedy Page Settings](https://github.com/Aceik/Sitecore-Speedy/wiki/07---Page-Settings)

## Troubleshooting

[Read more about how to troubleshoot display issues ...](https://github.com/Aceik/Sitecore-Speedy/wiki/05--Complex-Page-Speed-Issues)

## References and Inspiration

* [Deep dive into the murky waters of script loading](https://www.html5rocks.com/en/tutorials/speed/script-loading/)
* [Async Vs Defer](https://bitsofco.de/async-vs-defer/)
* [Efficiently load JavaScript with defer and async](https://flaviocopes.com/javascript-async-defer/)
* [Critical](https://www.npmjs.com/package/critical)
* [LoadCSS](https://github.com/filamentgroup/loadCSS/blob/master/README.md) -- Looking to implement this in a future version
* https://www.browserless.io/


## FAQ

### Q) My custom libraries written in Jquery won't load via async, what do I do ?
### A) 

If you have a browse around this has been [blogged about a bit](https://idiallo.com/javascript/async-jquery). 
When loading external libraries async its likely that the DOM Ready event that Jquery fires has already passed. 
Ideally the javascript library would be written so that it will initialise when loaded (regardless of async/defer). If this isn't the case try to find the initialization function within the library.  You would call this initialization inside of 
`Speedy.fallbackExperienceAfterLoad = function () {` which can be found [here](https://github.com/Aceik/Sitecore-Speedy/blob/master/src/Foundation/Speedy/code/Views/Speedy/SpeedyJavascriptLoader.cshtml)

At this stage Speedy doesn't support a hybrid model of loading some scripts in main upfront and some async. It might be something to consider in a future release. 


### Q) What happened in the Sitecore 10 Release of Speedy ?
### A) 

A fairly large overhaul that's what. We got feedback that content editors don't really want be generating optimised critical CSS chunks. Or at least its not really fair to put this burden on them. At the same time developers role off projects and don't want to maintain it. So in the V10 release we simply started reading the entire CSS bundle inline into the critical region in the <head> block of the page. This isn't optimal but its a lot more practical and its still give you far better scores than loading all of those external CSS files seperately as render blocking network calls. One caveat to this is that you need to consider [GZIP your HTML payload.](https://aceik.com.au/2020/10/05/page-speed-the-smaller-7th-pillar-micro-boosts/)
