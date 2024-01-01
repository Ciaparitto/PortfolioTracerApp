
function test() {
	console.log("dziala");
	alert("dziala");
}
yValues = [];
xValues = [];
today = new Date();
temporaryArrayx = [];
temporaryArrayy = [];


document.addEventListener('DOMContentLoaded', function () {

    const fetchData = async () => {
        for (let i = 0; i < 10; i++) {
            const year = (today.getFullYear()-2) - i;
            const month = today.getMonth();
            const currentDate = new Date(year, month, 1);
            const formattedDate = `${(currentDate.getFullYear() + 1)}-01-01`;

            temporaryArrayx.push(formattedDate);

    
            const output = await GetMetalPrice("XAU", "Metal", formattedDate.substring(0, 4), formattedDate.substring(5, 7), formattedDate.substring(8, 10));
            temporaryArrayy.push(output);
            console.log(output);
        }

        xValues = temporaryArrayx.reverse();
        yValues = temporaryArrayy.reverse();
        console.log(xValues);
        console.log(yValues);

        createChart();
    };

    fetchData();

    function createChart() {
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
    }
});
async function changeprice(symbol, year, month, day, year2, month2, day2) {
	console.log("start");
	const response = await fetch(`/Api/Convert?curr=USD&symbol=${symbol}&year=${year}&month=${month}&day=${day}`);
	const result = await response.json();

	var ResultRounded = (result.result).toFixed(2);

	const response2 = await fetch(`/Api/Convert?curr=USD&symbol=${symbol}&year=${year2}&month=${month2}&day=${day2}`);
	const result2 = await response2.json();
	var ResultRounded2 = (result2.result).toFixed(2);


	var Change = (ResultRounded - ResultRounded2).toFixed(2);

	var OutPutMessage = "error"
	if (Change > 0) {
		OutPutMessage = `twoje zloto usroslo o ${Change} USD`;
	}
	if (Change < 0) {
		OutPutMessage = `twoje zloto spadlo o ${Math.abs(Change)} USD`;
	}
	if (Change === 0) {
		OutPutMessage = `twoje zloto jest warte ${Change} USD`;
	}

	document.getElementById("price").innerHTML = OutPutMessage;
}


async function GetMetalPrice(symbol, TypeOfAsset, year, month, day) {
	const response = await fetch(`/Api/Convert?curr=USD&symbol=${symbol}&ammount=1&year=${year}&month=${month}&day=${day}`);
	const result = await response.json();

	return result.info.quote;
}
