'use strict';

(function () {
  angular.module('hackathon').controller('Leaderboard', ['$scope', '$state', '$stateParams', '$http', Leaderboard]);
  function Leaderboard($scope, $state, $stateParams, $http) {
    console.log("Leaderboard Controller");
    $scope.testMessage = "Test Message for Leaderboard";
  }
}());
