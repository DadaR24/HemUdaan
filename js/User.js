var app = angular.module("Userapp", []);
app.controller("UserController", function ($scope, $http) {
    $scope.logindata = function () {        
        var query = "username=" + $scope.User.username + "&password=" + $scope.User.password;
        $http({
            method: "Get",
            url: `user/getData` + `?` + query,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (result) {            

            debugger
           
               
                if (result.data.responsestatus == "Success")
                {
                    window.location.href =result.data.responpage;
                }
                else if (result.data.responsestatus == "Error")
                {
                    window.location.href = result.data.responpage;
                }
                else if (result.data.responsestatus == "Info")
                {
                    Swal.fire(result.data.responsemsg);
                    
                }
                else {
                    Swal.fire(result.data.responsestatus);
                }

           
            // console.log("result", result);
           // alert("OK");             
            ///window.location.href = "../DMS/Index";
        }, function (error) {
            alert("Username Or Password Is wrong");
            console.log(error);
        });
    }
});