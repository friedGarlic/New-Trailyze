let gradeData = [80, 79, 78];

const chartData = {
	chart: {
		type: 'area',
		height: 100,
		toolbar: { show: false },
		zoom: { enable: false }
	},
	colors: ['#3498db'],
	series: [{ name: 'Grade', data: gradeData }],
	dataLabels: { enable: false },
	stroke: { width: 5, curve: 'smooth' },
	fill: {
		type: 'gradient',
		gradient: {
			shadeIntensity: 1,
			opacityFrom: 0.7,
			opacityTo: 0,
			stops: [0, 90, 100]
		}
	},
	xaxis: {
		categories: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
		axisBorder: { show: false },
		labels: { style: { colors: '#a7a7a7', fontFamily: 'system-ui' } }
	},
	yaxis: { show: false },
	grid: { borderColor: 'rgba(0,0,0,0)', padding: { top: -30, bottom: -8, left: 12, right: 12 } },
	tooltip: {
		disable: true,
		y: { formatter: value => '${value}K' },
		style: { fontFamily: 'system-ui' }
	},
	markers: { show: false }
};

error: function (xhr, status, error) {
	console.error(xhr.responseText);
	// Handle errors
}