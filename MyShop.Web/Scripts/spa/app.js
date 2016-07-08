/// <reference path="../plusgin/angular/angular.js" />

var myApp = angular.module('myModule', []);

myApp.controller("schoolController", schoolController);
myApp.service("ValidatorService", ValidatorService);
myApp.directive("myShopDirective", myShopDirective);

schoolController.$inject['$scope', 'ValidatorService'];
//khai báo
function schoolController($scope,ValidatorService) {
   
    $scope.checkNumber=function (){
        $scope.message=  ValidatorService.checkNumber($scope.num);
    }
    $scope.num = 1;
}

function ValidatorService($window) {
    return {
        checkNumber: checkNumber
    }
    function checkNumber(input) {
        if (input % 2 == 0)
            return "This is even";
        else
            return "This is odd";
    }
}
function myShopDirective() {
    return {
        templateUrl: "/Scripts/spa/myShopDirective.html"
    }
}