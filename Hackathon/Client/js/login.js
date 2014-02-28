'use strict';

(function () {
  angular.module('hackathon').factory('Login', ['$http', Login]);
  function Login($http) {
    return {
      SetAuth: function (userId, authToken) {
        console.log(userId);
        console.log(authToken);
        $http.defaults.headers.common['FacebookUserId'] = userId;
        $http.defaults.headers.common['FacebookAuthToken'] = authToken;
      },
      IsUserLoggedIn: function () {
        var hey = !!$http.defaults.headers.common['FacebookUserId'];
        console.log(hey);
        return hey;
      },
      LogUserOut: function () {
        delete $http.defaults.headers.common['FacebookUserId'];
      }

    };
  }
}());

window.FacebookLogin = function () {
  console.log(angular.element(document.getElementById('hackathon')));
  var injector = angular.element(document.getElementById('hackathon')).injector();
  console.log(injector);
  return injector.get("Login");
};