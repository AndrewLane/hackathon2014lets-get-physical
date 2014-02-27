window.fbAsyncInit = function() {
    FB.init({
      appId      : '582451755180049',
      status     : true,
      //cookie     : true,
      xfbml      : true
    });

    var permissions = {scope:'email,user_about_me,user_checkins,friends_checkins,friends_about_me,user_activities,friends_activities,user_birthday,friends_birthday,user_events,friends_events,user_groups,friends_groups,user_hometown,friends_hometown,user_likes,friends_likes,user_location,friends_location,user_notes,friends_notes,user_photos,friends_photos,user_relationships,friends_relationships,user_relationship_details,friends_relationship_details,user_status,friends_status,read_stream,read_friendlists'};

    FB.Event.subscribe('auth.authResponseChange', function(response) {
        if (response.status === 'connected') {
            testAPI();
        } else if (response.status === 'not_authorized') {
            FB.login(function(response) {
               // handle the response
             }, permissions);
        } else {
            FB.login(function(response) {
               // handle the response
             }, {scope:'email,user_about_me,user_checkins,friends_checkins,friends_about_me,user_activities,friends_activities,user_birthday,friends_birthday,user_events,friends_events,user_groups,friends_groups,user_hometown,friends_hometown,user_likes,friends_likes,user_location,friends_location,user_notes,friends_notes,user_photos,friends_photos,user_relationships,friends_relationships,user_relationship_details,friends_relationship_details,user_status,friends_status,read_stream,read_friendlists'});
        }
    });
};

(function(d, s, id){
 var js, fjs = d.getElementsByTagName(s)[0];
 if (d.getElementById(id)) {return;}
 js = d.createElement(s); js.id = id;
 js.src = "//connect.facebook.net/en_US/all.js";
 fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function testAPI() {
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function(response) {
        console.log('Good to see you, ' + response.name + '.');
    });
    FB.getLoginStatus(function(response) {
        if (response.status === 'connected') {

        var uid = response.authResponse.userID;
        var accessToken = response.authResponse.accessToken;
        globalUID = uid;
        globalAuthToken = accessToken;
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

var globalUID = '';
var globalAuthToken = '';