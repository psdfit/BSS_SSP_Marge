// Build the chart
Highcharts.chart('chartdiv', {
    colors: ['#49666d', '#b3cfb8', '#7ca397'],
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: {
        text: null
    },
    credits: {
        enabled: false
    },
    exporting: {
        enabled: false
    },
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: false
            },
            showInLegend: true,
            innerSize: 270,
            depth: 45
        }
    },
    series: [{
        name: 'Brands',
        colorByPoint: true,
        data: [{
            name: 'Lahore',
            y: 61.41,
            sliced: true,
            selected: true
        }, {
            name: 'Karachi',
            y: 11.84
        }, {
            name: 'Quetta',
            y: 4.67
        }]
    }]
});
