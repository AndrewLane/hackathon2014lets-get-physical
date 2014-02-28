"use strict";

(function () {
  var appModule = angular.module('hackathon', ['ui.router', 'ngTouch']);
  appModule.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', routeConfig]);
  function routeConfig($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.html5Mode(false).hashPrefix('');
    $urlRouterProvider.otherwise('/home');

    $stateProvider.state('home', {
      url: '/home',
      templateUrl: '/Client/partials/home.html',
      controller: 'Home'
    });

    $stateProvider.state('test', {
      url: '/test',
      templateUrl: '/Client/partials/test.html',
      controller: 'Test'
    });

    $stateProvider.state('leaderboard', {
      url: '/leaderboard/:index',
      templateUrl: '/Client/partials/leaderboard.html',
      controller: 'GetInfoCards'
    });

    $stateProvider.state('suggest', {
      url: '/suggest/:index',
      templateUrl: '/Client/partials/suggest.html',
      controller: 'Suggest'
    });

    $stateProvider.state('friend', {
      url: '/friend/:index',
      templateUrl: '/Client/partials/friend.html',
      controller: 'GetInfoCards'
    });

    $stateProvider.state('logintest', {
      url: '/logintest',
      templateUrl: '/Client/partials/logintest.html',
      controller: 'LoginTest'
    });
  }
}());

