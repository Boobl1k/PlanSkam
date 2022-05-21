function likePlaylist(id) {
    btn = document.getElementById('likePlaylistBtn');
    if (btn.classList.contains('fi-rr-check')) {
        sendAjax("POST", 'document', `/Playlists/UnlikePlaylist?id=${id}`, function () {
            btn.classList.remove('fi-rr-check');
            btn.classList.add('fi-rr-plus');
            btn.innerHTML = "Add";
            updateLayoutPlaylists();
        });
    }
    else {
        sendAjax("POST", 'document', `/Playlists/LikePlaylist?id=${id}`, function () {
            btn.classList.remove('fi-rr-plus');
            btn.classList.add('fi-rr-check');
            btn.innerHTML = "In favorites";
            updateLayoutPlaylists();
        });
    }
}

function updateLayoutPlaylists() {
    container = document.getElementById('layoutPlaylistsContainer');
    sendAjax("GET", 'document', '/Playlists/LayoutPlaylists/', function () {
        container.innerHTML = request.response.body.innerHTML;
    })
}

function addTrackToFavourite(id) {
    btn = document.getElementById(`trackFavBtn${id}`);
    if (id == JSON.parse(localStorage.playlist).trackIds[localStorage.nowPlayed])
    {
        setFavourite(!btn.classList.contains('fi-sr-heart'));
    }
    if (btn.classList.contains('fi-sr-heart'))
        sendAjax("POST", 'json', `/Tracks/RemoveTrackFromFavourite/${id}`, function () {
            setBtnFavourite(btn, false);
        });
    else
        sendAjax("POST", 'json', `/Tracks/AddTrackToFavourite/${id}`, function () {
            setBtnFavourite(btn, true);
        });
}

function setBtnFavourite(btn, isLiked) {
    if (isLiked) {
        btn.classList.remove('fi-rr-heart');
        btn.classList.add('fi-sr-heart');
    }
    else {
        btn.classList.remove('fi-sr-heart');
        btn.classList.add('fi-rr-heart');
    }
}

