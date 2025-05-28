$('#DeleteProductForm').submit(function () {
    if ($("#DeleteProductForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});