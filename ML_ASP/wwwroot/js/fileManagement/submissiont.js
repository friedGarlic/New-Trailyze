var dataTable1;

$(document).ready(function () {
    loadDataTable1();

    // Handle click event on the "Hide" button
    $('#tblData').on('click', '.group-btn', function () {
        var folderId = $(this).data('folderid');
        toggleGroupVisibility(folderId);
    });
});

function loadDataTable1() {
    dataTable1 = $('#tblData').DataTable({
        "ajax": {
            "url": "/FileManagement/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "approvalStatus", "width": "15%" },
            { "data": "fileName", "width": "15%" },
            { "data": "date", "width": "15%" },
            { "data": "grade", "width": "15%"}
        ],
        "rowGroup": {
            "dataSrc": "folderId",
            "startRender": function (rows, group) {
                return '<button class="btn btn-primary btn-sm group-btn" data-folderid="' + group + '">Toggle</button> Folder ID: ' + group;
            }
        }
    });
}

function toggleGroupVisibility(folderId) {
    var rows = dataTable1.rows(function (idx, data, node) {
        return data.folderId === folderId;
    }).nodes();

    $(rows).toggle(); // Toggle visibility of the rows with the specified folderId
}
