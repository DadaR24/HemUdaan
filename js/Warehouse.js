var app = angular.module("warehouseapp", []);
app.controller("warehouseController", function ($scope, $http) {
    //$scope.config = Warehouse.config;
    //$scope.model = Warehouse.model;
    angular.element(document).ready(function () {
       // debugger
       
        var locationId = "locationId=" + 0;
        $http({
            method: "Get",
            url: `/Location/BindHomePageDropDown` + `?` + locationId,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            //debugger
            $scope.stateList = response.data.stateList;
            $scope.stateListData = [];
            for (var i = 0; i < $scope.stateList.length; i++) {
                $scope.stateListData.push({
                    label: $scope.stateList[i].stateId,
                    title: $scope.stateList[i].stateName,
                    value: $scope.stateList[i].stateId
                });
            }
            $scope.bankList = response.data.bankList;
            $scope.bankListData = [];
            for (var i = 0; i < $scope.bankList.length; i++) {
                $scope.bankListData.push({
                    label: $scope.bankList[i].bankID,
                    title: $scope.bankList[i].bankClient,
                    value: $scope.bankList[i].bankID
                });
            }
            $scope.searchlist();
            
            
        }, function (error) {
            console.log(error);
        });


        $('#stateId').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select State',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });

        $('#bankID').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Bank',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
         

    });

    $scope.getBankList = function () {
        $http({
            method: "Get",
            url: `/Location/BankList`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            $scope.BankList = response.data.BankList;
            $scope.BankListData = [];
            for (var i = 0; i < $scope.BankList.length; i++) {
                $scope.BankListData.push({
                    label: $scope.BankList[i].BankID,
                    title: $scope.BankList[i].BankClient,
                    value: $scope.BankList[i].BankID
                });
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.getLocationList = function () {
        var id = "id=" + $scope.stateId + "&BankId=" + $scope.bankID;
        $http({
            method: "Get",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/LocationList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.locationList = response.data.locationList;
            $scope.locationListData = [];
            for (var i = 0; i < $scope.locationList.length; i++) {
                $scope.locationListData.push({
                    label: $scope.locationList[i].locationId,
                    title: $scope.locationList[i].location,
                    value: $scope.locationList[i].locationId
                });
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.searchlist = function () {


        //debugger
        $http({
            method: "Get",
            url: '/warehouse/WarehouseRequestList',
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {

            $scope.warehouseList = response.data.warehouseList;
            
            //$('#tableID').DataTable();
            //$('#tableID thead tr').clone(true).addClass('filters').appendTo('#tableID thead');
            //var table = $('#tableID').DataTable(
            //    {
            //        data: [],
            //        dom: 'Bfrtlip', buttons: ['copy', 'csv', 'excel', 'pdf', 'pdfHtml5', 'print'],
            //        orderCellsTop: true,
            //        fixedHeader: true,
            //        info: false,
            //        paging: false,
            //        initComplete: function () {
            //            var api = this.api(); api.columns().eq(0).each(function (colIdx) {
            //                var cell = $('.filters th').eq($(api.column(colIdx).header()).index()); var title = $(cell).text(); $(cell).html('<input type=\"text\" placeholder=\"' + title + '\" />');
            //                $('input', $('.filters th').eq($(api.column(colIdx).header()).index())).off('keyup change').on('keyup change', function (e) {
            //                    e.stopPropagation();
            //                    $(this).attr('title', $(this).val()); var regexr = '({search})'; var cursorPosition = this.selectionStart; api.column(colIdx).search(this.value != '' ? regexr.replace('{search}', '(((' + this.value + ')))') : '', this.value != '', this.value == '').draw(); $(this).focus()[0].setSelectionRange(cursorPosition, cursorPosition);
            //                });
            //            });
            //        },
            //    });

            // $scope.warehouseList.push(response.data.warehouseList) = response.data.warehouseList;

        }, function (error) {
            console.log(error);

        });
    }


    $scope.UpdateApprovalStatus = function (warehouseid) {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "warehouseid=" + warehouseid + "&strremarks=" + 'testing' + "&intstatus=" + 1;

        $http({
            method: "POST",
            url: `/Warehouse/UpdateApprovalStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Warehouse Approval Details Updated Successfully", "index");
            }
            else {
                ShowMSG("WARNING", response.data, "");
                // alert("Somehing is wrong. Please try again.");
            }
        }, function (error) {
            ShowMSG("ERROR", response.data, "");
            //console.log(error);
        });
    }

    $scope.UpdateRejectStatus = function (warehouseid) {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "warehouseid=" + warehouseid + "&strremarks=" + 'testing' + "&intstatus=" + 2;

        $http({
            method: "POST",
            url: `/Warehouse/UpdateApprovalStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Warehouse Approval Details Updated Successfully", "index");
            }
            else {
                ShowMSG("WARNING", response.data, "");
                // alert("Somehing is wrong. Please try again.");
            }
        }, function (error) {
            ShowMSG("ERROR", response.data, "");
            //console.log(error);
        });
    }
    $scope.DeleteRecord = function (warehouseid) {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "warehouseid=" + warehouseid + "&intstatus=" + 2;

        $http({
            method: "POST",
            url: `/Warehouse/DeleteRecordStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Warehouse Deleted Successfully", "index");
            }
            else {
                ShowMSG("WARNING", response.data, "");
                // alert("Somehing is wrong. Please try again.");
            }
        }, function (error) {
            ShowMSG("ERROR", response.data, "");
            //console.log(error);
        });
    }
    $scope.saveUpdateData = function () {
        //debugger

        if ($scope.bankID == undefined) {
            ShowMSG("WARNING", "Please Select Back Name", "");
        }
        //if ($scope.WareHouse.warehousename.value == undefined) {
        //    ShowMSG("WARNING", "Please Select Warehouse Name", "");

        //}


        var id = "Bankid=" + $scope.bankID + "&State_id=" + $scope.stateId + "&warehouseName=" + $scope.WareHouse.warehousename + "&whaddress=" + $scope.WareHouse.warehouseaddress + "&whcontact=" + $scope.WareHouse.warehousenumber;

        $http({
            method: "POST",
            url: `/warehouse/AddWarehouseRequest` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            //debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Warehouse Request Added  successfully", "index");
            }
            else
            {
                if (response.data == "SESSION")
                {
                    exampleFunction();
                    ShowMSG("SESSION", "Your Session has been Ended!Please Login Again!", "../user/index");
                }
                else
                {
                    ShowMSG("WARNING", response.data, "");
                }
                // alert("Somehing is wrong. Please try again.");
            }
        }, function (error) {
            ShowMSG("ERROR", error, "");
            //console.log(error);
        });
    }

});