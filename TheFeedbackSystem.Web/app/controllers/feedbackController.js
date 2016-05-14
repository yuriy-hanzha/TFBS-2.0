'use strict';
app.controller('feedbackController', ['$scope', '$http', 'ngAuthSettings',
    function ($scope, $http, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        $scope.comments = [];
        $scope.currentUserAvatar = "";
        $scope.currentUserName = "";

        function Comment(author, text, likes, dislikes, avatar) {
            this.author = author;
            this.text = text;
            this.likes = likes;
            this.dislikes = dislikes;
            this.avatar = avatar;
        };

        $scope.AddNewComment = function () {
            if ($scope.newCommentText != undefined && $scope.newCommentText != null && $scope.newCommentText != '' & $scope.newCommentText.trim() != '') {
                var value = $scope.newCommentText;
                $http.post(serviceBase + 'api/feedback/add', { 'commentText': $scope.newCommentText })
                .then(function (res) {
                    var success = JSON.parse(res.data)
                    if (success) {
                        $scope.comments.push(
                            new Comment($scope.currentUserName, $scope.newCommentText, 0, 0, $scope.currentUserAvatar));
                        $scope.txtcomment = "";
                    }
                });
            }
        };

        $scope.VoteUp = function(index) {
            $http.post(serviceBase + 'api/feedback/like', { 'name': $scope.comments[index].author, 'text': $scope.comments[index].text })
                .then(function(res) {
                    var responce = JSON.parse(res.data);
                    responce = JSON.parse(responce);
                    if (responce.res == '+') {
                        $scope.comments[index].likes++;
                    }
                    else if (responce.res == '-') {
                        $scope.comments[index].likes--;
                    }
                });
        };

        $scope.VoteDown = function(index) {
            $http.post(serviceBase + 'api/feedback/dislike', { 'name': $scope.comments[index].author, 'text': $scope.comments[index].text })
                .then(function(res) {
                    var responce = JSON.parse(res.data);
                    responce = JSON.parse(responce);
                    if (responce.res == '+') {
                        $scope.comments[index].dislikes++;
                    }
                    else if (responce.res == '-') {
                        $scope.comments[index].dislikes--;
                    }
                });
        };

        $scope.$on('$viewContentLoaded', function () {
            $http.get(serviceBase + 'api/feedback/getinitdata')
                .then(function (res) {
                    var data = JSON.parse(res.data);
                    var respComm;
                    data = JSON.parse(data);
                    $scope.currentUserName = data[0];
                    $scope.currentUserAvatar = data[1];
                    respComm = data[2];
                    for (var i = 0; i < data[2].length; i++) {
                        $scope.comments.push(
                            new Comment(respComm[i].author, respComm[i].text, respComm[i].likes, respComm[i].dislikes, respComm[i].avatar));
                    }
                });
        });
    }
]);