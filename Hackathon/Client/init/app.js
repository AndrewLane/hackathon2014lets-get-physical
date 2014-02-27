"use strict";

(function () {
  var appModule = angular.module('hackathon', ['ui.router']);
  appModule.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', routeConfig]);
  function routeConfig($stateProvider, $urlRouterProvider, $locationProvider) {
    $locationProvider.html5Mode(false).hashPrefix('');
    $urlRouterProvider.otherwise('/home');

    $stateProvider.state('home', {
      url: '/home',
      templateUrl: '/Client/partials/home.html'
    });

    $stateProvider.state('test', {
      url: '/test',
      templateUrl: '/Client/partials/test.html',
      controller: 'Test'
    });
  }
}());
