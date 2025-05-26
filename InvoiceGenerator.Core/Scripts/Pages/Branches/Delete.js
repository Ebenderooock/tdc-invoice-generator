$('#DeleteBranchForm').submit(function () {
    if ($("#DeleteBranchForm").valid()) {
        $('#FormSubmit').prop('disabled', true);
    }
});