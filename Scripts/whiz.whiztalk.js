try {
    var uploadPercentage = 1;
    var uploadCont = true;

    function gowhiztalkSave() {
        if (!IsValidInputs()) {
            return false;
        }
        $("#dviprocession").removeClass("hidden");
        $("#divForm").addClass("hidden");
        $("#divForm1").addClass("hidden");
        $("#spanupload").html(uploadPercentage);
        $("#divupload").css("width", uploadPercentage + "%");
        $("#divupload").attr("aria-valuenow", uploadPercentage + "%");
        youtubeupload();
        window.setInterval(function () { updateUploading() }, 5000);
    }

    function goload() {
        $("#dviprocession").addClass("hidden");
        $("#divForm").removeClass(hidden);
        $("#divForm1").removeClass(hidden);
        $("#btnSave").removeClass(hidden);
    }

    function updateUploading() {
        if (uploadCont) {
            if (uploadPercentage <= 20) {
                uploadPercentage = uploadPercentage + 1;
            }
            else if (uploadPercentage >= 21 && uploadPercentage <= 40) {
                uploadPercentage = uploadPercentage + 0.5; GetUploadProgressStatus();
                if (uploadPercentage == 25) { WC_Waiting = true; }

            }
            else if (uploadPercentage >= 41 && uploadPercentage <= 75) {
                uploadPercentage = uploadPercentage + 0.5; GetUploadProgressStatus();
                if (uploadPercentage == 45) { WC_Waiting = true; }
                if (uploadPercentage == 70) { WC_Waiting = true; }
            }
            else if (uploadPercentage >= 100) {
                uploadPercentage = 95; WC_Waiting = true; GetUploadProgressStatus();
            }
            else { uploadPercentage = uploadPercentage + 0.1; GetUploadProgressStatus(); }
            $("#spanupload").html(uploadPercentage);
            $("#divupload").css("width", uploadPercentage + "%");
            $("#divupload").attr("aria-valuenow", uploadPercentage + "%");
        }
    }
} catch (e) { alert(e); }

function IsValidInputs() {
    var result = true;
    var errorcount = 0;
    if ($("#txtFileName").val() == null || $("#txtFileName").val() == "") {
        $('#txtFileName_err').text('Please enter file name');
        errorcount++;
    }
    else {
        $('#txtFileName_err').text('');
    }

    if ($("#txtDescription").val() == null || $("#txtDescription").val() == "") {
        $('#txtDescription_err').text('Please enter file description');
        errorcount++;
    }
    else {
        $('#txtDescription_err').text('');
    }
    var arrayextension = ['mp4', 'avi', 'mpg', 'mpeg', 'm4v', '3gp', 'flv'];
    if ($("#file-1").val() == null || $("#file-1").val() == "") {
        $('#video_err').text('Please select video file');
        errorcount++;
    }
    else {
        $('#video_err').text('');
    }
    //Document Check Format
    var docsarrayextension = ['docx', 'doc', 'pdf', 'xlsx', 'pdf', 'pptx', 'ppt'];
    if ($("#WhizTalkfile").val() != null || $("#WhizTalkfile").val() != "") {
        if ($("#WhizTalkfile").contents.length > 0) {
            var sFileName = $("#WhizTalkfile").val();
            if (sFileName != null && sFileName != "") {
                var blnValid = false;
                for (var j = 0; j < docsarrayextension.length; j++) {
                    var sCurExtension = docsarrayextension[j];
                    if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                        blnValid = true;
                        break;
                    }
                }
            }
            else {
                blnValid = true;
            }
            if (!blnValid) {
                errorcount++;
                $('#WhizTalkimage-error').text('Invalid File Format');
                return false;
            } else {                
                $('#WhizTalkimage-error').text('');
            }
        }
    }

    if (errorcount == 0) {
        result = true;
    }
    else {
        result = false
    }
    return result;
}

function youtubeupload() {
    var topicForm = $("#WhizActivity");
    $(topicForm).validate();
    if ($(topicForm).valid()) {
        $(topicForm).ajaxSubmit(function (data) {
            var response = jQuery.parseJSON(data);
            if (response.Success) {
            }
            else {
            }
        });
    }
}

var WC_Status = "";
var WC_Percentage = "";
var WC_Video_Id = "";
var WC_Waiting = true;

function GetUploadProgressStatus() {
    if (WC_Waiting) {
        WC_Waiting = false;
        $.ajax({
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: { 'MobileNo': 'mobileno' },
            url: '/Student/GetUploadProgressStatus',
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                location.reload();
            },
            success: function (data) {
                if (data.Success == true) {
                    uploadPercentage = 99;
                    updateUploading();
                    uploadCont = false;
                    WC_Waiting = false;
                    location.reload();
                } else {
                    WC_Status = data.Message;
                    if (WC_Status != "") {
                        var r = confirm(WC_Status);
                        if (r == true) {
                            location.reload(); WC_Waiting = false;
                        } else {

                        }
                    }
                    else {
                        WC_Waiting = true;
                    }
                }
            }
        });
    }
}

$("#file-1").change(function () {
    var fileInput = $('#file-1');
    var type_reg = /^video\/(mp4|avi|mpg|mpeg|m4v|3gp|flv)$/;
    var type = fileInput.get(0).files[0].type;
    if (!type_reg.test(type)) {
        $('#video_err').text('Please select a valid video (.mp4/.avi/.mpg/.mpeg/.m4v/.3gp/.flv)');
        this.value = null;
    }
    else {
        $('#video_err').text('');
    }
});

function getYoutubeStatistics(url) {
    var getUrl = "https://www.googleapis.com/youtube/v3/videos?part=statistics&id=" + url + "&key=AIzaSyARU2KHo4iBTyBKLhVYQIUneUA02PS-uV8";
    $.get(getUrl, function (data) {
        if (data != null) {
            if (data.items != null) {
                var count = data.items[0].statistics;
                $("#sp_LikeCount").html("");
                $("#sp_ViewCount").html("");
                $("#sp_CommentsCount").html("");
                $("#sp_Coins").html("");
                $("#sp_LikeCount").html(count.likeCount);
                $("#sp_ViewCount").html(count.viewCount);
                $("#sp_CommentsCount").html(count.commentCount);
                var coins = parseInt(count.likeCount * 10);
                $("#sp_Coins").html(coins);
            }
        }
    });
}