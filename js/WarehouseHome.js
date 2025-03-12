
var app = angular.module("WarehouseHome", []);
app.controller("WarehouseHomeController", function ($scope, $http) {
    //$scope.config = Warehouse.config;
    //$scope.model = Warehouse.model;
    angular.element(document).ready(function () {
        //debugger
        $http({
            method: "Get",
            url: '/warehouse/WarehouseHomeList',
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {

            $scope.warehouseList = response.data.warehouseList;



        }, function (error) {
            console.log(error);

        });


    });


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

  

   

  

});