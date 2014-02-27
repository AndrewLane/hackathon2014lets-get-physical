angular.module( 'hackathon').controller( 'GetFriends', function ( $scope, $http, $stateParams )
{
	$http.get( '/api/friends/' + $stateParams.index || "" ).success( function ( data )
	{
		$scope.friendInfoCards = data;
	} );
});

