var dataTable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	dataTable = $('#timeTable').DataTable({
		"ajax": {
			"url": "/TimeLog/GetAll", // Endpoint to fetch data from the controller "Admin"  **IMPORTANT
		},
		"columns": [
			{ "data": "fullName" },
			{ "data": "dateTime" },
			{ "data": "log" },
			{
				"data": "approvalStatus",
				"render": function (data, type, row) {
					var options = ["Pending", "Declined", "Revised", "Approved"];

					// i dont understand this anymore sadly it got too complicated
					var selectHtml = '<select name="approvalStatus">';
					for (var i = 0; i < options.length; i++) {
						var isSelected = row.approvalStatus === options[i] ? 'selected="selected"' : '';
						selectHtml += '<option value="' + options[i] + '" ' + isSelected + '>' + options[i] + '</option>';
					}
					selectHtml += '</select>';

					var hiddenInputApproval = '<input type="hidden" name="originalApprovalStatus" value = "' + row.approvalStatus + '" />'; //to pass in controller
					var hiddenInputHtml = '<input type="hidden" name="id" value="' + row.id + '">'; // to pass in controller
					return selectHtml + hiddenInputApproval + hiddenInputHtml;
				}
			},
			{
				"data" : "approvalStatus"
			},
			{
				"data": null,
				"render": function (data, type, row) {
					return '<button class="btn btn-secondary btn-sm view-pdf" data-id="' + row.logImageUrl + '">View</button>';
				}
			}
		],
		"rowGroup": {
			"dataSrc": "fullName",
			"startRender": function (rows, group) {
				return '<label class="btn btn-primary btn-sm group-btn" data-folderid="' + group + '">Group</label> Folder ID: ' + group;
			}
		}
	});

	$('#timeTable').on('click', '.view-pdf', function () {
		var getId = $(this).data('id');

		window.open('../FileManagement/ViewImage?fileName=' + getId, '_blank');
	});
}
