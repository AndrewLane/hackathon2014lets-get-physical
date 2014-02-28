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

    function setCookie(cname,cvalue,exdays) {
      var d = new Date();
      d.setTime(d.getTime()+(exdays*24*60*60*1000));
      var expires = "expires="+d.toGMTString();
      document.cookie = cname + "=" + cvalue + "; " + expires;
    }
    function getCookie(cname) {
      var name = cname + "=";
      var ca = document.cookie.split(';');
      for(var i=0; i<ca.length; i++)
        {
        var c = ca[i].trim();
        if (c.indexOf(name)==0) return c.substring(name.length,c.length);
      }
      return "";
    }

    $scope.recordAction = function() {
        var curval = getCookie("actionCount");
        if(curval) {
            setCookie("actionCount",parseInt(curval) + 1,1);
        } else {
            setCookie("actionCount",1,1);
        }
    };

    $scope.getActionCount = function() {
        var curval = getCookie("actionCount");
        if(curval) {
            return curval;
        }
        return 0;
    }
  }
}());
