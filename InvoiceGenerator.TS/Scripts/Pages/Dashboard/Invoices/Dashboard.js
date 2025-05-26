$(function () {
    'use strict'
    var bodycolor = getComputedStyle(document.body).getPropertyValue('--bodycolor');
    var theme = 'light';

    var options = {
        theme: {
            mode: theme
        },
        chart: {
            height: 350,
            type: 'line',
            zoom: {
                enabled: false
            },
            dropShadow: {
                enabled: true,
                color: '#000',
                top: 18,
                left: 7,
                blur: 10,
                opacity: 0.2
            },
        },
        colors: ['#62BB46'],
        series: [],
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'straight'
        },
        title: {

            align: 'left',
            style: {
                color: bodycolor

            }
        },
        grid: {
            row: {
                colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
                opacity: 0.5
            },
        },
        xaxis: {
            title: {
                text: 'Months'
            },
            categories: []
        },
        yaxis: {
            title: {
                text: 'Total Invoices'
            },
            show: true,
            labels: {
                style: {
                    color: bodycolor
                },
                formatter: function (val) {
                    return val.toFixed(0);
                }
            }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val
                }
            }
        },
    }

    var chart = new ApexCharts(
        document.querySelector("#invoicesPerMonthChart"),
        options
    );

    chart.render();

    function GetInvoicesPerMonth() {
        var url = '/home/GetInvoicesPeMonth';
        $.getJSON(url, function (response) {
            var data = [{
                name: 'Total Invoices',
                data: response.Invoices
            }];
            chart.updateSeries(data);
            chart.updateOptions({
                xaxis: {
                    type: 'string',
                    categories: response.Months,
                },
            });
        });
    }

    GetInvoicesPerMonth();
});