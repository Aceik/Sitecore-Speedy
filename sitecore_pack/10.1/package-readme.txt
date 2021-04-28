Supports SXA 10.1.0
All documentation and setup instructions head over to: https://github.com/Aceik/Sitecore-Speedy

28/04/2020 - Support for 10.1, Removed RestSharp dependency
28/04/2020 - Fix for CD Database context bug and CD instructions added
10/05/2020 - Added in the Services config
18/05/2020 - Fix for having styles off and JS on.  Plus fix layout Flag


Changelog

Oct 2020 - Sitecore 10 updates
- FontMap field removed, this is no longer needed
- main.js improved logging
- Critical NPM package updated to 2.0.4 from 1.x
- Ability to set entire CSS Theme into critical added. The lazy mans critical. 
- Sections of Speedy in the pipeline involved in script gathering added to cache. 
- Missing field in package RemoteFontSwitch added back in
- Invalid SSL certificates on Dev now need to be added to the Trusted Root SA.
- Bypass speedy when in Experience Editor Mode.
- Fix for logic switch when Critical is on but CSS is not
- Added Preloader for javascript files, to speed up resource availabilty over the network

- Complete overhaul in V2.  No longer require a node server. 