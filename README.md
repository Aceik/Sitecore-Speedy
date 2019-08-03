![Sitecore Speedy](https://aceiksolutions.files.wordpress.com/2019/06/speedylogo.png?w=200)

## Sitecore Speedy (SXA Version) 
<img src='https://img.shields.io/github/tag/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/issues/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/license/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/languages/code-size/Aceik/Sitecore-Speedy.svg' />

Use best practice page load techniques to achieve Outstanding Page Speed scores for your Sitecore pages. 

## What does it do ?

Speedy provides a Sitecore Layout and Asset provider that structures your HTML in accordance with Google's recommendations.  Google ranks your website with a score out of 100 and provides recommendations on how to achieve better scores.
Implementing Critical CSS and Deferred Javascript loading can be tricky. This module provides a framework and brings together the tools needed to automate the process. 

[Results demo screencast ... click here](https://www.youtube.com/watch?v=S8aIy-dK75g&feature=youtu.be) 

[Installation and Usage on a developer machine screencast ... click here](https://youtu.be/-1SA12qax1g)

Results: 

<img src="https://aceiksolutions.files.wordpress.com/2019/07/results.png?w=768"/>

Before / After (Full Screenshots):

<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=200"/></a>
<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=200"/></a>

## What does Speedy solve in regard to Page Speed ?

In order to get great page speed scores there are several aspects you need to address. [Read more here.](https://github.com/Aceik/Sitecore-Speedy/wiki/Extra-Info---Page-Speed-Considerations)

This module addresses Critical CSS and Deferred asset loading, which is perhaps one of the hardest parts of Page Speed to get right.

<img src="https://aceiksolutions.files.wordpress.com/2019/07/critical_plus_defer.png?w=720"/>

## Is it easy to use ?

It is likely that a developer will be required to setup and tweak the settings before the first full deployment. 

In production mode the Content Editor will have a button they can use to re-generate the Critical CSS for any given page. (This feature is supported by a simple node API application that can be hosted on an Azure Free plan and a subscription to https://www.browserless.io/ if you go over the free limits.)
<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/generatebutton.png"><img src="https://aceiksolutions.files.wordpress.com/2019/07/generatebutton.png"/></a>

## Installation prerequisites and notes

1) <img src="https://img.shields.io/badge/requires-node-blue.svg?style=flat-square" alt="requires node">  (Required in [local development mode](https://github.com/Aceik/Sitecore-Speedy/wiki/08---Development-Mode))
2) <img src="https://img.shields.io/badge/requires-sitecore-blue.svg?style=flat-square" alt="requires sitecore">
  * <img src="https://img.shields.io/badge/supports-sitecore%20v9.0.2-green.svg?style=flat-square" alt="requires sitecore 9.0.2">
  * <img src="https://img.shields.io/badge/supports-sitecore%20v9.1-green.svg?style=flat-square" alt="requires sitecore 9.1">
  * <img src="https://img.shields.io/badge/supports-helix-green.svg?style=flat-square" alt="requires Helix Foundation"/>

## Getting Started Steps
1) Installation
- Option 1: [via Sitecore Package](https://github.com/Aceik/Sitecore-Speedy/wiki/00-Installation-Via-Sitecore-Package)
- Option 2: [Via Source](https://github.com/Aceik/Sitecore-Speedy/wiki/01--Installation-Via-Helix-Source)
2) [Critical Generation](https://github.com/Aceik/Sitecore-Speedy/wiki/02---Critical-Generation-Options)
3) [Usage on each page](https://github.com/Aceik/Sitecore-Speedy/wiki/03---Usage-on-a-Page)
4) [Tweak, Adapt and test](https://github.com/Aceik/Sitecore-Speedy/wiki/04---Tweak,-Adapt-and-Test)

### Sitecore Settings
* [Global Settings](https://github.com/Aceik/Sitecore-Speedy/wiki/06---Global-Settings)
* [Page Settings](https://github.com/Aceik/Sitecore-Speedy/wiki/07---Page-Settings)

## Troubleshooting

[Read more about how to troubleshoot display issues ...](https://github.com/Aceik/Sitecore-Speedy/wiki/05--Complex-Page-Speed-Issues)

## References and Inspiration

* [Deep dive into the murky waters of script loading](https://www.html5rocks.com/en/tutorials/speed/script-loading/)
* [Async Vs Defer](https://bitsofco.de/async-vs-defer/)
* [Efficiently load JavaScript with defer and async](https://flaviocopes.com/javascript-async-defer/)
* [Critical](https://www.npmjs.com/package/critical)
* [LoadCSS](https://github.com/filamentgroup/loadCSS/blob/master/README.md) -- Looking to implement this in a future version
* https://www.browserless.io/
