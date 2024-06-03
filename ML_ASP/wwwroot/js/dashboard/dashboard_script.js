var dataTable;

$(document).ready(function () {
    loadDataTable();

});

function loadDataTable() {
    dataTable = $('#overTimeTable').DataTable({
        "ajax": {
            "url": "/Dashboard/GetOvertime",
        },
        "columns": [
            { "data": "createdDate", "title": "Date Created" },
            { "data": "overtimeEndTime", "title": "Overtime Ends" },
            { "data": "requestDate", "title": "Date Request" },
            {
                "data": null, "title": "Action",
                "render": function (data, type, row) {
                    return '<button class="btn btn-secondary btn-sm view-pdf" data-id="' + row.fileId + '">View</button>';
                }
            },
            {
                "data": null, "title": "Action",
                "render": function (data, type, row) {
                    return '<button class="btn btn-secondary btn-sm delete-btn" data-id="' + row.id + '">Delete</button>';
                }
            },
        ],
    });

    $('#overTimeTable').on('click', '.view-pdf', function () {
        var getId = $(this).data('id');
        window.open('../Admin/ViewOvertimePdf?id=' + getId, '_blank');
    });

    $('#overTimeTable').on('click', '.delete-btn', function () {
        var getId = $(this).data('id');
        deleteOvertime(getId);

        location.reload();
        window.location = '/Dashboard/Dashboard';
    });
}

function deleteOvertime(id, fileId) {
    if (confirm('Are you sure you want to delete this overtime request?')) {
        $.ajax({
            url: '/Dashboard/DeleteOvertime',
            type: 'POST',
            data: { id: id},
            success: function (response) {
                if (response.success) {
                    alert('Overtime request deleted successfully.');
                } else {
                    alert('Failed to delete overtime request.');
                }
            },
            error: function () {
                alert('An error occurred while deleting the overtime request.');
            }
        });
    }
}