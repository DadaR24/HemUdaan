
/* Light YouTube Embeds by @@labnol */
/* Web: http://labnol.org/?p=27941 */

document.addEventListener("DOMContentLoaded",
    function () {
        var div, n,
            v = document.getElementsByClassName("youtube-player");
        for (n = 0; n < v.length; n++) {
            div = document.createElement("div");
            //alert(n);
            div.setAttribute("data-id", v[n].dataset.id);
            div.innerHTML = labnolThumb(v[n].dataset.id);
            div.setAttribute("data-activiyname", v[n].dataset.activiyname);
            div.setAttribute("data-activiyid", v[n].dataset.activiyid);
            div.onclick = labnolIframe;
            v[n].appendChild(div);
        }
    });

function labnolThumb(id) {

    var url = id;
    var videoid = url.match(/(?:https?:\/{2})?(?:w{3}\.)?youtu(?:be)?\.(?:com|be)(?:\/watch\?v=|\/)([^\s&]+)/);
    if (videoid != null) {
        id = videoid[1];
    } else {

    }
    var imgsrc = "https://i.ytimg.com/vi/ID/hqdefault.jpg";
    var thumb = '<img src="https://i.ytimg.com/vi/ID/hqdefault.jpg">',
        play = '<div class="play"></div>';
    return thumb.replace("ID", id) + play;
}

function labnolIframe() {
    var iframe = document.createElement("iframe");
    var embed = "https://www.youtube.com/embed/ID?autoplay=1";
    var url = this.dataset.id;
    var activiyname = this.dataset.activiyname;
    var activiyid = this.dataset.activiyid;
    //alert(activiyname + ' ' + activiyid);
    //var topicID = 
    var videoid = url.match(/(?:https?:\/{2})?(?:w{3}\.)?youtu(?:be)?\.(?:com|be)(?:\/watch\?v=|\/)([^\s&]+)/);
    if (videoid != null) {
        url = videoid[1];
    } else {

    }
    var youtubelink = embed.replace("ID", url);
    //ht tps://www.youtube.com/embed/yz8S1mVNDS4?autoplay=1&rel=0&modestbranding=1&autohide=1&showinfo=0&controls=0
    youtubelink = youtubelink + '&rel=0';
   // alert(youtubelink);
    iframe.setAttribute("src", youtubelink);
    iframe.setAttribute("frameborder", "0");
    iframe.setAttribute("allowfullscreen", "1");
    this.parentNode.replaceChild(iframe, this);
    Logs(activiyid, activiyname, url);
}