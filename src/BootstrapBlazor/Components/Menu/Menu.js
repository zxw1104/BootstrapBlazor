(function ($) {
    $.extend({
        bb_side_menu_expand: function (el, expand) {
            if (expand) {
                $(el).find('.collapse').collapse('show');
            }
            else {
                $(el).find('.collapse').collapse('hide');
                var handler = window.setTimeout(function () {
                    window.clearTimeout(handler);
                    $.bb_auto_expand($(el));
                }, 400);
            }
        },
        bb_auto_expand: function ($el) {
            // 自动展开
            var actives = $el.find('.nav-link.expand')
                .map(function (index, ele) {
                    return $(ele).removeClass('expand');
                })
                .toArray();
            var $link = $el.find('.active');
            do {
                var $ul = $link.parentsUntil('.submenu.collapse').parent();
                if ($ul.length === 1 && $ul.not('.show')) {
                    $link = $ul.prev();
                    if ($link.length !== 0) {
                        actives.push($link);
                    }
                }
                else {
                    $link = null;
                }
            }
            while ($link != null && $link.length > 0);

            while (actives.length > 0) {
                $link = actives.shift();
                $link[0].click();
            }
        },
        bb_init_side_menu: function ($el) {
            var accordion = $el.hasClass('accordion');
            var $root = $el.children('.submenu');
            $root.find('.submenu').each(function (index, ele) {
                var $ul = $(this);
                $ul.addClass('collapse').removeClass('d-none');
                if (accordion) {
                    var $li = $ul.parentsUntil('.submenu')
                    if ($li.prop('nodeName') === 'LI') {
                        var rootId = $li.parent().attr('id');
                        $ul.attr('data-bs-parent', '#' + rootId);
                    }
                }
                else {
                    $ul.removeAttr('data-bs-parent');
                }

                var ulId = $ul.attr('id');
                var $link = $ul.prev();
                $link.attr('data-bs-toggle', 'collapse');
                $link.attr('href', '#' + ulId);
            });
            if (accordion) {
                var collapses = $root.find('.collapse');
                collapses.each(function (index, ele) {
                    var $ele = $(ele);
                    if (bootstrap.Collapse.getInstance(ele)) {
                        $ele.collapse('dispose');
                    }
                    var parent = '';
                    if (accordion) parent = $ele.attr('data-bs-parent');
                    $ele.collapse({ parent: parent, toggle: false });
                });
            };
        },
        bb_side_menu: function (el) {
            var $el = $(el);

            // 初始化组件
            $.bb_init_side_menu($el);

            // 自动展开
            $.bb_auto_expand($el);
        },
        bb_tip_menu: function (el, interop, isBreadcrumb, method) {
            //记录状态
            var state = { hoverTipMenu: false, hoverTipSelect: false };

            //禁用链接为#的点击
            el.querySelectorAll('a[href="#"],a.has-leaf')
                .forEach(function (link) {
                    link.onclick = function () { return false; }
                });

            //光标移入
            el.addEventListener('mouseenter', function (e) {
                var target = this.querySelector('.tip-select');
                if (target == null) {
                    return false;
                }

                var tipbar = this;
                var element = target.cloneNode(true);//每次重新拷贝
                var tooltip = new bootstrap.Tooltip(tipbar, {
                    html: true,
                    sanitize: false,
                    trigger: 'manual',
                    placement: isBreadcrumb ? 'bottom' : 'right',
                    offset: [isBreadcrumb ? 0 : -34, 0],
                    customClass: 'tipmenu-item',
                    title: element
                });
                this.addEventListener('show.bs.tooltip', function () {
                    element.addEventListener('mouseenter', function (e) {
                        state.hoverTipMenu = false;
                        state.hoverTipSelect = true;
                        e.stopImmediatePropagation();
                    });
                    element.addEventListener('mouseleave', function (e) {
                        tooltip.dispose();
                        state.hoverTipMenu = false;
                        state.hoverTipSelect = false;
                        e.stopImmediatePropagation();
                    });

                    //点击触发回发
                    element.querySelectorAll('a.nav-link:not(.dropdown-item)')
                        .forEach(function (a) {
                            a.onclick = function () {
                                interop.invokeMethodAsync(method, a.dataset.key);
                                tooltip.dispose();
                                state.hoverTipMenu = false;
                                state.hoverTipSelect = false;
                                e.stopImmediatePropagation();
                            }
                        });
                });

                tooltip.show();
                state.hasPopper = true;
                state.hoverTipSelect = false;
                e.stopImmediatePropagation();
            });

            //光标移出
            el.addEventListener('mouseleave', function (e) {
                var tipbar = this;
                var handler = window.setTimeout(function () {
                    var tooltip = bootstrap.Tooltip.getInstance(tipbar);
                    if (tooltip != null) {
                        state.hoverTipMenu = false;
                        if (state.hoverTipSelect === false) {
                            tooltip.dispose();
                        }
                    }
                    window.clearInterval(handler);
                }, isBreadcrumb ? 50 : 30);//值必须调整合适，不然会出现切换未隐藏的情况
                e.stopImmediatePropagation();
            });
        }
    });

    $(function () {
        $(document).on('click', '.menu a[href="#"]', function (e) {
            return false;
        });
    });
})(jQuery);
