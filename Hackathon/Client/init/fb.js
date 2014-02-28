"use strict";
(function () {
  angular.module("hackathon").factory("FacebookAuth", ["Login", "$q", "$state", FacebookAuth]);
  function FacebookAuth(Login, $q, $state) {
    var authQ = $q.defer();

    window.fbAsyncInit = function () {
      FB.init({
        appId: '582451755180049',
        status: true,
        //cookie     : true,
        xfbml: true
      });

      FB.Event.subscribe('auth.authResponseChange', function (response) {
        if (response.status === 'connected') {
          testAPI();
        } else if (response.status === 'not_authorized') {
          authQ.reject("Not authorized");
          authQ = $q.defer();
          $state.transitionTo("/home");
          FB.login(function (response) {
            // handle the response
          });
        } else {
          FB.login(function (response) {
            // handle the response
          });
        }
      });
    };
    (function (d, s, id) {
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { return; }
      js = d.createElement(s); js.id = id;
      js.src = "//connect.facebook.net/en_US/all.js";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function testAPI() {
      console.log('Welcome!  Fetching your information.... ');
      FB.api('/me', function (response) {
        console.log('Good to see you, ' + response.name + '.');
      });
      FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {

          var uid = response.authResponse.userID;
          var accessToken = response.authResponse.accessToken;
          FacebookLogin().SetAuth(uid, accessToken);
          authQ.resolve("Authorized!");
          console.log('uid: ' + uid);
          console.log('accessToken: ' + accessToken);
        } else if (response.status === 'not_authorized') {
          // the user is logged in to Facebook,
          // but has not authenticated your app
        } else {
          // the user isn't logged in to Facebook.
        }
      });
    }
    return {
      withAuth: function () {
        return authQ.promise;
      },
    };
  }
}());