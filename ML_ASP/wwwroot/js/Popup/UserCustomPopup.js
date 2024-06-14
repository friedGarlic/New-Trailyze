document.addEventListener('DOMContentLoaded', function () {
    var popup = document.getElementById('popup');
    var popup2 = document.getElementById('popup2');
    var popup3 = document.getElementById('popup3');

    var addDocumentButton = document.getElementById('addDocumentButton');
    var addDocumentButton2 = document.getElementById('addDocumentButton2');
    var addDocumentButton3 = document.getElementById('addDocumentButton3');

    var closeButton = document.querySelector('.close-btn');
    var closeButton2 = document.querySelector('.close-btn2');
    var closeButton3 = document.querySelector('.close-btn3');

    function showPopup() {
        popup.style.display = 'flex';
    }
    function showPopup2() {
        popup2.style.display = 'flex';
    }
    function showPopup3() {
        popup3.style.display = 'flex';
    }

    function hidePopup() {
        popup.style.display = 'none';
    }
    function hidePopup2() {
        popup2.style.display = 'none';
    }
    function hidePopup3() {
        popup3.style.display = 'none';
    }

    // Prevent default form submission when "Add Document" button is clicked
    addDocumentButton.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup(); // Show the popup instead
    });
    addDocumentButton2.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup2(); // Show the popup instead
    });
    addDocumentButton3.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup3(); // Show the popup instead
    });

    closeButton.addEventListener('click', hidePopup);
    closeButton2.addEventListener('click', hidePopup2);
    closeButton3.addEventListener('click', hidePopup3);


});
