//https://github.com/andygup/splash-screen-js/blob/master/index.html

var SpeedyVanilla = {};
SpeedyVanilla.Loader = {};
SpeedyVanilla.Loader.MyModule = {
    bootup: function () {
        setTimeout(function () {
            SpeedyVanilla.Loader.MyModule.injectLoad();
        }, 150);
    }, injectLoad: function () {
        var elem = document.createElement('div');
        var innerElem = document.createElement('div');
        elem.id = 'loader-wrapper';
        innerElem.id = 'loader';
        document.body.appendChild(elem);
        elem.appendChild(innerElem);

        SpeedyVanilla.Loader.MyModule.checkGlobalFallbackExperienceCompleted();

    }, removeLoader: function () {
        // Get a reference to the loader's div
        var loaderDiv = document.getElementById("loader-wrapper");

        // When the transition ends remove loader's div from display
        // so that we can access the map with gestures or clicks
        if (loaderDiv) {
            loaderDiv.addEventListener("transitionend", function () {
                loaderDiv.style.display = "none";
            }, true);
            // Kick off the CSS transition
            loaderDiv.style.opacity = 0.0;
        }
    }, checkGlobalFallbackExperienceCompleted: function () {
        if (!Speedy.settings.fallBackExperienceComplete) {
            setTimeout(function () {
                SpeedyVanilla.Loader.MyModule.checkGlobalFallbackExperienceCompleted();
            }, 1000);
        } else {
            SpeedyVanilla.Loader.MyModule.removeLoader();
        }
    }
};

if (Speedy.isMobile) {
    SpeedyVanilla.Loader.MyModule.bootup();
}