$(document).ready(function(){
    //Parallax init
    $('.parallax').parallax();
    
    //Sidenav init
    $('.sidenav').sidenav();

    //Images init
    $('.materialboxed').materialbox();

    //Pushpin init
    $('.target').pushpin();

    //Carousel init
    $('.carousel').carousel();

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

    //Scroll to replace navbar
    $('.pushpin-demo-nav').each(function() {
        var $this = $(this);
        var $target = $('#' + $(this).attr('data-target'));
        $this.pushpin({
            top: $target.offset().top,
            bottom: $target.offset().top + $target.outerHeight() - $this.height()
        });
    });
});