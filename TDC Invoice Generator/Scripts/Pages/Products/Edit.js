$('#EditProductForm').submit(function () {
    if ($("#EditProductForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});