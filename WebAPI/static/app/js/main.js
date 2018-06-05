var app = angular.module("ppa", ["ngRoute"]);

app.config(["$routeProvider", function($routeProvider) {
	$routeProvider
	.when('/', {
		templateUrl: '/static/app/html/partial/movies.html'
	})
    .when('/movies', {
        templateUrl: '/static/app/html/partial/movies.html'
        })
    .when('/actors', {
        templateUrl: '/static/app/html/partial/actors.html'
        })
    .when('/movie_details', {
        templateUrl: '/static/app/html/partial/movie_details.html'
    })
	.otherwise({
		redirectTo: '/'
	});
}
]);

//Data-share via service
app.service("DataShare", function(){
    var id = "";
	
	var addId = function(mId) {
		id = mId;
	};

	var getId = function(){
		return id;
	};

	return {addId: addId, getId: getId};

});

//==============================================================================================
//								MOVIES CONTROLLER
//==============================================================================================
app.controller("moviesCtrl", function ($scope, $location, $http, $routeParams, DataShare, $rootScope) {

    var URLmovies = "/api/movies";

    $scope.movies = [];
    $scope.countries = ["FRA", "ITA", "GER"];
    $scope.languages = ["FRA", "ITA", "GER"];

    $scope.TotalMovies = "";
    $scope.TotalPages = "";
    $scope.pageNum = 0;

    $scope.newMovie = {};
    $scope.newMovie.Title = "";
    $scope.newMovie.Year = "";
    $scope.newMovie.Genres = "";
    $scope.newMovie.Country = "";
    $scope.newMovie.Language = "";
    $scope.newMovie.ReleaseDate = "";
    $scope.newMovie.Storyline = "";
    $scope.newMovie.Directors = "";
    $scope.newMovie.Actors = "";

    $scope.movieName = "";

    //==============================================================================================
    //	CRUD METODE
    //==============================================================================================

    var getMovies = function () {

        var config = { params: {} };

        if ($scope.movieName != "") {
            config.params.movieName = $scope.movieName;
        }
        config.params.pageNum = $scope.pageNum;

        var promise = $http.get(URLmovies, config);
        console.log(config);
        promise.then(
            function success(response) {
                console.log(response.data)
                $scope.movies = response.data.Movies;
                $scope.TotalPages = response.data.TotalPages;
                $scope.TotalMovies = response.data.TotalMovies;
            },
            function error(response) {
                console.log(response.data);
            }
        );
    };

    getMovies();

    $scope.save = function () {
        if ($scope.newMovie.Id == null) {
            var promise = $http.post(URLmovies, $scope.newMovie);
            promise.then(
                function success(response) {
                    alert("Uspesno ste dodali film!");
                    getMovies();
                    $scope.newMovie = null;
                },
                function error(response) {
                    alert("Greska pri dodavanju filma!");
                    console.log(response.data);
                }
            );
        } else {
            var promise = $http.put(URLmovies + "/" + $scope.newMovie.Id, $scope.newMovie);
            promise.then(
                function success(response) {
                    alert("Uspesno ste izmenili film!");
                    getMovies();
                    $scope.newMovie = null;
                },
                function error(response) {
                    alert("Nije moguce izmeniti film!");
                    console.log(response.data);
                }
            );
        }

    };

    $scope.editHere = function (Id) {
        var promise = $http.get(URLmovies + "/" + Id);
        promise.then(
            function success(response) {
                $scope.newMovie = response.data;
            },
            function error(response) {
                alert("Nije moguce dobaviti film za izmenu!");
                console.log(response.data);
            }
        );
    };

    $scope.delete = function (Id) {
        var promise = $http.delete(URLmovies + "/" + Id);
        promise.then(
            function success(response) {
                getMovies();
            },
            function error(response) {
                alert("Nije moguce obrisati film!");
                console.log(response.data);
            }
        );
    }

    $scope.view = function (mId) {
        DataShare.addId(mId);
        $location.path('/movie_details');
    }

    //==============================================================================================
    //	DODATNE METODE
    //==============================================================================================


    // CHANGE VIEW Method
    $scope.option = "Pretraga";
    $scope.optionText = "Unos novog filma";
    $scope.iconClass = "glyphicon glyphicon-search";
    $scope.show = true;

    $scope.changeView = function () {
        $scope.show = !$scope.show;
        if (!$scope.show) {
            $scope.option = "Unos";
            $scope.iconClass = "glyphicon glyphicon-plus";
            $scope.optionText = "Pretraga filmova";
        } else {
            $scope.option = "Pretraga";
            $scope.iconClass = "glyphicon glyphicon-search";
            $scope.optionText = "Unos novog filma";
        }
    }

    $scope.search = function () {
        $scope.pageNum = 0;
        getMovies();
    };


    // PROMENA STRANICE
    // >> NEXT method
    $scope.next = function () {
        if ($scope.pageNum < $scope.TotalPages - 1) {
            $scope.pageNum = $scope.pageNum + 1;
            getMovies();
        }
    };

    // << PREV method
    $scope.prev = function () {
        if ($scope.pageNum > 0) {
            $scope.pageNum = $scope.pageNum - 1;
            getMovies();
        }
    };
});

//==============================================================================================
//								MOVIE DETAILS CONTROLLER
//==============================================================================================
app.controller("movieDetailsCtrl", function ($scope, $location, $http, $routeParams, DataShare) {

    var URLmovies = "/api/movies";
    var URLactors = "/api/actors";
    $scope.movie = {};
    $scope.movieId = DataShare.getId();
    $scope.actors = [];

    //==============================================================================================
    //	CRUD METODE
    //==============================================================================================

    var getMovie = function () {
        var promise = $http.get(URLmovies + "/" + movieId);
        promise.then(
            function success(response) {
                $scope.actors = response.data;
            },
            function error(response) {
                console.log(response.data);
            }
        );
    };

    var getActors = function () {
        var promise = $http.get(URLactors + "/MovieId/" + movieId);
        promise.then(
            function success(response) {
                $scope.actors = response.data;
            },
            function error(response) {
                console.log(response.data);
            }
        );
    };

   

    getActors();
    getMovie();

    //==============================================================================================
    //	DODATNE METODE
    //==============================================================================================

    $scope.back = function () {
        $location.path('/movies');
    };

});

//==============================================================================================
//								ACTORS CONTROLLER
//==============================================================================================
app.controller("actorsCtrl", function ($scope, $location, $http, $routeParams, DataShare) {

    URLactors = "/api/actors";
    $scope.actorsByMovieId = [];
    $scope.newActor = {};
    $scope.newActor.Name = "";
    $scope.newActor.Character = "";

    $scope.movieId = DataShare.getId();

    //==============================================================================================
    //	CRUD METODE
    //==============================================================================================

    $scope.save = function () {
        var promise = $http.post(URLactors, $scope.newActor);
        promise.then(
            function success(response) {
                alert("Uspesno ste dodali glumca!");
                getActorsByMovieId();
                $scope.newActor = null;
            },
            function error(response) {
                alert("Greska pri dodavanju glumca!");
                console.log(response.data);
            }
       );
    };

    //==============================================================================================
    //	DODATNE METODE
    //==============================================================================================

    $scope.back = function () {
        $location.path('/movies');
    };

});

