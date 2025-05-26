$('#DeleteUserForm').submit(function () {
    if ($("#DeleteUserForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});