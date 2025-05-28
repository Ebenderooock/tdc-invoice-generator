$('#UploadProductsForm').submit(function () {
    if ($("#UploadProductsForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});