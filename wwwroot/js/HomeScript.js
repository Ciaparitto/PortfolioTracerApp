
yValues = [];
xValues = [];
today = new Date();
temporaryArrayx = [];
temporaryArrayy = [];
let myChart;

function createChart() {
  
   myChart = new Chart("Chart", {
        type: "line",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: "rgb(0,13,26)",
                borderColor: "rgb(0,64,128)",
                data: yValues
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: { display: false }
        }
    });
}

async function CalcAssetValue(DateArg, typeOfAsset)
{
   
    const Rates = await GetRates(DateArg);
    let response;

    if (typeOfAsset === null || typeOfAsset === "All" || typeOfAsset === "") {
        response = await fetch("/Account/GetUserAssets");
    } else {
        response = await fetch(`/Account/GetUserAssetsByType?Type=${typeOfAsset}`);
    }

    const AssetDict = await response.json();
   
    let value = 0
    
    for (const [Key, AssetValue] of Object.entries(AssetDict))
    {
      
        value += (1 / parseFloat(Rates.rates[Key])) * parseFloat(AssetValue);
    }
      
    return value.toFixed(2);
}
async function GetData(NumberOfDays, typeOfAsset)
{
    if (myChart)
    {
        myChart.destroy();
        myChart = null;
    }

    document.getElementById("LoadingMessage").innerHTML = "Loading...";
    document.getElementById("AssetValueChange").innerHTML = "";

    var yValuesTemp = [];
    var xValuesTemp = [];
    const dictionaryValues = {};
    let response;

    if (typeOfAsset === null || typeOfAsset === "All" || typeOfAsset === "")
    {
        response = await fetch("/Account/GetUserAssets");
    } else {
        response  = await fetch(`/Account/GetUserAssetsByType?Type=${typeOfAsset}`);
    }
    const AssetDict = await response.json();
   
    for (let i = 0; i < NumberOfDays; i++) {
        const today = new Date();
        const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - i); // Day - 1 becouse API have errors sometimes when i call today date
        const year = NewDate.getFullYear();
        const month = NewDate.getMonth() + 1;
        const day = NewDate.getDate();  
       
        const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
        const Rates = await GetRates(formattedDate);
        console.log(Rates);
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
    document.getElementById("LoadingMessage").innerHTML = null;
    createChart();
       
    const today = new Date();
    const NewDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1); // Day - 1 becouse API have errors sometimes when i call today date
    const year = NewDate.getFullYear();
    const month = NewDate.getMonth() + 1;
    const day = NewDate.getDate();
    const formattedDate = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
  
    const NewDate2 = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1 - NumberOfDays) // Day - 1 becouse API have errors sometimes when i call today date
    const year2= NewDate2.getFullYear();
    const month2 = NewDate2.getMonth() + 1;
    const day2 = NewDate2.getDate();
    const formattedDate2 = `${year2}-${month2.toString().padStart(2, '0')}-${day2.toString().padStart(2, '0')}`;
    const AssetValue = await CalcAssetValue(formattedDate, typeOfAsset);
    const AssetValue2 = await CalcAssetValue(formattedDate2, typeOfAsset); 
    const AssetValueChange = (AssetValue - AssetValue2).toFixed(2);
    if (AssetValueChange > 0)
    {
        document.getElementById("AssetValueChange").innerHTML = `Your Assets in category ${typeOfAsset} has increased by ${AssetValueChange}$`;
    }
    if (AssetValueChange < 0)
    {
        document.getElementById("AssetValueChange").innerHTML = `Your Assets in category ${typeOfAsset} has decreased by ${AssetValueChange}$`;
    }
    if (AssetValueChange == 0)
    {
        document.getElementById("AssetValueChange").innerHTML = `Your Assets in category ${typeOfAsset} didnt change value`;
    }

}

async function GetRates(date)
{
    const response = await fetch(`/Api/GetRatesByDay?date=${date}`);
    const rates = await response.json();
    return rates;
}
