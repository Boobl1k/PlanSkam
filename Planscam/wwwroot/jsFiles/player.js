const
    artist = document.getElementById('artist'),
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

let playingNow = false;

if (audio.playingNow)
    setPauseIcon();
else
    setPauseIcon();

function play() {
    if (!playingNow)
        audio.play();
    else
        audio.pause();
    playingNow = !playingNow;
}

function testFunc() {
    artist.innerHTML = "test complete";
}

function progressBarUpdate(e) {
    let { duration, currentTime } = e.srcElement;
    progressBar.style.width = `${currentTime / duration*100}%`;
}

function setProgress(e) {
    audio.currentTime = e.offsetX/progressContainer.clientWidth*audio.duration;
}

function setPlayIcon() {
    playButton.classList.remove('fi-rr-play');
    playButton.classList.add('fi-rr-pause');
}

function setPauseIcon() {
    playButton.classList.remove('fi-rr-pause');
    playButton.classList.add('fi-rr-play');
}

setMute();
volumeSlider.style.height = `${audio.volume * 100}%`;

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

function LoadTrackInPlaylist (track) {
    audio.src=track.src;
    artist.innerHTML = track.artist;
    trackName.innerHTML = track.trackName;
}

progressContainer.addEventListener('click', setProgress);
audio.addEventListener('timeupdate', progressBarUpdate);
audio.addEventListener('play', setPlayIcon);
audio.addEventListener('pause', setPauseIcon);
audio.addEventListener('volumechange', setMute);
volumeContainer.addEventListener('click', setVolume);
volumeBtn.addEventListener('click', hideVolume);

