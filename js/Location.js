var app = angular.module("locationapp", []);
app.controller("locationController",  function ($scope, $http,$location) {
    $scope.config = location.config;
    $scope.model = location.model;
    angular.element(document).ready(function () {
        //debugger
        //var queryParams = $location.search().myParam;
        //alert(queryParams);
        var locationId = "locationId=" + 0;
        $http({
            method: "Get",
            url: `/Location/BindHomePageDropDown` + `?` + locationId,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
           // debugger
          

           
            $scope.stateList = response.data.stateList;
            $scope.stateListData = [];
            for (var i = 0; i < $scope.stateList.length; i++) {
                $scope.stateListData.push({
                    label: $scope.stateList[i].stateId,
                    title: $scope.stateList[i].stateName,
                    value: $scope.stateList[i].stateId
                });
            }
            $scope.stateListBlock = response.data.stateList;
            $scope.stateListBlockData = [];
            for (var i = 0; i < $scope.stateList.length; i++) {
                $scope.stateListBlockData.push({
                    label: $scope.stateListBlock[i].stateId,
                    title: $scope.stateListBlock[i].stateName,
                    value: $scope.stateListBlock[i].stateId
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
            $scope.hubs = response.data.hubs;
            $scope.hubsData = [];
            for (var i = 0; i < $scope.hubs.length; i++) {
                $scope.hubsData.push({
                    label: $scope.hubs[i].icchub,
                    title: $scope.hubs[i].icchub,
                    value: $scope.hubs[i].icchub
                });
            }

        }, function (error) {
            console.log(error);
        });

        var id = "stateId=" + $scope.stateId + "&locationId=" + $scope.locationId;
        $http({
            method: "Get",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/SearchListForApproval` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            //debugger
            $scope.searchList = response.data.searchList;
            // $scope.searchList(response.data.searchList);
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
    $scope.ShowBlockModel = function () {
        $('#AddBlockModel').modal('show');
    }
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

    $scope.getLocationdetails = function (locationId) {
        debugger       
        var id = "locationId="+locationId;
        $http({
            method: "Get",
            url: `/Location/BindHomePageDropDown` + `?` + id,
            //url: `/Location/Create`+ `?` + locationId,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
           // debugger
            $scope.stateList = response.data.stateList;
            $scope.stateListData = [];
            for (var i = 0; i < $scope.stateList.length; i++) {
                $scope.stateListData.push({
                    label: $scope.stateList[i].stateId,
                    title: $scope.stateList[i].stateName,
                    value: $scope.stateList[i].stateId
                });
            }
            $scope.banksList = response.data.bankList;
            $scope.bankListData = [];
            for (var i = 0; i < $scope.banksList.length; i++) {
                $scope.bankListData.push({
                    label: $scope.banksList[i].bankID,
                    title: $scope.banksList[i].bankClient,
                    value: $scope.banksList[i].bankID
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
            $scope.districtLists = response.data.districtLists;
            $scope.districtListsData = [];
            for (var i = 0; i < $scope.districtLists.length; i++) {
                $scope.districtListsData.push({
                    label: $scope.districtLists[i].districtID,
                    title: $scope.districtLists[i].district_title,
                    value: $scope.districtLists[i].districtID
                });
            }
            $scope.blockLists = response.data.blockLists;
            $scope.blockListsData = [];
            for (var i = 0; i < $scope.blockLists.length; i++) {
                $scope.blockListsData.push({
                    label: $scope.blockLists[i].blockid,
                    title: $scope.blockLists[i].block_title,
                    value: $scope.blockLists[i].blockid
                });
            }
            debugger
            if (locationId!= 0) {
                $scope.banksID = response.data.searchList[0].client //'15'// response.data.searchList[0].string
                //$scope.stateName='5'

                $scope.stateName = response.data.searchList[0].state

                $scope.location = response.data.searchList[0].location

                $scope.activationDate = new Date(response.data.searchList[0].activationDate)

                $scope.districtListss = response.data.searchList[0].district

                $scope.blockid = response.data.searchList[0].block_ID.toString()

                $scope.zoneListt = response.data.searchList[0].zone

                $scope.icchub = response.data.searchList[0].icchub

                $scope.DistanceBetwwen = response.data.searchList[0].distanceBetween
                $scope.hddlocationid = response.data.searchList[0].locationId

                $scope.DateJoinCM = new Date(response.data.searchList[0].dateJoinCM)

                $scope.isaDD = response.data.searchList[0].isaDD
                $scope.isupdate = response.data.searchList[0].isupdate
                $scope.isdelete = response.data.searchList[0].isdelete
                $scope.isapproval = response.data.searchList[0].isapproval
                



            }
           
            if (locationId == '0') {
                debugger
                $scope.banksID ='0'
                //$scope.stateName='5'

                $scope.stateName = '0'

                $scope.location = ''

                $scope.activationDate = new Date();

                $scope.districtListss = '0'

                $scope.zoneListt = '0'

                $scope.icchub = ''

                $scope.DistanceBetwwen = '0'
                $scope.hddlocationid = '0'

                $scope.DateJoinCM = new Date();

                $scope.isaDD = response.data.searchList[0].isaDD
                $scope.isupdate = response.data.searchList[0].isupdate
                $scope.isdelete = response.data.searchList[0].isdelete
                $scope.isapproval = response.data.searchList[0].isapproval
            }
            

            //$scope.searchList = response.data.searchList;
            //$scope.banksList = response.data.bankList;
            //$scope.banksListData = [];
            //for (var i = 0; i < $scope.banksList.length; i++) {
            //    $scope.banksListData.push({
            //        label: $scope.banksList[i].bankID,
            //        title: $scope.banksList[i].bankClient,
            //        value: $scope.banksList[i].bankID
            //    });
            //}
            //$scope.stateList = response.data.stateList;
            //$scope.zoneList = response.data.zoneList;
           // $scope.Location.bankID = response.data.searchList[0].client;
        }, function (error) {
            alert(error);
            console.log(error);
        });
        ////showModalPopup("#AddLocationModel")
        $('#AddLocationModel').modal('show');
    }
    $scope.getDistrictList = function () {
        var id = "id=" + $scope.stateName;
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
    $scope.getBlockDistrictList = function () {
        var id = "id=" + $scope.stateNameforblock;
        $http({
            method: "GET",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/DistrictList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.districtListsblock = response.data.districtLists;
            $scope.districtListsblockData = [];
            for (var i = 0; i < $scope.districtListsblock.length; i++) {
                $scope.districtListsblockData.push({
                    label: $scope.districtListsblock[i].DistrictID,
                    title: $scope.districtListsblock[i].district_title,
                    value: $scope.districtListsblock[i].DistrictID
                });
            }
        }, function (error) {
            console.log(error);
        });
    }
    $scope.getBlockList = function () {
        var id = "id=" + $scope.districtListss;
        debugger
        $http({
            method: "GET",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/BlockList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.blockLists = response.data.blockLists;
            $scope.blockListsData = [];
            for (var i = 0; i < $scope.blockLists.length; i++) {
                $scope.blockListsData.push({
                    label: $scope.blockLists[i].blockid,
                    title: $scope.blockLists[i].block_title,
                    value: $scope.blockLists[i].blockid
                });
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.getLocationList = function () {
        debugger
        var id = "id=" + $scope.stateId + "&BankId=" + $scope.bankID;
        $http({
            method: "Get",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/AllLocationList` + `?` + id,
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

    $scope.search = function () {
        debugger
        var id = "stateId=" + $scope.stateId + "&locationId=" + $scope.locationId;
        $http({
            method: "Get",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/Location/SearchList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            //debugger
            $scope.searchList = response.data.searchList;
           // $scope.searchList(response.data.searchList);
        }, function (error) {
            console.log(error);
        });
    }

    $scope.AddLocation = function () {
        window.location.href = "/Location/Create?locationId=0";        
    }

    $scope.UpdateApprovalStatus= function () {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "locationId=" + $scope.hddlocationid + "&strremarks=" + $scope.Approval + "&intstatus=" + 1 ;

        $http({
            method: "POST",
            url: `/Location/UpdateApprovalStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Location Approval Details Successfully", "index");
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

    $scope.UpdateRejectStatus = function () {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "locationId=" + $scope.hddlocationid + "&strremarks=" + $scope.Approval + "&intstatus=" + 2;

        $http({
            method: "POST",
            url: `/Location/UpdateApprovalStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Location Approval Details Successfully", "index");
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

    $scope.DeleteRecord = function () {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "locationId=" + $scope.hddlocationid +"&intstatus=" + 2;

        $http({
            method: "POST",
            url: `/Location/DeleteRecordStatus` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Location Deleted Successfully", "index");
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

    $scope.saveBlockUpdateData = function () {
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "DistrictID=" + $scope.districtListssforblock + "&stateId=" + $scope.stateNameforblock + "&Block=" + $scope.Block;

        $http({
            method: "POST",
            url: `/Location/AddBlockUpdateLocation` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
                debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Block Saved Successfully", "index");
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
        debugger

        //if ($scope.Location == undefined) {
        //    alert("Please select OR enter all fields.");
        //}


        var id = "bankID=" + $scope.banksID + "&stateId=" + $scope.stateName + "&locationId=" + $scope.hddlocationid + "&Block_ID=" + $scope.blockid + "&location=" + $scope.location + "&activationDate=" + $scope.activationDate.toISOString() + "&DistrictID=" + $scope.districtListss + "&zone=" + $scope.zoneListt + "&icchub=" + $scope.icchub + "&distancebetween=" + $scope.DistanceBetwwen + "&dateJoinCM=" + $scope.DateJoinCM.toISOString();

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
});