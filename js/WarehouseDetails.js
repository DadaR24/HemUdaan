var app = angular.module("warehousecreateapp", ['ngFileUpload']);
app.controller("warehousecreateController", function ($scope, Upload, $timeout, $http) {
    //$scope.config = Warehouse.config;
    //$scope.model = Warehouse.model;
    angular.element(document).ready(function () {
       // debugger
        //var id = this.route.snapshot.paramMap.get('WarehouseId');
        $http({
            method: "Get",
            url: `/warehouse/warehouseDetails`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
           // debugger
            $scope.authorofgodown = response.data.whStatus;
            $scope.whStatusData = [];
            for (var i = 0; i < $scope.authorofgodown.length; i++) {
                $scope.whStatusData.push({
                    label: $scope.authorofgodown[i].status,
                    title: $scope.authorofgodown[i].status,
                    value: $scope.authorofgodown[i].status
                });
            }
            $scope.typeofgodown = response.data.whType;
            $scope.whTypeData = [];
            for (var i = 0; i < $scope.typeofgodown.length; i++) {
                $scope.whTypeData.push({
                    label: $scope.typeofgodown[i].type,
                    title: $scope.typeofgodown[i].type,
                    value: $scope.typeofgodown[i].type
                });
            }
            $scope.whcondition = response.data.whCondition;
            $scope.whConditionData = [];
            for (var i = 0; i < $scope.whcondition.length; i++) {
                $scope.whConditionData.push({
                    label: $scope.whcondition[i].condition,
                    title: $scope.whcondition[i].condition,
                    value: $scope.whcondition[i].condition
                });
            }
            $scope.locationList = response.data.whLocationList;
            $scope.whLocationListData = [];
            for (var i = 0; i < $scope.locationList.length; i++) {
                $scope.whLocationListData.push({
                    label: $scope.locationList[i].location,
                    title: $scope.locationList[i].location,
                    value: $scope.locationList[i].locationId
                });
            }
            $scope.takeOverStatuses = response.data.takeOverStatuses;
            $scope.takeOverStatusesdata = [];
            for (var i = 0; i < $scope.takeOverStatuses.length; i++) {
                $scope.takeOverStatusesdata.push({
                    label: $scope.takeOverStatuses[i].takeOver_Status,
                    title: $scope.takeOverStatuses[i].takeOver_Status,
                    value: $scope.takeOverStatuses[i].status_id
                });
            }
            $scope.warehouseTypes = response.data.warehouseTypes;
            $scope.warehousetypesdata = [];
            for (var i = 0; i < $scope.warehouseTypes.length; i++) {
                $scope.warehousetypesdata.push({
                    label: $scope.warehouseTypes[i].typE_DESC,
                    title: $scope.warehouseTypes[i].typE_DESC,
                    value: $scope.warehouseTypes[i].typE_ID
                });
            }
            $scope.bankClient = response.data.warehouseList[0].bankClient
            $scope.state = response.data.warehouseList[0].state
              //  debugger
            $scope.WarehouseName = response.data.warehouseList[0].warehouseName  
            $scope.whaddress = response.data.warehouseList[0].whaddress
            $scope.whcontact = response.data.warehouseList[0].whcontact
            $scope.WarehouseId = response.data.warehouseList[0].warehouseId
            //debugger
            $scope.InspectionReqDate =  new Date(response.data.warehouseList[0].inspectionReqDate)
            $scope.WhClubbed = response.data.warehouseList[0].whClubbed
            $scope.ContactPerson = response.data.warehouseList[0].contactPerson
            $scope.EmailID = response.data.warehouseList[0].emailID
            $scope.locationID = response.data.warehouseList[0].location
            $scope.warehouseOwnName = response.data.warehouseList[0].nameOfWHOwner
            $scope.warehousetype = response.data.warehouseList[0].whtypestr
            // response.data.warehouseList[0].locationID
           // $scope.locationIdDD = response.data.warehouseList[0].locationID
            ///$scope.locationId.locationId = response.data.warehouseList[0].locationID
            
           /// $scope.locationId = "KHAGARIA-A 18/05/2017"
            $scope.capacity = response.data.warehouseList[0].capacity
            $scope.TypeOfFlooring = response.data.warehouseList[0].typeOfFlooring
            $scope.TypeOfRoof = response.data.warehouseList[0].typeOfRoof
            $scope.PlinthHeight = response.data.warehouseList[0].plinthHeight
            $scope.MobileNo = response.data.warehouseList[0].mobileNo
            debugger
            $scope.BANKWHCODE = response.data.warehouseList[0].BANKWHCODE
            $scope.typeofgodownn = response.data.warehouseList[0].typeofgodown
            $scope.categoryofgodown = response.data.warehouseList[0].categoryofgodown
            $scope.authorofgodownn = response.data.warehouseList[0].authorofgodown
            $scope.noofdoors = response.data.warehouseList[0].noofdoors
            $scope.length = response.data.warehouseList[0].length
            $scope.breadth = response.data.warehouseList[0].breadth
            $scope.Height = response.data.warehouseList[0].height
            $scope.TypeManpower = response.data.warehouseList[0].typeManpower
            $scope.latitude = response.data.warehouseList[0].latitude
            $scope.longitude = response.data.warehouseList[0].longitude
          // $scope.TakeOver_Status ="'"+ response.data.warehouseList[0].takeOver_Status +"'"
            //$scope.test = response.data.warehouseList[0].takeOver_Status
          //  $('#TakeOver_Status').SelectedText = "test";
            //var test = response.data.warehouseList[0].takeOver_Status;
            $scope.TakeOver_Status = response.data.warehouseList[0].takeOver_StatusStr;
            $scope.TakeOver_Date = new Date( response.data.warehouseList[0].takeOver_Date)
            $scope.NoOfCm = response.data.warehouseList[0].noOfCm
            $scope.NoOfWM = response.data.warehouseList[0].noOfWM
            $scope.WHCharges = response.data.warehouseList[0].wHCharges
            $scope.RequiredManpower = response.data.warehouseList[0].requiredManpower
            $scope.SurveyDate=  new Date(response.data.warehouseList[0].surveyDate)
            $scope.AgreementStartDate1 = new Date( response.data.warehouseList[0].agreementStartDate1)
            $scope.AgreementExpiryDate1 = new Date( response.data.warehouseList[0].agreementExpiryDate1)
            $scope.BankBranchName = response.data.warehouseList[0].bankBranchName
            $scope.remarks = response.data.warehouseList[0].remarks
            $scope.WORKINGSTATUS = response.data.warehouseList[0].workingstatus
            $scope.isupdate = response.data.warehouseList[0].isupdate
            $scope.isdelete = response.data.warehouseList[0].isdelete
            $scope.isapproval = response.data.warehouseList[0].isapproval

            $scope.wHLicenseNo = response.data.warehouseList[0].wHLicenseNo
            $scope.FilesID = response.data.warehouseList[0].file_LOG_ID
            $scope.getFilesList();
            
           
            
           

        }, function (error) {
            console.log(error);
            ShowMSG('Error', error.log, '');
        });

        //$scope.TakeOver_Status ='2'
        $('#authorofgodown').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Status',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });

        $('#typeofgodown').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Type',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        $('#categoryofgodown').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Condition',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        //$('#locationid').select2({
        //    selectAllText: 'Select All',
        //    SelectedText: 'Select Location',
        //    includeSelectAllOption: true,
        //    enableCaseInsensitiveFiltering: true,
        //    filterPlaceholder: 'Search',
        //    maxHeight: 180,
        //    includeFilterClearBtn: true,
        //    dropdownAutoWidth: true
        //});
        $('#warehousetype').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Location',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        $('#TakeOver_Status').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Location',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        $('#WORKINGSTATUS').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Location',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
        $('#WhClubbed').select2({
            selectAllText: 'Select All',
            SelectedText: 'Select Location',
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            filterPlaceholder: 'Search',
            maxHeight: 180,
            includeFilterClearBtn: true
        });
    });
    $scope.UploadFilesss = function (files) {
        debugger
        debugger
        $scope.SelectedFiles = files;
        if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            Upload.upload({
                url: "/api/FileAPI/UploadFiles",
                data: {
                    files: $scope.SelectedFiles
                }
            }).then(function (response) {
                //$timeout(function () {
                //    $scope.Result = response.data;
                //});
            }, function (response) {
                //if (response.status > 0) {
                //    var errorMsg = response.status + ': ' + response.data;
                //    alert(errorMsg);
               // $scope.warehouseTypes = response.data.warehouseTypes;
                //}
            }, function (evt) {
                //var element = angular.element(document.querySelector('#dvProgress'));
                //$scope.Progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                //element.html('<div style="width: ' + $scope.Progress + '%">' + $scope.Progress + '%</div>');
            });
        }
    };

    $scope.DownloaddedFile = function (id) {
       /// debugger
        window.open('/warehouse/downloadfile?Fileid=' + id, "_blank")
    };
    $scope.test = function (id) {
        /// debugger
       
    };
    $scope.DownloadUploadedFile = function (id) {
        ////$http({
        ////    url: `/warehouse/downloadfile ? Fileid =` + id,
        ////    method: 'Get',
        ////    params: {},
        ////    headers: {
        ////        'Content-type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        ////    },
        ////    responseType: 'arraybuffer'
        ////}).success(function (data, status, headers, config) {
        ////    // TODO when WS success
        ////    debugger
        ////    var file = new Blob([data], {
        ////        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        ////    });
        ////    //trick to download store a file having its URL
        ////    var fileURL = URL.createObjectURL(file);
        ////    var a = document.createElement('a');
        ////    a.href = fileURL;
        ////    a.target = '_blank';
        ////    a.download = 'yourfilename.xlsx';
        ////    document.body.appendChild(a); //create the link "a"
        ////    a.click(); //click the link "a"
        ////    document.body.removeChild(a); //remove the link "a"
        ////}).error(function (data, status, headers, config) {
        ////    //TODO when WS error
        ////});
        //window.open(`/warehouse/downloadfile?Fileid=7');
        $http({
            method: "Get",
            url: `/warehouse/downloadfile?Fileid=`+id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            $scope.Base64 = response.data
            new FileReader().readAsBinaryString(new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }));
        var linkElement = document.createElement('a');
        try {
            var blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var url = window.URL.createObjectURL(blob);

            linkElement.setAttribute('href', url);
            linkElement.setAttribute("download", 'yourfilename.xlsx');

            var clickEvent = new MouseEvent("click", {
                "view": window,
                "bubbles": true,
                "cancelable": false
            });
            linkElement.dispatchEvent(clickEvent);
        } catch (ex) {
            console.log(ex);
        }

        }, function (error) {
            console.log(error);
        });
    };
    $scope.DeleteUploadedFile = function (id) {
        $http({
            method: "Get",
            url: `/warehouse/deletefile?Fileid=`+id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            debugger
            if (response.data.responsestatus == "Success") {
               
                $scope.getFilesList();
                showtoast("success", response.data.responsemsg)
            }
            else {
                showtoast("Info", response.data.responsemsg)
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.UploadFiles = function (files) {
        debugger
        $scope.SelectedFiles = files
        if ($scope.SelectedFiles[0].type != 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
            if ($scope.SelectedFiles[0].type != 'application/vnd.ms-excel') {

                ShowMSG('Info', 'Only excel file allowd');
                return
            }
        }
        if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            Upload.upload({
                //url: 'https://localhost:44328/print/upload',
                url: '/warehouse/upload?WarehouseId=' + $scope.WarehouseId + '&FilesLogID=' + $scope.FilesID,
                data: {
                    files: $scope.SelectedFiles
                }
            }).then(function (response) {
                debugger
                if (response.data.responsestatus == "Success") {
                    $scope.FilesID = response.data.responseoutparam
                    $scope.getFilesList();
                    showtoast("success", response.data.responsemsg)
                }
                else
                {
                    showtoast("Info", response.data.responsemsg)
                }
               
            }, function (response) {
                if (response.status > 0) {
                    ShowMSG("ERROR", response.data, "");
                    // errorMsg = response.status + ': ' + response.data;
                    alert(errorMsg);
                }
            }, function (evt) {
                //var element = angular.element(document.querySelector('#dvProgress'));
                //$scope.Progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                //element.html('<div style="width: ' + $scope.Progress + '%">' + $scope.Progress + '%</div>');
            });
        }
    };

    //$scope.Upload = function () {
    //    debugger
    //    $http.post("/warehouse/SaveFiles", $scope.files,
    //        {
    //            withCredentials: true,
    //            headers: { 'Content-Type': undefined },
    //            transformRequest: angular.identity
    //        })
    //        .success(function (d) {
    //           // defer.resolve(d);
    //        })
    //        .error(function () {
    //           // defer.reject("File Upload Failed!");
    //        });
    //}
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

    $scope.getFilesList = function () {
        var id = "FileID=" + $scope.FilesID;
        $http({
            method: "Get",
            //url: `/Location/LocationList` + '?' +  $scope.stateId,
            url: `/warehouse/FileList` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            $scope.FileLists = response.data.fileLists;
            
        }, function (error) {
            console.log(error);
        });
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
            url: '/warehouse/warehouseList',
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
       // debugger
        if ($scope.bankClient == "HDFC Bank") {
            if ($scope.BANKWHCODE == undefined) {
                ShowMSG("WARNING", "Please Enter ECBG WH Code", "");
                return;
            }
        }


        if ($scope.FileLists.length < 1) {
            
            showtoast('Error', 'Please  Upload Survey Report Files');
            return;

        }

        if ($scope.bankClient == undefined) {
            ShowMSG("WARNING", "Please Select Back Name", "");
            return;
        }
        if ($scope.WarehouseName == undefined) {
            ShowMSG("WARNING", "Please Select MobileNo Name", "");
            return;
        }
        //if ($scope.WareHouse.warehousename.value == undefined) {
        //    ShowMSG("WARNING", "Please Select Warehouse Name", "");

        //}
        debugger

        var id = "bankClient=" + $scope.bankClient + "&BANKWHCODE=" + $scope.BANKWHCODE + "&wHLicenseNo=" + $scope.wHLicenseNo + "&nameOfWHOwner=" + $scope.warehouseOwnName + "&wH_TYPE_ID=" + $scope.warehousetype + "&warehouseId=" + $scope.WarehouseId + "&state=" + $scope.state + "&InspectionReqDate=" + $scope.InspectionReqDate.toISOString() + "&WhClubbed=" + $scope.WhClubbed + "&warehouseName=" + $scope.WarehouseName + "&whaddress=" + $scope.whaddress + "&whcontact=" + $scope.whcontact + "&MobileNo=" + $scope.MobileNo + "&ContactPerson=" + $scope.ContactPerson + "&EmailID=" + $scope.EmailID + "&LocationID=" + $scope.locationID + "&capacity=" + $scope.capacity + "&PlinthHeight=" + $scope.PlinthHeight + "&TypeOfFlooring=" + $scope.TypeOfFlooring + "&TypeOfRoof=" + $scope.TypeOfRoof + "&Typeofgodown=" + $scope.typeofgodownn + "&categoryofgodown=" + $scope.categoryofgodown + "&authorofgodown=" + $scope.authorofgodownn + "&noofdoors=" + $scope.noofdoors + "&length=" + $scope.length + "&breadth=" + $scope.breadth + "&Height=" + $scope.Height + "&TypeManpower=" + $scope.TypeManpower + "&latitude=" + $scope.latitude + "&longitude=" + $scope.longitude + "&TakeOver_Status=" + $scope.TakeOver_Status + "&TakeOver_Date=" + $scope.TakeOver_Date.toISOString() + "&NoOfCm=" + $scope.NoOfCm + "&NoOfWM=" + $scope.NoOfWM + "&WHCharges=" + $scope.WHCharges + "&RequiredManpower=" + $scope.RequiredManpower + "&SurveyDate=" + $scope.SurveyDate.toISOString() + "&AgreementStartDate1=" + $scope.AgreementStartDate1.toISOString() + "&AgreementExpiryDate1=" + $scope.AgreementExpiryDate1.toISOString() + "&BankBranchName=" + $scope.BankBranchName + "&remarks=" + $scope.remarks + "&WORKINGSTATUS=" + $scope.WORKINGSTATUS;
        debugger
        $http({
            method: "POST",
            url: `/warehouse/AddWarehouseDetails` + `?` + id,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            //debugger
            if (response.data == "Success") {
                ShowMSG("Save", "Warehouse Request Updated  successfully", "index");
            }
            else {
                if (response.data == "SESSION") {
                   // exampleFunction();
                    ShowMSG("SESSION", "Your Session has been Ended!Please Login Again!", "../user/index");
                }
                else {
                    ShowMSG("WARNING", response.data, "");
                }
                // alert("Somehing is wrong. Please try again.");
            }
        }, function (error) {
            ShowMSG("ERROR", response.data, "");
            //console.log(error);
        });
    }

});