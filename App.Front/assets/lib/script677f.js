
document.addEventListener('DOMContentLoaded', function () {
    $('.body_preloader').addClass('loaded');
})

document.addEventListener('DOMContentLoaded', function () {
    //responsive menu
    $("#menu-toggle, .body_overlay").click(function (e) {
        e.preventDefault();
        var $show_menu = $('#menu_xs, .body_overlay, body, #menu-toggle');
        $show_menu.toggleClass("toggled");
    });
    $('#menu_xs ul a span').click(function (e) {
        e.preventDefault();
        $(this).parent().next().toggle(200);
        if ($(this).text() == '+')
            $(this).text('-');
        else
            $(this).text('+');
    })
    //end responsive menu
})

document.addEventListener('DOMContentLoaded', function () {
    //EGA.search.headerSearch();
})

document.addEventListener('DOMContentLoaded', function () {
    $("#sidebar-wrapper ul li a span").click(function (event) {
        event.preventDefault();
        var li = $(this).parents('li');
        li.children('ul').toggle('slow');
    });
})

document.addEventListener('DOMContentLoaded', function () {
    $('#slider').slick({
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        autoplay: true,
        autoplaySpeed: 5000,
        fade: true,
        responsive: [
            {
                breakpoint: 991,
                settings: {
                    dots: true,
                    arrows: false
                }
            }
        ]
    })
});

document.addEventListener('DOMContentLoaded', function () {
    $('.testimonials_content').slick({
        slidesToShow: 2,
        slidesToScroll: 1,
        arrows: false,
        dots: true,
        responsive: [
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 3,
                    arrows: false
                }
            }
        ]
    });
});

if (typeof EGA === 'undefined') EGA = {};
EGA.product = {};
EGA.plugin = {};
EGA.addScript = {};
EGA.header = {};
EGA.filter = {};
EGA.options = {
    money_format: "{{amount}}₫",
}


EGA.product = {
	/*
* Tu.Nguyen
* v.1.0 20160729
* update variant
*/
	updateVariant: function(options) {
		var variant = options.variant,
				$tagProductSection = options.selector;

		// selector 
		var $tagPrice = $($tagProductSection).find('.product-price ins'),
				$tagPriceScroll = $('#detail-two .variant-price'),
				$tagPriceCompare = $($tagProductSection).find('.product-price del'),
				$tagPriceCompareScroll = $('#detail-two .price_compare del'),
				$tagSku = $($tagProductSection).find('.sku'),
				$tagSkuScroll = $('#detail-two h4'),
				//$tagPriceCompareScroll = $('#detail-two .variant-price'),
				$tagSale = $($tagProductSection).find('.sale-flash'),
				$tagAddToCart = $($tagProductSection).find('.add_to_cart');
		//unit_price = 0,
		//unit_price_compare = 0;

		if (variant) {
			// Select a valid variant if available
			if (variant.available) {
				$tagAddToCart.html('Thêm vào giỏ'); 
				$tagAddToCart.removeAttr('disabled');  
			}
			else {
				$tagAddToCart.html('Cháy hàng'); 
				$tagAddToCart.prop('disabled', true); 
			}

			// Regardless of stock, update the product price
			$tagPrice.html(Haravan.formatMoney(variant.price, EGA.options.money_format)).show();
			$tagPriceScroll.html('Giá: ' + Haravan.formatMoney(variant.price, EGA.options.money_format)).show();
			$($tagProductSection).find('.unit_price_not_formated').val(variant.price); 

			// Also update and show the product's compare price if necessary
			if (variant.compare_at_price > variant.price) {
				var pro_sold = variant.price;
				var pro_comp = variant.compare_at_price / 100;
				var sale = 100 - (pro_sold / pro_comp) ;
				var kq_sale = Math.floor(sale);

				//show onsale label
				$tagPriceCompare.html(Haravan.formatMoney(variant.compare_at_price, EGA.options.money_format)).show();
				$tagPriceCompareScroll.html(Haravan.formatMoney(variant.compare_at_price, EGA.options.money_format)).show();
				$tagSale.html('-' + kq_sale + '%');
				$tagSale.removeClass('hidden');
				$('.price_off').html(Haravan.formatMoney(
					variant.compare_at_price - variant.price, EGA.options.money_format
				) + ' (-' + kq_sale + '%)');
			} else {
				$tagPriceCompare.hide();
			}
		}
		else {
			// The variant doesn't exist, disable submit button.
			// This may be an error or notice that a specific variant is not available.
			// To only show available variants, implement linked product options:
			//   - http://docs.haravan.com/manual/configuration/store-customization/advanced-navigation/linked-product-options
			$tagPrice.hide();
			$tagPriceScroll.hide();
			$tagPriceCompare.hide();
			$tagAddToCart.html('Cháy hàng'); 
			$tagAddToCart.prop('disabled', true);
		}
		if(variant.sku != undefined) {
			$tagSku.html('Mã SP: ' + variant.sku).show();
			$tagSkuScroll.html('Mã SP: ' + variant.sku).show();
		}
		else {
			$tagSku.hide();
			$tagSkuScroll.hide();
		}
	},


	/*
* Tu.Nguyen 
* v.1.0 20160727
* refreshProductSelections
*/
	refreshProductSelections: function($tag_select_arr, option_arr) {
		for(i in $tag_select_arr) {
			if(option_arr[i] != null && option_arr[i] != '') {
				$($tag_select_arr[i] + ' option[value="' + option_arr[i] + '"]').prop('selected', true); // option-0 => Shape...  okok 
				$($tag_select_arr[i]).change();
			}
		}
	},

	/*
* Tu.Nguyen
* v.1.0 20160727
* update variant on change dropdown box 
*/
	variantChange: function(options) {
		var $dropdown_list = options.$dropdown_list,
				$tag_select_arr = options.$tag_select_arr;
		$($dropdown_list).find('option:first').attr('selected', 'selected');
		$($dropdown_list).on('change', function(index, value) {
			var option_arr = ['', '', ''];
			var option_group = {};
			$.each($('.sizePicker select'), function(i, v) {
				option_arr[i] = $(this).val();
			})

			EGA.product.refreshProductSelections($tag_select_arr, option_arr);
		})
	},


	/*
* Tu.Nguyen
* v.1.0 20160728
* Change quantity
*/
	updateQuantity: function() {
		$.each($('.product-quantity'), function() {
			var quantity = parseInt($(this).find('input.qty').val());

			$(this).find('.minus').click(function() {
				if (quantity > 0) {
					if (quantity == 1) {
						$(this).parents('[class^="product-page"]').find('.add_to_cart').attr('disabled','disabled');
					}
					quantity -= 1;
				}
				else {
					quantity = 0;
				}
				$(this).parent().find('input.qty').val(quantity);

			});
			$(this).parent().find('.plus').click(function() {
				$(this).parents('[class^="product-page"]').find('.add_to_cart').removeAttr('disabled');
				if (quantity < 100) {
					quantity += 1;
				}
				else {
					quantity = 100;
				}
				$(this).parent().find('input.qty').val(quantity);
			});

			$(this).find('input.qty').on('change', function(){
				var $qty = parseInt($(this).val()); 
				if($qty <= 0){
					$(this).parents('[class^="product-page"]').find('.add_to_cart').attr('disabled','disabled');
				}
				else{
					$(this).parents('[class^="product-page"]').find('.add_to_cart').removeAttr('disabled');
				}
			});
		})
	},


	/*
* Tu.Nguyen 
* v.1.0 20160727
*	check cart empty 
*/
	check_topcart_empty: function(cart) {

		if(cart.item_count <= 0) {
			var top_cart_empty = '<div class="minicart-header">Chưa có sản phẩm trong giỏ!</div>'
			+ '<div class="minicart-footer">'
			+ '<div class="minicart-actions clearfix">'
			+ '<a class="button" href="/collections/all"><span class="text">VÀO CỬA HÀNG</span></a>'
			+ '</div></div>';

			$('.top-cart-title, .top-cart-action').remove();
			top_cart_no_item = $('.top-cart-block .top-cart-content').html(top_cart_empty);
			return true;
		}
		else {
			return false;
		}
	},


	/*
* Tu.Nguyen 
* v.1.0 20160727
*	Update Cart Ajax
*/
	updateCartAjax: function(cart) {
		//if(!EGA.product.check_topcart_empty(cart)) {
		//	var total_price_format = Haravan.formatMoney(cart.total_price, EGA.options.money_format);
		//	var html = '';
		//	html += '<div class="top-cart-title "><h4>Giỏ hàng</h4></div>'
		//	+ '<div class="top-cart-items">';
		//	$.each(cart.items, function(i, item) {
		//		var line_price_format = Haravan.formatMoney(item.line_price, EGA.options.money_format);
		//		var img_url = Haravan.resizeImage(item.image, 'small');
		//		html += '<div class="top-cart-item clearfix">'
		//		+ '<div class="top-cart-item-image">'
		//		+ '<a href="' + item.url + '"><img src="' + img_url + '" alt="' + item.title + '" /></a>'
		//		+ '</div>'
		//		+ '<div class="top-cart-item-desc">'
		//		+ '<a href="' + item.url + '">' + item.title + '</a>'
		//		+	'<span class="top-cart-item-price">' + line_price_format + '</span>'
		//		+	'<span class="top-cart-item-quantity">x ' + item.quantity + '</span>'
		//		+ '<a class="top_cart_item_remove" onclick = "EGA.product.deleteItem(' + item.id + ');">'
		//		+	'<i class="fa fa-times-circle"></i>'
		//		+	'</a>'
		//		+	'</div>'
		//		+	'</div>';
		//	})

		//	html += '</div><div class="top-cart-action clearfix ">'
		//	+ '<span class="fleft top-checkout-price">' + total_price_format + '</span>'
		//	+ '<button onclick="window.location.href=&quot;/cart&quot;" class="button button-small nomargin fright">Xem giỏ hàng</button>'
		//	+ '</div>';

		//	$('.top-cart-content').html(html);
		//}
		//$('.top_cart_qty').html(cart.item_count);
	},


	/*
* Tu.Nguyen 
* v.1.0 20160727
*	Add item to cart
*/
	addItemSuccess: function(data) {
		addToCartPopup(data);
		Haravan.getCart(EGA.product.updateCartAjax);
	},
	addItem: function(form_id) {
		Haravan.addItemFromForm(form_id, EGA.product.addItemSuccess);
	},


	/**
* Tu.Nguyen 
* v.1.0 20160727
 * Remove item in cart ajax
 */
	deleteItem: function(variant_id) {
		Haravan.removeItem(variant_id, EGA.product.updateCartAjax);
	},


}

jQuery(document).ready(function($){
	// Update variant on change dropdown box
	EGA.product.variantChange({
		$dropdown_list: '#ProductDetailsForm .sizePicker select',
		$tag_select_arr: ['#product-select-option-0', '#product-select-option-1', '#product-select-option-2']
	});

	//change qty...
	EGA.product.updateQuantity();

	//add to cart 
	$(".add_to_cart").on('click', function(e) {  //.click(function(e){ // 
		e.preventDefault();
		var form_id = $(this).closest('form').attr('id');
		EGA.product.addItem(form_id);
	});


	// buy now
	$('.buynow').on('click', function(e) {
		var form = $(this).closest('form').attr('id');
		e.preventDefault();
		buyNow(form)
	});
	// end buy now

	$('.buynow_scroll').on('click', function(e) {
		e.preventDefault();
		$('#ProductDetailsForm .buynow').click();
	})
});  

buyNow = function(form) {
	var callback = function() {
		window.location = '/checkout';
	}
	Haravan.addItemFromForm(form, callback);
}
// >>>>>> product END

/*
	* Tu.Nguyen 
* v.1.0 20160727
 * Popup notify add-to-cart
 */
function addToCartPopup(jqXHR, textStatus, errorThrown) {
	var $info = '<div class="row"><div class="col-md-4"><a href="'+ jqXHR['url'] +'"><img width="70px" src="'+ Haravan.resizeImage(jqXHR['image'], 'small') +'" alt="'+ jqXHR['title'] +'"/></a></div><div class="col-md-8"><div class="jGrowl-note"><a class="jGrowl-title" href="'+ jqXHR['url'] +'">'+ jqXHR['title'] +'</a><ins>'+ Haravan.formatMoney(jqXHR['price'], EGA.options.money_format) +'</ins></div></div></div>';
	var wait = setTimeout(function(){
		$.jGrowl($info,{
			life: 5000
		});	
	});
}



// phong.nguyen 20160406: EGANY declare default values.  
var $str_gallery_id = '.slider-wrap'; 
var ega_default_pickers_params = { 
	// options-picker (for changing images) 
	id_options_all: '#ProductDetailsForm .product-page-options',
	id_option0_picker: '#ProductDetailsForm #option0Picker',
	id_option1_picker: '#ProductDetailsForm #option1Picker',
	id_option2_picker: '#NULL', 
	// options-selection (for doing add-to-cart) 
	tagSelectOption0: '#product-select-option-0',
	tagSelectOption1: '#product-select-option-1',
	tagSelectOption2: '#product-select-option-2', 

	// gallery section: ids, functions 
	str_gallery_id: $str_gallery_id,
	str_gallery_tag_with_option_code: $str_gallery_id + ' div.slide',
	func_init_gallery_and_callback: function(){}, 
	func_init_gallery_raw_content: false // function(){} 
};  
var boDefaultPro = false; // phong.nguyen 20160406: determine default-product (1 variant, 1 option) 

/* 
 * Widget "images_by_options": Select images by relevant-options. 
 * Function in detail: picker events, refresh gallery, refresh options-selection. 
 * 
 * @author: phong.nguyen 20160406  
 * @version: 2.0 
 */ 
var images_by_options = { // ibo ~ IPO :D 
	init: function(params){
		// phong.nguyen 20160408: params as widget-options 
		this.opts = params; 

		// phong.nguyen 20160408: fixbug conflict QuickView vs product single-page.  
		this.func_set_params_to_pickers = function($current_options_pickerOK, $widget_opts){ 
			// phong.nguyen note: this === images_by_options 
			// options-picker (for changing images) + functions 
			$current_options_pickerOK.id_options_all = $widget_opts.id_options_all; 
			$current_options_pickerOK.id_option0_picker = $widget_opts.id_option0_picker; 
			$current_options_pickerOK.id_option1_picker = $widget_opts.id_option1_picker; 
			$current_options_pickerOK.id_option2_picker = $widget_opts.id_option2_picker;  
			// options-selection (for doing add-to-cart) 
			$current_options_pickerOK.tagSelectOption0 = $widget_opts.tagSelectOption0; 
			$current_options_pickerOK.tagSelectOption1 = $widget_opts.tagSelectOption1; 
			$current_options_pickerOK.tagSelectOption2 = $widget_opts.tagSelectOption2;  
			// gallery section: ids, functions 
			$current_options_pickerOK.str_gallery_id = $widget_opts.str_gallery_id; 
			$current_options_pickerOK.str_gallery_tag_with_option_code = $widget_opts.str_gallery_tag_with_option_code; 
			$current_options_pickerOK.html_gallery_content_raw = $($widget_opts.str_gallery_id).html(); 
			// functions: render RAW-content, init gallery. 
			$current_options_pickerOK.func_init_gallery_and_callback = $widget_opts.func_init_gallery_and_callback;  
			$current_options_pickerOK.func_init_gallery_raw_content = $widget_opts.func_init_gallery_raw_content;  
		};  

		// grant values, run 1-time
		$(images_by_options.opts.id_option0_picker + ' ul li').each(function(){  
			images_by_options.func_set_params_to_pickers(this, images_by_options.opts); 
		}); 
		// grant values, run 1-time
		$(images_by_options.opts.id_option1_picker + ' ul li').each(function(){  
			images_by_options.func_set_params_to_pickers(this, images_by_options.opts); 
		}); 
		// grant values, run 1-time
		$(images_by_options.opts.id_option2_picker + ' ul li').each(function(){  
			images_by_options.func_set_params_to_pickers(this, images_by_options.opts); 
		}); 


		/* 
	 * refreshGalleryByOptions
	 * must be customized by slider business-flow. Ex. for Galleria: gallery.destroy()...	Galleria.run('#galleria',...});... 
	 * 
	 * @author: phong.nguyen 20160312 
	 */ 
		this.refreshGalleryByOptions = function ($option0_code, $option1_code, $current_options_picker)
		{

			// set RAW-content for gallery.
			if ($current_options_picker.func_init_gallery_raw_content == false) { // ($current_options_picker.html_gallery_content_raw != ''){  
				$($current_options_picker.str_gallery_id).html($current_options_picker.html_gallery_content_raw); 
			} 
			else{
				$current_options_picker.func_init_gallery_raw_content(); 
			}

			// remove invalid-slides.  
			if( boDefaultPro == false ){
				$($current_options_picker.str_gallery_tag_with_option_code).each(function(){   
					if //(&&
						( ($(this).attr('data_option0_code') != $option0_code)  
						 //| ($(this).attr('data_option1_code') != $option1_code)
						) // )
					{ 
						$(this).remove(); 
						// $(this).addClass('hidden'); 
					}
					else{ 
						// $(this).removeClass('hidden'); 
					}
				});
				if($($current_options_picker.str_gallery_tag_with_option_code).length == 0){
					$($current_options_picker.str_gallery_id).html($current_options_picker.html_gallery_content_raw); 

				}
			}
			else{
				// hidden options-pickers 
				$($current_options_picker.id_options_all).addClass('hidden'); 

			} // end if:  boDefaultPro == false 
			// init gallery 
			$current_options_picker.func_init_gallery_and_callback();   

		};    


		/* 
	 * show_relevant_options1 by option0 
	 * 
	 * @author: phong.nguyen 20160405  
	 */ 
		this.show_relevant_options1 = function($option0_code, $current_options_picker, $bo_select_first_option1 ){ 

			$first_option1 = {}; 
			$bo_selected_first = false; 
			$($current_options_picker.id_option1_picker + ' ul li').each(function(){                            
				if($(this).attr('data_option0_code')!=$option0_code)
				{
					$(this).addClass('hidden');
				}
				else 
				{
					$(this).removeClass('hidden'); 
					if($bo_select_first_option1 == true && $bo_selected_first == false)
					{
						// select first color-icon ... 
						$first_option1['title'] = $(this).attr('title');   
						$first_option1['code'] = $(this).attr('data_option1_code');   
						$(this).addClass('selected'); 
						$bo_selected_first = true; 
					}
				}

			});
			return $first_option1; 
		};

		/* 
	 * function: click option0-values 
	 * 
	 * @author: phong.nguyen 20160312 
	 */ 
		$(images_by_options.opts.id_option0_picker + ' ul li').click(function(){ 

			//select current li  
			$(this.id_option0_picker + ' ul li').removeClass('selected');   
			$(this).addClass('selected');    

			//showing only suitable options 
			$option0_code = $(this).attr('data_option0_code');
			$(this.id_option1_picker + ' ul li').removeClass('hidden'); 
			$(this.id_option1_picker + ' ul li').removeClass('selected'); 
			$first_option1_code = '';  
			$option1_title = '';  
			$bo_select_first_option1 = true; 
			$first_option1 = images_by_options.show_relevant_options1($option0_code, this, $bo_select_first_option1);  
			$first_option1_code = $first_option1['code'];  
			$option1_title = $first_option1['title'];  
			// alert($option0_code +'; '+ $first_option1_code);   

			//change select inside  form add-item-form    
			$option0_title = $(this).attr('title'); 
			// $option1_title = ...; 
			$option2_title = null;
			EGA.product.refreshProductSelections([this.tagSelectOption0,
																						this.tagSelectOption1,
																						this.tagSelectOption2],
																					 [$option0_title,
																						$option1_title, 		 
																						$option2_title]); 

			//refresh gallery...  
			images_by_options.refreshGalleryByOptions($option0_code, $first_option1_code, this); 

			//update total amount... 
			//update_total(); 
		}); // end func: click option0 

		/* 
	 * function: click option1-values 
	 * 
	 * @author: phong.nguyen 20160312 
	 */ 
		$(images_by_options.opts.id_option1_picker + ' ul li').click(function(){
			//change select inside  form add-item-form  
			$option0_title = $(this.id_option0_picker + ' ul li.selected').attr('title'); 
			$option1_title = $(this).attr('title'); 
			$option2_title = null; 
			// refreshProductSelections($shape, $option1_title);
			EGA.product.refreshProductSelections([this.tagSelectOption0, 
																						this.tagSelectOption1,
																						this.tagSelectOption2],
																					 [$option0_title,
																						$option1_title,
																						$option2_title]);  

			//select current li  
			$(this.id_option1_picker + ' ul li').removeClass('selected');   
			$(this).addClass('selected');  

			$option0_code = $(this.id_option0_picker + ' ul li.selected').attr('data_option0_code'); 
			$option1_code = $(this.id_option1_picker + ' ul li.selected').attr('data_option1_code'); 
			//refresh gallery...  
			images_by_options.refreshGalleryByOptions($option0_code, $option1_code, this); 

			//update total amount... 
			//update_total(); 
		}); // end func: click option1 

		/* 
	 * function: click option1-values 
	 * 
	 * @author: phong.nguyen 20160408 
	 */ 
		//this.set_selected_for_options = function($option0_code, $option1_code, $object_current_options_picker){
		//	// remove all selected-items. 
		//	//$($object_current_options_picker.id_option0_picker + ' ul li').removeClass('selected'); 
		//	//$($object_current_options_picker.id_option1_picker + ' ul li').removeClass('selected'); 

		//	// option0: set selected  ... 
		//	var $optionMatching0 = '[data_option0_code="'+ $option0_code + '"]'; 
		//	var $optionMatching1 = $optionMatching0 + '[data_option1_code="'+ $option1_code + '"]';  
		//	//var $jqOption0 = $($object_current_options_picker.id_option0_picker + ' ul li'+ $optionMatching0 +':first'); 
		//	//var $objOption0 = $jqOption0[0]; // object HTML "options_picker"...  
		//	//$jqOption0.addClass('selected');  

		//	// option1: show only relevant option1(s) (by option0)  && selected for option1 
		//	$bo_select_first_option1 = false; 
		//	$first_option1 = images_by_options.show_relevant_options1($option0_code, $objOption0, $bo_select_first_option1); 
		//	$($object_current_options_picker.id_option1_picker + ' ul li'+ $optionMatching1 +':first').addClass('selected'); 


		//	// loading gallery... 
		//	images_by_options.refreshGalleryByOptions($option0_code, $option1_code, $objOption0); 
		//}; 

		// phong.nguyen 20160408: make default-view. 
		// select first option0,1,2. 
		//$jq_option0_first = $(this.opts.id_option0_picker + " ul li:first");  
		//var $option0_code = $jq_option0_first.attr('data_option0_code'); 
		//var $option1_code = $(this.opts.id_option1_picker + ' ul li[data_option0_code="'+ $option0_code + '"]:first').attr('data_option1_code'); 
		//this.set_selected_for_options($option0_code, $option1_code, $jq_option0_first[0]); 


	}// end function init() 
};  // end object: init_select_images_by_options 


/* Quick Add 
----------------------------------- */
var currentIMG = '';
$(document).on('click', '.product_quick_add', function (e) {
	e.stopPropagation();
	e.preventDefault();
	// quick add 
	currentIMG = $(this).parents('.product').find('.product-image img'); 
	quickAddProduct($(this).attr('href')); // make-up "#product-quick-add";
});
var callBackQuickAdd = function(variant, selector) {  
	// console.log('callBackQV  is functioning...');
	var $tagProductSection = '#ProductDetailsForm_QuickAdd'; 
	EGA.product.updateVariant({
		variant: variant,
		selector: $tagProductSection
	});
}
var quickAddProduct = function (purl) {
	$.getJSON( purl + '.js', function (product) { 
		//make-up "#product-quick-add"   
		// assign hidden values 
		$('#product-quick-add .product_title_hd').val(product.title);  
		$('#product-quick-add .product_url').val(product.url);  
		$('#product-quick-add .product_img_small').val(product.featured_image);  // AAA!!!  

		//render selections (NOT formatted yet)
		$('select#product-select-qa').html('');  
		$('.selector-wrapper').remove(); // remove all selectors by  Haravan.OptionSelectors  
		// var $arrSize = [];
		// var $arrColor = []; 
		$.each(product.variants, function (i, v) {
			$('select#product-select-qa').append("<option value='" + v.id + "'>" + v.title + ' - ' + v.price + "</option>");
		});  

		new Haravan.OptionSelectors("product-select-qa", { 
			product: product, 
			onVariantSelected: callBackQuickAdd 
		});
		// select default variants

		//auto add current variant... 
		// $('#ProductDetailsForm_QuickAdd').submit(); 
		$("#addtocartQA").click(); 

	});//end: success 

	//$('.modal-backdrop').css('opacity', '0');
	return false;
}


jQuery(document).ready(function($){
	//add to cart for QuickView
	$("#addtocartQA").on('click', function(e) {  //.click(function(e){ // 
		e.preventDefault();
		EGA.product.addItem('ProductDetailsForm_QuickAdd');
	}); 
}); 
/*** end quick add cart ***/