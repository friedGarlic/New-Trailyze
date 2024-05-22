var counter = 1;
setInterval(function () {
    var radioElement = document.getElementById('radio' + counter);

    if (radioElement) {
        radioElement.checked = true;
        counter++;
    } else {
        counter = 1;
    }
}, 3000);
