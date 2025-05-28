$('#EditBranchForm').submit(function () {
    if ($("#EditBranchForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});