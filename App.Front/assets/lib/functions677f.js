var $ = jQuery.noConflict();

/*
* Tu.Nguyen
* v.1.0 20160728
* EGA function
*/

EGA.productViewedHtml = '';
EGA.plugin = {
	quickView: function() {
		//phong 20150702: add fancyBox data 'product_url' & beforeShow
		//ref: http://stackoverflow.com/questions/2961496/fancybox-get-id-of-clicked-anchor-element
		jQuery(".fancybox-fast-view").each(function(e){
			$(this).fancybox({ 
				'product_url': $(this).attr('product_url'), 
				beforeShow: function(){
					quickViewProduct(this.product_url); // make-up "#product-pop-up" 
				}, 
			});
		});
	},
	lazyLoad: function() {
		var lazyLoadEl = $('[data-lazyload]');
		if( lazyLoadEl.length > 0 ) {
			lazyLoadEl.each( function(){
				var element = $(this),
						elementImg = element.attr( 'data-lazyload' );
				element.attr( 'src', '//hstatic.net/495/1000133495/1000194031/blank.svg?v=226' ).css({ 'background': 'url(//hstatic.net/495/1000133495/1000194031/preloader.gif?v=226) no-repeat center center #FFF' });
				element.appear(function () {
					element.css({ 'background': 'none' }).removeAttr( 'width' ).removeAttr( 'height' ).attr('src', elementImg);
				},{accX: 0, accY: 120},'easeInCubic');
			});
		}
	},
	tooltip: function() {
		$('[data-toggle="tooltip"]').tooltip({
			html: true
		})
	},
	menu: function(options) {
		var $menu = (options && options.$menu) ? options.$menu : '#primary-menu > ul';
		$($menu).superfish();
	},
	counter: function() {
		var $counterEl = $('.counter:not(.counter-instant)');
		if( $counterEl.length > 0 ){
			$counterEl.each(function(){
				var element = $(this);
				var counterElementComma = $(this).find('span').attr('data-comma');
				if( !counterElementComma ) { counterElementComma = false; } else { counterElementComma = true; }
				element.appear( function(){
					EGA.plugin.runCounter( element, counterElementComma );
				},{accX: 0, accY: -120},'easeInCubic');
			});
		}
	},
	runCounter: function( counterElement,counterElementComma ){
		if( counterElementComma == true ) {
			counterElement.find('span').countTo({
				formatter: function (value, options) {
					value = value.toFixed(options.decimals);
					value = value.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
					return value;
				}
			});
		} else {
			counterElement.find('span').countTo();
		}
	},


	// Viewed Products
	getViewedProducts: function(cur_handle) {
		var pro_arr = (localStorage.viewedProductsList) ? JSON.parse(localStorage.viewedProductsList) : [];

		// Check if curProduct in curProArr 
		// and push product handle to localStorage
		if($.inArray(cur_handle, pro_arr) > -1) {
			pro_arr.splice($.inArray(cur_handle, pro_arr), 1);
		}
		pro_arr.unshift(cur_handle);
		localStorage.setItem('viewedProductsList', JSON.stringify(pro_arr));
	},
	buildViewedProducts: function(template, product, img_url) {
		var product_url = product.url,
				product_img = Haravan.resizeImage(product.featured_image, img_url),
				product_title = product.title,
				product_price = Haravan.formatMoney(product.variants[0].price, EGA.options.money_format),
				product_compare_price = (product.variants[0].compare_at_price != 0) ? Haravan.formatMoney(product.variants[0].price, EGA.options.money_format) : null,
				product_price_number = Number(product.variants[0].price),
				product_compare_price_number = Number(product.variants[0].compare_at_price),
				product_sale = (product.variants[0].compare_at_price != 0) ? Math.round(((product_compare_price_number - product_price_number) / product_compare_price_number * 100)) : null;

		var source = $(template).html(),
				template = Handlebars.compile(source),
				context = {
					product_url: product_url,
					product_img: product_img,
					product_title: product_title,
					product_price: product_price,
					product_compare_price: product_compare_price,
					product_sale: product_sale,
				},
				html = template(context);
		EGA.productViewedHtml += html;
	},
	setViewedProducts: function(options) {
		var location_wrapper = options.location_wrapper,
				template = options.template,
				location = options.location,
				limit = options.limit,
				img_url = options.img_url;
		var pro_arr = (localStorage.viewedProductsList) ? JSON.parse(localStorage.viewedProductsList) : [];

		//hide viewed products block if pro_arr is empty
		if(pro_arr.length <= 0) {
			$(location_wrapper).addClass('hidden');
		}
		else {
			// Get product by ajax
			var i = 0;
			$.each(pro_arr, function(index, handle) {
				if(i < limit) {
					$.ajax({
						type: 'get',
						url: '/products/' + handle + '.js',
						async: false,
						dataType: 'json',
						success: function(data) {
							EGA.plugin.buildViewedProducts(template, data, img_url);
						}
					})
					i++;
				}
				else {
					return false;
				}
			})
			$(location).append(EGA.productViewedHtml);
		}
	},
	// End Viewed Products
}


EGA.header = {
	sticky: function(options) {
		var $header_el = (options && options.$header_el) ? options.$header_el : '#header',
				element_offset = (options && options.element_offset != 'undefined') ? options.element_offset : $('#top-bar').height(),
				use_sticky = ($($header_el).attr('data-sticky')) ? $($header_el).attr('data-sticky') : 'true';
		if(use_sticky == 'true') {
			if($(window).scrollTop() > Number(element_offset)) {
				$($header_el).addClass('sticky-header');
			}
			else {
				$($header_el).removeClass('sticky-header');
			}
		}
	}
}

EGA.footer = {
	toggleMenu: function() {
		$('.prefooter_one .widget_trigger').on('click',function() {
			$(this).find('i').toggleClass('fa-plus-square fa-minus-square');
			$(this).next().slideToggle();
		})
	}
}

EGA.addScript = {
	handleScroll: function($options) {
		$options = $options || {};
		var el = (typeof $options.el != undefined) ? $options.el : '';
		var cl = (typeof $options.cl != undefined) ? $options.cl : '' ;
		var top = (typeof $options.top != undefined) ? $options.top : '' ;
		var bottom = (typeof $options.bottom != undefined) ? $options.bottom : '';
		$(window).on('scroll', function() {
			if($(window).scrollTop() > Number(top)) {
				el.addClass(cl);
			}
			else {
				el.removeClass(cl);
			}
		})
	},
	topLinksToggle: function() {
		var $topbar_trigger = $('#topbar_trigger'),
				$topbar_mb = $('#topbar_mb');
		$(window).load(function() {
			if($(window).width() <= 991) {
				$(document).click(function() {
					$topbar_mb.hide();
				});
				$topbar_trigger.click(function(e) {
					e.preventDefault();
					e.stopPropagation();
					$topbar_mb.toggle();
				});
			}
			if($(window).width() <= 767) {
				$('.filter_list').removeClass('in');
				// mobile filter
				$('#filter_group').removeClass('in');
				$(".widget_links li input").click(function(){
					$('#filter_group').removeClass('in');
				});

				// category menu
				$('.left_menu .nav-pills > li > a i').click(function(e) {
					e.preventDefault();
					var $show_menu = $(this).closest('li.menu').find('.submenu');
					$('.submenu').slideUp();
					if($show_menu.css('display') == 'none') {
						$show_menu.slideDown();
					}
					else {
						$show_menu.slideUp();
					}
				})
				// endcategory menu
			}
		});
	},
	goToTop: function(options) {
		var $go_top_el = options && options.$go_top_el,
				element_scroll_speed = (options && options.element_scroll_speed) ? options.element_scroll_speed : 700,
				element_scroll_easing = (options && options.element_scroll_easing) ? options.element_scroll_easing : 'easeOutQuad',
				element_offset = (options && options.element_offset) ? options.element_offset : 450;

		var goToTopShow = function() {
			if( $(window).scrollTop() > Number(element_offset) ) {
				$($go_top_el).fadeIn();
			} 
			else {
				$($go_top_el).fadeOut();
			}
		};
		var goToTopScroll = function() {
			$($go_top_el).click(function() {
				$('body,html').stop(true).animate({
					'scrollTop': 0
				}, Number( element_scroll_speed ), element_scroll_easing);
				return false;
			});
		};

		$(window).on('scroll', goToTopShow);
		goToTopScroll();
	},
	productOverlay: function() {
		$('.product_overlay.style_1').click(function(e) {
			if(!$(e.target).is('.product_quick_add') && !$(e.target).is('.item-quick-view')) {
				var url = $(this).attr('product_url');
				location.href=url;
			}
		})
	}
}

EGA.search = {
	headerSearch: function() {
		var $search_trigger = $('.header_search > a'),
				$search_form = $('.header_search form'),
				$menu_wrap = $('.primary_menu_wrap');
		$search_trigger.on('click', function(e) {
			$(this).find('i').toggleClass('glyphicon-search glyphicon-remove ')
			e.preventDefault();
			$search_form.toggleClass('show');
		})
		$(window).resize(function() {
			if($(window).width() > 992) {
				$search_form.width($menu_wrap.width());
			}
		}).trigger('resize');
	}
}




EGA.init = function() {
	EGA.plugin.quickView();
	EGA.plugin.lazyLoad();
	EGA.plugin.tooltip();
	EGA.plugin.counter();
}
$(document).ready(function() {
	EGA.init();
	EGA.footer.toggleMenu();
	EGA.plugin.menu();
	EGA.addScript.topLinksToggle();
	EGA.addScript.goToTop({
		$go_top_el: '#gotoTop',
		element_scroll_speed: 700,
		element_scroll_easing: 'easeOutQuad'
	});
	EGA.addScript.productOverlay();
})
$(window).on('scroll', function() {
	EGA.header.sticky();
})