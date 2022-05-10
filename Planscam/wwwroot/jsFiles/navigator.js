function loadPage(controller, method, query) {
    sendAjax("GET", 'document', `/${controller}/${method}/${query}`, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}

function loadPage(controller, method) {
    sendAjax("GET", 'document', `/${controller}/${method}`, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}

function loadPage(uri) {
    sendAjax("GET", 'document', uri, function () {
        page = document.getElementById('page');
        page.innerHTML = request.response.body.innerHTML;
    });
}