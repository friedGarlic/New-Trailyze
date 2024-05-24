var dataTable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	dataTable = $('#overtimeTable').DataTable({
		"ajax": {
			"url": "/Admin/GetAllOvertime", // Endpoint to fetch data from the controller "Admin"  **IMPORTANT
		},
		"columns": [
			{ "data": "userName" , "title": "Name"},
			{ "data": "createdDate", "title": "Date Created" },
			{ "data": "overtimeEndTime", "title": "Overtime Ends" },
			{ "data": "requestDate", "title": "Date Request" },
			{
				"data": "approvalStatus",
				"render": function (data, type, row) {
					var options = ["Pending", "Declined", "Approved"];

					// i dont understand this anymore sadly it got too complicated
					var selectHtml = '<select name="approvalStatus">';
					for (var i = 0; i < options.length; i++) {
						var isSelected = row.approvalStatus === options[i] ? 'selected="selected"' : '';
						selectHtml += '<option value="' + options[i] + '" ' + isSelected + '>' + options[i] + '</option>';
					}
					selectHtml += '</select>';
					var hiddenInputApproval = '<input type="hidden" name="originalApprovalStatus" value = "' + row.approvalStatus + '" />';
					var hiddenInputHtml = '<input type="hidden" name="id" value="' + row.id + '">'; // to pass in controller
					var hiddentInputUserId = '<input type="hidden" name="userId" value="' + row.userId + '">';
					return selectHtml + hiddenInputHtml + hiddentInputUserId + hiddenInputApproval;
				}
			},
		],
	});

}
