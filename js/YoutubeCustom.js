
/* Light YouTube Embeds by @@labnol */
/* Web: http://labnol.org/?p=27941 */

document.addEventListener("DOMContentLoaded",
    function () {
        var div, n,
            v = document.getElementsByClassName("youtube-player-custom");
        for (n = 0; n < v.length; n++) {
            div = document.createElement("div");
            div.setAttribute("data-id", v[n].dataset.id);
            div.innerHTML = labnolThumb(v[n].dataset.id, v[n].dataset.taskId);
            div.setAttribute("data-activiyname", v[n].dataset.activiyname);
            div.setAttribute("data-activiyid", v[n].dataset.activiyid);
            div.onclick = labnolIframe;
            v[n].appendChild(div);
        }
    });

function labnolThumb(id,taskid) {
    var url = id;
    var videoid = url.match(/(?:https?:\/{2})?(?:w{3}\.)?youtu(?:be)?\.(?:com|be)(?:\/watch\?v=|\/)([^\s&]+)/);
    if (videoid != null) {
        id = videoid[1];
    } else {

    }
    if (id.match("hemvirtues")) {
        var imgsrc = taskid;
        var thumb = '<img src="' + taskid + '">',
            play = '<div class="play"></div>';
    } else {
        var imgsrc = "https://i.ytimg.com/vi/ID/hqdefault.jpg";
        var thumb = '<img src="https://i.ytimg.com/vi/ID/hqdefault.jpg">',
            play = '<div class="play"></div>';
    }
    
    return thumb.replace("ID", id) + play;
}

function labnolIframe() {
    var embed = "https://www.youtube.com/embed/ID?autoplay=1";
    var url = this.dataset.id;
    var activiyname = this.dataset.activiyname;
    var activiyid = this.dataset.activiyid;
    var videoid = url.match(/(?:https?:\/{2})?(?:w{3}\.)?youtu(?:be)?\.(?:com|be)(?:\/watch\?v=|\/)([^\s&]+)/);
    var youtubelink = "";
    if (videoid != null) {
        url = videoid[1];
        youtubelink = embed.replace("ID", url);
        youtubelink = youtubelink + '&rel=0';
    } else {
        youtubelink = url;
    }
    $("#Competition_ifrm_Video").attr("src", youtubelink);
    $("#CompetitionStudentVideo").modal('show');
    Logs(activiyid, activiyname, url);
}

function reloadOnCallback() {
    var div, n,
        v = document.getElementsByClassName("youtube-player-custom");
    for (n = 0; n < v.length; n++) {
        div = document.createElement("div");
        div.setAttribute("data-id", v[n].dataset.id);
        div.innerHTML = labnolThumb(v[n].dataset.id);
        div.setAttribute("data-activiyname", v[n].dataset.activiyname);
        div.setAttribute("data-activiyid", v[n].dataset.activiyid);
        div.onclick = labnolIframe;
        v[n].appendChild(div);
    }
}
