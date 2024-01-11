
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
async function CalcPrice(DateArg, typeOfAsset)
{
  
    const Rates = await GetRates(DateArg);

    if (typeOfAsset === null || typeOfAsset === "All" || typeOfAsset === "") {
        var AssetDict = await fetch("/Account/GetUserAssets");
    } else {
        var AssetDict = await fetch(`/Account/GetUserAssetsByType?Type=${typeOfAsset}`);
    }
  
    let value = 0
    for (const [Key, AssetValue] of Object.entries(AssetDict))
    {
        value += (1 / parseFloat(Rates.rates[Key])) * parseFloat(AssetValue);
    }
      
    
    return value.toFixed(2);
}
async function GetData(NumberOfDays, typeOfAsset) {

    var yValuesTemp = [];
    var xValuesTemp = [];
    const dictionaryValues = {};

    if (typeOfAsset === null || typeOfAsset === "All" || typeOfAsset === "") {
        var AssetDict = await fetch("/Account/GetUserAssets");
    } else {
        var AssetDict = await fetch(`/Account/GetUserAssetsByType?Type=${typeOfAsset}`);
    }


    for (let i = 0; i < NumberOfDays; i++) {
        const today = new Date();
        const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - i); // Day - 1 becouse API have errors sometimes when i call today date
        const year = NewDate.getFullYear();
        const month = NewDate.getMonth() + 1;
        const day = NewDate.getDate();

        const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
        const Rates = await GetRates(formattedDate);
        for (const period of Object.keys(AssetDict)) {
            let value = 0
            for (const [Key, AssetValue] of Object.entries(AssetDict)) {
                value += (1 / parseFloat(Rates.rates[Key])) * parseFloat(AssetValue);
            }
            dictionaryValues[formattedDate] = value.toFixed(2);
        }
    }
    for (const Asset in dictionaryValues) {
        xValuesTemp.push(Asset);
        yValuesTemp.push(parseFloat(dictionaryValues[Asset]));
    }
    xValues = xValuesTemp.reverse();
    yValues = yValuesTemp.reverse();

    createChart();
    // it dont work V
    const today = new Date();
    const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1); // Day - 1 becouse API have errors sometimes when i call today date
    const year = NewDate.getFullYear();
    const month = NewDate.getMonth() + 1;
    const day = NewDate.getDate();
    const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;

  
    const NewDate2 = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - NumberOfDays) // Day - 1 becouse API have errors sometimes when i call today date
    const year2= NewDate2.getFullYear();
    const month2 = NewDate.getMonth() + 1;
    const day2 = NewDate.getDate();
    const formattedDate2 = `${year2}-${month2.toString().padStart(2, '0')}-${day2.toString().padStart(2, '0')}`;
   
    const PriceChange = CalcPrice(formattedDate, typeOfAsset) - CalcPrice(formattedDate2, typeOfAsset); 
    document.getElementById("PriceChange").innerHTML = PriceChange;


}
async function GetDataLegacy(period,typeOfAsset) {
   
    var yValuesTemp = [];
    var xValuesTemp = [];
    const dictionaryValues = {};
    console.log(period);
    if (typeOfAsset === null || typeOfAsset === "All") {
        var response = await fetch("/Account/GetUserAssets");
    } else
    {
        var response = await fetch(`/Account/GetUserAssetsByType?Type=${typeOfAsset}`);
    }
    const AssetDict = await response.json();

    switch (period) {
        case "Week":
            for (let i = 0; i < 7; i++) {
                const today = new Date();
                const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - i); // Day - 1 becouse API have errors sometimes when i call today date
                const year = NewDate.getFullYear();
                const month = NewDate.getMonth() + 1;
                const day = NewDate.getDate();
               
                const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
                const Rates = await GetRates(formattedDate);
                for (const period of Object.keys(AssetDict)) {
                    let value = 0
                    for (const [Key, AssetValue] of Object.entries(AssetDict)) {                     
                        value += (1 / parseFloat(Rates.rates[Key])) * parseFloat(AssetValue);
                    }
                    dictionaryValues[formattedDate] = value.toFixed(2);                
                }
            }
            break;
        case "Month":
            for (let i = 0; i < 30; i++) {
                const today = new Date();
                const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - i); // Day - 1 becouse API have errors sometimes when i call today date
                const year = NewDate.getFullYear();
                const month = NewDate.getMonth() + 1;
                const day = NewDate.getDate();

                const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
                const Rates = await GetRates(formattedDate);
                for (const period of Object.keys(AssetDict)) {
                    let value = 0
                    for (const [Key, AssetValue] of Object.entries(AssetDict)) {

                        value += (1 / parseFloat(Rates.rates[Key])) * parseFloat(AssetValue);
                    }
                    dictionaryValues[formattedDate] = value.toFixed(2);
                }
            }
            break;
        case "Year":
            for (let i = 0; i < 365; i++) {

                const year = today.getFullYear();
                const month = today.getMonth() + 1;
                const day = today.getDate() - 1 - i;
                const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
                const Rates = await GetRates(formattedDate);
                for (const period of Object.keys(AssetDict)) {
                    let value = 0
                    for (const [Key, AssetValue] of Object.entries(AssetDict)) {

                        value += parseFloat(Rates.rates[Key]) * parseFloat(AssetValue);
                    }
                    dictionaryValues[formattedDate] = value.toFixed(2);
                }
            }
            break;
        case "TenYears":
            for (let i = 0; i < 3650; i++) {

                const year = today.getFullYear();
                const month = today.getMonth() + 1;
                const day = today.getDate() - 1 - i;
                const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
                const Rates = await GetRates(formattedDate);
                for (const period of Object.keys(AssetDict)) {
                    let value = 0
                    for (const [Key, AssetValue] of Object.entries(AssetDict)) {
                        value += parseFloat(Rates.rates[Key]) * parseFloat(AssetValue);
                    }
                    dictionaryValues[formattedDate] = value.toFixed(2);
                }
            }
            break;
        default:
            break;
    }   
                for (const Asset in dictionaryValues)
                {
                    console.log(`Asset: ${Asset}, Value: ${dictionaryValues[Asset]}`);
                    xValuesTemp.push(Asset);
                    yValuesTemp.push(parseFloat(dictionaryValues[Asset]));
                }
            xValues = xValuesTemp.reverse();
            yValues = yValuesTemp.reverse();
            console.log(xValues);
            console.log(yValues);
            createChart();
    
   

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
    const response = await fetch(`/Api/GetRatesByDay?date=${date}`);
    const rates = await response.json();
    return rates;
}
