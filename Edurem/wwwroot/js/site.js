window.setCookies = function (newCookies, toReload) {
    if (document.cookie != newCookies) {
        document.cookie = newCookies;

        document.location.reload();
    }    
}