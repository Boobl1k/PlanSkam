function search() {
    input = document.getElementById('searchInput');
    sendAjax("GET", 'document', `/Tracks/RemoveTrackFromFavourite?query=${input.value}`, function () {
        document.getElementById('searchReslutContainer').innerHTML = request.response.body.innerHTML;
    });
}