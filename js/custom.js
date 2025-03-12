var isformValid = true;

//Convert Entire Form Data into Json Data Format. For e.g. $("#Form1").serializeObject();
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//
/** 
*  Function return whether the provided value matches with the provided regular expression pattern. If matches than it will return true else false.
*  @regx - Regular expression pattern
*  @value - Value/String that need to be matched
*/
var ck_no_special_character = /^[a-zA-Z0-9\s]+$/; // Allow Only Alphanumeric and white space.

//
var allowKey = new Array();
allowKey[0] = 1;
allowKey[1] = 8;
allowKey[2] = 9;
allowKey[3] = 14;
allowKey[4] = 15;
function ContainKey(array, value) {
    for (i = 0; i < array.length; i++) {
        if (array[i] == value) { return true; }
    }
    return false;
}
function ContainAllowedKey(key) {
    var result = ((key > 0 && key < 31) || (key == 127) || ContainKey(allowKey, key));
    return result;
}
function ck_regx(regx, value) {
    var isValid = regx.test(value);
    return isValid;
}

function ck_alphaNumericOnly(unicode) {
    if (ContainKey(allowKey, unicode)) { return true; }
    else if ((unicode >= 65 && unicode <= 90) || (unicode >= 97 && unicode <= 122) || (unicode >= 48 && unicode <= 57)) { return true; }
    else { return false; }
}

// 
//
var divError = "#divError";
var spanError = "#spanError";
var divSuccess = "#divSuccess";
var spanSuccess = "#spanSuccess";
function showError(text) {
    hideMessage();
    $(spanError).html(text);
    $(divError).removeClass("hidden");
    $(divError).focus();
    btnSubmitedStatus(false, 'Save'); focusMessage();
}

function showSuccess(text) {
    hideMessage();
    $(spanSuccess).html(text);
    $(divSuccess).removeClass("hidden");
    $(divSuccess).focus();
    btnSubmitedStatus(false, 'Save'); focusMessage();
}

function whiz_googler_show_error(text) {
    hideMessage();
    $(spanError).html(text);
    $(divError).removeClass("hidden");
    btnSubmitedStatus(false, 'Save'); focusMessage();
}

function hideMessage() {
    $(divError).addClass("hidden");
    $(divSuccess).addClass("hidden");
}

function focusMessage() {
    $(".scroll-to-top").trigger("click");
}

function resetForm() {
    btnSubmitedStatus(false, 'Save');
    $(".reset").val("");
    var alias = "";
    $(".reset").each(function (index) {
        alias = $(this).attr("alias");
        //alert(alias);
        if (alias != null && alias != undefined && alias != "") { $(this).val(alias); }
    });
}

function goRefreshPage()
{
    location.reload();
}
//
function typeheadpainner(iname, load) {
    iname = "#" + iname;
    if (load) {
        $(iname).removeClass("fa fa-spinner");
        $(iname).addClass("fa fa-search");
    }
    else {
        $(iname).removeClass("fa fa-search");
        $(iname).addClass("fa fa-spinner");
    }
}
//Disabled button on Ajax load
function btnSubmitedStatus(status, text) {
    $(".btnsave").val(text);
    $(".btnsave").prop('disabled', status);
    if (status == false) {
        var alis = "";
        $(".btnsave").each(function (index) {
            alis = $(this).attr("alis");
            if (alis != null && alis != undefined && alis != "") { $(this).val(alis); }
        });
    }
}

function btnSubmitedStatusAjax(btn, status, text) {
    btn.html(text);
    btn.prop('disabled', status);
    if (status == false) {
        var alis = btn.attr("alis");
        if (alis != null && alis != undefined && alis != "") { $(this).val(alis); } else { $(this).val(text);}
    }
}
//
//
function popupUrl(url) {
    var returnValue = "";
    if (window.showModalDialog) {
        returnValue = window.showModalDialog(url, "PopUp", "dialogWidth:500px;dialogHeight:500px");
    }
    else {
        returnValue = window.open(url, "PopUp", 'width=500,height=500,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=no');
    }
}

function popupAppendDropdown(url, dropdown) {
    var returnValue = "";
    if (window.showModalDialog) {
        returnValue = window.showModalDialog(url, dropdown, "dialogWidth:500px;dialogHeight:500px");
    }
    else {
        returnValue = window.open(url, dropdown, 'width=500,height=500,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=no');
    }
    if (returnValue != "" && returnValue != "undefined" && returnValue != undefined) {
        dropdown = "#" + dropdown;
        var retrunkey = returnValue.split(",")[0];
        var retruntext = returnValue.split(",")[1];
        //alert(dropdown+" : "+retrunkey + "-" + retruntext);
        $(dropdown).append($("<option></option>").attr("value", retrunkey).text(retruntext));
        $(dropdown).val(retrunkey);
    }
}

function popupAppendTextBox(url, textBox) {
    var returnValue = "";
    if (window.showModalDialog) {
        returnValue = window.showModalDialog(url, textBox, "dialogWidth:500px;dialogHeight:500px");
    }
    else {
        returnValue = window.open(url, textBox, 'width=500,height=500,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=no');
    }
    if (returnValue != "" && returnValue != "undefined" && returnValue != undefined) {
        textBox = "#" + textBox;
        var retrunkey = returnValue.split(",")[0];
        var retruntext = returnValue.split(",")[1];
        $(textBox).val(retrunkey);
    }
}
//Source:http://stackoverflow.com/questions/16037165/displaying-a-number-in-indian-format-using-javascript
function ConvertToIndianRupee(x) {
    x = x.toString();
    var lastThree = x.substring(x.length - 3);
    var otherNumbers = x.substring(0, x.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    return res;
}

