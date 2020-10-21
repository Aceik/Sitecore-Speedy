var SpeedyVanilla = {};
SpeedyVanilla.Loader = {};
SpeedyVanilla.Loader.MyModule = {
    myVariable: 1,
    myFunction: function () { return 2; }
};

//https://github.com/andygup/splash-screen-js/blob/master/index.html
if (Speedy.isMobile) {
    console.log('mobile loaders kicking in')
    setTimeout(function () {
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
    }, 200);

}