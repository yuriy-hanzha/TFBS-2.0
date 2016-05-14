'use strict';
app.controller('confirmationController', ['$scope', '$http', '$routeParams', 'ngAuthSettings',
    function ($scope, $http, $routeParams, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        $scope.id = $routeParams.id;
        $scope.message = "400 (Bad Request)";

        $scope.$on('$viewContentLoaded', function () {
            $http.post(serviceBase + 'api/confirmation/check', { "id": $scope.id })
                .then(function (res) {
                    if (res.status == 200) {
                        var data = JSON.parse(res.data);
                        data = JSON.parse(data);
                        $scope.message = data.name + ", thanks for registration!";
                    }
                    else
                        $scope.message = "400 Bad Request";
                });
        });
    }
]);