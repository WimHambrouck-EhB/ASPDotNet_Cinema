// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var invoer = "";
$(document).keydown(function (evt) {
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 0);

    var volgorde = "38384040373937396665"; //up, up, down, down, left, right, left, right, b, a

    invoer += charCode;

    if (invoer.length > volgorde.length) {
        invoer = invoer.substr((invoer.length - volgorde.length));
    }

    if (invoer == volgorde) {
        konami();
    }

    //console.log(invoer);
})

function konami() {
    $.ajax({
        url: "/Home/SelfDestruct",
        type: "GET",
        success: function () {
            location.href = '/Home/Index';
        }
    });
}