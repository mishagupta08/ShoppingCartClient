$(document).ready(function () {
    $(".preloader").hide();

    $("div[name=productDetail]").bind("click", function () {
        GetProductDetailView($(this).attr("id"));
    });
    $("i[name=rating]").bind("click", function () {
        document.getElementById("rating").value = $(this).attr("value");
    });
    $("a[name=addToCart]").bind("click", function () {
        AddProductToCart($(this).attr("data-id"));
    });
});

function AddProductToCart(id) {
    $(".preloader").show();
    var qty = 1;
    if ($("#quantity_value").length > 0)
    {
        qty =  document.getElementById("quantity_value").value;
    }
   
    $.ajax({
        url: '/Home/AddProductToCart',
        type: 'Post',
        datatype: 'Json',
        data: { "ProductId": id, "Quantity": qty }
    }).done(function (result) {
        $("#checkout_items").html("");
        $("#checkout_items").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });
}

function GetProductDetailView(id) {
    $(".preloader").show();
    var url = "/Home/GetProductDetailView?ProdId=" + id;
    window.location.href = url;

    // var id = $(this).attt("id");
    //$.ajax({
    //    url: '/Home/GetProductDetailView',
    //    type: 'Post',
    //    datatype: 'Json',
    //    data: { "ProdId": id }
    //}).done(function (result) {
    //    $("#container").html("");
    //    $("#container").html(result);
    //    $(".preloader").hide();
    //}).fail(function (error) {
    //    $("#error").html(error.statusText);
    //    $(".preloader").hide();
    //});
}

function AddReviewForm() {
    var formData = $('#reviewform').serialize();
    $.ajax({
        url: "/Home/AddProductReview",
        type: "post",
        data: formData,
    }).done(function (result) {
        if (result == "") {
            $("#status").html("Review Submitted.");
        }
        else {
            $("#status").html(result);
        }
        $(".preloader").hide();

    }).fail(function (error) {
        $("#status").html(error.statusText);
        $(".preloader").hide();
    });

    return false;

}


function GetSelectedMenuAction(menuName, menuType, catId) {
    $(".preloader").show();
    $.ajax({
        url: '/Home/GetSelectedMenu',
        type: 'Post',
        datatype: 'Json',
        data: { "SelectedMenu": menuName, "Type": menuType, "catId": catId }
    }).done(function (result) {
        $("#container").html("");
        $("#container").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function ValidateLoginUser() {
    $("#loginError").html("");
    var loginDetail = $('#loginform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Home/ValidateUser',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {

        if (result == "") {
            window.location.href = 'Index';
        }
        else {
            $("#loginError").html(result);
            $(".preloader").hide();
        }
    }).fail(function (error) {
        $("#loginError").html(error.statusText);
        $(".preloader").hide();
        $("#closeError").show();
        $(".preloader").hide();
    });

    return false;
}

function RegisterUser() {
    $("#loginError").html("");
    var loginDetail = $('#registerform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Home/RegisterUserDetail',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {

        if (result == "") {
            window.location.href = 'Index';
        }
        else {
            $("#registrationError").html(result);
            $(".preloader").hide();
        }
    }).fail(function (error) {
        $("#registrationError").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}