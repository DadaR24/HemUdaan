var app = angular.module("locationcreateapp", []);

app.controller("locationController", function ($scope, $http) {
    debugger
    $('#activationDate').datepicker({
        todayHighlight: true,
        format: 'mm/dd/yyyy',
        autoclose: true
    }).on('changeDate', function (date) {
        $scope.date = date.format();
        $scope.$apply();
    });

    $('.datepicker').datepicker({
        todayHighlight: true,
        format: 'mm/dd/yyyy',
        autoclose: true
    }).on('changeDate', function (date) {
        $scope.date = date.format();
        $scope.$apply();
    });

    angular.element(document).ready(function () {
        $http({
            method: "Get",
            url: `/Location/BindHomePageDropDown`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {

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
            $scope.zoneList = response.data.zoneList;
            $scope.zoneListData = [];
            for (var i = 0; i < $scope.zoneList.length; i++) {
                $scope.zoneListData.push({
                    label: $scope.zoneList[i].srNo,
                    title: $scope.zoneList[i].zone,
                    value: $scope.zoneList[i].srNo
                });
            }


        }, function (error) {
            console.log(error);
        });

        $http({
            method: "Get",
            url: `/Location/BankList`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {

            $scope.banksList = response.data.bankList;
            $scope.bankListData = [];
            for (var i = 0; i < $scope.banksList.length; i++) {
                $scope.banksListData.push({
                    label: $scope.banksList[i].bankID,
                    title: $scope.banksList[i].bankClient,
                    value: $scope.banksList[i].bankID
                });
            }

        }, function (error) {
            console.log(error);
        });


        $('#stateName').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select State',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });

        $('#Client').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Bank',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        $('#Zone').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Zone',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
    });


    $scope.getDistrictList = function () {
        var id = "id=" + $scope.Location.StateName;
        $http({
            method: "GET",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/DistrictList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.districtLists = response.data.districtLists;
            $scope.districtListsData = [];
            for (var i = 0; i < $scope.districtLists.length; i++) {
                $scope.districtListsData.push({
                    label: $scope.districtLists[i].DistrictID,
                    title: $scope.districtLists[i].district_title,
                    value: $scope.districtLists[i].DistrictID
                });
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.saveUpdateDataa = function () {
        debugger
        var id = "stateName=" + $scope.LocationData.StateName + "&locationName=" + $scope.LocationData.Location + "&noofCM=" + $scope.LocationData.NoofCM + "&noSG=" + $scope.LocationData.NoSG + "&matrixAmount=" + $scope.LocationData.MatrixAmount;

        $http({
            method: "POST",
            url: `/Location/AddUpdateLocation` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.stateList = response.data.stateList;
            $scope.stateListData = [];
            for (var i = 0; i < $scope.stateList.length; i++) {
                $scope.stateListData.push({
                    label: $scope.stateList[i].stateId,
                    title: $scope.stateList[i].stateName,
                    value: $scope.stateList[i].stateId
                });
            }
        }, function (error) {
            console.log(error);
        });
    }
    $scope.saveUpdateData = function () {
        debugger

        if ($scope.Location == undefined) {
            alert("Please select OR enter all fields.");
        }


        var id = "bankID=" + $scope.Location.bankID + "&stateId=" + $scope.Location.StateName + "&location=" + $scope.Location.location + "&activationDate=" + $scope.Location.activationDate + "&DistrictID=" + $scope.Location.districtLists + "&zone=" + $scope.Location.zoneList + "&icchub=" + $scope.Location.icchub + "&distancebetween=" + $scope.Location.DistanceBetwwen + "&dateJoinCM=" + $scope.Location.DateJoinCM;

        $http({
            method: "POST",
            url: `/Location/AddUpdateLocation` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Location Saved Successfully", "index");
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

    $scope.doTheBack = function () {
        window.location.href = "/Location/Index";
    }

});