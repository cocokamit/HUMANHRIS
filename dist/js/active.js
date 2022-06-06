var CURRENT_URL = window.location.href.split('#')[0].split('?')[0], $SIDEBAR_MENU = $('#sidebar_menu');

$(document).ready(function() {
    init_sidebar();
});

function init_sidebar() {
    $SIDEBAR_MENU.find('a').filter(function () {
        return this.href == CURRENT_URL;
    }).parent('li').addClass('active');

    $SIDEBAR_MENU.find('a').filter(function () {
        return this.href == CURRENT_URL;
    }).parent('li').addClass('current-page').parents('ul').slideDown(function () {
       // setContentHeight();
    }).parent().addClass('active');
}

	