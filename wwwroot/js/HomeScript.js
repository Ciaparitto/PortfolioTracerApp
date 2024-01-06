
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
 
});

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

const GetDataLegacy = async (AssetCode) => {
  
    for (let i = 0; i < 10; i++) {
        const year = (today.getFullYear() - 2) - i;
        const month = today.getMonth();
        const currentDate = new Date(year, month, 1);
        const formattedDate = `${(currentDate.getFullYear() + 1)}-01-01`;

        temporaryArrayx.push(formattedDate);
        const AssetAmount = await fetch(`/Account/GetAmmountOfAsset?AssetCode=${AssetCode}`);
        const output = await GetMetalPrice(AssetCode,AssetAmount, formattedDate.substring(0, 4), formattedDate.substring(5, 7), formattedDate.substring(8, 10));
        temporaryArrayy.push(output);
        console.log(output);
    }

    xValues = temporaryArrayx.reverse();
    yValues = temporaryArrayy.reverse();
    console.log(xValues);
    console.log(yValues);

    createChart();
};
function GetData(period) {
    var dictionary = {};
    const AssetDict = await fetch("/Account/GetUserAssets");
    if (period = "Day")
    {
        for (let i = 0, i < 10; i++)
        {
            const year = today.getFullYear();
            const month = today.getMonth();
            const day = (today.day() - 1) - i;
            var Rates = GetRates(`${year}-${month}-${day}`);
            for (const Asset in AssetDict)
            {
                //DO IT I DONT HAVE IDEA HOW  TO DO IT 
            }
        }
    }

}
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


async function GetRates(date) {
    const response = await fetch(`/Api/GetRatesByDay?Date=${date}`);
    return response;
}
