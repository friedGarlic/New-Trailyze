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

    // Prevent form submission when "Add Document" button is clicked
    addDocumentButton.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default form submission
        showPopup(); // Show the popup instead
    });

    closeButton.addEventListener('click', hidePopup);
});
