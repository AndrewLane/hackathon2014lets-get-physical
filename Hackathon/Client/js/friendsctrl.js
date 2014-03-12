angular.module('hackathon').controller('GetInfoCards', function ($scope, $http, $state, $stateParams, FacebookAuth, UserInfoCache) {
  FacebookAuth.withAuth().then(function () {
    var fInd = $stateParams.index || '';
    $scope.index = parseInt(fInd);
    $scope.alreadyliked_bool = false;

    $scope.alreadyliked = function() {
        return $scope.alreadyliked_bool
    }

    if ((fInd === '') || (fInd === 0)) {
      // get all
      $http.get('/api/friends/').success(function (data) {
        $scope.friendInfoCards = data;
      });
    }
    else {
      // get specific
      $http.get('/api/friends/' + fInd).then(function (response) {
        $scope.friendInfoCards = response.data;
        UserInfoCache.put(response.data.FriendId, response.data);
        $http.get('/api/friend/' + $scope.friendInfoCards.FriendId).success(function (data) {
          $scope.userExtraInfo = data;
          var statusId = $scope.userExtraInfo.LastStatusId;
          FB.api('/' + statusId + '/likes',function(response) {
              $scope.$apply(function() {
                  var aliked_tmp = false;
                  var responseData = response.data
                  for(i=0;i<responseData.length;i++) {
                      if(responseData[i].id === $http.defaults.headers.common['FacebookUserId']) {
                          aliked_tmp = true;
                      }
                  }
                  $scope.alreadyliked_bool = aliked_tmp;
              });
          });
        });
      }, function () {
        $state.transitionTo("leaderboard", { index: 1 });
      });
    }
    function friendID() {
      return $scope.friendInfoCards.FriendId;
    }
    $scope.sendMessage = function () {
      FB.ui({
        method: 'send',
        link: 'http://hackathonletsgetphysical.apphb.com/Client/partials/send_message_static.html',
        to: friendID()
      });
      $scope.recordAction();
    };

    $scope.postFeed = function() {
      FB.ui({
        method:'feed'
        name: 'Facebook Dialogs'
        link: 'https://developers.facebook.com/docs/dialogs/'
        description: 'This is a description'
        caption: 'This is a caption'

      });
    };

    $scope.suggestActivity = function () {
      $scope.recordAction();
      $state.transitionTo("suggest", { index: $scope.friendInfoCards.FriendId });
    };

    $scope.gotoPrevious = function () {
      if ($scope.index > 1) {
        $state.transitionTo("leaderboard", { index: $scope.index - 1 });
      }
    }
    $scope.gotoNext = function () {
      $state.transitionTo("leaderboard", { index: $scope.index + 1 });
    }

    $scope.like = function() {
      var statusId = $scope.userExtraInfo.LastStatusId;
      FB.api('/' + statusId + '/likes','post',function(response) {});
      $scope.alreadyliked_bool = true;
      $scope.recordAction();
    }

  });
});

angular.module('hackathon').controller('GetExtraInfo', function ($scope, $http, $stateParams, FacebookAuth) {
  FacebookAuth.withAuth().then(function () {
    $http.get('/api/friend/' + $stateParams.uid).success(function (data) {
      $scope.userExtraInfo = data;
    });
  });
});

angular.module('hackathon').controller('NotifyFriend', function ($scope, $http, $stateParams) {
  var toAdd = $stateParams.toAdd || '';
  var msg = $stateParams.toMsg || 'Hello there';

  $scope.notifyFriendSuccess = false;

  $http.get('/api/notifyfriend/' + toAdd + '/' + msg).success(function (data) {
    $scope.notifyFriendSuccess = true;
  });
});

