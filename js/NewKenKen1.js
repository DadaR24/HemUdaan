/*Bind the data from DB*/
var DataMaster = [];
var DataTarget = [];
var DataCage = [];
var DataSolution = [];

/*Set the data from DB*/
var SetDataMaster = [];
var SetDataTarget = [];
var SetDataCage = [];
var SetDataSolution = [];
var NP = 0;
var GlobalKMID;
var CourseID;

$(document).ready(function () {

    var GetCID = $("#CourseID").val();
    document.onkeydown = function (e) {
        return false;
    }


    $.ajax({
        url: '/GameCompetition/GetKenKenDetails',
        type: "POST",
        data: { CID: GetCID },
        success: function (response) {

            if (response[3].length >= !3) {


                if (response[4] == "OK") {

                    DataMaster.push(response[0]);
                    DataTarget.push(JSON.parse(response[2]));
                    DataCage.push(response[3]);
                    DataSolution.push(response[1]);

                    var arr = JSON.parse(DataMaster);
                    arr.forEach(function (elementObject) {
                        var keys = Object.keys(elementObject);
                        keys.forEach(function (key) {

                            if (key == "KMID") {
                                var xp = elementObject[key];
                                CheckSetTKenKen(xp);
                            }
                        })
                    });
                    const KenMFirst = SetDataMaster.find(element => element);
                    const KemTargetFirst = SetDataTarget.find(element => element);
                    const KemCagetFirst = SetDataCage.find(element => element);

                    generateElement(KenMFirst, KemTargetFirst, KemCagetFirst);
                }
            }
            else {
                swal("Congrats!", "You have Completed all levels.......", "success");
                setTimeout(function () {
                    location.url = '@Url.Action("Profile","Student")';
                }, 5000);
            }
        },
        error: function (response) {
            alert('Something went wrong');
        }
    });
});

function prevItem() {

    if (NP === 0) { // i would become 0
        NP = SetDataMaster.length;
        NP = SetDataTarget.length; // so put it at the other end of the array
        NP = SetDataCage.length;
    }
    NP = NP - 1; // decrease by one
    if (NP == 0) {
        $('#btn_Prev').attr('disabled', 'disabled');
        $('#btn_Next').removeAttr('disabled', 'disabled');
    }
    else {
        $('#btn_Prev').removeAttr('disabled', 'disabled');
    }

    return [SetDataMaster[NP], SetDataTarget[NP], SetDataCage[NP]]; // give us back the item of where we are now
}

function btn_Next() {
    debugger;
    for (var k = 1; k <= 9; k++) {
        document.getElementById('txt3' + k).value = "";
    }
    let [a, b, c] = nextItem();
    generateElement(a, b, c);
}
function nextItem() {
    debugger;
    NP = NP + 1; // increase i by one
    NP = NP % SetDataMaster.length;
    NP = NP % SetDataTarget.length;
    NP = NP % SetDataCage.length;

    // Check if we've gone too high, start from `0` again
    if (NP == 0) {
        $('#btn_Next').attr('disabled', 'disabled');
        $('#btn_Prev').removeAttr('disabled', 'disabled');
    } else {
        $('#btn_Next').removeAttr('disabled', 'disabled');
    }

    // Check for empty input when NP is 0
    if (NP == 0) {
        var txtAns = document.getElementById('txt_User_Input').value;
        if (txtAns == "") {
            debugger;
            emptySave();
        }
    }

    return [SetDataMaster[NP], SetDataTarget[NP], SetDataCage[NP]]; // give us back the item of where we are now
}


function btn_Prev() {

    for (var k = 1; k <= 9; k++) {
        document.getElementById('txt3' + k).value = "";
    }
    let [a1, b2, c3] = prevItem();
    generateElement(a1, b2, c3);
}
function CheckSetTKenKen(KMID) {

    var Retrive1 = JSON.parse(DataMaster[0]).filter((x) => x.KMID == KMID);
    SetDataMaster.push(Retrive1);

    var Retrive2 = DataTarget[0].filter((x) => x.KMID == KMID);
    SetDataTarget.push(Retrive2);

    var Retrive3 = JSON.parse(DataCage[0]).filter((x) => x.KMID == KMID);
    SetDataCage.push(Retrive3);
}


function ClearData() {
    document.getElementById('Id').value = "";
    document.getElementById('Size').value = "";
    document.getElementById('Operation').value = "";
    document.getElementById('Difficulty').value = "";
    document.getElementById('IsActive').value = "";
    document.getElementById('Title').value = "";
}

function hasClass(el, className) {
    if (el.classList)
        return el.classList.contains(className);
    return !!el.className.match(new RegExp('(\\s|^)' + className + '(\\s|$)'));
}

function addClass(el, className) {
    if (el.classList)
        el.classList.add(className)
    else if (!hasClass(el, className))
        el.className += " " + className;
}

function removeClass(el, className) {
    if (el.classList)
        el.classList.remove(className)
    else if (hasClass(el, className)) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        el.className = el.className.replace(reg, ' ');
    }
}

function createTextBox(size) {

    txtArray = [];
    var val = parseInt(size);
    switch (val) {
        case 3:
            changeBoxSize("sizeClassthree");
            break;
        case 4:
            changeBoxSize("sizeClassfour");
            break;
        case 5:
            changeBoxSize("sizeClassfive");
            break;
        default:
            changeBoxSize("sizeClassthree");
    }

    for (i = 1; i <= size; i++) {
        for (var j = 1; j <= size; j++) {
            var txtIds = {
                targetid: 'T_R' + i + '_C' + j,
                //solutionid: 'S_R' + i + '_C' + j,
                //cageid: 'C_R' + i + '_C' + j,
            }
            txtArray.push(txtIds);
        }
    }
}
function changeBoxSize(className) {

    var class1 = document.getElementById("TargetClass").className.match('sizeClass').input;
    removeClass(document.getElementById("TargetClass"), class1);
    addClass(document.getElementById("TargetClass"), className);

    //var class2 = document.getElementById("CageClass").className.match('sizeClass').input;
    //removeClass(document.getElementById("CageClass"), class2);
    //addClass(document.getElementById("CageClass"), className);

    //var class3 = document.getElementById("SolutionClass").className.match('sizeClass').input;
    //removeClass(document.getElementById("SolutionClass"), class3);
    //addClass(document.getElementById("SolutionClass"), className);
}

function fillLineArray() {

    lineArray = [];
    if (size == 3) {
        var tlineType = 'target';
        var tLineDetail1 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt31").val(),
            Column2: $("#txt32").val(),
            Column3: $("#txt33").val(),
        }
        lineArray.push(tLineDetail1);
        var tlineType = 'target';
        var tLineDetail2 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt34").val(),
            Column2: $("#txt35").val(),
            Column3: $("#txt36").val(),
        }
        lineArray.push(tLineDetail2);
        var tLineDetail3 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt37").val(),
            Column2: $("#txt38").val(),
            Column3: $("#txt39").val(),
        }
        lineArray.push(tLineDetail3);

    }
    else if (size == 4) {
        var tlineType = 'target';
        var tLineDetail1 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt41").val(),
            Column2: $("#txt42").val(),
            Column3: $("#txt43").val(),
            Column4: $("#txt44").val(),
        }
        lineArray.push(tLineDetail1);
        var tlineType = 'target';
        var tLineDetail2 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt45").val(),
            Column2: $("#txt46").val(),
            Column3: $("#txt47").val(),
            Column4: $("#txt48").val(),
        }
        lineArray.push(tLineDetail2);
        var tLineDetail3 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt49").val(),
            Column2: $("#txt410").val(),
            Column3: $("#txt411").val(),
            Column4: $("#txt412").val(),
        }
        lineArray.push(tLineDetail3);
        var tLineDetail4 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt413").val(),
            Column2: $("#txt414").val(),
            Column3: $("#txt415").val(),
            Column4: $("#txt416").val(),

        }
        lineArray.push(tLineDetail4);
    }
    else if (size == 5) {
        var tlineType = 'target';
        var tLineDetail1 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt51").val(),
            Column2: $("#txt52").val(),
            Column3: $("#txt53").val(),
            Column4: $("#txt54").val(),
            Column5: $("#txt55").val(),
        }
        lineArray.push(tLineDetail1);
        var tlineType = 'target';
        var tLineDetail2 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt56").val(),
            Column2: $("#txt57").val(),
            Column3: $("#txt58").val(),
            Column4: $("#txt59").val(),
            Column5: $("#txt510").val(),
        }
        lineArray.push(tLineDetail2);
        var tLineDetail3 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt511").val(),
            Column2: $("#txt512").val(),
            Column3: $("#txt513").val(),
            Column4: $("#txt514").val(),
            Column5: $("#txt515").val(),
        }
        lineArray.push(tLineDetail3);
        var tLineDetail4 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt511").val(),
            Column2: $("#txt512").val(),
            Column3: $("#txt513").val(),
            Column4: $("#txt514").val(),
            Column5: $("#txt515").val(),
        }
        lineArray.push(tLineDetail4);

        var tLineDetail5 = {
            //RowNo: tr1.charAt(tr1.length - 1),
            LineType: tlineType,
            Column1: $("#txt516").val(),
            Column2: $("#txt517").val(),
            Column3: $("#txt518").val(),
            Column4: $("#txt519").val(),
            Column5: $("#txt520").val(),
        }
        lineArray.push(tLineDetail5);
    }
}

function generateElement(KenMaster, TargetData, CageData) {

    if (KenMaster != null && TargetData != null && CageData != null) {
        SetDataSolution.length = 0;
        var Retrive4 = JSON.parse(DataSolution[0]).filter((x) => x.KMID == KenMaster[0]["KMID"]);
        SetDataSolution.push(Retrive4);

        var boxsize = KenMaster[0]["Size"];

        GlobalKMID = 0; CourseID = 0;
        GlobalKMID = KenMaster[0]["KMID"];
        CourseID = KenMaster[0]["CourseID"];

        if (boxsize != null && boxsize != "") {
            size = boxsize;
            if (boxsize == '3') {
                $('#three').removeClass("hidden");
                $('#four').addClass("hidden");
                $('#five').addClass("hidden");
            }
            else if (boxsize == '4') {
                $('#three').addClass("hidden");
                $('#four').removeClass("hidden");
                $('#five').addClass("hidden");
            }
            else if (boxsize == '5') {
                $('#three').addClass("hidden");
                $('#four').addClass("hidden");
                $('#five').removeClass("hidden");
            }
            else {

            }
        } else {
            size = document.getElementById("Size").value;
        }
        document.getElementById("Target").innerHTML = "";
        //document.getElementById("Cage").innerHTML = "";
        //document.getElementById("Solution").innerHTML = "";
        KenKenHeader.Size = size;
        createTextBox(size);
        var targethtml = "";
        //var cagehtml = "";
        //var solutionhtml = "";
        var targetString = '<div class="col-md-3" style="width: 70px;margin-bottom: 5px;" ><label for="floatingInputInvalid">{Invalid}</label><input type="text" id ="{targetid}" name="{targetid}"  maxlength="2" style="width: 66px;border-color:black;height: 66px; {ColorPK}" class="form-control input-sm" ></div>';

        DataTarget = [];

        for (var i = 0; i < TargetData.length; i++) {

            var arrTarget = TargetData[i];
            for (let key in arrTarget) {

                if (key != "KTID" && key != "KMID") {

                    var E1 = arrTarget[key];
                    if (E1 != 0) {
                        DataTarget.push(E1);
                    }
                }
            }
        }
        var count = 1;
        for (var i = 0; i < DataTarget.length; i++) {
            document.getElementById("lbl" + boxsize + count).innerHTML = DataTarget[i];
            count++;
        }
        DataCage = [];

        for (var i = 0; i < CageData.length; i++) {

            var arrCage = CageData[i];
            for (let key in arrCage) {

                if (key != "KCID" && key != "KTID" && key != "KMID") {
                    var E1 = arrCage[key];
                    if (E1 != 0) {
                        DataCage.push(E1);
                    }
                }
            }
        }

        txtArray.forEach(function (item, i) {

            var KP = DataTarget[i];
            var CG = DataCage[i];
            var CT = 1;
            var ColorPK;
            for (var i = 0; i < DataCage.length; i++) {
                var v = document.getElementById("txt" + boxsize + CT);
                if (DataCage[i] == "A") {
                    ColorPK = "background-color:#daeef3";
                    v.style.backgroundColor = '#daeef3';
                    CT++;
                }
                if (DataCage[i] == 'B') {
                    ColorPK = "background-color:#d6e3bc";
                    v.style.backgroundColor = '#d6e3bc';
                    CT++;
                }
                if (DataCage[i] == 'C') {
                    ColorPK = "background-color:#fde9d9";
                    v.style.backgroundColor = '#fde9d9';
                    CT++;
                }
                if (DataCage[i] == 'D') {
                    ColorPK = "background-color:#f2dbdb";
                    v.style.backgroundColor = '#f2dbdb';
                    CT++;
                }
                if (DataCage[i] == 'E') {
                    ColorPK = "background-color:#FFFFE0";
                    v.style.backgroundColor = '#FFFFE0';
                    CT++;
                }
                if (DataCage[i] == 'F') {
                    ColorPK = "background-color:#ffe0b3";
                    v.style.backgroundColor = '#ffe0b3';
                    CT++;
                }
                if (DataCage[i] == 'G') {
                    ColorPK = "background-color:#ffcce6";
                    v.style.backgroundColor = '#ffcce6';
                    CT++;
                }
                if (DataCage[i] == 'H') {
                    ColorPK = "background-color:#ccccff";
                    v.style.backgroundColor = '#ccccff';
                    CT++;
                }
                if (DataCage[i] == 'I') {
                    ColorPK = "background-color:#b3ffcc";
                    v.style.backgroundColor = '#efb8b8"';
                    CT++;
                }
                if (DataCage[i] == 'J') {
                    ColorPK = "background-color:#ffa64d";
                    v.style.backgroundColor = '#ffa64d';
                    CT++;
                }
                if (DataCage[i] == 'K') {
                    ColorPK = "background-color:#ff9999";
                    v.style.backgroundColor = '#ff9999';
                    CT++;
                }
                if (DataCage[i] == 'L') {
                    ColorPK = "background-color:#ecffb3";
                    v.style.backgroundColor = '#ecffb3';
                    CT++;
                }
                if (DataCage[i] == 'M') {
                    ColorPK = "background-color:#ecffb3";
                    v.style.backgroundColor = '#FFDAB9';
                    CT++;
                }
            }


        });

        document.getElementById("Target").innerHTML = targethtml;

    }
    else {
        $("#dv_show_coins").removeClass("hidden");
        $("#dv_mcq_box").addClass("hidden");
    }

}

function SaveDataRecord() {

    fillLineArray();
    var newlst = [];
    var IsChecked = false;
    for (var i = 0; i < lineArray.length; i++) {
        if (size == 3) {

            var C1 = lineArray[i]['Column1'];
            var C2 = lineArray[i]['Column2']
            var C3 = lineArray[i]['Column3']

            var Sol1 = SetDataSolution[0][i]['S1'];
            var Sol2 = SetDataSolution[0][i]['S2'];
            var Sol3 = SetDataSolution[0][i]['S3'];

            if (parseInt(C1) != parseInt(Sol1) || parseInt(C2) != parseInt(Sol2) || parseInt(C3) != parseInt(Sol3)) {
                IsChecked = false;
                break;
            }
            else {
                IsChecked = true;

                var row = {
                    RowNo: lineArray[i].RowNo,
                    Column1: C1,
                    Column2: C2,
                    Column3: C3,
                    Column4: C4,
                    Column5: C5,
                }
                newlst.push(row);
            }
        }
        if (size == 4) {

            var C1 = lineArray[i]['Column1'];
            var C2 = lineArray[i]['Column2']
            var C3 = lineArray[i]['Column3']
            var C4 = lineArray[i]['Column4']

            var Sol1 = SetDataSolution[0][i]['S1'];
            var Sol2 = SetDataSolution[0][i]['S2'];
            var Sol3 = SetDataSolution[0][i]['S3'];
            var Sol4 = SetDataSolution[0][i]['S4'];

            if (parseInt(C1) != parseInt(Sol1) || parseInt(C2) != parseInt(Sol2) || parseInt(C3) != parseInt(Sol3) || parseInt(C4) != parseInt(Sol4)) {
                IsChecked = false;
                break;
            }
            else {
                IsChecked = true;

                var row = {
                    RowNo: lineArray[i].RowNo,
                    Column1: C1,
                    Column2: C2,
                    Column3: C3,
                    Column4: C4,
                    Column5: C5,
                }
                newlst.push(row);
            }
        }
        if (size == 5) {

            var Sol1 = SetDataSolution[0][i]['S1'];
            var Sol2 = SetDataSolution[0][i]['S2'];
            var Sol3 = SetDataSolution[0][i]['S3'];
            var Sol4 = SetDataSolution[0][i]['S4'];
            var Sol5 = SetDataSolution[0][i]['S5'];

            var C1 = lineArray[i]['Column1'];
            var C2 = lineArray[i]['Column2'];
            var C3 = lineArray[i]['Column3'];

            var C4 = lineArray[i]['Column4'];
            var C5 = lineArray[i]['Column4'];


            if (parseInt(C1) != parseInt(Sol1) || parseInt(C2) != parseInt(Sol2) || parseInt(C3) != parseInt(Sol3) || parseInt(C4) != parseInt(Sol4) || parseInt(C5) != parseInt(Sol5)) {
                IsChecked = false;
                break;
            }
            else {
                IsChecked = true;

                var row = {
                    RowNo: lineArray[i].RowNo,
                    Column1: C1,
                    Column2: C2,
                    Column3: C3,
                    Column4: C4,
                    Column5: C5,
                }
                newlst.push(row);
            }
        }
    }

    if (IsChecked != false) {
        Swal.fire("Congrats!", "Valid Solution!!! Please wait while its saving record....", "success");
        setTimeout(function () {
            SaveData(); 
        }, 5000);
        SaveData();
    }
    else {
        Swal.fire("Oops!", "Invalid Soultion..", "error");
        setTimeout(function () {
            reset();
        }, 5000);
    }
};
function reset() {
    location.reload();
}

function SaveData() {
    debugger;
    d = new Date();

    var ST = d;

    const SubTm = new Date();

    var IN_H = new Date(ST).getHours();
    var IN_M = new Date(ST).getMinutes();
    var IN_S = new Date(ST).getSeconds();

    var OUT_H = new Date(SubTm).getHours();
    var OUT_M = new Date(SubTm).getMinutes();
    var OUT_S = new Date(SubTm).getSeconds();

    var GetF_H = parseInt(OUT_H) - parseInt(IN_H);
    var GetF_M = parseInt(OUT_M) - parseInt(IN_M);
    var GetF_S = parseInt(OUT_S) - parseInt(IN_S);

    var HrsDiff = Math.floor(GetF_H * 60 * 60 + GetF_M * 60 + GetF_S);
    var SecDiff = Math.floor(GetF_M * 60 + GetF_S);
    var DIFF = GetF_H + " Hrs" + " : " + GetF_M + " Mins" + " : " + GetF_S + " Secs";
    // var KemMasterID = KMID;
    $.ajax({
        url: '/GameCompetition/SaveKenKenData',
        type: "POST",
        data: { KCMID: GlobalKMID, CourseID: CourseID, PractID: GetPractID, startTime: ST, endTime: SubTm, Timediff: HrsDiff, UserAnswer: 1 },
        success: function (data) {

            if (data[0] == "OK") {
                $('#ModalCongrats').modal('show');
                btn_Next();
            }
            if (data[0] == "RNOTOk") {
                $('#ModalCongrats').modal('show');
                setTimeout(function () {
                    window.location.href = "/Home/GameCompetition";
                }, 2000);
            }
        }
    });
}

function emptySave() {
    debugger;
    d = new Date();

    var ST = d;

    const SubTm = new Date();

    var IN_H = new Date(ST).getHours();
    var IN_M = new Date(ST).getMinutes();
    var IN_S = new Date(ST).getSeconds();

    var OUT_H = new Date(SubTm).getHours();
    var OUT_M = new Date(SubTm).getMinutes();
    var OUT_S = new Date(SubTm).getSeconds();

    var GetF_H = parseInt(OUT_H) - parseInt(IN_H);
    var GetF_M = parseInt(OUT_M) - parseInt(IN_M);
    var GetF_S = parseInt(OUT_S) - parseInt(IN_S);

    var HrsDiff = Math.floor(GetF_H * 60 * 60 + GetF_M * 60 + GetF_S);
    var SecDiff = Math.floor(GetF_M * 60 + GetF_S);
    var DIFF = GetF_H + " Hrs" + " : " + GetF_M + " Mins" + " : " + GetF_S + " Secs";
    // var KemMasterID = KMID;
    $.ajax({
        url: '/GameCompetition/SaveKenKenData',
        type: "POST",
        data: { KCMID: GlobalKMID, CourseID: CourseID, PractID: GetPractID, startTime: ST, endTime: SubTm, Timediff: HrsDiff, UserAnswer: 0 },
        success: function (data) {

            if (data[0] == "OK") {
                $('#ModalCongrats').modal('show');
                btn_Next();
            }
            if (data[0] == "RNOTOk") {
                $('#ModalCongrats').modal('show');
                Scores();
            }
        }
    });
}

function backToProfile() {
    window.location = '/Student/Profile';
}
var textbox = '', txt_box_text = '';
function txt_click(clicked_id) {


    $('#' + clicked_id).attr('readonly', 'readonly').focus;
    // $('#')
    textbox = clicked_id;
    txt_box_text = document.getElementById(clicked_id).value;

}
function display(val) {

    let GetValues = document.getElementById(val).innerHTML;

    if (textbox != null) {
        if (txt_box_text == '') {

            var C = document.getElementById(textbox).value;
            var CL = document.getElementById(textbox).value.length;

            if (C == "" && CL == 0) {
                document.getElementById(textbox).value += document.getElementById(val).innerHTML;
                document.getElementById(textbox).blur();
            }
        }
        else {
            swal.fire("Oops!", "Only one input is allowed....", "error");
        }
    }
    return val
}
function reset() {
    location.reload();
}
function erase() {

    if (textbox != null) {
        if (txt_box_text !== '') {
            document.getElementById(textbox).value = '';
        }
    }
}

function SkippedSave() {
    debugger;

    var d = new Date();
    var ST = d;

    const SubTm = new Date();

    var IN_H = new Date(ST).getHours();
    var IN_M = new Date(ST).getMinutes();
    var IN_S = new Date(ST).getSeconds();

    var OUT_H = new Date(SubTm).getHours();
    var OUT_M = new Date(SubTm).getMinutes();
    var OUT_S = new Date(SubTm).getSeconds();

    var GetF_H = parseInt(OUT_H) - parseInt(IN_H);
    var GetF_M = parseInt(OUT_M) - parseInt(IN_M);
    var GetF_S = parseInt(OUT_S) - parseInt(IN_S);

    var HrsDiff = Math.floor(GetF_H * 60 * 60 + GetF_M * 60 + GetF_S);
    var SecDiff = Math.floor(GetF_M * 60 + GetF_S);
    var DIFF = GetF_H + " Hrs" + " : " + GetF_M + " Mins" + " : " + GetF_S + " Secs";

    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/GameCompetition/SkipKenKen',
        data: { StartTime: ST, EndTime: SubTm, timediff: HrsDiff, UserAnswer: 0 },

        success: function (listobj) {
            if (listobj[0] == "1") {
                swal("Congrats!", "Record Save Successfully", "success");
                setTimeout(function () {
                    window.location.href = "/Home/GameCompetition";
                }, 2000);
            } else {
                swal("Error!", "Record not Saved", "error");
                setTimeout(function () {
                    window.location.href = "/Home/GameCompetition";
                }, 2000);
            }
        },
        error: function (response) {
            swal("Error!", "An error occurred", "error");
            console.error("Error: ", response);
            setTimeout(function () {
                window.location.href = "/Home/GameCompetition";
            }, 2000);
        }
    });



