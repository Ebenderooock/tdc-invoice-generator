$('#DeleteTransporterForm').submit(function () {
    if ($("#DeleteTransporterForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});