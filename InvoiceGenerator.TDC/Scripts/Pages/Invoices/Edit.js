$(document).ready(function () {
    $('.tags').tagify({
        originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')
    });

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

    // variables
    var i = 0;
    var selectId = 0;
    var itemCount = 0;
    var productsList = "";

    // Populate dropdown with list of products
    const url = '/Invoices/GetProducts?invoiceId=' + $('#Id').val();

    $.getJSON(url, function (data) {
        $.each(data, function (key, entry) {
            productsList += '<option value="' + entry.Id + '">' + entry.Name + ' | ' + entry.Code + '</option>';
        });

        //onload: call the above function
        $(".select-to-select2").each(function () {
            initializeSelect2($(this));
        });

    });

    $('.delete_row').each(function () {
        i++;
        itemCount++;
        selectId++;
    });

    //function to initialize select2
    function initializeSelect2(selectElementObj) {
        selectElementObj.select2({
            tags: false,
            theme: 'bootstrap4',
        });
    }

    // initialize datepicker
    $(".datePicker").flatpickr({
        enableTime: false,
    });

    // remove row control
    $(document).on('click', '.delete_row', function () {
        if (i > 1) {
            $(this).parent().parent().remove();
            i--;
        }
    });
    
    function addRow() {
        var rowBody = `
                    <tr class="indexBlock">
                        <td><select class="select-to-select2 validateItem  reorder" name="InvoiceItems[`+ itemCount + `].ProductId" id="select` + selectId + `" style="width: 100%"><option value="" disabled selected>Select Product</option>` + productsList + `</select></td>
                        <td><input type="number" min="0" name="InvoiceItems[`+ itemCount + `].Quantity" placeholder="Qty" class="form-control reorder calculateKg quantity" /></td>
                        <td><input type="number" min="0" name="InvoiceItems[`+ itemCount + `].UnitSize" placeholder="Unit Size" class="form-control reorder calculateKg unitSize" /></td>
                        <td><input type="number" min="0" name="InvoiceItems[`+ itemCount + `].TotalKg" placeholder="Total Kg" class="form-control reorder totalKg" readonly /></td>
                        <td><input type="text" name="InvoiceItems[`+ itemCount + `].Pallets" placeholder="Total Pallets" class="form-control reorder" /></td>
                        <td>
                            <input pattern="[0-9]+" tags="tags" name="InvoiceItems[`+ itemCount + `].BatchNumbers" placeholder="Batch Numbers" value="" class="tags reorder">
                        </td>
                        <td><button type="button" class="btn btn-danger btn_remove delete_row">X</button></td>
                    </tr>
                `;
        $("#invoiceBody").append(rowBody);
        var selectElement = $("#select" + selectId);
        initializeSelect2(selectElement);
        $('[tags=tags]').tagify({
            originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')
        });



        if ($(this)) {
            $(this).get(0).scrollIntoView({ behavior: 'smooth', block: 'end' });
        }
        
        i++;
        selectId++;
        itemCount++;
    }

    //dynamically added selects
    $(".add-row-btn").on("click", addRow);

    // Reorder input indexs
    function reindexBlocks() {
        $(".indexBlock").each(function (index) {
            var prefix = "InvoiceItems[" + index + "]";
            $(this).find("input, select").each(function () {
                this.name = this.name.replace(/InvoiceItems\[\d+\]/, prefix);
            });
        });
    }

    $('#EditInvoiceForm').on('submit', function (event) {

        reindexBlocks();

        // adding rules for inputs with class 'comment'
        $('input.validateItem').each(function () {
            $(this).rules('add', {
                required: true
            });
        });

        $('select.validateItem').each(function () {
            $(this).rules('add', {
                required: true
            });
        });

        // prevent default submit action
        event.preventDefault();

        // test if form is valid
        if ($('#EditInvoiceForm').validate().form()) {
            event.currentTarget.submit();
        } else {
            console.log("does not validate");
        }
    })

    // initialize the validator
    $('#EditInvoiceForm').validate();

    // Auto calculate Total KG
    $(document).on('change', '.calculateKg', function (event) {
        var quantity = $(this).closest("tr.indexBlock").find('input.quantity[type=number]').val();
        var unitSize = $(this).closest("tr.indexBlock").find('input.unitSize[type=number]').val();
        console.log(typeof quantity);
        console.log(typeof unitSize);
        if ((quantity !== "" && unitSize !== "") && (IsNumeric(quantity) && IsNumeric(unitSize))) {
            var totalKg = quantity * unitSize;
            $(this).closest("tr.indexBlock").find('input.totalKg[type=number]').val(totalKg);
        }
    });

    const IsNumeric = (num) => /^-{0,1}\d*\.{0,1}\d+$/.test(num);
});

function onSaveAndDownloadClicked(val){
    document.getElementById('SaveAndDownload').value = val;
    $('#EditInvoiceForm').submit();
}