![Sitecore Speedy](https://aceiksolutions.files.wordpress.com/2019/06/speedylogo.png?w=600)

## Sitecore Speedy (SXA Version) 
<img src='https://img.shields.io/github/tag/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/issues/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/license/Aceik/Sitecore-Speedy.svg' />
<img src='https://img.shields.io/github/languages/code-size/Aceik/Sitecore-Speedy.svg' />

Use best practice page load techniques to achieve Outstanding Page Speed scores for your Sitecore pages. 

## What does it do ?

Speedy provides a Sitecore Layout and Asset provider that structures your HTML in accordance with Google's recommendations.  Google ranks your website with a score out of 100 and provides recommendation on how to achieve better scores.
Implementing Critical CSS and Deferred Javascript loading can be tricky. This module provides a framework and brings together the tools needed to automate the process. 

Result: 

<img src="https://aceiksolutions.files.wordpress.com/2019/07/results.png?w=768"/>

Before / After (Full Screenshots):

<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/after.png?w=200"/></a>
<a target="_blank" href="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=1800"><img src="https://aceiksolutions.files.wordpress.com/2019/07/before.png?w=200"/></a>

## What does Speedy solve in regard to Page Speed ?

In order to get great page speed scores there are several aspects you need to address. [Read more here.](https://github.com/Aceik/Sitecore-Speedy/wiki/Extra-Info---Page-Speed-Considerations)

This module addresses Critical CSS and Deferred asset loading, which is perhaps one of the hardest parts of Page Speed to get right.

<img src="https://aceiksolutions.files.wordpress.com/2019/07/critical_plus_defer.png?w=720"/>

## Installation prerequisites and notes

1) <img src="https://img.shields.io/badge/requires-node-blue.svg?style=flat-square" alt="requires node">  (Required in [local development mode](https://github.com/Aceik/Sitecore-Speedy/wiki/08---Development-Mode))
2) <img src="https://img.shields.io/badge/requires-sitecore-blue.svg?style=flat-square" alt="requires sitecore">
  * <img src="https://img.shields.io/badge/supports-sitecore%20v9.0.2-green.svg?style=flat-square" alt="requires sitecore 9.0.2">
  * <img src="https://img.shields.io/badge/supports-sitecore%20v9.1-green.svg?style=flat-square" alt="requires sitecore 9.1">
  * <img src="https://img.shields.io/badge/supports-helix-green.svg?style=flat-square" alt="requires Helix Foundation"/>

## Getting Started Steps
1) [Installation](https://github.com/Aceik/Sitecore-Speedy/wiki/01---Installation)
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
