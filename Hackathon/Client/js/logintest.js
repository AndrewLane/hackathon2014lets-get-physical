'use strict';

(function () {
  angular.module('hackathon').controller('LoginTest', ['$scope', '$http', 'Login', LoginTest]);
  function LoginTest($scope, $http, Login) {
    $scope.login = function () {
      FacebookLogin().SetAuth("dan", "TestToken");
      console.log("Setting Auth");
      $http.get("/api/logintest").then(function (response) {
        $scope.result = response.data;
      });
    };

    $scope.logout = function () {
      FacebookLogin().SetAuth("", "");
      console.log("Unsetting auth");
      $http.get("/api/logintest").then(function (response) {
        $scope.result = response.data;
      });
    };
  }
}());
