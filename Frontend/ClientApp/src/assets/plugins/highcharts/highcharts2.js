Highcharts.chart('chartdiv3', {
    colors: ['#7ca397', '#b3cfb8'],
    chart: {
        type: 'column'
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
    subtitle: {
    //    text: 'Source: WorldClimate.com'
    },
    xAxis: {
        categories: [
            'Jan',
            'Feb',
            'Mar',
            'Apr',
            'May',
            'Jun',
            'Jul',
            'Aug',
            'Sep',
            'Oct',
            'Nov',
            'Dec'
        ],
        crosshair: true
    },
    yAxis: {
        min: 0,
        title: {
            text: 'Visitors'
        }
    },
    tooltip: {
        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
            '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
        footerFormat: '</table>',
        shared: true,
        useHTML: true
    },
    plotOptions: {
        column: {
            pointPadding: 0.2,
            borderWidth: 0
        }
    },
    series: [{
        name: 'New Visitors',
        data: [49.9, 71.5, 106.4, 129.2, 49.9, 71.5, 106.4, 129.2, 49.9, 71.5, 106.4, 129.2]

    }, {
        name: 'Unique Visitors',
        data: [42.4, 33.2, 34.5, 39.7, 42.4, 33.2, 34.5, 39.7, 42.4, 33.2, 34.5, 39.7]

    }]
});
