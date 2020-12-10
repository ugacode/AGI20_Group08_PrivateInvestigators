$(document).ready(function(){
    //Parallax init
    $('.parallax').parallax();
    
    //Sidenav init
    $('.sidenav').sidenav();

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