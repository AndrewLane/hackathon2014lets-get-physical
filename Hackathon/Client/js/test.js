'use strict';

(function () {
  angular.module('hackathon').controller('Test', ['$scope', '$state', '$stateParams', '$http', Test]);
  function Test($scope, $state, $stateParams, $http) {
    console.log("Test Controller");
    $scope.testMessage = "Test Message";
  }
}());
