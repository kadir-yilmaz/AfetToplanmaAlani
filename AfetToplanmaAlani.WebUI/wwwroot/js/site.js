// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Auto-dismiss alerts after 5 seconds
    window.setTimeout(function () {
        $(".alert.alert-dismissible").fadeTo(500, 0).slideUp(500, function () {
            $(this).remove();
        });
    }, 5000);
});
