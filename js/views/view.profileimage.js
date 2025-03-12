'use strict';
/*
fraction.js
A Javascript fraction library.
Copyright (c) 2009  Erik Garrison <erik@hypervolu.me>
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


/* Fractions */
/* 
 *
 * Fraction objects are comprised of a numerator and a denomenator.  These
 * values can be accessed at fraction.numerator and fraction.denomenator.
 *
 * Fractions are always returned and stored in lowest-form normalized format.
 * This is accomplished via Fraction.normalize.
 *
 * The following mathematical operations on fractions are supported:
 *
 * Fraction.equals
 * Fraction.add
 * Fraction.subtract
 * Fraction.multiply
 * Fraction.divide
 *
 * These operations accept both numbers and fraction objects.  (Best results
 * are guaranteed when the input is a fraction object.)  They all return a new
 * Fraction object.
 *
 * Usage:
 *
 * TODO
 *
 */

/*
 * The Fraction constructor takes one of:
 *   an explicit numerator (integer) and denominator (integer),
 *   a string representation of the fraction (string),
 *   or a floating-point number (float)
 *
 * These initialization methods are provided for convenience.  Because of
 * rounding issues the best results will be given when the fraction is
 * constructed from an explicit integer numerator and denomenator, and not a
 * decimal number.
 *
 *
 * e.g. new Fraction(1, 2) --> 1/2
 *      new Fraction('1/2') --> 1/2
 *      new Fraction('2 3/4') --> 11/4  (prints as 2 3/4)
 *
 */

var Fraction = function (numerator, denominator) {
    /* double argument invocation */
    if (typeof numerator !== 'undefined' && denominator) {
        if (
            (typeof (numerator) === 'number' || numerator instanceof Number)
            &&
            (typeof (denominator) === 'number' || denominator instanceof Number)
        ) {
            this.numerator = numerator;
            this.denominator = denominator;
        } else if (
            (typeof (numerator) === 'string' || numerator instanceof String)
            &&
            (typeof (denominator) === 'string' || denominator instanceof String)
        ) {
            // what are they?
            // hmm....
            // assume they are floats?
            this.numerator = parseFloat(numerator.replace(",", "."));
            this.denominator = parseFloat(denominator.replace(",", "."));
        }
        // TODO: should we handle cases when one argument is String and another is Number?
        /* single-argument invocation */
    } else if (typeof denominator === 'undefined') {
        var num = numerator; // swap variable names for legibility
        if (num instanceof Fraction) {
            this.numerator = num.numerator;
            this.denominator = num.denominator;
        } else if (typeof (num) === 'number' || num instanceof Number) {  // just a straight number init
            this.numerator = num;
            this.denominator = 1;
        } else if (typeof (num) === 'string' || num instanceof String) {
            var a, b;  // hold the first and second part of the fraction, e.g. a = '1' and b = '2/3' in 1 2/3
            // or a = '2/3' and b = undefined if we are just passed a single-part number
            var arr = num.split(' ');
            if (arr[0]) a = arr[0];
            if (arr[1]) b = arr[1];
            /* compound fraction e.g. 'A B/C' */
            //  if a is an integer ...
            if (a % 1 === 0 && b && b.match('/')) {
                return (new Fraction(a)).add(new Fraction(b));
            } else if (a && !b) {
                /* simple fraction e.g. 'A/B' */
                if ((typeof (a) === 'string' || a instanceof String) && a.match('/')) {
                    // it's not a whole number... it's actually a fraction without a whole part written
                    var f = a.split('/');
                    this.numerator = f[0]; this.denominator = f[1];
                    /* string floating point */
                } else if ((typeof (a) === 'string' || a instanceof String) && a.match('\.')) {
                    return new Fraction(parseFloat(a.replace(",", ".")));
                    /* whole number e.g. 'A' */
                } else { // just passed a whole number as a string
                    this.numerator = parseInt(a);
                    this.denominator = 1;
                }
            } else {
                return undefined; // could not parse
            }
        }
    }
    this.normalize();
}


Fraction.prototype.clone = function () {
    return new Fraction(this.numerator, this.denominator);
}

/* pretty-printer, converts fractions into whole numbers and fractions */
Fraction.prototype.toString = function () {
    if (isNaN(this.denominator))
        //	if (this.denominator !== this.denominator) //They say it would be faster. (?)
        return 'NaN';
    var result = '';
    if ((this.numerator < 0) != (this.denominator < 0))
        result = '-';
    var numerator = Math.abs(this.numerator);
    var denominator = Math.abs(this.denominator);

    var wholepart = Math.floor(numerator / denominator);
    numerator = numerator % denominator;
    if (wholepart != 0)
        result += wholepart;
    if (numerator != 0) {
        if (wholepart != 0)
            result += ' ';
        result += numerator + '/' + denominator;
    }
    return result.length > 0 ? result : '0';
}

/* pretty-printer to support TeX notation (using with MathJax, KaTeX, etc) */
Fraction.prototype.toTeX = function (mixed) {
    if (isNaN(this.denominator))
        return 'NaN';
    var result = '';
    if ((this.numerator < 0) != (this.denominator < 0))
        result = '-';
    var numerator = Math.abs(this.numerator);
    var denominator = Math.abs(this.denominator);

    if (!mixed) {
        //We want a simple fraction, without wholepart extracted
        if (denominator === 1)
            return result + numerator;
        else
            return result + '\\frac{' + numerator + '}{' + denominator + '}';
    }
    var wholepart = Math.floor(numerator / denominator);
    numerator = numerator % denominator;
    if (wholepart != 0)
        result += wholepart;
    if (numerator != 0)
        result += '\\frac{' + numerator + '}{' + denominator + '}';
    return result.length > 0 ? result : '0';
}

/* destructively rescale the fraction by some integral factor */
Fraction.prototype.rescale = function (factor) {
    this.numerator *= factor;
    this.denominator *= factor;
    return this;
}

Fraction.prototype.add = function (b) {
    var a = this.clone();
    if (b instanceof Fraction) {
        b = b.clone();
    } else {
        b = new Fraction(b);
    }
    var td = a.denominator;
    a.rescale(b.denominator);
    a.numerator += b.numerator * td;

    return a.normalize();
}


Fraction.prototype.subtract = function (b) {
    var a = this.clone();
    if (b instanceof Fraction) {
        b = b.clone();  // we scale our argument destructively, so clone
    } else {
        b = new Fraction(b);
    }
    var td = a.denominator;
    a.rescale(b.denominator);
    a.numerator -= b.numerator * td;

    return a.normalize();
}


Fraction.prototype.multiply = function (b) {
    var a = this.clone();
    if (b instanceof Fraction) {
        a.numerator *= b.numerator;
        a.denominator *= b.denominator;
    } else if (typeof b === 'number') {
        a.numerator *= b;
    } else {
        return a.multiply(new Fraction(b));
    }
    return a.normalize();
}

Fraction.prototype.divide = function (b) {
    var a = this.clone();
    if (b instanceof Fraction) {
        a.numerator *= b.denominator;
        a.denominator *= b.numerator;
    } else if (typeof b === 'number') {
        a.denominator *= b;
    } else {
        return a.divide(new Fraction(b));
    }
    return a.normalize();
}

Fraction.prototype.equals = function (b) {
    if (!(b instanceof Fraction)) {
        b = new Fraction(b);
    }
    // fractions that are equal should have equal normalized forms
    var a = this.clone().normalize();
    var b = b.clone().normalize();
    return (a.numerator === b.numerator && a.denominator === b.denominator);
}


/* Utility functions */

/* Destructively normalize the fraction to its smallest representation. 
 * e.g. 4/16 -> 1/4, 14/28 -> 1/2, etc.
 * This is called after all math ops.
 */
Fraction.prototype.normalize = (function () {

    var isFloat = function (n) {
        return (typeof (n) === 'number' &&
            ((n > 0 && n % 1 > 0 && n % 1 < 1) ||
                (n < 0 && n % -1 < 0 && n % -1 > -1))
        );
    }

    var roundToPlaces = function (n, places) {
        if (!places) {
            return Math.round(n);
        } else {
            var scalar = Math.pow(10, places);
            return Math.round(n * scalar) / scalar;
        }
    }

    return (function () {

        // XXX hackish.  Is there a better way to address this issue?
        //
        /* first check if we have decimals, and if we do eliminate them
         * multiply by the 10 ^ number of decimal places in the number
         * round the number to nine decimal places
         * to avoid js floating point funnies
         */
        if (isFloat(this.denominator)) {
            var rounded = roundToPlaces(this.denominator, 9);
            var scaleup = Math.pow(10, rounded.toString().split('.')[1].length);
            this.denominator = Math.round(this.denominator * scaleup); // this !!! should be a whole number
            //this.numerator *= scaleup;
            this.numerator *= scaleup;
        }
        if (isFloat(this.numerator)) {
            var rounded = roundToPlaces(this.numerator, 9);
            var scaleup = Math.pow(10, rounded.toString().split('.')[1].length);
            this.numerator = Math.round(this.numerator * scaleup); // this !!! should be a whole number
            //this.numerator *= scaleup;
            this.denominator *= scaleup;
        }
        var gcf = Fraction.gcf(this.numerator, this.denominator);
        this.numerator /= gcf;
        this.denominator /= gcf;
        if (this.denominator < 0) {
            this.numerator *= -1;
            this.denominator *= -1;
        }
        return this;
    });

})();


/* Takes two numbers and returns their greatest common factor. */
//Adapted from Ratio.js
Fraction.gcf = function (a, b) {
    if (arguments.length < 2) {
        return a;
    }
    var c;
    a = Math.abs(a);
    b = Math.abs(b);
    /*  //It seems to be no need in these checks
        // Same as isNaN() but faster
        if (a !== a || b !== b) {
            return NaN;
        }
        //Same as !isFinite() but faster
        if (a === Infinity || a === -Infinity || b === Infinity || b === -Infinity) {
            return Infinity;
         }
         // Checks if a or b are decimals
         if ((a % 1 !== 0) || (b % 1 !== 0)) {
             throw new Error("Can only operate on integers");
         }
    */

    while (b) {
        c = a % b;
        a = b;
        b = c;
    }
    return a;
};

//Not needed now
// Adapted from: 
// http://www.btinternet.com/~se16/js/factor.htm
Fraction.primeFactors = function (n) {

    var num = Math.abs(n);
    var factors = [];
    var _factor = 2;  // first potential prime factor

    while (_factor * _factor <= num)  // should we keep looking for factors?
    {
        if (num % _factor === 0)  // this is a factor
        {
            factors.push(_factor);  // so keep it
            num = num / _factor;  // and divide our search point by it
        }
        else {
            _factor++;  // and increment
        }
    }

    if (num != 1)                    // If there is anything left at the end...
    {                                // ...this must be the last prime factor
        factors.push(num);           //    so it too should be recorded
    }

    return factors;                  // Return the prime factors
}


Fraction.prototype.snap = function (max, threshold) {
    if (!threshold) threshold = .0001;
    if (!max) max = 100;

    var negative = (this.numerator < 0);
    var decimal = this.numerator / this.denominator;
    var fraction = Math.abs(decimal % 1);
    var remainder = negative ? Math.ceil(decimal) : Math.floor(decimal);

    for (var denominator = 1; denominator <= max; ++denominator) {
        for (var numerator = 0; numerator <= max; ++numerator) {
            var approximation = Math.abs(numerator / denominator);
            if (Math.abs(approximation - fraction) < threshold) {
                return new Fraction(remainder * denominator + numerator * (negative ? -1 : 1), denominator);
            }
        }
    }

    return new Fraction(this.numerator, this.denominator);
};

/* If not in browser */
if (typeof module !== "undefined")
    module.exports.Fraction = Fraction

var imageCrop = '';

//function popupResult(result) {
//    var html;
//    html = '<img src="' + result.src + '" />';
//    swal({
//        title: 'Image Result',
//        html: html,
//        text: null,
//        allowOutsideClick: true
//    });
//    setTimeout(function () {
//        $('.sweet-alert').css('margin', function () {
//            var top = -1 * ($(this).height() / 2),
//                left = -1 * ($(this).width() / 2);

//            return top + 'px 0 0 ' + left + 'px';
//        });
//    }, 1);
//}

function reset_image(imageCrop) {
    imageCrop ? imageCrop.croppie('destroy') : '';

    $('.avatar_image').removeClass("cr-original-image").addClass("img-fluid mx-auto d-block");
    $(".image_overlay .view.overlay").prepend($(".image_overlay .view.overlay > div").html());
    $(".image_overlay .view.overlay > div").remove();
}

function initCropImage(width, height, type, enable_resize) {
    var imageCrop = $('.avatar_image').croppie({
        enableExif: true,
        enableOrientation: true,
        enableZoom: true,
        enforceBoundary: true,
        mouseWheelZoom: true,
        showZoomer: true,
        enableResize: enable_resize,
        /*customClass: "wowy",*/
        viewport: {
            width: width,
            height: height,
            type: type/*circle*/
        }/*,
         boundary: {
         width: "100%",
         height: "100%"
         }*/
    });
    return (imageCrop);
}

var new_image = '', crop_type = 'square', original_image = '', ratio_width = '250', ratio_height = '250',
    pixels_w = '', pixels_h = '', ratio_aspect = '';

$('#flProfilePic').on('change', function () {
    var selected_file_name = $(this).val();
    if (selected_file_name.length > 0) {

        var fileInput = $('#flProfilePic');
        var maxSize = 2097152;//Define Max Size 2 MB

        if (!validateImage(fileInput, maxSize)) {
            $('.img-actions').addClass('hidden');
            return;
        }


        /* Some file selected */
        var reader = new FileReader();
        reader.onload = function (e) {
            var image = new Image();
            image.src = reader.result;

            image.onload = function () {

                pixels_w = image.width;
                pixels_h = image.height;

                $("input[name=pixels_width]").val(pixels_w);


                $("input[name=pixels_height]").val(pixels_h);
                ratio_aspect = new Fraction(Math.round(pixels_w / pixels_h * 100) / 100);

                //console.log(Math.round(pixels_w / pixels_h * 100) / 100);
                $('input[name="ratio_width"]').val(ratio_aspect.numerator);
                $('input[name="ratio_height"]').val(ratio_aspect.denominator);

            }

            if (!$(".image_overlay .cr-boundary").length) {
                imageCrop = initCropImage(ratio_width, ratio_height, crop_type, false);
                $('.avatar_image').removeClass("img-fluid mx-auto d-block");
                $(".rotate_left, .rotate_right, .view_picture, .image_settings,  .md-form, .form-group,  .switch, #common_presets").removeClass("hidden-xs-up");
            };
            new_image = e.target.result;

            imageCrop.croppie('bind', {
                url: e.target.result
            });

        };
        reader.readAsDataURL(this.files[0]);

        $('.img-actions').removeClass('hidden');
    } else {
        $('.img-actions').addClass('hidden');
    }
    //$(this).val('');
});

$('.rotate_left, .rotate_right').on('click', function (ev) {
    console.log(document.getElementById("flProfilePic").files.length);
    if (document.getElementById("flProfilePic").files.length > 0) {
        imageCrop.croppie('rotate', parseInt($(this).data('deg')));
    }
});

$("input[type=number]").on("keyup keydown", function (e) {

    var max = parseInt($(this).attr('max'));
    var min = parseInt($(this).attr('min'));
    if ($(this).val() > max && e.keyCode != 46 && e.keyCode != 8) {
        e.preventDefault();
        $(this).val(max);
    }
});

var ratio_w = '', ratio_h = '', ratios = '';
$('select[name="common_presets"]').on("change", function () {
    ratios = $('select[name="common_presets"]').val().split(":");
    $('input[name="ratio_width"]').val(ratios[0]);
    $('input[name="ratio_height"]').val(ratios[1]);
    $('input[name="pixels_height"]').val(Math.round($('input[name="pixels_width"]').val() * ratios[1] / ratios[0]));
});


$('input[name="pixels_width"]').on("input", function () {
    ratio_w = $('input[name="ratio_width"]').val();
    ratio_h = $('input[name="ratio_height"]').val();
    $('input[name="pixels_height"]').val(Math.round($('input[name="pixels_width"]').val() * ratio_h / ratio_w));
});

$('input[name="ratio_width"]').on("input", function () {
    ratio_h = $('input[name="ratio_height"]').val();
    $('input[name="pixels_height"]').val(Math.round($('input[name="pixels_width"]').val() * ratio_h / $(this).val()));
});

$('input[name="ratio_height"]').on("input", function () {
    ratio_w = $('input[name="ratio_width"]').val();
    $('input[name="pixels_height"]').val(Math.round($('input[name="pixels_width"]').val() * $(this).val() / ratio_w));
});

$(".image_settings").click(function () {

    /*Read aspect ratio settings*/
    ratio_width = $('input[name="ratio_width"]').val() * 10;
    ratio_height = $('input[name="ratio_height"]').val() * 10;

    /*Read type of crop*/
    if ($("#crop-type input[type=checkbox]").is(":checked")) {
        crop_type = 'circle';
    }
    else {
        crop_type = 'square';
    }

    if ($('#original_image').is(":checked")) {
        reset_image(imageCrop);
        imageCrop = initCropImage(ratio_width, ratio_height, crop_type, false);
        $(".avatar_image").removeClass("img-fluid mx-auto d-block");
        imageCrop.croppie('bind', {
            url: new_image
        });
    }
    else {
        imageCrop.croppie('result', {
            type: 'canvas',
            size: 'viewport'
        }).then(function (resp) {
            $(".avatar_image").attr({ "src": resp });
            reset_image(imageCrop);
            imageCrop = initCropImage(ratio_width, ratio_height, crop_type, false);
            $('.avatar_image').removeClass("img-fluid mx-auto d-block");
        });
    }

});

$(".view_picture").click(function () {

    if (document.getElementById("flProfilePic").files.length > 0) {

        var fileInput = $('#flProfilePic');
        var maxSize = 2097152;//Define Max Size 2 MB

        if (!validateImage(fileInput, maxSize)) {
            $('.img-actions').addClass('hidden');
            return;
        }
        //Valid image
        btnSubmitedStatusAjax($(this),true, 'Processing...');
        imageCrop.croppie('result', {
            type: 'canvas',
            size: 'viewport'
        }).then(function (resp) {
            var imageData = resp.split(',');

            $('#imageData').val(imageData[1]);
           
            $('#WJForm').ajaxSubmit(function (data) {
                var response = jQuery.parseJSON(data);
                    if (response.Success) {
                        btnSubmitedStatusAjax($(".view_picture"), false, 'Done');
                       
                    $.alert({
                        title: 'Success',
                        content: 'Profile Image Updated',
                        buttons: {
                            ok: {
                                btnClass: 'btn-blue',
                                action: function () {
                                    location.reload();
                                }
                            },
                        }
                    });
                    } else {
                       
                        $.alert({
                            title: 'Error',
                            content: 'Something Went Wrong! Contact Administrator',
                            type : 'Warning',
                            buttons: {
                                ok: function () {
                                    btnSubmitedStatusAjax($(".view_picture"), false, 'Upload Picture');
                                },
                            }
                        });
                    //showError(response.Message);
                }
            });
        });
    }
});

function validateImage(fileInput, maxSize) {
    var fileSize = fileInput.get(0).files[0].size; // Get Size in Byte
    var type = fileInput.get(0).files[0].type;//Get Type for select image
    var type_reg = /^image\/(jpg|png|jpeg)$/;//Define type of image

    if (!type_reg.test(type)) {//Check Type of image
        $('#Profileimage_err').text('Please select a valid profile image (.png/.jpg/.jpeg)');
        $('#file').value = null;
        return false;
    }
    else if (fileSize > maxSize) {//Check Image size
        $('#Profileimage_err').text('Allowed file size exceeded. (Max size. 2 MB)');
        $('#file').value = null;
        return false;
    }
    $('#Profileimage_err').text('');
    return true;
}