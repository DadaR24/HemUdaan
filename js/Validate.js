function ShowDiv(mainDiv, oldDiv, currentDiv) {
    var MainDivShow = "#" + mainDiv;
    var CurrentDivHide = "#" + currentDiv;
    var OldDiv = "#" + oldDiv;;
    $(MainDivShow).css({ display: 'block' });
    $(CurrentDivHide).css({ display: 'none' });
    $(OldDiv).css({ display: 'none' });
    $("#lblError").html('');
    $("#ErrorDiv").css({ display: 'none' });

    return false;
};
function Validate() {

    var errorMsg = "";
    var username, password= "";

    username = $("#txtUserId").val();
    password = $("#inputPassword").val();
    company = $("ddlCompany option:selected").text();

    if (username == "") {
        errorMsg += "Enter UserName";
    }
    if (password == "") {
        errorMsg += "<br/>Enter Password";
    }
    if (company == "--Select Domain--") {
        errorMsg = "<br/>Select Domain";
    }

    if (username == "") {
        errorMsg += "<br/>Enter UserName";
    }
  
    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);

        return false;
    }
    else {
        //$("#lblError").html('');
        //$("#ErrorDiv").css({ display: 'none' });
        //$("#divMain").css({ display: 'none' });
        //$("#divChangePassword").css({ display: 'block' });
        return true;
    }


};

$(document).ready(function () {
    //$("#divResendOTP").hide();
    //$(".aClose").click(function () {
    //    window.close();
    //});
    //window.setTimeout(function () {
    //    $('#divResendOTP').show();
    //}, 60000 * 10);

});
function ValidateResetPassword() {
    var errorMsg = "";
    var username, emailid, MobileNo, company = "";
    username = $("#txtResetUserId").val();
    emailid = $("#txtResetEmailId").val();
    MobileNo = $("#txtResetMobileNo").val();
    company = $("#ddlResetCompanyName").find(":selected").text();


    if (username == "") {
        errorMsg = "Enter UserName";
    }
    if (!ValidateEmail(emailid)) {
        errorMsg += "<br/>Enter valid EmailId";
    }
    if (!ValidateNumber(MobileNo, 10)) {
        errorMsg += "<br/>Enter MobileNo";
    }
    if (company == "--Select--") {
        errorMsg += "<br/>Select Company";
    }

    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);
        return false;
    }
    else {

        return true;
    }

    return false;
};

function ValidateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function ValidateOTP() {
    var OTP, errorMsg = "";
    OTP = $("#txtResetPasswordOTP").val();
    if (!ValidateNumber(OTP, 6)) {
        errorMsg = "Enter Valid OTP";

    }

    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);
        return false;
    }
    else {
        return true;
    }

    return false;

};

function ValidateNumber(number, len) {
    var regex;
    if (len == 6)
        regex = /^\d{6}$/;
    else if (len == 10)
        regex = /^\d{10}$/;
    return regex.test(number);
};

function ValidateConfirmPassword() {
    var oldPassword, newPassword, confirmPassword = "";
    var errorMsg = "";
    oldPassword = $("#txtCurrentPassword").val();
    newPassword = $("#txtNewPassword").val();
    confirmPassword = $("#txtConfirmPassword").val();

    if (oldPassword == "") {
        errorMsg += "Enter old Password";
    }
    if (newPassword == "") {
        errorMsg += "<br/>Enter New Password";
    }
    if (confirmPassword == "") {
        errorMsg += "<br/>Enter Confirm Password";
    }

    if (newPassword != "" && confirmPassword != "") {
        if (newPassword != confirmPassword) {
            errorMsg += "<br/>Confirm Password should match";
        }
    }
    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);
        return false;
    }
    else {
        return true;
    }

};

function ValidatebtnConfirmReset() {
    var errorMsg, OTP = "";
    OTP = $("#txtResetPassOTP").val();

    if (!ValidateNumber(OTP, 6)) {
        errorMsg = "Enter Valid OTP";
    }
    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);
        return false;
    }
    else {

        return true;
    }

    return false;
};

function ValidateResetConfirmPassword() {
    var errorMsg = "";
    var NewPassword, ConfirmPassword = "";

    NewPassword = $("#txtResetNewPassword").val();
    ConfirmPassword = $("#txtResetConfirmPassword").val();
    if (NewPassword == "")
    { errorMsg += "Enter New Password"; }
    if (ConfirmPassword == "")
    { errorMsg += "Enter Confirm password"; }
    if (NewPassword != "" && ConfirmPassword != "") {
        if (NewPassword != ConfirmPassword) {
            errorMsg += "<br/>Confirm Password should match";
        }
    }
    if (errorMsg.length > 0) {

        $("#ErrorDiv").css({ display: 'block' });
        $("#lblError").css({ color: '#ff0000' });
        $("#lblError").html(errorMsg);
        return false;
    }
    else {
        return true;
    }
};
