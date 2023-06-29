var dataTable;

$(document).ready(function () {
    loadList();
});


function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/product",  //will go to the productcontroller
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //should not be capital (except 2nd words like listPrice)
            { "data": "name", "width": "25%" },
            { "data": "listPrice", render: $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "manufacturer.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center"> 
                                <a href="/Admin/Products/Upsert?id=${data}" class="btn btn-success text-white" style="cursor: pointer; width: 100px;" > 
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a href="/Admin/Products/Delete?id=${data}" class="btn btn-danger text-white" style="cursor: pointer; width: 100px;" >   
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                            </div> `;

                }, "width": "30%"
            }


        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%",
        "order": [[2, "asc"]]
    });
}
