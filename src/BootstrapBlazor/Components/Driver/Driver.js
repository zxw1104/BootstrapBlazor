(function ($) {
    $.extend({
        bb_driver_init: function (el, driverOptions, stepOptions) {
            var driver = new Driver(driverOptions);
            var $el = $(el);
            $el.data('bb_driver', driver);
            if (stepOptions) {
                driver.defineSteps(stepOptions);
            }
        },
        bb_driver_start: function (el) {
            var driver = $(el).data('bb_driver');
            driver.start();
        },
        bb_driver_refresh: function (el) {
            var driver = $(el).data('bb_driver');
            driver.refresh();
        },
        bb_driver_reset: function (el) {
            var driver = $(el).data('bb_driver');
            driver.reset();
        },
        bb_driver_movePrevious: function (el) {
            var driver = $(el).data('bb_driver');
            driver.movePrevious();
        },
        bb_driver_moveNext: function (el) {
            var driver = $(el).data('bb_driver');
            driver.moveNext();
        },
        bb_driver_reset: function (el) {
            var driver = $(el).data('bb_driver');
            driver.reset();
        },
        bb_driver_isActivated: function (el) {
            var driver = $(el).data('bb_driver');
            return driver.isActivated();
        },
        bb_driver_hasHighlightedElement: function (el) {
            var driver = $(el).data('bb_driver');
            return driver.hasHighlightedElement();
        },
        bb_driver_hasNextStep: function (el) {
            var driver = $(el).data('bb_driver');
            return driver.hasNextStep();
        },
        bb_driver_hasPreviousStep: function (el) {
            var driver = $(el).data('bb_driver');
            return driver.hasPreviousStep();
        },
        bb_driver_hidePopover: function (el) {
            var driver = $(el).data('bb_driver');
            var activeElement = driver.getHighlightedElement();
            if (activeElement) {
                activeElement.hidePopover();
            }
        },
        bb_driver_showPopover: function (el) {
            var driver = $(el).data('bb_driver');
            var activeElement = driver.getHighlightedElement();
            if (activeElement) {
                activeElement.showPopover();
            }
        },
    });
})(jQuery);
