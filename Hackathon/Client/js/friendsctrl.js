angular.module('hackathon').controller('GetInfoCards', function ($scope, $http, $state, $stateParams, FacebookAuth, UserInfoCache) {
  FacebookAuth.withAuth().then(function () {
    var fInd = $stateParams.index || '';
    $scope.index = parseInt(fInd);

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
      })
    };

    $scope.suggestActivity = function () {
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

