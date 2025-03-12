// the game itself
var game;
// the spinning wheel
var wheel;
// can the wheel spin?
var canSpin;
// slices (prizes) placed in the wheel
var slices = 8;
// prize names, starting from 12 o'clock going clockwise
if (PaidStatus == 'True' || PaidStatus == 'true') {
    var slicePrizes = ["100 Coins", "250 Coins", "1000 Coins", "25 Gems", "500 Coins", "5000 Coins", "50 Coins", "50 Gems"];
} else {
    var slicePrizes = ["MI Power Bank", "MI POCO F1", "MI SOUND BAR", "MI TRUCK BUILDER", "MI SPEAKER", "MI EAR PHONE", "MI REDMI 7S", "MI BAND 2"];
}

// the prize you are about to win
var prize;
// text field where to show the prize
var prizeText;

var text = '';
var titletext = '';

window.onload = function () {
    // creation of a 458x488 game
    game = new Phaser.Game(458, 488, Phaser.AUTO, "");
    // adding "PlayGame" state
    game.state.add("PlayGame", playGame);
    // launching "PlayGame" state
    game.state.start("PlayGame");
}
// PLAYGAME STATE

var playGame = function (game) { };

playGame.prototype = {
    // function to be executed once the state preloads
    preload: function () {
        // preloading graphic assets
        if (PaidStatus == 'True' || PaidStatus == 'true') {
            game.load.image("wheel", "/Content/img/wheel/wheel2.png");
            game.load.image("pin", "/Content/img/wheel/pin.png");
        } else {
            game.load.image("wheel", "/Content/img/wheel/Wheel-new-final.png");
            game.load.image("pin", "/Content/img/wheel/S-pin.png");
        }
    },
    // funtion to be executed when the state is created
    create: function () {
        // giving some color to background
        game.stage.backgroundColor = "#ffffff";
        // adding the wheel in the middle of the canvas
        wheel = game.add.sprite(game.width / 2, game.width / 2, "wheel");
        // setting wheel registration point in its center
        wheel.anchor.set(0.5);
        // adding the pin in the middle of the canvas
        var pin = game.add.sprite(game.width / 2, game.width / 2, "pin");
        // setting pin registration point in its center
        pin.anchor.set(0.5);
        // adding the text field
        prizeText = game.add.text(game.world.centerX, 480, "");
        // setting text field registration point in its center
        prizeText.anchor.set(0.5);
        // aligning the text to center
        prizeText.align = "center";
        // the game has just started = we can spin the wheel
        canSpin = true;
        // waiting for your input, then calling "spin" function
        game.input.onDown.add(this.spin, this);
    },
    // function to spin the wheel
    spin() {
        // can we spin the wheel?
        if (canSpin) {
            // resetting text field
            prizeText.text = "";
            // the wheel will spin round from 2 to 4 times. This is just coreography
            var rounds = game.rnd.between(2, 4);
            // then will rotate by a random number from 0 to 360 degrees. This is the actual spin
            var degrees = game.rnd.between(0, 360);
            // before the wheel ends spinning, we already know the prize according to "degrees" rotation and the number of slices
            if (PaidStatus == 'False' || PaidStatus == 'false') {
                if (SpinStatus == 'True' || SpinStatus == 'true') {
                    degrees = 270;
                }
            }

            //Anil Edited
            prize = slices - 1 - Math.floor(degrees / (360 / slices));
            // now the wheel cannot spin because it's already spinning
            canSpin = false;
            // animation tweeen for the spin: duration 3s, will rotate by (360 * rounds + degrees) degrees
            // the quadratic easing will simulate friction
            var spinTween = game.add.tween(wheel).to({
                angle: 360 * rounds + degrees
            }, 3000, Phaser.Easing.Quadratic.Out, true);
            // once the tween is completed, call winPrize function
            spinTween.onComplete.add(this.winPrize, this);
        }
    },
    // function to assign the prize
    winPrize() {
        // now we can spin the wheel again
        canSpin = false;
        // writing the prize you just won
        var prizeval = slicePrizes[prize];
        StartConfetti();
        //RestartConfetti();
        if (PaidStatus == 'True' || PaidStatus == 'true') {
            $.alert("Congratulation You have Won " + prizeval);
            text = '' + '<span class="center text-center">Sit back, While we Redirect you.....</span>';
            titletext = 'Go to Dashboard';
        } else {
            $.alert("Congratulation You are eligible to Win " + prizeval + " of Whizjuniors on Subscription");
            text = '' + '<span class="center text-center">Congratulations you are eligible for a free ' + prizeval + ' with your subscription. *This offer is valid for 24 hrs only</span>';
            titletext = 'Subscribe Now';
        }

        AddWinner(prizeval);
        setTimeout(function () {
            DeactivateConfetti();
            RestartConfetti();
        }, 1000);

    }
}


function AddWinner(Prize) {
    setTimeout(function () {
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: 'redirecting!',
            content: text,
            backgroundDismiss: false,
            backgroundDismissAnimation: 'shake',
            autoClose: '' + titletext + '|1000',
            buttons: {
                ok: {
                    text: titletext,
                    action: function () {
                        if (PaidStatus == 'True' || PaidStatus == 'true') {
                            window.location.href = '/Student/Profile';
                        } else {
                            window.location.href = '/Payment/Payment';
                        }
                    }
                }
            }
        });        
    }, 1000);

    $.ajax({
        type: "Get",
        url: '/Student/AddWinner',
        dataType: 'json',
        data: { PrizeWon: Prize },
        accept: 'application/json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (PaidStatus == 'True' || PaidStatus == 'true') {
                window.location.href = '/Student/Profile';
            } else {
                window.location.href = '/Payment/Payment';
            }
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}