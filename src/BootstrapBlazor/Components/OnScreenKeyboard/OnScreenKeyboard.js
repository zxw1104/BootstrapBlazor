(function ($) {
    $.extend({
        bb_OnScreenKeyboard: function (className, option) {
            console.info(className, option);
            KioskBoard.run('.' + className, option);
            return true;
        }
    });
})(jQuery);
