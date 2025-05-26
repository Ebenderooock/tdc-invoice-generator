$('#DeleteClientForm').submit(function () {
    if ($("#DeleteClientForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});