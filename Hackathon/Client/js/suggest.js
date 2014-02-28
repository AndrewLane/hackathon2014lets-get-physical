'use strict';

var ActivitySet =
  [
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
      actionDisabled: true
    },
    {
      type: "Sports",
      schedule: "May 27th, 2018",
      info: "Pacquiao vs Mayweather - Grudge Match",
      actionCaption: "Order PPV"
    },
    {
      type: "Sports",
      schedule: "June 12 to July 13",
      info: "2014 FIFA World Cup - Brazil",
      actionCaption: "Trip Planner"
    },
    {
      type: "Dinner",
      schedule: "Now until Mar 7",
      info: "Restaurant Week in NYC",
      actionCaption: "Pick Restaurant"
    },
    {
      type: "Movie",
      schedule: "Now",
      info: "The Lego Movie",
      actionCaption: "Find Theater Near You"
    }
  ];




(function () {
  angular.module('hackathon').controller('Suggest', ['$scope', '$http', '$timeout', '$stateParams', 'FacebookAuth', 'UserInfoCache', '$modal', Suggest]);
  function Suggest($scope, $http, $timeout, $stateParams, FacebookAuth, UserInfoCache, $modal) {
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
              $scope.activities = ActivitySet;
            });
          });
        })
      });

      function EventModal($scope, activity, $modalInstance) {
        $scope.create = function () {
          $modalInstance.close();
        };
        $scope.cancel = function () {
          $modalInstance.close();
        };
      }

      $scope.createEvent = function (i) {
        var modalInstance = $modal.open({
          templateUrl: '/Client/partials/create_event_modal.html',
          controller: EventModal,
          resolve: {
            activity: function () {
              return $scope.activities[i];
            }
          }
        });
      };

      //      });
    });
  }
}());
