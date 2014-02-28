"use strict";
(function () {
  angular.module("hackathon").factory("FacebookAuth", ["Login", "$q", "$state", "$rootScope", FacebookAuth]);
  function FacebookAuth(Login, $q, $state, $rootScope) {
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
          $rootScope.$apply(function () {
            authQ.reject("Not authorized");
          });
          authQ = $q.defer();
          $state.transitionTo("/home");
          /*FB.login(function (response) {
            // handle the response
          });*/
        } else {
          $rootScope.$apply(function () {
            authQ.reject("Not authorized");
          });
          authQ = $q.defer();
          /*FB.login(function (response) {
            // handle the response
          });*/
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
          $rootScope.$apply(function () {
            authQ.resolve("Authorized!");
          });
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
      login: function() {
        FB.login(function (response) {$state.transitionTo("/leaderboard",{index:1}); $rootScope.$apply();},{scope:"email,user_about_me,user_checkins,friends_checkins,friends_about_me,user_activities,friends_activities,user_birthday,friends_birthday,user_events,friends_events,user_groups,friends_groups,user_hometown,friends_hometown,user_likes,friends_likes,user_location,friends_location,user_notes,friends_notes,user_photos,friends_photos,user_relationships,friends_relationships,user_relationship_details,friends_relationship_details,user_status,friends_status,read_stream,read_friendlists,user_videos,friends_videos"});
      },
      logout: function () {
        FB.logout(function() {$state.transitionTo("/home"); $rootScope.$apply(); });
      },
      withAuth: function () {
        return authQ.promise;
      },
    };
  }
}());