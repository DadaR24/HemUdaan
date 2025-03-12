$(document).ready(function () {
    $(".Numeric").keydown(function (event) {
        if (event.keyCode == 190 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey === true) || (event.keyCode >= 35 && event.keyCode <= 39)) { return; }
        else { if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) { event.preventDefault(); } }
    });
    $(".Mobile").keydown(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey === true) || (event.keyCode >= 35 && event.keyCode <= 39)) { return; }
        else
            if (this.value.length >= 13) { event.preventDefault(); }
            else
                if (event.keyCode == 107 && this.value.length == 0) { return; }
                else if (this.value.length >= 10 && this.value.charAt(0) != '0' && this.value.charAt(0) != '+') { event.preventDefault(); }
                else { if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) { event.preventDefault(); } }
});
$(".particulars").keydown(function (event) {
    if (event.shiftKey) {
        if (event.keyCode == 188) { return false; }
        if (event.keyCode == 190) { return false; }
    }

});

$(".box_close1").click(function (e) {
    e.preventDefault();
    window.history.back()
});


$(document).bind('keydown keyup', function (e) {
    if (e.which === 116) {
        alert('You have pressed F5, For Security reason your session will get expired. Please login to the system again.');
        window.location = "loggedout.aspx";
        //return false;
    }
    if (e.which === 82 && e.ctrlKey) {
        alert('You have pressed F5, For Security reason your session will get expired. Please login to the system again.');
        window.location = "loggedout.aspx";
        //return false;
    }
});

$('.closePopup,.overlay,.closeprint').click(function () {
    $('.lightbox_heading').hide();
    $('.overlay').fadeOut(700);
    $('.openPopUp').fadeOut();
    $('.openPopUpPrint').fadeOut();
    $('.openPopUpForComments').fadeOut();
    $('.openPopUpEmpDetail').fadeOut();
    $('.openPopUpForGlCode').fadeOut();
    $('.BigImg').attr('src', '');
    $("#iframepdf").attr('src', '');
});
$('.glHelp').click(function () {
    $('.overlay').fadeIn(700);
    $('.openPopUpForGlCode').fadeIn(700);
    var height = -220; // (($(window).height() - $('.container-fluid').height()) / 2 - $(window).scrollTop()) + $(document).scrollTop();
    $('.openPopUpForGlCode').css({ 'top': height, 'left': (($(window).width() - $('.openPopUpForGlCode').width()) / 2) + $(document).scrollLeft() });
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        $('.lightbox_heading').hide();
        $('.overlay').fadeOut(700);
        $('.openPopUp').fadeOut();
        $('.openPopUpPrint').fadeOut();
        $('.openPopUpForComments').fadeOut();
        $('.openPopUpEmpDetail').fadeOut();
        $('.openPopUpForGlCode').fadeOut();
        $('.BigImg').attr('src', '');
        $("#iframepdf").attr('src', '');
    }
});
});



function funShowMsg(num) {
    alert("Only " + num + " expense items are allowed to add in one expense sheet. Please create new expense sheet.");
}
function checkDate(Date) {
    var matches = /^(\d{2})[-\/](\d{2})[-\/](\d{4})$/.exec(Date);
    if (matches == null) {
        //Here you can add code to highlight, show error etc.
        return false;
    }
}


function funcheckpdfExtensionAndIE(extension, url, showmsg) {
    if (navigator.appName == "Microsoft Internet Explorer") {
        ie = true;
        var flag = true;
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})");
        if (re.exec(ua) != null) {
            ieVersion = parseInt(RegExp.$1);
        }
        if (ieVersion <= 8) {
            switch (extension) {
                case 'jpg':
                case 'jpeg':
                case 'JPG':
                case 'JPEG':
                    flag = true;
                    break;
                default:
                    if (showmsg == true) {
                        alert('Preview of PDF has been disabled in this browser version as there is a problem viewing PDF files, kindly logout from this browser and try to view the bills from other browser or upgrade your current browser.');
                        flag = false;
                    }
                    else {
                        window.open(url, 'EMS', 'width=400,height=800')
                        return false;
                    }
            }
            return flag;
        }
        else {
            return flag;
        }
    }
    else {
        return true;
    }
}


function checkFileExtension(file) {
    var flag = true;
    var extension = file.substr((file.lastIndexOf('.') + 1));

    //            if (navigator.appName == "Microsoft Internet Explorer") {
    //                //Set IE as true
    //                ie = true;
    //                //Create a user agent var
    //                var ua = navigator.userAgent;
    //                //Write a new regEx to find the version number
    //                var re = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})");
    //                //If the regEx through the userAgent is not null
    //                if (re.exec(ua) != null) {
    //                    //Set the IE version
    //                    ieVersion = parseInt(RegExp.$1);
    //                }
    //                if (ieVersion<=8) {
    //                    switch (extension) {
    //                        case 'jpg':
    //                        case 'jpeg':
    //                        case 'JPG':
    //                        case 'JPEG':
    //                            flag = true;
    //                            break;
    //                        default:
    //                            alert('You can Upload JPG,JPEG extensions files from this browser, upgrade it or Try to upload the PDF files from other browser.');
    //                            flag = false;
    //                    }
    //                    return flag;
    //                }
    //                
    //            }
    //            else {
    switch (extension) {
        case 'jpg':
        case 'jpeg':
        case 'JPG':
        case 'JPEG':
        case 'pdf':
        case 'PDF':
            flag = true;
            break;
        default:
            alert('You can upload only jpg,jpeg,pdf extensions files.');
            flag = false;
    }
    return flag;
    //}
}

function validateDateDDMMYYY(ExpenseDate) {

    var Char1 = ExpenseDate.charAt(2);
    var Char2 = ExpenseDate.charAt(5);
    // alert(Char1); alert(Char2);

    var flag = false;

    if (Char1 == '/' && Char2 == '/') {
        // alert ('valid positions of non numeric characters.');
        flag = true;
    }
    else {
        // alert('invalid position of non numeric symbols');
        flag = false;
    }

    var day;
    var month;
    var year;

    day = ExpenseDate.substring(0, 2);
    month = ExpenseDate.substring(3, 5);
    year = ExpenseDate.substring(6, 10);

    // alert(day); alert(month);alert(year);
    if (validDay(day) && validMonth(month, day, year) && validYear(year) && (flag == true)) {
        // alert(' Valid Date')
        return true;
    }
    else {
        //alert('Invalid Date Format: Please enter in DD/MM/YYYY format');
        return false;
    }

} // end func

function IsNumeric(sText) {
    var ValidChars = "0123456789.";
    var IsNumber = true;
    var Char;

    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }

    return IsNumber;
} // end func 


function validDay(day) {
    if (IsNumeric(day)) {
        if (day > 0 && day < 32) {
            return true;
        }
        else {
            alert('Please enter valid Date.');
            return false;
        }
    }
    else {
        alert('Please enter valid Date.');
        return false;
    }

} // end func 


function validMonth(month, day, year) {
    if (IsNumeric(month)) {
        if (month > 0 && month < 13) {
            if (month == 04 || month == 06 || month == 09 || month == 11) {
                if (day > 30) {
                    alert('Please enter valid Date for the month you have entered.');
                    return false;
                }
            }

            if (month == 02) {
                var lyear = false;
                if ((!(year % 4) && year % 100) || !(year % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (day >= 29)) {
                    alert('Please enter valid Date for the month you have entered.');
                    return false;
                }
            }

            return true;
        }
        else {
            alert('Please enter valid month.');
            return false;
        }
    }
    else {
        alert('Please enter valid month.');
        return false;
    }
} // end func  



function validYear(year) {
    var d = new Date();
    var currentYear = d.getFullYear();

    if (year.length != 4) {
        alert('Please enter valid Year.');
        return false;
    }

    if (IsNumeric(year)) {
        if (year > 0 && year <= currentYear) {
            return true;
        }
        else {
            alert('Expense date can not be selected for Future Year.');
            return false;
        }

    }
    else {
        alert('Please enter valid Year.');
        return false;
    }

} // end func

function funcheckDateExpenseOnAdd(ExpenseDate, k) {
    var todaysDate = new Date();
    var one_day = 1000 * 60 * 60 * 24;
    var day;
    var month;
    var year;
    var priorDate = new Date().setDate(todaysDate.getDate() - k);
    alert(priorDate);
    return false;
    day = ExpenseDate.substring(0, 2);
    month = ExpenseDate.substring(3, 5);
    year = ExpenseDate.substring(6, 10);
    var nextWeek = Date.parse(new Date(year, month, day - k));
    var date2_ms = todaysDate.getTime();
    var difference_ms = date2_ms - date1_ms;
    var diffDays = (Math.round(difference_ms / one_day));

    var month = k / 30;
    if (todaysDate.getHours() < 12) {
        if (diffDays < 0) {
            alert('Expense date can not be selected as a future expense date.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else if (diffDays > k) {
            alert('Expense date can only be selected for last ' + month + ' month(s).');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else {
            return true;
        }
    }
    else if (todaysDate.getHours() >= 12) {
        if (diffDays <= 0) {
            alert('Expense date can not be selected as a future expense date.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else if (diffDays > k) {
            alert('Expense date can only be selected for last ' + month + ' month(s).');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else {
            return true;
        }
    }
}


function funcheckDateExpenseCommon(sender, args, k) {
    var todaysDate = new Date();
    var one_day = 1000 * 60 * 60 * 24;
    var date1_ms = sender._selectedDate.getTime();
    var date2_ms = todaysDate.getTime();
    var difference_ms = date2_ms - date1_ms;
    var diffDays = (Math.round(difference_ms / one_day));

    var month = k / 30;
    if (todaysDate.getHours() < 12) {
        if (diffDays < 0) {
            alert('Expense date can not be selected as a future expense date.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else if (diffDays > k) {
            alert('Expense date can only be selected for last ' + month + ' month.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else {
            return true;
        }
    }
    else if (todaysDate.getHours() >= 12) {
        if (diffDays <= 0) {
            alert('Expense date can not be selected as a future expense date.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else if (diffDays > k) {
            alert('Expense date can only be selected for last ' + month + ' month.');
            sender._textbox.set_Value(todaysDate.format("dd/MM/yyyy"));
            return false;
        }
        else {
            return true;
        }
    }
}

 