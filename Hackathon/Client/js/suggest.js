'use strict';

(function () {
  angular.module('hackathon').controller('Suggest', ['$scope', '$http', '$timeout', '$stateParams', 'FacebookAuth', 'UserInfoCache', Suggest]);
  function Suggest($scope, $http, $timeout, $stateParams, FacebookAuth, UserInfoCache) {
    FacebookAuth.withAuth().then(function () {
//      $http.get('/api/friend/' + $stateParams.index).success(function (data) {
      //        $scope.userInfo = data;
      $scope.friends = [];
      for (var i = 10; i < 15; i++) {
        $http.get('/api/friends/' + i).then(function (response) {
          $scope.friends.push(response.data);
        });
      }
      $scope.userInfo = UserInfoCache.get($stateParams.index);
        $timeout(angular.noop, 2000).then(function () {
          $scope.step = 1;
          $timeout(angular.noop, 2000).then(function () {
            $scope.step = 2;
            $timeout(angular.noop, 2000).then(function () {
              $scope.step = 3;
              $timeout(angular.noop, 2000).then(function () {
                $scope.done = true;
                $scope.activities = [
                  {
                    type: "Theatre",
                    schedule: "Opens March 23 2014",
                    info: "Watch Les Misérables on Broadway",
                    actionCaption: "Buy tickets"
                  },
                  {
                    type: "Brunch",
                    schedule: "Ongoing",
                    info: "ABC Kitchen in Union Square",
                    actionCaption: "Make reservation"
                  },
                  {
                    type: "Dinner",
                    schedule: "Ongoing",
                    info: "Kunjip in K-Town (Herald Square)",
                    actionCaption: "Just be there",
                    actionDisabled:true
                  },
                  {
                    type: "Sports",
                    schedule: "May 27th, 2018",
                    info: "Pacquiao vs Mayweather - Grudge Match",
                    actionCaption: "Order PPV"
                  }
                ];
              });
            });
          })
        });
//      });
    });
  }
}());
