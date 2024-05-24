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

    // Prevent efualt form submission when "Add Document" button is clicked
    addDocumentButton.addEventListener('click', function (event) {
        event.preventDefault();
        showPopup(); // Show the popup instead
    });

    closeButton.addEventListener('click', hidePopup);

    $(document).ready(function () {
        $('#submitOvertime').click(function () {
            // Get values from input fields and textarea
            var description = $('#description').val();
            var endtime = $('#endTime').val();
            var overtimeDate = $('#overtimeDate').val();

            var dataToSend = {
                description: description,
                endtime: endtime,
                overtimeDate: overtimeDate
            };

            $.ajax({
                url: '/Dashboard/AddOvertimeRequest',
                type: 'POST',
                data: dataToSend,
                success: function (response) {
                    console.log('Success:', response);
                    alert('Overtime submitted successfully.');
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                    alert('An error occurred while submitting overtime.');
                }
            });
            location.reload();
            window.location = '/Dashboard/Dashboard';
            window.location = '/Dashboard/Dashboard';
        });
    });

});

