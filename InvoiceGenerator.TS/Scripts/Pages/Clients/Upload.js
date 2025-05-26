$('#UploadClientsForm').submit(function () {
    if ($("#UploadClientsForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});