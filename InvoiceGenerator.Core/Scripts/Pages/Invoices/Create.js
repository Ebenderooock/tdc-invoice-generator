$(document).ready(function () {
    $('.tags').tagify({
        originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')
    });


    // variables
    let i = 1;
    let selectId = 0;
    let itemCount = 1;
    let productsList = "";

    // Populate dropdown with list of products
    const url = '/Invoices/GetProducts';

    $.getJSON(url, function (data) {
        console.log(data);
        $.each(data, function (key, entry) {
            console.log(entry);

            productsList += '<option value="' + entry.Id + '">' + entry.Name + ' | ' + entry.Code + '</option>';
        });

        $("#firstSelect").append(productsList);

        //onload: call the above function
        $(".select-to-select2").each(function () {
            initializeSelect2($(this));
        });

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
        dateFormat: "Y-m-d",
        defaultDate: "today"
    });

    // remove row control
    $(document).on('click', '.delete_row', function () {
        if (i > 1) {
            $(this).parent().parent().remove();
            i--;
        }
    });

    function addRow() {
        const rowBody = `
                    <tr class="indexBlock">
                        <td><select class="select-to-select2 validateItem tabbable-input" name="InvoiceItems[` + itemCount + `].ProductId" id="select` + selectId + `" style="width: 100%"><option value="" disabled selected>Select Product</option>` + productsList + `</select></td>
                        <td><input type="number" min="0" name="InvoiceItems[` + itemCount + `].Quantity" placeholder="Qty" class="form-control calculateKg quantity tabbable-input" /></td>
                        <td><input type="number" min="0" name="InvoiceItems[` + itemCount + `].UnitSize" placeholder="Unit Size" class="form-control calculateKg unitSize tabbable-input" /></td>
                        <td><input type="number" min="0" name="InvoiceItems[` + itemCount + `].TotalKg" placeholder="Total Kg" class="form-control totalKg" readonly /></td>
                        <td><input type="text" name="InvoiceItems[` + itemCount + `].Pallets" placeholder="Total Pallets" class="form-control tabbable-input" /></td>
                        <td class="tag-container">
                            <input pattern="[0-9]+" tags="tags" name="InvoiceItems[` + itemCount + `].BatchNumbers" placeholder="Batch Numbers" value="" class="tags">
                        </td>
                        <td><button type="button" class="btn btn-danger btn_remove delete_row">X</button></td>
                    </tr>
                `;

        $("#invoiceBody").append(rowBody);
        const selectElement = $("#select" + selectId);
        initializeSelect2(selectElement);
        $('[tags=tags]').tagify({
            originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')
        });

        if ($(this)) {
            $(this).get(0).scrollIntoView({behavior: 'smooth', block: 'end'});
        }

        i++;
        selectId++;
        itemCount++;
    }

    $(document).keypress('.tabbable-input', function (e) {
        if (e.which === 13) {
            e.preventDefault();

            const currentTd = $(e.target).parent('td');
            const nextTd = $(currentTd).next();
            let nextInput;


            if ($(nextTd).children().first().hasClass('tagify')) {
                console.log($(nextTd).children().first());
                nextInput = $(nextTd).find('.tagify__input');

            } else {
                nextInput = $(e.target).parent().next().find('input');
                // Skip readonly inputs
                while (nextInput.length > 0 && nextInput.prop('readonly')) {
                    nextInput = nextInput.parent().next().find('input');
                }
            }


            // if the input is a tagify element, focus no the span:

            if (nextInput.length > 0) {
                nextInput.focus();
            }
        }
    });
    //dynamically added selects
    $(".add-row-btn").on("click", addRow);

    // Reorder input indexs
    function reindexBlocks() {
        $(".indexBlock").each(function (index) {
            const prefix = "InvoiceItems[" + index + "]";
            $(this).find("input, select").each(function () {
                this.name = this.name.replace(/InvoiceItems\[\d+\]/, prefix);
            });
        });
    }

    $('#CreateInvoicesForm').on('submit', function (event) {

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
        if ($('#CreateInvoicesForm').validate().form()) {
            event.currentTarget.submit();
        } else {
            console.log("does not validate");
        }
    })

    // initialize the validator
    $('#CreateInvoicesForm').validate();

    // Auto calculate Total KG
    $(document).on('change', '.calculateKg', function (event) {
        const quantity = $(this).closest("tr.indexBlock").find('input.quantity[type=number]').val();
        const unitSize = $(this).closest("tr.indexBlock").find('input.unitSize[type=number]').val();
        if ((quantity !== "" && unitSize !== "") && (IsNumeric(quantity) && IsNumeric(unitSize))) {
            const totalKg = quantity * unitSize;
            $(this).closest("tr.indexBlock").find('input.totalKg[type=number]').val(totalKg);
        }
    });

    const IsNumeric = (num) => /^-{0,1}\d*\.{0,1}\d+$/.test(num);
});

function onSaveAndDownloadClicked(val, hasSaved) {
    document.getElementById('SaveAndDownload').value = val;

    $('#CreateInvoicesForm').submit();
 

}