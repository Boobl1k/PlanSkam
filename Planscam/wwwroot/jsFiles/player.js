const artist = document.getElementById('artist'),
    trackLogo = document.getElementById('trackLogo'),
    trackName = document.getElementById('trackName'),
    playButton = document.getElementById('play'),
    audio = document.getElementById('audio'),
    progressBar = document.getElementById('progress'),
    progressContainer = document.getElementById('progeressContainer'),
    muted = document.getElementById('muted'),
    volumeBtn = document.getElementById('volumeBtn'),
    volumeWindow = document.getElementById('volumeWindow'),
    volumeContainer = document.getElementById('volumeContainer'),
    volumeSlider = document.getElementById('volumeSlider');

function play() {
    if (audio.paused)
        audio.play();
    else
        audio.pause();
}

function sendAjax(responseType, req, func) {
    request = new XMLHttpRequest();
    request.responseType = responseType;
    request.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            func();
        }
    };
    request.open("GET", req);
    request.send();
}

function prevTrack() {
    if (localStorage.nowPlayed == '0')
        localStorage.nowPlayed = JSON.parse(localStorage.playlist).tracks.length - 1;
    else
        localStorage.nowPlayed = parseInt(localStorage.nowPlayed) - 1;
    loadTrack(JSON.parse(localStorage.playlist).tracks[localStorage.nowPlayed].id);
}

function nextTrackEnded() {
    nextTrack();
}

function nextTrack() {
    localStorage.nowPlayed = parseInt(localStorage.nowPlayed) + 1;
    if (localStorage.nowPlayed >= JSON.parse(localStorage.playlist).tracks.length)
        localStorage.nowPlayed = 0;
    loadTrack(JSON.parse(localStorage.playlist).tracks[localStorage.nowPlayed].id);
}

function progressBarUpdate() {
    let { duration, currentTime } = audio;
    progressBar.style.width = `${currentTime / (duration || 1) * 100}%`;
}

function setProgress(e) {
    audio.currentTime = e.offsetX / progressContainer.clientWidth * audio.duration;
}

function setPlayIcon() {
    playButton.classList.remove('fi-rr-play');
    playButton.classList.add('fi-rr-pause');
}

function setPauseIcon() {
    playButton.classList.remove('fi-rr-pause');
    playButton.classList.add('fi-rr-play');
}

function setMute() {
    if (audio.volume == 0)
        muted.style.display = 'flex';
    else
        muted.style.display = 'none';
}

function setVolume(e) {
    let offset = e.offsetY;
    let vol = offset / volumeBackground.clientHeight;
    if (vol > 0.94)
        vol = 1;
    if (vol < 0.06)
        vol = 0;
    audio.volume = vol;
    localStorage.volume = vol;
    volumeSlider.style.height = `${vol * 100}%`;
}

function hideVolume(e) {
    if (volumeWindow.style.display == 'flex') {
        clearTimeout(volumeBtn.closeTimeout);
        volumeBtn.style.color = 'white';
        volumeWindow.style.display = 'none';
    }
    else {
        volumeBtn.style.color = '#5800FF';
        volumeWindow.style.display = 'flex';
        volumeBtn.closeTimeout = setTimeout(function () {
            volumeBtn.style.color = 'white';
            volumeWindow.style.display = 'none';
        }, 2000);
    }
}


function loadPlaylist(id) {
    sendAjax('json', `/Playlists/GetData/${id}`, function () {
        localStorage.playlist = JSON.stringify(request.response);
    });
}

function afterLoadTrack(track) {
    artist.innerHTML = track.author;
    trackName.innerHTML = track.name;
    audio.src = 'data:audio/mp3;base64,' + track.data;
    trackLogo.src = 'data:image/jpg;base64,' + track.picture;
    progressBarUpdate();
}

function loadTrack(id) {
    sendAjax('json', `/Tracks/GetTrackData/${id}`, function () {
        afterLoadTrack(request.response);
    });
}

function loadPage(controller, method, query) {
    sendAjax('document', `/${controller}/${method}/${query}`, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}

function loadPage(controller, method) {
    sendAjax('document', `/${controller}/${method}`, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}

function loadPage(uri) {
    sendAjax('document', uri, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}

function setVolumeCloseTimeout() {
    volumeBtn.closeTimeout = setTimeout(function () {
        volumeBtn.style.color = 'white';
        volumeWindow.style.display = 'none';
    }, 2000);
}

function clearVolumeCloseTimeout() {
    clearTimeout(volumeBtn.closeTimeout);
}

progressContainer.addEventListener('click', setProgress);
audio.addEventListener('timeupdate', progressBarUpdate);
audio.addEventListener('ended', nextTrackEnded);
audio.addEventListener('play', setPlayIcon);
audio.addEventListener('pause', setPauseIcon);
audio.addEventListener('volumechange', setMute);
volumeContainer.addEventListener('click', setVolume);
volumeBtn.addEventListener('click', hideVolume);
volumeWindow.addEventListener('mouseenter', clearVolumeCloseTimeout);
volumeWindow.addEventListener('mouseleave', setVolumeCloseTimeout);

function initPage() {
    if (localStorage.playlist == null)
        loadPlaylist(4);

    localStorage.nowPlayed = 0;
    loadTrack(JSON.parse(localStorage.playlist).tracks[localStorage.nowPlayed].id);
    audio.volume = parseFloat(localStorage.volume);
    setMute();
    volumeSlider.style.height = `${audio.volume * 100}%`;
}
initPage();

