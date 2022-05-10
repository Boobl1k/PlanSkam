const artist = document.getElementById('artist'),
    trackLogo = document.getElementById('trackLogo'),
    trackName = document.getElementById('trackName'),
    playButton = document.getElementById('play'),
    audio = document.getElementById('audio'),
    progressBar = document.getElementById('progress'),
    progressContainer = document.getElementById('progeressContainer'),
    muted = document.getElementById('muted'),
    likeBtn = document.getElementById('likeBtn'),
    addBtn = document.getElementById('addBtn'),
    addWindow = document.getElementById('addWindow'),
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

function sendAjax(requestType, responseType, req, func) {
    request = new XMLHttpRequest();
    request.responseType = responseType;
    request.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            func();
        }
    };
    request.open(requestType, req);
    request.send();
}

function prevTrack() {
    if (localStorage.nowPlayed == '0')
        localStorage.nowPlayed = JSON.parse(localStorage.playlist).trackIds.length - 1;
    else
        localStorage.nowPlayed = parseInt(localStorage.nowPlayed) - 1;
    loadTrack(JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed]);
}

function nextTrackEnded() {
    playButton.classList.remove('fi-rr-play');
    playButton.classList.add('fi-rr-pause');
    nextTrack();
}

function nextTrack() {
    localStorage.nowPlayed = parseInt(localStorage.nowPlayed) + 1;
    if (localStorage.nowPlayed >= JSON.parse(localStorage.playlist).trackIds.length)
        localStorage.nowPlayed = 0;
    loadTrackAndPlay(JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed]);
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

function setFavourite(isLiked) {
    if (isLiked) {
        likeBtn.IsLiked = isLiked;
        likeBtn.classList.remove('fi-rr-heart');
        likeBtn.classList.add('fi-sr-heart');
    }
    else {
        likeBtn.IsLiked = isLiked;
        likeBtn.classList.remove('fi-sr-heart');
        likeBtn.classList.add('fi-rr-heart');
    }
}

function trackToFavourite() {
    if (likeBtn.IsLiked)
        sendAjax("POST", 'json', `/Tracks/RemoveTrackFromFavourite/${JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed]}`, function () {
            setFavourite(false);
        });
    else
        sendAjax("POST", 'json', `/Tracks/AddTrackToFavourite/${JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed]}`, function () {
            setFavourite(true);
        });
}

function loadPlaylist(id) {
    sendAjax("GET", 'json', `/Playlists/GetData/${id}`, function () {
        localStorage.playlist = JSON.stringify(request.response);
    });
}

function afterLoadTrack(track) {
    artist.innerHTML = track.author;
    setFavourite(track.isLiked);
    trackName.innerHTML = track.name;
    audio.src = 'data:audio/mp3;base64,' + track.data;
    trackLogo.src = 'data:image/jpg;base64,' + track.picture;
    progressBarUpdate();
}

function loadTrack(id) {
    sendAjax("GET", 'json', `/Tracks/GetTrackData/${id}`, function () {
        afterLoadTrack(request.response);
    });
}

function loadTrackAndPlay(id) {
    sendAjax("GET", 'json', `/Tracks/GetTrackData/${id}`, function () {
        afterLoadTrack(request.response);
        play();
    });
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

function setVolumeCloseTimeout() {
    volumeBtn.closeTimeout = setTimeout(function () {
        volumeBtn.style.color = 'white';
        volumeWindow.style.display = 'none';
    }, 2000);
}

function clearVolumeCloseTimeout() {
    clearTimeout(volumeBtn.closeTimeout);
}

function hideAdd(e) {
    if (addWindow.style.display == 'flex') {
        clearTimeout(addBtn.closeTimeout);
        addBtn.style.color = 'white';
        addWindow.style.display = 'none';
    }
    else {
        addBtn.style.color = '#5800FF';
        addWindow.style.display = 'flex';
        addBtn.closeTimeout = setTimeout(function () {
            addBtn.style.color = 'white';
            addWindow.style.display = 'none';
        }, 2000);
        sendAjax("GET", 'document', `/Playlists/AddPlayedTrack/${JSON.parse(localStorage.playlist)[localStorage.nowPlayed}`, function () {
            addWindow.innerHTML = request.response.body.innerHTML;
        });
    }
}

function setAddCloseTimeout() {
    addBtn.closeTimeout = setTimeout(function () {
        addBtn.style.color = 'white';
        addWindow.style.display = 'none';
    }, 2000);
}

function clearAddCloseTimeout() {
    clearTimeout(addBtn.closeTimeout);
}

function addToPlaylist(playlistId) {
    sendAjax("POST", "json", `/Playlists/AddTrackToPlaylist?playlistId=${playlistId}&trackId=${JSON.parse(localStorage.playlist)[localStorage.nowPlayed]}`, function () {
        btn = document.getElementById(`addButton${playlistId}`);
        btn.classList.remove("fi-rr-plus");
        btn.classList.add("fi-rr-check");
        btn.onclick = null;
    })
}

progressContainer.addEventListener('click', setProgress);
audio.addEventListener('timeupdate', progressBarUpdate);
audio.addEventListener('ended', nextTrackEnded);
audio.addEventListener('play', setPlayIcon);
audio.addEventListener('pause', setPauseIcon);
audio.addEventListener('volumechange', setMute);
addBtn.addEventListener('click', hideAdd);
addWindow.addEventListener('mouseenter', clearAddCloseTimeout);
addWindow.addEventListener('mouseleave', setAddCloseTimeout);
volumeContainer.addEventListener('click', setVolume);
volumeBtn.addEventListener('click', hideVolume);
volumeWindow.addEventListener('mouseenter', clearVolumeCloseTimeout);
volumeWindow.addEventListener('mouseleave', setVolumeCloseTimeout);

function initPage() {
    if (localStorage.playlist == null)
        loadPlaylist(4);

    localStorage.nowPlayed = 0;
    loadTrack(JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed]);
    audio.volume = parseFloat(localStorage.volume);
    setMute();
    volumeSlider.style.height = `${audio.volume * 100}%`;
}
initPage();

