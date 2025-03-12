var slideIndex = 0;
var studenttype = "topper";
$('#CompetitionStudentVideo').on('hidden.bs.modal', function () {
    $("#Competition_ifrm_Video").attr("src", "");
});
//glide1.mount();
function GetCoinsLog(StudentID, ID, StudentName) {
    $("#Dv_StudentName_Coins").html("" + StudentName + " Coins's Log");
    if (StudentID > 0 && ID > 0) {
        var param = { 'StudentID': StudentID, 'ID': ID };
        try {
            $.ajax({
                type: "Get",
                url: '/Student/GetCoinsLog',
                dataType: 'json',
                data: param,
                success: function (data) {
                    if (data.length > 0) {
                        $("#dv_coins_list").empty();
                        var coins = 0;
                        if (data != null && data.length > 0) {
                            for (i = 0; i < data.length; i++) {
                                var temp = "<tr><td title='{CoinID}'>{Count}</td><td>{CoinFor}</td><td>{CreatedOn}</td><td>{Coins}</td></tr>";
                                var item = data[i];
                                temp = temp.replace("{Count}", i + 1);
                                temp = temp.replace("{CoinID}", item.CoinID);
                                temp = temp.replace("{CoinFor}", item.CoinFor);
                                temp = temp.replace("{Coins}", item.Coins);
                                coins = coins + item.Coins;
                                temp = temp.replace("{CreatedOn}", ConvertedDate(item.CreatedOn));
                                $("#dv_coins_list").append(temp);
                            }
                            $("#dv_coins_list").append("<tr><td colspan='3' class='text-center'>TotalCoins</td><td>" + coins + "</td></tr>");
                        }
                    } else {
                        $("#dv_coins_list").append("<tr><td>No Data Available to Show</td></tr>");
                    }
                    $('#Dv_Gems_modal').modal('show');
                }
            });
        } catch (e) {
            alert(e.message);
        }
    }
}
function ConvertedDate(date) {
    var dateString = date.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = day + "/" + month + "/" + year;
    return date;
}

function viewWinner() {
    studenttype = "winner";
    $('#toggle_Winners').addClass('hidden');
    $('#toggle_Toppers').removeClass('hidden');
    GetStudentData(0, studenttype);
}
function viewTopper() {
    studenttype = "topper";
    $('#toggle_Toppers').addClass('hidden');
    $('#toggle_Winners').removeClass('hidden');
    GetStudentData(0, studenttype);

}
GetStudentData(0, studenttype);
function GetStudentData(id, type) {
    type = studenttype;
    $("#Dv_studentLoader_" + id).removeClass("hidden");
    $("#div_studentgroup_" + id).empty();
    $("#lblStudent").text("");
    if (studenttype == "topper") {
        $("#lbl_student_type_" + id).html("Toppers");
        $("#lbl_date_winners").addClass("hidden");
        $("#lbl_date_toppers").removeClass("hidden");
    } else {
        $("#lbl_student_type_" + id).html("Winners");
        $("#lbl_date_toppers").addClass("hidden");
        $("#lbl_date_winners").removeClass("hidden");
    }
    switch (parseInt(id)) {
        case 0: $("#lblStudent").text("All");
            break;
        case 1: $("#lblStudent").text("Group A");
            break;
        case 2: $("#lblStudent").text("Group B");
            break;
        case 3: $("#lblStudent").text("Group C");
            break;
        case 4: $("#lblStudent").text("Group D");
            break;
        case 4: $("#lblStudent").text("Group E");
            break;
        default:
    }
    $.ajax({
        type: "Get",
        url: '/Event/GetStudentData',
        data: { id: id, type: type },
        dataType: 'json',
        accept: 'application/json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.Success) {
                $("#div_studentgroup_" + id).empty();
                $("#Dv_studentLoader_" + id).addClass("hidden");
                if (response.Data != null) {
                    for (i = 0; i < 3; i++) {
                        var temp = $("#Dv_Student_Winner").html();
                        var item = response.Data[i];
                        if (item != null) {
                            temp = temp.replace("{ID}", 8);
                            temp = temp.replace("{UserID}", item.UserID);
                            temp = temp.replace("{StudentID}", item.UserID);
                            temp = temp.replace("{FirstName}", item.FirstName);
                            temp = temp.replace("{LastName}", item.LastName);
                            temp = temp.replace("{StudentName}", item.FirstName + " " + item.LastName);
                            temp = temp.replace("{CityName}", item.CityName);
                            temp = temp.replace("{Certificates}", item.Certificates);
                            temp = temp.replace("{PhotoUrl}", item.PhotoUrl);
                            temp = temp.replace("{GoogleRank}", item.GoogleRank);
                            temp = temp.replace("{GoogleCoins}", item.GoogleCoins);
                            temp = temp.replace("{Standard}", item.Standard);
                            temp = temp.replace("{StudentLevel}", item.StudentLevel);
                            temp = temp.replace("{InstituteName}", item.InstituteName);
                            $("#div_studentgroup_" + id).append(temp);
                        }
                    }
                } else {
                    studenttype = "winner";
                    GetStudentData(id, studenttype)
                }
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
}