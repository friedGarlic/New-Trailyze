var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#requirementFilesTable').DataTable({
        "ajax": {
            "url": "/Admin/GetAllReqFile",
        },
        "columns": [
            { "data": "fullName", "title": "Full Name" },
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<span class="toggle-details">Clicked Here to Expand</span>',
                "title": "Requirements"
            },
            {
                "data": "registrationPermission", "title": "Registration Permission",
                "render": function (data, type, row) {
                    var options = ["Pending", "Declined", "Revised", "Approved"];

                    // i dont understand this anymore sadly it got too complicated
                    var selectHtml = '<select name="registrationPermission">';
                    for (var i = 0; i < options.length; i++) {
                        var isSelected = row.registrationPermission === options[i] ? 'selected="selected"' : '';
                        selectHtml += '<option value="' + options[i] + '" ' + isSelected + '>' + options[i] + '</option>';
                    }
                    selectHtml += '</select>';
                    var hiddenInputApproval = '<input type="hidden" name="originalApprovalStatus" value = "' + row.registrationPermission + '" />';
                    var hiddenInputHtml = '<input type="hidden" name="id" value="' + row.id + '">'; // to pass in controller
                    return selectHtml + hiddenInputHtml  + hiddenInputApproval;
                }
            },
        ],
        "order": [[1, 'asc']]
    });

    $('#requirementFilesTable tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
            $(this).find('.toggle-details').text('Clicked Here to Expand');
        } else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            $(this).find('.toggle-details').text('Clicked Here to Collapse');
        }
    });
}

function format(data) {
    var table = '<table class="table table-bordered table-hover">';
    table += '<thead><tr><th>File Name</th><th>Approval Status</th><th>Action</th></tr></thead><tbody>';

    data.requirements.forEach(function (requirement) {
        //select html section--
        var approvalOptions = ["Pending", "Declined", "Revised", "Approved"];
        var selectHtml = '<select name="fileApprovalStatus">';
        for (var i = 0; i < approvalOptions.length; i++) {
            var isSelected = requirement.approvalStatus === approvalOptions[i] ? 'selected="selected"' : '';
            selectHtml += '<option value="' + approvalOptions[i] + '" ' + isSelected + '>' + approvalOptions[i] + '</option>';
        }
        selectHtml += '</select>';
        var hiddenInputApproval = '<input type="hidden" name="originalFileApprovalStatus" value = "' + requirement.approvalStatus + '" />';
        var hiddenInputId = '<input type="hidden" name="fileId" value="' + requirement.id + '">'; // to pass in controller
        var hiddenInputUserId = '<input type="hidden" name="fileUserId" value="' + requirement.userId + '">';
        //select html section----ENDS

        table += '<tr>' +
            '<td>' + requirement.fileName + '</td>' +
            '<td>' + selectHtml + hiddenInputApproval + hiddenInputId + hiddenInputUserId + '</td>' +
            '<td><button class="btn btn-primary btn-sm group-btn view-pdf" data-id="' + requirement.fileId + '">VIEW</button></td>' +
            '</tr>';
    });

    $(document).on('click', '.view-pdf', function () {
        var getId = $(this).data('id');
        window.open('../Admin/RequirementViewPdf2?id=' + getId, '_blank');
    });

    table += '</tbody></table>';
    return table;
}
