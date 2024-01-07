
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
async function GetData(period) {
    const today = new Date();
    const yValuesTemp = [];
    const xValuesTemp = [];
    const dictionaryValues = {};
    console.log(period);
    const response = await fetch("/Account/GetUserAssets");
    const AssetDict = await response.json();

    switch (period) {
        case "Week":
            for (let i = 0; i < 7; i++) {
                let value = 0;
                const year = today.getFullYear();
                const month = today.getMonth() + 1;
                const day = today.getDate() - 1 - i;
                const formattedDate = new Date(year, month - 1, day).toLocaleDateString("en-CA");
                const Rates = await GetRates(formattedDate);

                for (const Asset of AssetDict) {
                    value += Rates[Asset.Key] * Asset.Value;
                }
                dictionaryValues[formattedDate] = value;
            }
            break;
        case "Month":
            for (let i = 0; i < 30; i++) {
                let value = 0;
                const year = today.getFullYear();
                const month = today.getMonth();
                const day = today.getDate() - 1 - i;
                const formattedDate = new Date(year, month, day).toLocaleDateString("en-CA");
                const Rates = await GetRates(formattedDate);

                for (const Asset of AssetDict) {
                    value += Rates[Asset.Key] * Asset.Value;
                }
                dictionaryValues[formattedDate] = value;
            }
            break;
        case "Year":
            for (let i = 0; i < 365; i++) {
                let value = 0;
                const year = today.getFullYear();
                const month = today.getMonth();
                const day = today.getDate() - 1 - i;
                const formattedDate = new Date(year, month, day).toLocaleDateString("en-CA");
                const Rates = await GetRates(formattedDate);

                for (const Asset of AssetDict) {
                    value += Rates[Asset.Key] * Asset.Value;
                }
                dictionaryValues[formattedDate] = value;
            }
            break;
        case "TenYears":
            for (let i = 0; i < 3650; i++) {
                let value = 0;
                const year = today.getFullYear();
                const month = today.getMonth();
                const day = today.getDate() - 1 - i;
                const formattedDate = new Date(year, month, day).toLocaleDateString("en-CA");
                const Rates = await GetRates(formattedDate);

                for (const Asset of AssetDict) {
                    value += Rates[Asset.Key] * Asset.Value;
                }
                dictionaryValues[formattedDate] = value;
            }
            break;
        default:
            break;

            for (const Asset in dictionaryValues)
            {
                xValuesTemp = Asset.Key;
                yValuesTemp = Asset.Value;
            }
            xValues = xValuesTemp.reverse();
            yValues = yValuesTemp.reverse();
            console.log(xValues);
            console.log(yValues);
            createChart();
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
    const response = await fetch(`/Account/GetRates?date=${date}`);
    const rates = await response.json();
    return rates;
}
