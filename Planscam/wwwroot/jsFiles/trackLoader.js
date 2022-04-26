const request = new XMLHttpRequest();
request.responseType = 'JSON';
function LoadTrack(id, func) {
    request.onreadystatechange() = function () {
        if (this.readyState == 4 && this.status == 200) {
            func(request);
        }
    };
    request.open("GET", `/Tracks/LoadTrack?id=${id}`)
    request.send();
}