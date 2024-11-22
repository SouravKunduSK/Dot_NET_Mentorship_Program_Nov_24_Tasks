// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Find the alert element
    var alertElement = document.getElementById('autoToast');

    // If the alert is present, hide it after 5 seconds
    if (alertElement) {
        setTimeout(function () {
            var alert = new bootstrap.Alert(alertElement);  // Bootstrap Alert
            alert.close();  // Close the alert
        }, 7000);  // 5000ms = 5 seconds
    }
});