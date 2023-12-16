
function changeprice(x)
{
  
    document.getElementById("price").innerHTML = x;
}
async function GetMetalPrice(symbol,TypeOfAsset,year,month,day,year2,month2,day2)
{
    const response = await fetch("/Api/Convert?symbol=${symbol}&typeofmetal=${typeofmetal}&year=${year}&month=${month}&day=${day}");
    const result = await response.text();
    return result;
}
