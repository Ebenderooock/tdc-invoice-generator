$(document).ready(function () {
    $('.tags').tagify();
});

$('#DeleteInvoiceForm').submit(function () {
    if ($("#DeleteInvoiceForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});
