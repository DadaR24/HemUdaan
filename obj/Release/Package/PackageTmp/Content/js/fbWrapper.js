/***
Initialze or instatiate socialWrapper Object
***/
var socialWrapper = socialWrapper || {};


/***
Initialze fbWrapper Object
***/
socialWrapper.fb = {};

/***
FB JS SDK Loaded
returns (bool)
***/
socialWrapper.fb.isSDKLoaded = function(){
	if(!FB.getLoginStatus){
		return false;
	}
	return true;
}


/***
Login Status
returns (bool)
***/
socialWrapper.fb.getLoginStatus = function(cb){
	FB.getLoginStatus(cb);  
};


/***
FB Login 
returns (bool)
***/
socialWrapper.fb.login = function(cb){
	var permissions = {};
	permissions.scope = 'public_profile,user_friends,email,publish_actions';
	FB.login(cb,permissions);

};

/***
FB Logged In User
returns (obj)
***/

socialWrapper.fb.loggedInUserDetails = function(cb,token){
	var obj = {};
	obj.access_token = token;
	obj.fields = "email,first_name,last_name";
	FB.api('/me',obj,cb);
};


/***
FB Post to Logged In User Wall
params (string) Message 
returns (obj)
***/
socialWrapper.fb.postToUserWall = function(msg,cb,token){
	var obj = {};
	obj.access_token = token;
	obj.message = msg ? msg : "";

	FB.api('/me/feed','post',obj,cb);
};


/***
FB Logged In User Friends List
returns (obj)
***/
socialWrapper.fb.getFriendList = function(cb,token){
	var obj = {};
	obj.access_token = token;
	FB.api('/me/friends',obj,cb);
};


/***
FB Post to Friend Wall
params (string) user_id ,(string) Message 
returns (obj)
***/
socialWrapper.fb.postToFriendWall = function(id,msg,cb,token){
	var obj = {};
	obj.access_token = token;
	obj.message = msg ? msg : "";
	var userID = id;
	FB.api('/'+ userID +'/feed','post',obj,cb);
};