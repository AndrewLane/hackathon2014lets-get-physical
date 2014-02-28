'use strict';

(function () {
  angular.module('hackathon').controller('Root', ['$scope', 'FacebookAuth', 'Login', Root]);
  function Root($scope, FacebookAuth, Login) {
    $scope.login = function () { FacebookAuth.login(); };
    $scope.logout = function () { FacebookAuth.logout(); };
    $scope.loggedIn = function () {
      return Login.IsUserLoggedIn();
    };
    FacebookAuth.withAuth().then();
  }
}());
