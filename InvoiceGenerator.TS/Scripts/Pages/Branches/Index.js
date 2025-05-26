(function ($) {
    "use strict";
    var editor;
    $('#branchesTable').DataTable({
        searchHighlight: true,
        columnDefs: [
            {
                targets: 1,
                searchable: false,
                searchHighlight: false,
                orderable: false
            },
        ],
        buttons: [
            {
                extend: 'copy',
                exportOptions: {
                    columns: [0]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0]
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: [0]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0]
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