var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/residents/GetAllResidents",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "roomNumber", "width": "10%" },
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            { "data": "dateOfBirthString", "width": "10%" },
            { "data": "cellTelephoneNumber", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/residents/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer; max-width: 25%;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a href="/Medications/Index?residentId=${data}" class='btn btn-info text-white'
                                    style='cursor:pointer; max-width: 25%;'> <i class='fas fa-tablets'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/residents/Delete/${data}") class='btn btn-danger text-white'
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