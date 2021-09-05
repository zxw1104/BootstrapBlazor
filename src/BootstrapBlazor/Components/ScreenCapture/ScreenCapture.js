(function ($) {
    $.extend({
        bb_toggleFullScreen: function (el) {
            if (!document.fullscreenElement) {
                document.documentElement.requestFullscreen();
            } else {
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                }
            }
        }
    });
})(jQuery);
