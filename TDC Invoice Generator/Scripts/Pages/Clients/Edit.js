$('#EditClientForm').submit(function () {
    if ($("#EditClientForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});