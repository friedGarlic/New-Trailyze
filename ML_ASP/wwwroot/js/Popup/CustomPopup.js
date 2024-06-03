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

    /*-------------------------------------2nd POPUP*/

    var popup2 = document.getElementById('popup2');
    var addDocumentButton2 = document.getElementById('addDocumentButton2');
    var closeButton2 = document.querySelector('.close-btn2');

    function showPopup2() {
        popup2.style.display = 'flex';
    }

    function hidePopup2() {
        popup2.style.display = 'none';
    }

    // Prevent form submission when "Add Document" button is clicked
    addDocumentButton2.addEventListener('click', function (event) {
        event.preventDefault(); 
        showPopup2();
    });

    closeButton2.addEventListener('click', hidePopup2);

    /*-------------------------------------3RD POPUP*/

    var popup3 = document.getElementById('popup3');
    var addDocumentButton3 = document.getElementById('addDocumentButton3');
    var closeButton3 = document.querySelector('.close-btn3');

    function showPopup3() {
        popup3.style.display = 'flex';
    }

    function hidePopup3() {
        popup3.style.display = 'none';
    }

    // Prevent form submission when "Add Document" button is clicked
    addDocumentButton3.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup3();
    });

    closeButton3.addEventListener('click', hidePopup3);

    /*-------------------------------------4RD POPUP*/

    var popup4 = document.getElementById('popup4');
    var addDocumentButton4 = document.getElementById('addDocumentButton4');
    var closeButton4 = document.querySelector('.close-btn4');

    function showPopup4() {
        popup4.style.display = 'flex';
    }

    function hidePopup4() {
        popup4.style.display = 'none';
    }

    // Prevent form submission when "Add Document" button is clicked
    addDocumentButton4.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup4();
    });

    closeButton4.addEventListener('click', hidePopup4);

    /*-------------------------------------5RD POPUP*/

    var popup5 = document.getElementById('popup5');
    var addDocumentButton5 = document.getElementById('addDocumentButton5');
    var closeButton5 = document.querySelector('.close-btn5');

    function showPopup5() {
        popup5.style.display = 'flex';
    }

    function hidePopup5() {
        popup5.style.display = 'none';
    }

    // Prevent form submission when "Add Document" button is clicked
    addDocumentButton5.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup5();
    });

    closeButton5.addEventListener('click', hidePopup5);
});
