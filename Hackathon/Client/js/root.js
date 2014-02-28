'use strict';

(function () {
  angular.module('hackathon').controller('Root', ['$scope', 'FacebookAuth', 'Login', '$state', Root]);
  function Root($scope, FacebookAuth, Login, $state) {
    $scope.login = function () { FacebookAuth.login(); };
    $scope.logout = function () { FacebookAuth.logout(); };
    $scope.loggedIn = function () {
      return Login.IsUserLoggedIn();
    };
    FacebookAuth.withAuth().then();
    $scope.homeSweetHome = function () {
      return $state.is("home");
    };
  }
}());
