$(function () {
    if (IsValidUser == 0) {
        $.confirm({
            title: 'Warning!',
            content: Massage,
            buttons: {
                ok: function () {
                    location.href = RequestURL;
                }
            }
        });
    }

    $(".draggable").draggable();
    session = OT.initSession(apiKey, sessionId);
    publishVideo();
    sessionEvents(publisher);
    connectSession();
    initPageFunction(); // chat function
    //Enable Disable Camera
    $("#btnStopCamera").on("click", function () {
        el = $(this);
        if (el.data('camera') == 1) {
            publisher.publishVideo(false);
            el.find('img').attr('src', '/Content/img/Student/no_video.svg');
            el.data("camera", '0');
        } else {
            publisher.publishVideo(true);
            el.find('img').attr('src', '/Content/img/Student/video-icon.svg');
            el.data("camera", '1');
        }
    });
    //Enable Disable Audio
    $("#btnStopAudio").on("click", function () {
        el = $(this);
        if (el.data('audio') == 1) {
            if (streamMode == "camera") {
                publisher.publishAudio(false);
            } else {
                screenPublisher.publishAudio(false);
            }
            el.find('img').attr('src', '/Content/img/Student/mic-muted.svg');
            el.data("audio", '0');
        } else {
            if (streamMode == "camera") {
                publisher.publishAudio(true);
            } else {
                screenPublisher.publishAudio(true);
            }
            el.find('img').attr('src', '/Content/img/Student/mic.svg');
            el.data("audio", '1');
        }
    });
    $("#btnShareScreen").on("click", function () {
        el = $(this);
        if (el.data('screen') == 0) {
            publishScreen();
            connectSession();
            el.data('screen', '1');
            el.find('img').attr('src', '/Content/img/Student/video-icon.svg');
            el.parent().find('p').html("Camera");
        }
        else {
            publishVideo();
            connectSession();
            el.data('screen', '0');
            el.find('img').attr('src', '/Content/img/Student/screenshare.svg');
            el.parent().find('p').html("Screen");
        }
    });
    $("#btnEndCall").on("click", function () {
        $.confirm({
            title: 'Confirm!',
            content: 'Are you sure ?',
            buttons: {
                confirm: function () {
                    //If Moderator then disconnect session
                    //else unpublish publicsher
                    if (isModerator == 1) {
                        SessionDisconnecct();
                    } else {
                        streamMode == 'camera' ? endStream(publisher) : endStream(screenPublisher);
                    }

                    var data = {
                        conferenceID: ConferenceID,
                        isModerator: isModerator
                    };
                    $.postJSON('/end-session', "POST", JSON.stringify(data), endSessionCallBack, endSessionErrorCallBack);
                },
                cancel: function () {
                }
            }
        });
    });

    $('.toggle-chat-window').click(function () {
        //$(this).closest('.chatbox').toggleClass('chatbox-min');

        toggleChatWindow();

    });
    $('.close-chat-window').click(function () {
        //$(this).closest('.chatbox').hide();
        closeChatWindow();
    });

});
function publishVideo() {
    console.log(screenPublisher);
    if (screenPublisher != undefined) {
        endStream(screenPublisher);
    }
    createPublisherDiv();
    publisher = OT.initPublisher('publisher', function (error) {
        if (error) {
            // Look at error.message to see what went wrong.
        } else {
            session.publish(publisher, function (error) {
                if (error) {
                    // Look error.message to see what went wrong.
                } else {
                    streamMode = "camera";
                }
            });
        }
    });
}
function publishScreen() {
    OT.checkScreenSharingCapability(function (response) {
        if (!response.supported || response.extensionRegistered === false) {
            // This browser does not support screen sharing.
            $.alert({
                title: 'Alert!',
                content: 'Your Browser Does Not Support Screen Sharing',
            });
        } else if (response.extensionInstalled === false) {
            // Prompt to install the extension.
        } else {
            //Screen sharing available
            endStream(publisher);
            createPublisherDiv();
            const publish = Promise.all([
                OT.getUserMedia({
                    videoSource: 'screen'
                }),
                OT.getUserMedia({
                    videoSource: null
                })
            ]).then(([screenStream, micStream]) => {
                return screenPublisher = OT.initPublisher('publisher', {
                    videoSource: screenStream.getVideoTracks()[0],
                    audioSource: micStream.getAudioTracks()[0],
                    fitMode: "contain"
                });
            });
            publish.then(screenPublisher => {
                session.publish(screenPublisher, function (error) {
                    if (error) {
                        // Look error.message to see what went wrong.
                    } else {
                        streamMode = "screen";
                    }
                });
            }).catch(handleError);
        }
    });
    //screenPublisher = OT.initPublisher('publisher', { videoSource: 'screen' });
    //screenPublisher = OT.initPublisher('publisher',
    //    { videoSource: 'screen'},
    //        function (error) {
    //            if (error) {
    //                // Look at error.message to see what went wrong.
    //            } else {
    //                session.publish(screenPublisher, function (error) {
    //                    if (error) {
    //                        // Look error.message to see what went wrong.
    //                    } else {
    //                        streamMode = "screen";
    //                    }
    //                });
    //            }
    //        }
    //    );
}
function createPublisherDiv() {
    $('.caller-list').find('li').empty();
    var html = '<div id="publisher" class="draggable user-camscreen"></div>';
    $('.caller-list').find('li').append(html);
}
function endStream(streamPublisher) {
    session.unpublish(streamPublisher, function () {
    });
}
function connectSession() {
    session.connect(token, function (error) {
        if (error) {
            console.error('Failed to connect', error);
        }
    });
}
function sessionEvents(streamPublisher) {
    session.on({
        sessionConnected: function (event) {
            // Publish the publisher we initialzed earlier (this will trigger 'streamCreated' on other
            // clients)
            session.publish(streamPublisher, function (error) {
                if (error) {
                    console.error('Failed to publish', error);
                } else {
                    console.log("Publish success");
                }
            });
        },
        // This function runs when another client publishes a stream (eg. session.publish())
        streamCreated: function (event) {
            // Create a container for a new Subscriber, assign it an id using the streamId, put it inside
            // the element with id="subscribers"
            console.log(event.stream);
            var subOptions = {
                width: '100%', height: '100%'
            };
            var subContainer = document.createElement('div');
            subContainer.id = 'stream-' + event.stream.streamId;
            document.getElementById('subscribers').appendChild(subContainer);
            // Subscribe to the stream that caused this event, put it inside the container we just made
            subscriber = session.subscribe(event.stream, subContainer, subOptions, function (error) {
                if (error) {
                    console.error('Failed to subscribe', error);
                }
            });
        }
    });

    // Receive a message and append it to the history
    session.on('signal:msg', function signalCallback(event) {   //signal:msg here msg decalre that only type msg will accept as massage rest will be ignore to accept all type of massage remove msg
        appendMassage(event);
    });
}
function handleError(err) {
    if (err) console.log(err.message);
}
function SessionDisconnecct() {
    session.disconnect();
}

function endSessionCallBack(data) {
    var returnValue = data;// JSON.parse(data);
    if (!returnValue.error) {
        var mainResult = returnValue.result;
        location.href = mainResult.url;
    }
}
function endSessionErrorCallBack(data) {
    $.alert({
        title: 'Alert!',
        content: 'Ooops! Something went wrong',
    });
}

// to send chat
function sendMassage() {

    var ChatMassage = document.querySelector('#txtChatMassage');
    if (ChatMassage.value != '') {
        chatData = {
            'message': ChatMassage.value,
            'name': userName
        };
        session.signal({
            // to: connection1, // is used to send a specific person with connection ID
            type: 'msg',
            data: JSON.stringify( chatData),
        }, function signalCallback(error) {
            if (error) {
                console.error('Error sending signal:', error.name, error.message);
            } else {
                ChatMassage.value = '';
            }
        });
    }
    else {
        showMassage('Alert!', 'Ooops! enter massage')
    }
    
}

// recive and append chat
function appendMassage(event) {

    var responseData = $.parseJSON(event.data);
    if (userName != responseData.name )
        $('#lblReceiverUserName').text(responseData.name);
    
    var msgChatHistory = document.querySelector('#ChatHistory');
    var MassageBody = document.createElement('DIV');
    //msg.textContent = responseData;
    MassageBody.className = 'message-box-holder';
    MassageBody.innerHTML = event.from.connectionId === session.connection.connectionId ? '<div class="message-box">' + responseData.message + '</div>' : '<div class="message-sender">' + responseData.name + '</div><div class="message-box message-partner">' + responseData.message + '</div>';
    //msg.className = event.from.connectionId === session.connection.connectionId ? 'mine' : 'theirs';
    msgChatHistory.appendChild(MassageBody);
    //msgChatHistory.innerHTML(MassageBody);
    MassageBody.scrollIntoView();
    toggleChatWindow('chat');
};

//initialize page event to send a chat
function initPageFunction() {
    // Text chat
    var form = document.querySelector('form');
    // Send a signal once the user enters data in the form
    form.addEventListener('submit', function submit(event) {
        event.preventDefault();
        sendMassage();
    });
    
}

function toggleChatWindow(from = '') {

    var chatBox = $('.chatbox');
    if (!chatBox.is(":visible")) {
        chatBox.show();
    }
    
    if (from == '') {

        chatBox.toggleClass('chatbox-min');
    } else {
        chatBox.removeClass('chatbox-min');
    }

    $('#txtChatMassage').focus();
}

    function closeChatWindow() {
        $('.chatbox').hide();
    }

    function showMassage(type, massage) {
        var alertType = type === 'Alert!' ? 'red' : 'orange'

        $.alert({
            title: type,
            content: massage,
            type: alertType
        });
    }
