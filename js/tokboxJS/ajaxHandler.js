(function ($) {
    $.extend({
        // postJSON: function (url, method, jsonData, callbackSuccess, options) {
        //     var config = {
        //         url: url,
        //         type: method,
        //         //data: jsonData ? JSON.stringify(jsonData) : null,
        //         //dataType: "json",
        //         data: jsonData,
        //         //contentType: "application/json; charset=utf-8",
        //         cache: false,
        //         contentType: false,
        //         processData: false,
        //         success: callbackSuccess
        //     };
        postJSON: function (url, method, jsonData, callbackSuccess, callBackError) {
            var config = {
                url: url,
                type: method,
                //data: jsonData ? JSON.stringify(jsonData) : null,
                dataType: "json",
                data: jsonData,
                contentType: "application/json",
                //cache: false,
                //contentType: false,
                //processData: false,
                success: callbackSuccess,
                error: callBackError
            };

            // $.ajax($.extend(options, config)); // jQuery 1.4+
            $.ajaxSetup(config); // jQuery 1.5+
            $.ajax();

            // reset back so that future users aren't affected
            config = {
                url: null,
                type: "GET",
                data: null,
                dataType: null,
                contentType: "application/x-www-form-urlencoded",
                success: null,
                error: null
            };
            $.ajaxSetup(config);
        }
    });
})(jQuery);