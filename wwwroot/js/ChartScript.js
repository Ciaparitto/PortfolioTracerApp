yValues = [7, 8, 8, 9, 9, 9, 10, 11, 14, 14, 15];
xValues = [50,60,70,80,90,100,110,120,130,140,150];
new Chart("myChart", {
    type: "line",
    data: {
        labels: xValues,
        datasets: [{
            backgroundColor: "rgba(51, 51, 51,0.2)",
            borderColor: "rgba(51, 51, 51,0.5)",
            data: yValues
        }]
    },
   
});

document.addEventListener('DOMContentLoaded', function () {

	console.log('dziala chart.js');
});