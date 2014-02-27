angular.module( 'hackathon').controller( 'GetInfoCards', function ( $scope, $http, $stateParams )
{
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

