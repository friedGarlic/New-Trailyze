var dataTable;

$(document).ready(function () {
    loadDataTable();

    // handle click event on the "Hide" button
	$('#submissionTable').on('click', '.group-btn', function () {
		//event.preventDefault();
        var folderId = $(this).data('folderid');
        toggleGroupVisibility(folderId);
    });
});

function loadDataTable() {
	dataTable = $('#submissionTable').DataTable({
		"ajax": {
			"url": "/Admin/GetAll", // Endpoint to fetch data from the controller "Admin"  **IMPORTANT
		},
		"columns": [
			{ "data": "name" },
			{ "data": "date" },
			{ "data": "fileName" },
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
					var hiddenInputApproval = '<input type="hidden" name="originalApprovalStatus" value = "' + row.approvalStatus + '" />';
					var hiddenInputHtml = '<input type="hidden" name="id" value="' + row.id + '">'; // to pass in controller
					var hiddentInputUserId = '<input type="hidden" name="userId" value="' + row.submissionUserId + '">';
					return selectHtml + hiddenInputHtml + hiddentInputUserId + hiddenInputApproval;
				}
			},
			{
				"data": null,
				"render": function (data, type, row) {
					return '<button class="btn btn-secondary btn-sm view-pdf" data-id="' + row.fileName + '" data-folderid="' + row.folderId + '">View</button>';
				}
			}
		],
		"rowGroup": {
			"dataSrc": "folderId",
			"startRender": function (rows, group) {
				return '<button class="btn btn-primary btn-sm group-btn" data-folderid="' + group + '">Toggle</button> Folder ID: ' + group;
			}
		}
	});

	$('#submissionTable').on('click', '.view-pdf', function () {
		var getId = $(this).data('id');
		var getFolderId = $(this).data('folderid');
		if (getFolderId == null) {
			window.open('../Admin/ViewPdf2?id=' + getId, '_blank');
		}
		else {
			window.open('../Admin/ViewPdf2?id=' + getFolderId + '/' + getId, '_blank');
		}
	});

}

function toggleGroupVisibility(folderId) {
    var rows = dataTable.rows(function (idx, data, node) {
        return data.folderId === folderId;
    }).nodes();

    $(rows).toggle(); // Toggle visibility of the rows with the specified folderId
}

function showWorkloadPopup() {
	Swal.fire({
		title: 'Add Reminder',
		html:
			'<input id="swal-input1" class="swal2-input" placeholder="Add Workload">' +
			`<textarea id="swal-input4" rows="4" cols="50">
				  Enter your details here...
				</textarea>` +
			`<input id="swal-input2" type="datetime-local" class="swal2-input" placeholder="Select Date and Time">` +
			'<input id="swal-input3" class="swal2-input" placeholder="Type of Course">',
		showCancelButton: true,
		confirmButtonText: 'Submit',
		cancelButtonText: 'Cancel',
		preConfirm: () => {
			const nameOfReminder = Swal.getPopup().querySelector('#swal-input1').value;
			const dateTime = Swal.getPopup().querySelector('#swal-input2').value;
			const typeOfCourse = Swal.getPopup().querySelector('#swal-input3').value;
			const description = Swal.getPopup().querySelector('#swal-input4').value;
			// perform validation or additional processing here
			return { nameOfReminder: nameOfReminder, dateTime: dateTime, typeOfCourse: typeOfCourse, description: description };
		},
		allowOutsideClick: () => !Swal.isLoading()
	}).then((result) => {
		if (result.isConfirmed) {
			// data to the controller using AJAX
			sendToController(result.value);
			location.reload();
		}
	});
}

function sendToController(data) {
	$.ajax({
		url: '/Admin/AddWorkload',
		type: 'POST',
		dataType: 'json',
		data: data,
		success: function (response) {
			console.log("SUCCESS");
		},
		error: function (error) {
			console.error(error);
		}
	});
}