var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/applications/GetAllApplications",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "timeStampString", "width": "10%" },
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            { "data": "genderString", "width": "10%" },
            { "data": "dateOfBirthString", "width": "10%" },
            { "data": "identityNumber", "width": "10%" },
            { "data": "homeTelephoneNumber", "width": "10%" },
            { "data": "workTelephoneNumber", "width": "10%" },
            { "data": "cellTelephoneNumber", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/applications/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/applications/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}