$(window).on('load',function(){
    'use strict';

    // Tooltip
    $('[data-toggle="tooltip"]').tooltip();


    // Accordions
    $('.toggle').each(function(){
      $(this).find('.content').hide();
      $(this).find('h2:first').addClass('active').next().slideDown(500).parent().addClass('activate');
      $('h2', this).on('click',function(){
        if ($(this).next().is(':hidden')){
          $(this).parent().parent().find('h2').removeClass('active').next().slideUp(500).parent().removeClass('activate');
          $(this).toggleClass('active').next().slideDown(500).parent().toggleClass('activate');
        }
      });
    });

    // Side Menu
    $(document).on('click',".menu_btn", function () {
      $('.sidemenu_container').toggleClass('collapsed');
    });


    $('.sidemenu li.has-child > a').on('click', function () {
      $(this).parent().siblings('li').children('ul').slideUp();
      $(this).parent().siblings('li').removeClass('active');
      $(this).parent().children('ul').slideToggle();
      $(this).parent().toggleClass('active');
      return false;
    });

    


});
