angular.module('hackathon').controller('GetInfoCards', function ($scope, $http, $stateParams, FacebookAuth) {
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
      }, function () {
        $state.transitionTo("leaderboard", { index: 1 });
      });
    }
    $scope.sendMessage = function() {
      FB.ui({
          method:'send',
          link:'http://google.com',
          to:$scope.friendInfoCards.FriendID
      });

    }

  });
});

angular.module('hackathon').controller('GetExtraInfo', function ($scope, $http, $stateParams, FacebookAuth) {
  FacebookAuth.withAuth().then(function () {
    $http.get('/api/friend/' + $stateParams.uid).success(function (data) {
      $scope.userExtaInfo = data;
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

