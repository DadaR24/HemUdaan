


(function ($) {

    $.fn.extend({
        lightBox: function (options) {
            var defaults = {
                openPop: '',
                closePop: '',
                overlayAlpha: 0.8,
                animSpeed: 700,
                centerAlign: true,
                dynamicData: false
            };
            var options = $.extend(defaults, options);
            return this.each(function () {
                var o = options;
                var obj = $(this);
                var overlay;
                if ($('.overlay').size() != 1) {
                    $('body').append('<div class="overlay"></div>');
                }

                overlay = $('.overlay');
                //                overlay.css({ 'height': $(document).height(), 'opacity': o.overlayAlpha });
                //below line added by vidyasagar
                overlay.css({ 'opacity': o.overlayAlpha });


                $(o.openPop).live('click', openPop);

                $(o.closePop).live('click', closePop);
                function openPop() {

                    if (o.centerAlign) {
                        //                        overlay.css({ 'height': $(document).height(), 'opacity': o.overlayAlpha });
                        //below line added by vidyasagar
                        overlay.css({ 'opacity': o.overlayAlpha });
                        jQuery.fn.center = function () {
                            $(this).css({ 'position': 'fixed', 'top': (($(window).height() - $(this).height()) / 2 - $(window).scrollTop()) + $(document).scrollTop(), 'left': (($(window).width() - $(this).width()) / 2) + $(document).scrollLeft() });

                            return this;
                        };

                        obj.center();
                    };


                    if (o.dynamicData) {

                        obj.css({ 'visibility': 'visible' })
                        overlay.fadeIn(o.animSpeed);
                    } else {

                        obj.fadeIn(o.animSpeed);
                        overlay.fadeIn(o.animSpeed);
                    }


                    //return false;
                };

                function closePop() {

                    if (o.dynamicData) {
                        obj.css({ 'visibility': 'hidden' })
                        overlay.fadeOut(o.animSpeed);
                    }
                    else {
                        obj.fadeOut(o.animSpeed, function () {

                            $('.downloadDH').find('div.slide').css({ 'display': 'none' });
                            $('.downloadDH').find('div.slide:first').css({ 'display': 'block' });
                            $('.downloadDH').find('h2').removeClass('active');
                            $('.downloadDH').find('h2:first').addClass('active');

                            $('#rgtrC').css({ 'display': 'block' });
                            $('#thankUMess').css({ 'display': 'none' });
                            $('#rgtrC input.txt').attr({ 'value': '' });

                            $('#rgtrC td.errorTxt img').css({ 'visibility': 'hidden' })
                            $('#rgtrC td.errorTxt span').empty();

                        });
                        overlay.fadeOut(o.animSpeed);
                    }


                    return false;
                };


                /*$(document).click(function() {
                obj.fadeOut(o.animSpeed);
                overlay.fadeOut(o.animSpeed);
                })

                obj.click(function() {
                return false;
                })*/

                //                $(window).keydown(function (e) {
                //                    if (e.which == 27 || e.keyCode == 27) {
                //                        $('.LikeDiv').fadeOut(700);
                //                        $('.overlay').fadeOut(700);
                //                    }

                //                });




            });

        }
    });

})(jQuery)


