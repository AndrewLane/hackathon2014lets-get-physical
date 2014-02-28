'use strict';

(function () {
  angular.module('hackathon').factory('UserInfoCache', ['$cacheFactory', UserInfoCache]);
  function UserInfoCache($cacheFactory) {
    return $cacheFactory("UserInfo");
  }
}());
