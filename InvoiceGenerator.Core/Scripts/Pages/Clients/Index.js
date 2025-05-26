(function ($) {
    "use strict";
    var editor;
    $('#clientsTable').DataTable({
        searchHighlight: true,
        columnDefs: [
            {
                targets: 4,
                searchable: false,
                searchHighlight: false,
                orderable: false
            },
        ],
        buttons: [
            {
                extend: 'copy',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }
        ],
        dom:
            "<'row mb-4'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row mt-4'<'col-sm-4'i><'col-sm-4 text-center'l><'col-sm-4 text-sm-center'p>>",
        responsive: true
    });
})(jQuery);