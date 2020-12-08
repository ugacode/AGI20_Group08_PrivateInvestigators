$(document).ready(function(){
    //Parallax init
    $('.parallax').parallax();
    
    //Sidenav init
    $('.sidenav').sidenav();

    //Tabs init
    $('.tabs').tabs();

    //Modal init
    $('.modal').modal();

    //Feature discovery init: menu button
    var elemsTap = document.querySelector('#tap-menu');
    var instancesTap = M.TapTarget.init(elemsTap, {});
    instancesTap.open();
    setTimeout(() => instancesTap.close(), 3000);

	//Check to see if the window is top if not then display button
    $(window).scroll(function(){
        if ($(this).scrollTop() > 150) {
            $('.scrollToTop').fadeIn();
        } else {
            $('.scrollToTop').fadeOut();
        }
    });

    //Click event to scroll to top
    $('.scrollToTop').click(function(){
        $('html, body').animate({scrollTop : 0},800);
        return false;
    });
});