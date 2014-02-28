angular.module( 'hackathon').controller( 'GetInfoCards', function ( $scope, $http, $stateParams, FacebookAuth )
{
  FacebookAuth.withAuth().then(function () {
    var fInd = $stateParams.index || '';
	
    if ((fInd === '') || (fInd === 0))
    {
      // get all
      $http.get( '/api/friends/' ).success( function ( data )
      {
        $scope.friendInfoCards = data;
      } );
    }
    else
    {
      // get specific
      $http.get( '/api/friends/' + fInd ).success( function ( data )
      {
        $scope.friendInfoCards = data;
      } );
	
    }
  });
});

angular.module('hackathon').controller('NotifyFriend', function ($scope, $http, $stateParams) 
{

	var toAdd = $stateParams.toAdd || '';
	var msg = $stateParams.toMsg || 'Hello there';
	
	$scope.notifyFriendSuccess = false;
	
	$http.get( '/api/notifyfriend/' + toAdd + '/' + msg ).success( function ( data )
	{
		$scope.notifyFriendSuccess = true;
	} );

});

