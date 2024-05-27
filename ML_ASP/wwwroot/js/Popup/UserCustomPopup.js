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

    //$(document).ready(function () {
    //    $('#submitOvertime').click(function () {
    //        // Get values from input fields and textarea
    //        var description = $('#description').val();
    //        var endtime = $('#endTime').val();
    //        var overtimeDate = $('#overtimeDate').val();
    //        var fileInput = document.getElementById('attachDocumentButton');
    //        var file = fileInput.files[0];

    //        // Create FormData object and append form data
    //        var formData = new FormData();
    //        formData.append('description', description);
    //        formData.append('endtime', endtime);
    //        formData.append('overtimeDate', overtimeDate);
    //        formData.append('postedFiles', file);

    //        $.ajax({
    //            url: '/Dashboard/AddOvertimeRequest',
    //            type: 'POST',
    //            data: formData,
    //            contentType: false,
    //            processData: false,
    //            success: function (response) {
    //                console.log('Success:', response);
    //                alert('Overtime submitted successfully.');
    //                location.reload();
    //            },
    //            error: function (xhr, status, error) {
    //                console.error('Error:', error);
    //                alert('An error occurred while submitting overtime.');
    //            }
    //        });
    //    });
    //});
});
