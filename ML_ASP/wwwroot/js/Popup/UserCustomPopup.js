document.addEventListener('DOMContentLoaded', function () {
    var popup = document.getElementById('popup');
    var addDocumentButton = document.getElementById('addDocumentButton');
    var closeButton = document.querySelector('.close-btn');

    function showPopup() {
        popup.style.display = 'flex';
    }

    function hidePopup() {
        popup.style.display = 'none';
    }

    // Prevent default form submission when "Add Document" button is clicked
    addDocumentButton.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup(); // Show the popup instead
    });

    closeButton.addEventListener('click', hidePopup);

});
