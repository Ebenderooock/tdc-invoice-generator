$('#EditUserForm').submit(function () {
    if ($("#EditUserForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});