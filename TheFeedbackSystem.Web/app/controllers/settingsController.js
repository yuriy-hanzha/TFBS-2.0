'use strict';
app.controller('settingsController', ['$scope', '$http', 'ngAuthSettings',
function ($scope, $http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $scope.myImage = '';
    $scope.myCroppedImage = '';

    var handleFileSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        var reader = new FileReader();
        reader.onload = function (evt) {
            $scope.$apply(function ($scope) {
                $scope.myImage = evt.target.result;
            });
        };
        reader.readAsDataURL(file);
        document.getElementById("avatarDiv").style.display = "block";
    };

    angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

    $scope.saveImg = function () {
        $http.post(serviceBase + 'api/settings/SaveImage', { 'img': $scope.myCroppedImage })
            .then(function (res) {
                if (res.status == 200)
                    document.getElementById("avatarDiv").style.display = "none";
            });
    };

    $scope.CanselImg = function () {
        document.getElementById("avatarDiv").style.display = "none";
    };
}]);
