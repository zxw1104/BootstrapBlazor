(function ($) {
    $.extend({
        bb_driver_init: function (el, driverOptions, stepOptions) {
            const driver = new Driver(driverOptions);
            let $el = $(el);
            $el.data('bb_driver', driver);
            if (stepOptions) {
                driver.defineSteps(stepOptions);
            }
        },
        bb_driver_start: function(el) {
            const driver = $(el).data('bb_driver');
            driver.start();
        },
        bb_driver_movePrevious: function (el) {
            const driver = $(el).data('bb_driver');
            driver.moveNext();
        },
        bb_driver_moveNext: function (el) {
            const driver = $(el).data('bb_driver');
            driver.start();
        },
        bb_driver_reset: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_isActivated: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_hasHighlightedElement: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_hasNextStep: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_hasPreviousStep: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_isActivated: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_isActivated: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
        bb_driver_isActivated: function (el) {
            const driver = $(el).data('bb_driver');
            console.log("start", driver);
            driver.start();
        },
    });
})(jQuery);
