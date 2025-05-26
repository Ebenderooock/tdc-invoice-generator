$('#EditTransporterForm').submit(function () {
    if ($("#EditTransporterForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});