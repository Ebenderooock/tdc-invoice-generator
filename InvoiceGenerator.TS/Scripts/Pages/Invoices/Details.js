$(document).ready(function () {
    $('.tags').tagify();
    $('.totalKg').each(function (i, obj) {
        const n = parseFloat(this.value);
        const noZeroes = n.toString();
        $(this).val(noZeroes);
    });

    $('.unitSize').each(function (i, obj) {
        const n = parseFloat(this.value);
        const noZeroes = n.toString();
        $(this).val(noZeroes);
    });
});