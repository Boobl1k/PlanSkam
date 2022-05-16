function likePlaylist(id) {
    btn = document.getElementById('likePlaylistBtn');
    if (btn.classList.contains('fi-rr-check'))
    {
        sendAjax("POST",'document',`/Playlists/UnlikePlaylist?id=${id}`,function() {
            btn.classList.remove('fi-rr-check');
            btn.classList.add('fi-rr-plus');
            btn.innerHTML = "Add";
            //update playlists TODO
        });
    }
    else
    {
        sendAjax("POST",'document',`/Playlists/LikePlaylist?id=${id}`,function() {
            btn.classList.remove('fi-rr-plus');
            btn.classList.add('fi-rr-check');
            btn.innerHTML = "In favorites";
            //update playlists TODO
        });
    }
}
