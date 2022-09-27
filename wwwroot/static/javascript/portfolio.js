
$(window).scroll(function(){
  if($(this).scrollTop()>50){
    $('.nav-wrapper').removeClass("clear").addClass("blue-grey", 500)
    $('.navlink').removeClass('blackfont').addClass('whitefont')
  }
  else{
    $('.nav-wrapper').removeClass("blue-grey").addClass("clear", 500)
    $('.navlink').removeClass('whitefont').addClass('blackfont')

  }
})
$(window).resize(function () {
    var viewportWidth = $(window).width();
    if (viewportWidth < 600) {
            $(".techdiv").removeClass("valign");
            $("#technology").removeClass("valign-wrapper")
    }
    else{
      $(".techdiv").addClass("valign");
      $("#technology").addClass("valign-wrapper")
    }
});
$(document).ready(function(){
      $('.parallax').parallax();
      $(".button-collapse").sideNav({
        menuWidth: 200,
        closeOnClick: true
      });
      $(".dropdown-button").dropdown();
      $(".materialboxed").materialbox();
      $("#name, #title, nav").removeClass("banner-hidden")
      $("#name, #title, nav").addClass('vis')

      if( !/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ) {
        $(".project-link-label, .contact-label").hide();
      }
      $(".project-link").hover(function(){
        $(this).closest(".col").children("p").show("slow");
      }, function(){
        $(this).closest(".col").children("p").fadeOut("slow");
      })
      $(".contactimg").hover(function(){
        $(this).siblings().show("slow");
      }, function(){
        $(this).siblings().fadeOut("slow");
      })
      var options = [
        {selector: '#abouttextanimation', offset: 300, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#aboutpictures', offset: 100, callback: function(el) {
          Materialize.fadeInImage($(el));
        } },
        {selector: '#techimgs', offset: 100, callback: function(el) {
          Materialize.fadeInImage($(el));
        } },
        {selector: '#techanimation', offset: 200, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#projtitleanimation', offset: 300, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#getoutanimation', offset: 100, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#forgeanimation', offset: 200, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#mapshareanimation', offset: 200, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#iosanimation', offset: 200, callback: function(el) {
          Materialize.showStaggeredList($(el));
        } },
        {selector: '#contactimgs', offset: 100, callback: function(el) {
          Materialize.fadeInImage($(el));
        } },
      ];

      Materialize.scrollFire(options);
      var viewportWidth = $(window).width();
      if (viewportWidth < 600) {
              $(".techdiv").removeClass("valign");
              $("#technology").removeClass("valign-wrapper")
      }
      else{
        $(".techdiv").addClass("valign");
        $("#technology").addClass("valign-wrapper")
      }
      $('.aboutbtn').click(function(){
        var targetOffset = $('#about').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('.aboutbtn').click(function(){
        var targetOffset = $('#about').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('.techbtn').click(function(){
        var targetOffset = $('#technology').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('.projbtn').click(function(){
        var targetOffset = $('#projects').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('.contactbtn').click(function(){
        var targetOffset = $('#contact').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('#getoutbtn').click(function(){
        var targetOffset = $('#getout').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('#forgebtn').click(function(){
        var targetOffset = $('#forge').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('#mapsharebtn').click(function(){
        var targetOffset = $('#mapshare').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
      $('#iosbtn').click(function(){
        var targetOffset = $('#mapios').offset().top - 64;
        $('html, body').animate({
          scrollTop: targetOffset
        },  1000);
        $('.button-collapse').sideNav('hide');
        return false;
      })
});
