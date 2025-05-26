(function ($) {
    "use strict";

    $.fn.dataTable.moment('DD/MM/YYYY');


    $('#invoicesTable').DataTable({
        searchHighlight: true,        
        columnDefs: [
            {
                targets: 6,
                searchable: true,                
                searchHighlight: false,
                orderable: false
            },
            { type: 'formatted-num', targets: 3 }
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
        order: [[1, "desc"]],
        dom:
            "<'row mb-4'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row mt-4'<'col-sm-4'i><'col-sm-4 text-center'l><'col-sm-4 text-sm-center'p>>",
        responsive: true
    });

    // initialize datepicker
    $(".datePicker").flatpickr({
        enableTime: false,
        dateFormat: "Y-m-d",
        defaultDate: "today"
    });
})(jQuery);

jQuery.extend( jQuery.fn.dataTableExt.oSort, {
    "formatted-num-pre": function ( a ) {
        a = (a === "-" || a === "") ? 0 : a.replace( /[^\d\-\.]/g, "" );
        return parseFloat( a );
    },

    "formatted-num-asc": function ( a, b ) {
        return a - b;
    },

    "formatted-num-desc": function ( a, b ) {
        return b - a;
    }
} );