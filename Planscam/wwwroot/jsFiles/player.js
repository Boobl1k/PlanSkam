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

function prevTrack() {
    if (localStorage.nowPlayed == '0')
        localStorage.nowPlayed = JSON.parse(localStorage.playlist).tracks.length - 1;
    else
        localStorage.nowPlayed = parseInt(localStorage.nowPlayed) - 1;
    loadTrack(JSON.parse(localStorage.playlist).tracks[localStorage.nowPlayed].id);
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
    volumeSlider.style.height = `${vol * 100}%`;
}

function hideVolume(e) {
    if (volumeWindow.style.display == 'flex') {
        volumeBtn.style.color = 'white';
        volumeWindow.style.display = 'none';
    }
    else {
        volumeBtn.style.color = '#5800FF';
        volumeWindow.style.display = 'flex';
    }
}

progressContainer.addEventListener('click', setProgress);
audio.addEventListener('timeupdate', progressBarUpdate);
audio.addEventListener('play', setPlayIcon);
audio.addEventListener('pause', setPauseIcon);
audio.addEventListener('volumechange', setMute);
volumeContainer.addEventListener('click', setVolume);
volumeBtn.addEventListener('click', hideVolume);

function loadPlaylist(id) {
    request = new XMLHttpRequest();
    request.responseType = 'json';
    request.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200)
            localStorage.playlist = JSON.stringify(request.response);
    };
    request.open("GET", `/Playlists/GetData/${id}`);
    request.send();
}

function loadTrack(id) {
    request = new XMLHttpRequest();
    request.responseType = 'json';
    request.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            paused = audio.paused;
            track = request.response;
            artist.innerHTML = track.author;
            trackName.innerHTML = track.name;
            audio.src = 'data:audio/mp3;base64,' + track.data;
            trackLogo.src = 'data:image/jpg;base64,' + track.picture;
            play();
            if (paused)
                play();
            progressBarUpdate();
        }
    };
    request.open("GET", `/Tracks/GetTrackData/${id}`);
    request.send();
}

//required for init
if (localStorage.playlist == null)
    loadPlaylist(4);
setMute();
volumeSlider.style.height = `${audio.volume * 100}%`;

localStorage.nowPlayed = 0;
loadTrack(JSON.parse(localStorage.playlist).tracks[localStorage.nowPlayed].id);


