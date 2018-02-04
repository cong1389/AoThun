$(function () {
    window.setLocation = function (url) {
        window.location.href = url;
    }

    $("#ProductDetailsForm .product-attr .item input")
        .click(function () {
            $(".conten-attr .item input").parents(".item").removeClass("active");
            $(".conten-attr .item input").removeAttr('checked');
            $(this).parents(".item").toggleClass("active");
            $(this).attr('checked', 'checked');
            var value = this.value;

            $.post("/gallery-images.html",
                { postId: $(this).attr("data-post"), typeId: value },
                function (response) {
                    if (response.success) {
                        $("#gallery").data('royalSlider').destroy();
                        $("#gallery").html(response.data);
                        initGallery();
                    }
                });

            $.post("/getprice.html",
                { productId: $(this).attr("data-post"), attributeId: value },
                function (price) {

                    if (price === parseInt(price, 10)) {
                      
                        var priceOriginalUnit = Haravan.formatMoney(price, EGA.options.money_format);

                        //Gía nguyên thủy
                        $("#ProductDetailsForm .product-price #span-list-price").html(priceOriginalUnit);
                        $("#detail-two .variant-price ins span ").html(priceOriginalUnit);
                        
                        //product-discount
                        var discount = $('#product-discount').html();
                        var priceDiscount = price * parseInt(discount) / 100;//Số tiền được giảm
                        var priceAfterDicount = price - priceDiscount;//Giá đã trừ tiền giảm
                        var pricePromotionUnit = Haravan.formatMoney(priceAfterDicount, EGA.options.money_format);

                        $('#span-saving-price').html('(' + Haravan.formatMoney(priceDiscount, EGA.options.money_format) + ')');
                        $("#ProductDetailsForm .product-price ins").html(pricePromotionUnit);

                        $("#ProductDetailsForm #hddPrice").val(priceAfterDicount);
                    }
                    else {
                        $("#ProductDetailsForm .product-price ins").html(price);
                    }

                });

        });

});

jQuery(document).ready(function () {
    //if ($("#homeslide").length) {
    //    if ($(".product-items").length) {
    //        var page = 1;
    //        setInterval(function () {
    //            $(".product-items").each(function (index) {
    //                var id = $(this).attr("attr-id");
    //                if (id != undefined) {
    //                    $.post('/Post/GetProductNewHome', { page: page, id: $("input[name=" + id + "]").val() }, function (response) {
    //                        $("#" + id + "").empty().html(response.data);
    //                    });
    //                    page++;
    //                    if (page > 2) {
    //                        page = 1;
    //                    }
    //                }
    //            })
    //            //for (var i = 0; i < $(".product-items").length - 1; i++) {  
    //            //    var id = $($(".product-items")[i]).attr("attr-id"); 
    //            //    $.post('/Post/GetProductNewHome', { page: page, id: $("input[name="+id+"]").val() }, function (response) { 
    //            //            $("#" + id + "").empty().html(response.data);
    //            //        });
    //            //    page++;
    //            //    if (page > 2) {
    //            //        page = 1;
    //            //    }
    //            //}
    //        }, 5000);
    //        setTimeout(function () {
    //            $(".product-block").find('.animated').removeClass('go').addClass('go');
    //        }, 300);

    //    }


    //    //var pageOld = 1;
    //    //setInterval(function () {
    //    //    $.post('/Post/GetProductNewHome',
    //    //        { page: pageOld, id: $("#NewProduct").val(), isNew: false },
    //    //        function (response) {
    //    //            $(".old-block").empty().html(response.data);
    //    //        });
    //    //    pageOld++;
    //    //    if (pageOld > 2) {
    //    //        pageOld = 1;
    //    //    }
    //    //}, 5000);
    //    //setTimeout(function () {
    //    //    $(".old-block").find('.animated').removeClass('go').addClass('go');
    //    //}, 300);
    //    var pageAccess = 1;
    //    setInterval(function () {
    //        $.post('/Post/GetAccesssoriesHome',
    //            { page: pageAccess, id: $("#Accessories").val() },
    //            function (response) {
    //                $(".accessories-block").empty().html(response.data);
    //            });
    //        pageAccess++;
    //        if (pageAccess > 2) {
    //            pageAccess = 1;
    //        }
    //    }, 5000);
    //    setTimeout(function () {
    //        $(".accessories-block").find('.animated').removeClass('go').addClass('go');
    //    }, 300);
    //}
});

function LoadCategoryHome(virtualId, parentId) {
    $.post('/fixitem-content.html',
        { virtualId: virtualId },
        function (response) {
            if (response.success) {
                $(".leftProductContent_" + parentId + "").empty().html(response.data);
            }
        });
}

function showTool() {
    var heightToShow = $(".top-head").height() +
        $(".header").height() +
        $(".nav-menu").height() +
        $("#homeslide").height();

    $(window).scroll(function () {
        if ($(window).scrollTop() >= heightToShow) {
            $(".nav-tools").stop().animate({
                left: '0',
                top: $(window).height() / 3
            })
        } else {
            $(".nav-tools").stop().animate({
                left: '-80px',
                top: heightToShow
            });
        }
    });
}
function goToByScroll(id) {
    // Remove "link" from the ID
    id = id.replace("link", "");
    // Scroll
    $('html,body').animate({
        scrollTop: $("." + id).offset().top
    },
        'slow');
}

function handleError(msg) {
    $("#msg-error").html(msg).show();
}
function handleSuccess(msg) {
    $("#msg-success").html(msg).show();
}

$(function () {
    $("#CheckProduct").click(function (e) {
        var fromId = "#checking";
        formAjax(fromId);
        return false;
    });
    $("#BuyProduct").click(function (e) {
        var fromId = "#formbuy";
        formAjax(fromId);
        return false;
    });
    $("#checkorder").click(function (e) {
        var code = $("#oderCode").val();
        var name = $("#NameOrPhome").val();
        $.post('/home/CheckOrder'
            , { phone: name, ordercode: code }
            , function (response) {
                $(".result_check").empty().html(response.data);
            }
        )
        return false;
    });
});

function formAjax(element) {
    var $form = $(element);
    var options = {
        beforeSend: function () {
            $(".ajax-loading").show();
        },
        dataType: 'json',
        complete: function (responseText, statusText, xhr) {
            var resonse = responseText.responseJSON;
            if (resonse.success) {
                $form[0].reset();
                alert("Gửi thông tin thành công.");
            } else {
                $(".ajax-loading").hide();
                //show message
                alert(resonse.errors);
                $('html, body').animate({
                    scrollTop: $("#form-flow").offset().top
                },
                    2000);
            }
        }
    };

    if ($form.valid()) {
        $form.ajaxSubmit(options);
    }
    return false;
}
