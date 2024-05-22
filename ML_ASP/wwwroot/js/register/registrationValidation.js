
$(document).ready(function () {
    $('#Password').on('input', function () {
        var password = $(this).val();
        var specialCharError = $('#specialCharError');
        var numericCharError = $('#numericCharError');
        var lowercaseCharError = $('#lowercaseCharError');

        if (password.match(/[\W]/)) {
            specialCharError.hide();
        } else {
            specialCharError.show();
        }

        if (password.match(/\d/)) {
            numericCharError.hide();
        } else {
            numericCharError.show();
        }

        if (password.match(/[a-z]/)) {
            lowercaseCharError.hide();
        } else {
            lowercaseCharError.show();
        }
    });
});
