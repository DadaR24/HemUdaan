var app = angular.module("approval", []);
app.controller("approvalController", function ($scope, $http) {
       angular.element(document).ready(function () {
        //debugger
       // var locationId = "locationId=" + 0;
        $http({
            method: "Get",
            url: `/Approval/BindApprvalHomePage`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
            // debugger
            $scope.Approval = response.data.approvalLists;
           

        }, function (error) {
            console.log(error);
        });


    });
});