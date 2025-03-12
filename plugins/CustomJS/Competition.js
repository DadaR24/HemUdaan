var app = angular.module("Competitionapp", ['chieffancypants.loadingBar', 'ngAnimate']);
app.config(function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = true;
});
app.controller("CompetitionController", function ($scope, $http, $location, $timeout, cfpLoadingBar) {
    $scope.config = location.config;
    $scope.model = location.model;
    angular.element(document).ready(function () {
        //debugger
        //var queryParams = $location.search().myParam;
        //alert(queryParams);
       
        $scope.start(); $http({
            method: "Get",
            url: `../CompetitionDashboard/BindCompetititonDashboard`,
            cache: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(function (response) {
             debugger
            $scope.games = response.data.Games;
            $scope.unique = function (item) {
                return item; // This function simply returns the item itself
            };
            $scope.uniqueGames = [];
            angular.forEach($scope.games, function (game) {
                // Check if the game with this mainGameName already exists
                if ($scope.uniqueGames.findIndex(existingGame => existingGame.MainGameName === game.MainGameName) === -1) {
                    $scope.uniqueGames.push(game);
                }
            });


            $scope.complete();
        }, function (error) {
            console.log(error);
        });

       
       
    });
    $scope.selectedGame = null;
    $scope.callJavaScriptFunction = function (funcName, url) {
        debugger;
        console.log('Function name:', funcName, 'URL:', url); // Debug: Log input

        // Check if the function exists in the global scope
        if (typeof window[funcName] === 'function') {
            window[funcName](url);  // Call the function dynamically with the URL
        } else {
            console.error('Function not found:', funcName);
        }
    };
    $scope.startGame = function (gameName) {
        debugger
        $scope.selectedGame = gameName;
    };
    $scope.Game24 = function () {
        alert('test');
        ShowMSG('Rules', `In this game, you have to use 4 mathematical operators (+, -, *, /) in such a way that the result is 24. <br> You can use any sign but each of the four numbers given can be used only once.<br> It improves our mathematical skills problem solving skill and develops deep thinking. <br> It also develops a great sense of logic \nOR \n Click on the “Start Game” button. <br> Solve the Game 24 Activity.`, '../Game24/Index');

    };
    function Game24(url) {

        ShowMSG('Rules', `In this game, you have to use 4 mathematical operators (+, -, *, /) in such a way that the result is 24. <br> You can use any sign but each of the four numbers given can be used only once.<br> It improves our mathematical skills problem solving skill and develops deep thinking. <br> It also develops a great sense of logic \nOR \n Click on the “Start Game” button. <br> Solve the Game 24 Activity.`, '../Game24/Index');

    }

    function Futoshiki(url) {
        ShowMSG('Rules', `Futoshiki is a game consisting of greater than and less than sign <br> In 4x4 grid, the numbers 1, 2, 3 and 4 are to be filled in such a way that they should fulfill the inequalities signs provided. Numbers 1, 2, 3 and 4 gets repeated either in the row or in the column <br>  Click on the “Start Game” button <br> Solve the Futoshiki Activity.`, '../Futoshiki/Index');

    }

    function Kenken(url) {
        ShowMSG('Rules', `Ken Ken has an equal number of rows and columns in 3x3 grid. These grids have cages which can be identified by colors.
                    <br> Your task is to fill numbers 1, 2, or 3 in the cage such that their addition (+) , subtraction (-), multiplication(x) or division(/) will result in the number shown in the cage.
                    <br> Click on the “Start Game” button.
                    <br> Solve the Ken Ken Activity.`, '../Kenken/Index');

    }

    function Puzzle(url) {
        ShowMSG('Rules', `From a given set of laiddown alphabets, participants will have to search for words – horizontally, vertically, or diagonally.
                    <br> It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br> Solve the Puzzle Activity.`, '../Puzzle/Puzzle');

    }

    function Scramble(url) {
        ShowMSG('Rules', `In this word game, the letters of a word are scrambled.
                    <br> Participants will have to arrange the letters to form a meaningful word.
                    <br> It also develops a great sense of logic.
                    <br>  After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br> Solve the Scramble Activity.`, '../Scramble/Scramble');

    }

    function WordGame(url) {
        ShowMSG('Rules', `From the letters of a given  word, participants will have to form words based on the clues given for each word.
                    <br>  It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br>  Solve the WordGame Activity.`, '../WordGame/Index');

    }
    function Quiz1(url) {
        ShowMSG('Rules', `From the letters of a given  word, participants will have to form words based on the clues given for each word.
                    <br>  It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br>  Solve the WordGame Activity.`, '../Quiz/Index?QuizId=332&TopicID=6256&LevelID=1&LanguageID=1&IsInstituteQuiz=No&IsAttempt=False&ExamID=10&IsDB=SE');
    }

    function Quiz2(url) {
        ShowMSG('Rules', `From the letters of a given  word, participants will have to form words based on the clues given for each word.
                    <br>  It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br>  Solve the WordGame Activity.`, '../Quiz/Index?QuizId=332&TopicID=6257&LevelID=1&LanguageID=1&IsInstituteQuiz=No&IsAttempt=False&ExamID=10&IsDB=SE');
    }


    function Quiz3(url) {
        ShowMSG('Rules', `From the letters of a given  word, participants will have to form words based on the clues given for each word.
                    <br>  It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br>  Solve the WordGame Activity.`, '../Quiz/Index?QuizId=332&TopicID=6258&LevelID=1&LanguageID=1&IsInstituteQuiz=No&IsAttempt=False&ExamID=10&IsDB=SE');
    }

    function Quiz4(url) {
        ShowMSG('Rules', `From the letters of a given  word, participants will have to form words based on the clues given for each word.
                    <br>  It also develops a great sense of logic.
                    <br> After solving one question you will be moved to next one.
                    <br> Click on the “Start Game” button.
                    <br>  Solve the WordGame Activity.`, '../Quiz/Index?QuizId=332&TopicID=6259&LevelID=1&LanguageID=1&IsInstituteQuiz=No&IsAttempt=False&ExamID=10&IsDB=SE');
    }
    //$scope.filterSubGames = function (game, mainGameName) {

    //    debugger
    //    return game.mainGameName === mainGameName;
    //};

    //$scope.filterSubGames = function (games, mainGameName) {
    //    // ... your filter logic here ...
    //    console.log("Filtered Games:", filteredGames); // Log the filtered array
    //    return filteredGames;
    //};


    //$scope.filterSubGames = function (game, mainGameName) {
    //    debugger
    //    return game.mainGameName === mainGameName;
    //};
    $scope.start = function () {
        cfpLoadingBar.start();
    };

    $scope.complete = function () {
        cfpLoadingBar.complete();
    }

    $scope.start();
    $scope.fakeIntro = true;
    $timeout(function () {
        $scope.complete();
        $scope.fakeIntro = false;
    }, 750);
});