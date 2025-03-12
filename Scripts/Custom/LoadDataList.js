//
function reset() {
    $('#FilterForm')[0].reset();
}

function LoadDataList(actionType) {
    debugger;
    var htmlform = "#FilterForm";
    $(htmlform).find("#ActionType").val(actionType);
    //$('#FilterForm').submit();
    $(htmlform).ajaxSubmit(function (data) {
        var response = jQuery.parseJSON(data);
        if (response.Success) {
            var isPostback = response.Data.isPostback;
            if (isPostback == 1) {
                //alert(response.Data.TotalPages);
                var TotalRecords = response.Data.TotalRecords;
                var TotalPages = response.Data.TotalPages;
                var ddlPages = $("#PageNumber");
                $(ddlPages).find('option').remove();
                for (i = 1; i <= TotalPages; i++) {
                    $(ddlPages).append("<option value='" + i + "'>" + i + "</option>");
                }
                $("#lbTotalRecords").text(TotalRecords);
            }
            $("#PageNumber").val(response.Data.PageNumber);
            $("#TotalRecords").val(response.Data.TotalRecords);
            var tabletbody = $("#tabletbody");
            $(tabletbody).html("");
            for (i = 0; i < response.Data.Result.length; i++) {
                var item = response.Data.Result[i];
                appendDataTable(item, i);
            }
            if (TotalRecords == "0" || TotalRecords == 0) {
                $("#divLogMore").css("display", "none");
                $("#lblRecordNotFound").css("display", "");
                $("#lblRecordNotFound").html("No Record Found");
            }
            else {
                $("#divLogMore").css("display", "");
                $("#lblRecordNotFound").css("display", "none");
            }
        }
        else {
            showError(response.Message);
        }
    });
}

//function LoadDataList(actionType, htmlform, callback) {

//    //$('#FilterForm').submit();
//    $(htmlform).ajaxSubmit(function (data) {
//        var response = jQuery.parseJSON(data);
//        if (response.Success) {
//            var isPostback = response.Data.isPostback;
//            if (isPostback == 1) {
//                //alert(response.Data.TotalPages);
//                var TotalRecords = response.Data.TotalRecords;
//                var TotalPages = response.Data.TotalPages;
//                var ddlPages = $(htmlform).find("#PageNumber");
//                $(ddlPages).find('option').remove();
//                for (i = 1; i <= TotalPages; i++) {
//                    $(ddlPages).append("<option value='" + i + "'>" + i + "</option>");
//                }
//                $(htmlform).find("#lbTotalRecords").text(TotalRecords);
//            }
//            $(htmlform).find("#PageNumber").val(response.Data.PageNumber);
//            $(htmlform).find("#TotalRecords").val(response.Data.TotalRecords);
//            //var tabletbody = $(htmlform).find("#tabletbody");
//            var tabletbody = "#" + $(htmlform).attr("tabletbodyId");

//            if ($(htmlform).attr("paginationType") == "Scroll") {
//                //Will not clear Exsiting Data
//            }
//            else {
//                $(tabletbody).html("");
//            }
//            for (i = 0; i < response.Data.Result.length; i++) {
//                var item = response.Data.Result[i];
//                callback(item, i);
//            }
//        }
//        else {
//            showError(response.Message);
//        }
//    });
//}

//LoadDataList("NextPage", this, AppendSchoolBody);

//function AppendSchoolBody(item, rowno)
//{

//}
//
try {
    $(function () {
        //$('.datepicker').datepicker({
        //    format: 'dd/mm/yyyy'
        //});
        //$('.timepicker').timepicker();

        //
        //alert("LoadDataList.js");
        $('#Search').on("keypress", function (e) {
            if (e.keyCode == 13) {
                // Cancel the default action on keypress event
                e.preventDefault();
                LoadDataList('FilterRecord')
            }
        });

    });
} catch (e) { alert(e); }
//