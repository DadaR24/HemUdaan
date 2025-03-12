

function Contact() {
    var form = $("#contact");
    $(form).ajaxSubmit(function (data) {
        var response = jQuery.parseJSON(data);
        if (response.Success == true) {
            $('#divContactSuccess').removeClass('hidden');
            $("#contact").addClass('hidden');
        }
        else {
            $('#contactError').removeClass('hidden');
            $('#contactError').html(data.Message);
        }
        $(this.submitButton).button('reset');
    });
}


/**  Logs  */
function Logs(ID, Modulename, Ref) {
    try {
        //alert(ID + Modulename + Ref);
        var returnUrl = $("#txtreturnUrl").val();
        //alert(returnUrl);
        $.ajax({
            type: "Post",
            url: '/Home/Log',
            dataType: 'json',
            data: "{'ID': " + ID + ", 'Modulename': '" + Modulename + "' , 'Ref':'" + Ref + "' , 'returnUrl' : '" + returnUrl + "' }",
            accept: 'application/json',
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.Success) {

                }
                else {
                    alert(response.Message);
                }
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
        //'@Url.Action("Log","Home")',
    } catch (e) {
        console.log(e);
    }
}
/**  Logs -- End  */


function Email() {
    var email = document.getElementById('email').value;
    var filter = /^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
    if (!filter.test(email)) {
        $('#erremail').text("Please enter a valid email id");
        return false;
    }
}
$('.Only_Numbers').keypress(function (e) {
    var allowedChars = new RegExp("^[0-9\]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (allowedChars.test(str) || e.which == 8 || e.which == 0) {
        return true;
    }
    e.preventDefault();
    return false;
}).keyup(function () {
    var forbiddenChars = new RegExp("[^0-9\]", 'g');
    if (forbiddenChars.test($(this).val())) {
        $(this).val($(this).val().replace(forbiddenChars, ''));
    }
});

$('#school_name').change(function () {
    var school_name = $(this).val();
    if (school_name == '' || school_name == null) {
        $('#errschoolname').text('Please enter School name');
    }
    else {
        $('#errschoolname').text('');
    }
});
$('#contact_person_name').change(function () {
    var contact_person_name = $(this).val();
    if (contact_person_name == '' || contact_person_name == null) {
        $('#errcontactname').text('Please enter Contact person name');
    }
    else {
        $('#errcontactname').text('');
    }
});
$('#email').change(function () {
    var email = $(this).val();
    var filter = /^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
    if (email == '' || email == null) {
        $('#erremail').text('Please enter email Id');
    }
    else if (!filter.test(email)) {
        $('#erremail').text("Please enter a valid email id");
    }
    else {
        $('#erremail').text('');
    }
});
$('#country').change(function () {
    var country = $(this).val();
    if (country == '' || country == null) {
        $('#errcountry').text('Please Select Country');
    }
    else {
        $('#errcountry').text('');
    }
});
$('#state').change(function () {
    var state = $(this).val();
    if (state == '' || state == null) {
        $('#errstate').text('Please Select State');
    }
    else {
        $('#errstate').text('');
    }
});
$('#city').change(function () {
    var city = $(this).val();
    if (city == '' || city == null) {
        $('#errcity').text('Please Select City');
    }
    else {
        $('#errcity').text('');
    }
});

function vallidate() {
    var school_name = $('#school_name').val();
    var contact_person_name = $('#contact_person_name').val();
    var email = $('#email').val();
    var country = $('#country').find('option:selected').val();
    var state = $('#state').find('option:selected').val();
    var city = $('#city').find('option:selected').val();
    var filter = /^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

    var error_count = 0;

    if (school_name == '' || school_name == null) {
        error_count++;
        $('#errschoolname').text('Please enter School name');
    }
    else {
        $('#errschoolname').text('');
    }
    if (contact_person_name == '' || contact_person_name == null) {
        error_count++;
        $('#errcontactname').text('Please enter Contact person name');
    }
    else {
        $('#errcontactname').text('');
    }
    if (email == '' || email == null) {
        error_count++;
        $('#erremail').text('Please enter email Id');
    }
    else if (!filter.test(email)) {
        $('#erremail').text("Please enter a valid email id");
        error_count++;
    }
    else {
        $('#erremail').text('');
    }
    if (country == 0 || country == '' || country == null) {
        error_count++;
        $('#errcountry').text('Please Select Country');
    }
    else {
        $('#errcountry').text('');
    }
    if (state == 0 || state == '' || state == null) {
        error_count++;
        $('#errstate').text('Please Select State');
    }
    else {
        $('#errstate').text('');
    }
    if (city == 0 || city == '' || city == null) {
        error_count++;
        $('#errcity').text('Please Select City');
    }
    else {
        $('#errcity').text('');
    }

    if (error_count == 0) {
        return true;
    }
    else {
        return false;
    }
}

$('.school').change(function () {
    var school = $(this).val();
    if (school == '' || school == null) {
        $(this).next('span').text('Please enter School Name');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.fn').change(function () {
    var firstname = $(this).val();
    if (firstname == '' || firstname == null) {
        $(this).next('span').text('Please enter First Name');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.ln').change(function () {
    var lastname = $(this).val();
    if (lastname == '' || lastname == null) {
        $(this).next('span').text('Please enter Last Name');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.mn').change(function () {
    var mobile = $(this).val();
    var filter = /^[0-9]$/;
    if (mobile == '' || mobile == null) {
        $(this).next('span').text('Please enter Mobile number');
    }
    else if (mobile.length != 10) {
        $(this).next('span').text('Mobile number should be 10 digit long');
    }
    else if (!mobile.match(/^\d+$/)) {
        $(this).next('span').text('Please enter valid mobile number');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.emailid').change(function () {
    var email = $(this).val();
    var filter = /^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
    if (email == '' || email == null) {
        $(this).next('span').text('Please enter Email id');
    }
    else if (!filter.test(email)) {
        $(this).next('span').text('Please enter valid email id');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.onlynumbers').keypress(function (e) {
    var allowedChars = new RegExp("^[0-9\]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (allowedChars.test(str) || e.which == 8 || e.which == 0) {
        return true;
    }
    e.preventDefault();
    return false;
}).keyup(function () {
    var forbiddenChars = new RegExp("[^0-9\]", 'g');
    if (forbiddenChars.test($(this).val())) {
        $(this).val($(this).val().replace(forbiddenChars, ''));
    }
});
$('.onlyAlpha').keypress(function (e) {
    var allowedChars = new RegExp("^[a-zA-Z\ ]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (allowedChars.test(str) || e.which == 8 || e.which == 0) {
        return true;
    }
    e.preventDefault();
    return false;
}).keyup(function () {
    var forbiddenChars = new RegExp("[^a-zA-Z\ ]", 'g');
    if (forbiddenChars.test($(this).val())) {
        $(this).val($(this).val().replace(forbiddenChars, ''));
    }
});

$('.add1').change(function () {
    var address = $(this).val();
    if (address == '' || address == null) {
        $(this).next('span').text('Please enter Address');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.branch').change(function () {
    var Branch = $(this).val();
    if (Branch == '' || Branch == null) {
        $(this).next('span').text('Please enter Branch Name');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.City').change(function () {
    var City = $(this).val();
    if (City == '' || City == null) {
        $(this).next('span').text('Please select City');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.pin').change(function () {
    var Pincode = $(this).val();
    if (Pincode == '' || Pincode == null) {
        $(this).next('span').text('Please enter Pincode');
    }
    else {
        $(this).next('span').text('');
    }
});

$('.username').change(function () {
    var username = $(this).val();
    if (username == '' || username == null) {
        $(this).next('span').text('Please enter Username');
    }
    else {
        $(this).next('span').text('');
    }
});
$('.pass').change(function () {
    var pass = $(this).val();
    if (pass == '' || pass == null) {
        $(this).next('span').text('Please enter Password');
    }
    else {
        $(this).next('span').text('');
    }
});
$('.company').change(function () {
    var company = $(this).val();
    if (company == '' || company == null) {
        $(this).next('span').text('Please enter Company Name');
    }
    else {
        $(this).next('span').text('');
    }
});


function validateschoolaff(btnid) {
    var clicked_tab_id = btnid;
    var InstituteType = $('#' + clicked_tab_id + 'InstituteType').val();
    var InstituteName = $('#' + clicked_tab_id + 'InstituteName').val();
    var FirstName = $('#' + clicked_tab_id + 'FirstName').val();
    var LastName = $('#' + clicked_tab_id + 'LastName').val();
    var MobileNo = $('#' + clicked_tab_id + 'MobileNo').val();
    var AddressLine1 = $('#' + clicked_tab_id + 'AddressLine1').val();
    var AddressLine2 = $('#' + clicked_tab_id + 'AddressLine2').val();
    var AddressLine3 = $('#' + clicked_tab_id + 'AddressLine3').val();
    var EmailID = $('#' + clicked_tab_id + 'EmailID').val();
    var CityName = $('#' + clicked_tab_id + 'City option:selected').val();
    var Pincode = $('#' + clicked_tab_id + 'Pincode').val();
    var UserName = $('#' + clicked_tab_id + 'UserName').val();
    var Password = $('#' + clicked_tab_id + 'Password').val();
    var email_filter = /^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;
    var number_filter = /^[0-9]$/;
    var error_count = 0;
    if (InstituteName == '' || InstituteName == null) {
       
        if (clicked_tab_id == 'sc') {
            $('#' + clicked_tab_id + 'name').text('Please enter School Name');
            error_count++;
        }
    }
    else {
        $('#' + clicked_tab_id + 'name').text('');
    }
    if (FirstName == '' || FirstName == null) {
        error_count++;
        $('#' + clicked_tab_id + 'fname').text('Please enter First Name');
    }
    else {
        $('#' + clicked_tab_id + 'fname').text('');
    }
    if (LastName == '' || LastName == null) {
        error_count++;
        $('#' + clicked_tab_id + 'lname').text('Please enter Last Name');
    }
    else {
        $('#' + clicked_tab_id + 'lname').text('');
    }
    if (MobileNo == '' || MobileNo == null) {
        error_count++;
        $('#' + clicked_tab_id + 'mob').text('Please enter Mobile number');
    }
    else if (!MobileNo.match(/^\d+$/)) {
        error_count++;
        $('#' + clicked_tab_id + 'mob').text('Please enter valid Mobile number');
    }
    else if (MobileNo.length != 10) {
        error_count++;
        $('#' + clicked_tab_id + 'mob').text('Mobile number should be 10 digit long');
    }
    else {
        $('#' + clicked_tab_id + 'mob').text('');
    }
    if (AddressLine1 == '' || AddressLine1 == null) {
        error_count++;
        $('#' + clicked_tab_id + 'add1').text('Please enter Address');
    }
    else {
        $('#' + clicked_tab_id + 'add1').text('');
    }
    if (AddressLine2 == '' || AddressLine2 == null) {
        error_count++;
        $('#' + clicked_tab_id + 'add2').text('Please enter Address');
    }
    else {
        $('#' + clicked_tab_id + 'add2').text('');
    }
    if (AddressLine3 == '' || AddressLine3 == null) {
        error_count++;
        $('#' + clicked_tab_id + 'add3').text('Please enter Branch Name');
    }
    else {
        $('#' + clicked_tab_id + 'add3').text('');
    }

    if (EmailID == '' || EmailID == null) {
        error_count++;
        $('#' + clicked_tab_id + 'email').text('Please enter Email id');
    }
    else if (!email_filter.test(EmailID)) {
        error_count++;
        $('#' + clicked_tab_id + 'email').text('Please enter valid Email id');
    }
    else {
        $('#' + clicked_tab_id + 'email').text('');
    }
    if (CityName == '' || CityName == null) {
        error_count++;
        $('#' + clicked_tab_id + 'city').text('Please select City');
    }
    else {
        $('#' + clicked_tab_id + 'city').text('');
    }
    if (UserName == '' || UserName == null) {
        error_count++;
        $('#' + clicked_tab_id + 'uname').text('Please enter user name');
    }
    else {
        $('#' + clicked_tab_id + 'uname').text('');
    }
    if (Pincode == '' || Pincode == null) {
        error_count++;
        $('#' + clicked_tab_id + 'pin').text('Please enter Pincode');
    }
    else {
        $('#' + clicked_tab_id + 'pin').text('');
    }
    if (Password == '' || Password == null) {
        error_count++;
        $('#' + clicked_tab_id + 'pass').text('Please enter Password');
    }
    else {
        $('#' + clicked_tab_id + 'pass').text('');
    }
    if(error_count==0)
    {
        return true;
    }
    else if(error_count>0)
    {
        return false;
    }
}



function validateUserName(User_name) {
    var userName = User_name;
    $.ajax({
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { 'userName': userName },
        url: '/Account/IsUserExist',
        success: function (data) {
            console.log(data);
            if (data.Success == true) {
                return true;
            } else {
               
                return false;
            }
        }
    });
}
$('.img').change(function () {
    var image = $(this).val();
    if(image!='' && image!=null)
    {
        var ext = image.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
           $('#errprofile').text('Please select valid image file');
            $(this).val('');
        }
        else {
            readURL(this);
        }
    }
    else {
        $('#imgprev').attr('src', $('#hiddenprof').val());
        $('#errprofile').text('');
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imgprev').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
    else {
        $('#imgprev').attr('src', $('#hiddenprof').val());
        $('#errprofile').text('');
    }
}

function validateProfile()
{
    var error_count = 0;
    var first_name = $('#firstnamepro').val();
    var last_name = $('#lastname').val();
    var mobile_number = $('#mobileno').val();
    var Email = $('#emailid').val();
    var city = $('#citydisplay option:selected').val();
    var emailPattern=/^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/;

    if (first_name == '' || first_name == null)
    {
        error_count++;
        $('#errfirstname').text('Please enter First Name');
    }
    else {
        $('#errfirstname').text('');
    }
    if (last_name == '' || last_name == null) {
        error_count++;
        $('#errlastname').text('Please enter Last Name');
    }
    else {
        $('#errlastname').text('');
    }
    if (Email == '' || Email == null) {
        error_count++;
        $('#erremailid').text('Please enter EmailID');
    }
    else if (!emailPattern.test(Email)) {
        error_count++;
        $('#erremailid').text('Please enter valid EmailID');
    }
    else {
        $('#erremailid').text('');
    }
    if (mobile_number == '' || mobile_number == null) {
        error_count++;
        $('#errmob').text('Please enter Mobile number');
    }
    else {
        $('#errmob').text('');
    }
    if (city == '' || city == null || city==0) {
        error_count++;
        $('#errcitydisplay').text('Please select City');
    }
    else {
        $('#errcitydisplay').text('');
    }

    if (error_count == 0)
    {
        return true;
    }
    return false;
}

$('.userphotodetails').change(function () {
    var image = $(this).val();
    if (image != '' && image != null) {
        var ext = image.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('Please select valid image file');
            $(this).val('');
        }
        else {
            readURLinfo(this);
        }
    }
    else {
        $(this).parent('span').parent('div').parent('div').parent('div').prev('img').attr('src',$(this).next('input').val());
    }
});


function readURLinfo(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(input).parent('span').parent('div').parent('div').parent('div').prev('img').attr('src', e.target.result);
        //    $('#imgprev').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
    else {
        $(this).parent('span').parent('div').parent('div').parent('div').prev('img').attr('src', $(this).next('input').val());
    }
}

function validatephoto()
{
    var error_count = 0;
    var Hidden_pan_card = $('#hidden_pan_card').val();
    var pan_card = $('#pancard').val();
    var Hidden_addhar_card = $('#hidden_addhar').val();
    var addhar_card = $('#addhar_card').val();
    if ((Hidden_addhar_card == '' || Hidden_addhar_card == null) && (addhar_card == '' || addhar_card == null))
    {
        error_count++;
        alert('All fields are mandatory');
    }
    else if ((Hidden_pan_card == '' || Hidden_pan_card == null) && (pan_card == '' || pan_card == null)) {
        error_count++;
        alert('All fields are mandatory');
    }

    if (error_count == 0)
    {
        return true;
    }
    return false;
}

$('#Bank_name').change(function () {
    var bank_name = $(this).val();
    if(bank_name=='' || bank_name==null)
    {
        $(this).next('span').text('Please enter Bank Name');
    }
    else {
        $(this).next('span').text('');
    }
});
$('#Account_number').change(function () {
    var Account_number = $(this).val();
    if (Account_number == '' || Account_number == null) {
        $(this).next('span').text('Please enter Bank Account number');
    }
    else {
        $(this).next('span').text('');
    }
});
$('#ifsccode').change(function () {
    var IFSC_code = $(this).val();
    if (IFSC_code == '' || IFSC_code == null) {
        $(this).next('span').text('Please enter IFSC code');
    }
    else {
        $(this).next('span').text('');
    }
});

function validatebankdetails()
{
    var error_count = 0;
    var bank_name = $('#Bank_name').val();
    var Account_number = $('#Account_number').val();
    var IFSC_code = $('#ifsccode').val();
    if (bank_name == '' || bank_name == null) {
        error_count++;
        $('#Bank_name').next('span').text('Please enter Bank Name');
    }
    else {
        $('#Bank_name').next('span').text('');
    }
    if (Account_number == '' || Account_number == null) {
        error_count++;
        $('#Account_number').next('span').text('Please enter Bank Account number');
    }
    else {
        $('#Account_number').next('span').text('');
    }
    if (IFSC_code == '' || IFSC_code == null) {
        error_count++;
        $('#ifsccode').next('span').text('Please enter IFSC code');
    }
    else {
        $('#ifsccode').next('span').text('');
    }
    if (error_count == 0)
    {
        return true;
    }
    return false;
}