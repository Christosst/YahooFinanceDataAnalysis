using OoplesFinance.YahooFinanceAPI.Enums;
using OoplesFinance.YahooFinanceAPI;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using YFTest.Models;
using OoplesFinance.YahooFinanceAPI.Models;
using System.Dynamic;
using Excel.FinancialFunctions;
using MathNet.Numerics;
using System.Xml.Linq;
using System.Collections.Generic;

namespace YFTest
{
    internal class Program
    {
        static  void Main(string[] args)
        {
           FetchdStockataAsync().Wait( );
        }

        private static async Task<bool> FetchdStockataAsync()
        {

            var startDate = DateTime.Now.AddMonths(-3);
         
            var symbols = new string[] { /*"NVDA","META","GOOG","MSFT","AMZN","USB","SCHW","MET","TGT","GM"*/
                "ABT ",
                "ADBE",
                "AMD ",
                "HON ",
                "AXP ",
                "AIG ",
                "AMGN",
                "AAPL",
                "VZ  ",
                "BRK-B",
                "BA  ",
                "BMY ",
                "CVS ",
                "COF ",
                "CAT ",
                "JPM ",
                "CVX ",
                "CSCO",
                "KO  ",
                "CL  ",
                "DHR ",
                "TGT ",
                "DIS ",
                "DUK ",
                "EMR ",
                "XOM ",
                "NEE ",
                "FDX ",
                "GD  ",
                "GE  ",
                "GILD",
                "HD  ",
                "INTC",
                "IBM ",
                "JNJ ",
                "LLY ",
                "LMT ",
                "LOW ",
                "MCD ",
                "MDT ",
                "BK  ",
                "MSFT",
                "MMM ",
                "MS  ",
                "NKE ",
                "WFC ",
                "ORCL",
                "EXC ",
                "PEP ",
                "PFE ",
                "MO  ",
                "COP ",
                "PG  ",
                "QCOM",
                "T   ",
                "SCHW",
                "SPG ",
                "SO  ",
                "USB ",
                "SBUX",
                "TXN ",
                "TMO ",
                "C   ",
                "UNP ",
                "UNH ",
                "RTX ",
                "WMT ",
                "WBA ",
                "AMT ",
                "BAC ",
                "AMZN",
                "GS  ",
                "NVDA",
                "BKNG",
                "COST",
                "UPS ",
                "BLK ",
                "MET ",
                "F   ",
                "ACN ",
                "MDLZ",
                "NFLX",
                "CMCSA",
                "CRM ",
                "TMUS",
                "GOOGL",
                "MA  ",
                "PM  ",
                "V   ",
                "AVGO",
                "MRK ",
                "CHTR",
                "TSLA",
                "GM  ",
                "META",
                "ABBV",
                "GOOG",
                "KHC ",
                "PYPL",
                "LIN ",
                "DOW "

            };

            Dictionary<string, List<object>> allDataProcessed = new Dictionary<string, List<object>>();

            allDataProcessed = await GetProcessedData(startDate, symbols);
            var b = await GetTickerInfos(symbols);

            Console.WriteLine("Finished");
            Console.ReadLine();


            return true;
        }


        private static async Task<Boolean> GetTickerInfos(string[] symbols)
        {
            var TickerInfos_SummaryDetail = new List<object>();
            var yahooClient = new YahooClient();
            var j = 1;
            foreach (var symboltemp in symbols)
            {
                try
                {

                    Console.WriteLine($"Fetching Info for Symbol {symboltemp} {j} out of {symbols.Count()}");
                    j++;
                    dynamic data = new ExpandoObject();
                    var TickerInfo = new TickerInfo();
                    var symbol = symboltemp.TrimEnd();
                    var keyStatsList = await yahooClient.GetKeyStatisticsAsync(symbol);
                    var summaryDetailsList = await yahooClient.GetSummaryDetailsAsync(symbol);
                    var financialDataList = await yahooClient.GetFinancialDataAsync(symbol);
                    data.Symbol = symbol;
                    data.Beta = keyStatsList.Beta.Raw;
                    data.CurrentPrice = financialDataList.CurrentPrice.Raw;
                    data.TargetLowPrice = financialDataList.TargetMeanPrice.Raw;
                    data.TargetLowPrice = financialDataList.TargetLowPrice.Raw;
                    data.TargetHighPrice = financialDataList.TargetHighPrice.Raw;
                    data.RecommendationMean = financialDataList.RecommendationMean.Raw;
                    data.EarningsGrowth = financialDataList.EarningsGrowth.Raw;

                    data.EnterpriseToEbitda = keyStatsList.EnterpriseToEbitda.Raw;
                    data.FiftyTwoWeekHigh = summaryDetailsList.FiftyTwoWeekHigh.Raw;
                    data.FiftyTwoWeekLow = summaryDetailsList.FiftyTwoWeekLow.Raw;
                    data.FiftyDayAverage = summaryDetailsList.FiftyDayAverage.Raw;
                    data.TwoHundredDayAverage = summaryDetailsList.TwoHundredDayAverage.Raw;

                    data.DebtToEquity = financialDataList.DebtToEquity.Raw;
                    data.CurrentRatio = financialDataList.CurrentRatio.Raw;

                    data.EbitdaMargins = financialDataList.EbitdaMargins.Raw;
                    data.QuickRatio = financialDataList.QuickRatio.Raw;
                    data.GrossMargins = financialDataList.GrossMargins.Raw;
                    data.OperatingMargins = financialDataList.OperatingMargins.Raw;
                    data.OperatingCashflow = financialDataList.OperatingCashflow.Raw;
                    data.RevenuePerShare = financialDataList.RevenuePerShare.Raw;
                    data.TotalCash = financialDataList.TotalCash.Raw;
                    data.TotalRevenue = financialDataList.TotalRevenue.Raw;
                    data.PriceHint = keyStatsList.PriceHint.Raw;
                    data.PriceToBook = keyStatsList.PriceToBook.Raw;
                    data.PriceToSalesTrailing12Months = keyStatsList.PriceToSalesTrailing12Months.Raw;

                    data.ReturnOnEquity = financialDataList.ReturnOnEquity.Raw;
                    data.ReturnOnAssets = financialDataList.ReturnOnAssets.Raw;

                    data.MarketCapitilization = summaryDetailsList.MarketCap.Raw;
                    data.ForwardPE = summaryDetailsList.ForwardPE.Raw;
                    data.TrailingPE = summaryDetailsList.TrailingPE.Raw;
                    TickerInfos_SummaryDetail.Add(data);
                }
                catch (Exception e) { }
            }



            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };
            using (var writer = new StreamWriter($"E:\\CIIM\\Yahoo Data\\Temp\\TickerInfos_SummaryDetail {DateTime.Now.Ticks.ToString()}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(TickerInfos_SummaryDetail);
            }

            return true;


        }


        private static async Task<Dictionary<string, List<object>>> GetProcessedData(DateTime startDate, string[] symbols)
        {
            Dictionary<string, List<object>> allDataProcessed = new Dictionary<string, List<object>>();
            var yahooClient = new YahooClient();
            List<double> xdata_SP = new List<double>();
            
            List<double> xdata_SP_Price = new List<double>();
            
            
            var historicalDataList_SP = await yahooClient.GetHistoricalDataAsync("^SPX", DataFrequency.Daily, startDate);
            dynamic thistoricalDataList_SP = new List<object>();
            if (historicalDataList_SP != null)
            {
                List<HistoricalData> data_SP = new List<HistoricalData>();
                foreach (var item in historicalDataList_SP)
                { data_SP.Add(item); }


                for (int i = 0; i < data_SP.Count(); i++)
                {
                    var ItemCurrent = data_SP[i];
                    dynamic thistoricalData = new ExpandoObject();
                    thistoricalData.Date = ItemCurrent.Date;
                    thistoricalData.AdjClose = ItemCurrent.AdjClose;
                    xdata_SP_Price.Add(ItemCurrent.AdjClose);
                    if (i > 0)
                    {
                        var ItemPrevus = data_SP[i - 1];
                        xdata_SP.Add((ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1);
                        thistoricalData.Return = (ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1;
                    }
                    else
                    {
                        thistoricalData.Return = 0;
                    }
                    thistoricalDataList_SP.Add(thistoricalData);
                }
            }


            Dictionary<string, List<HistoricalData>> allData = new Dictionary<string, List<HistoricalData>>();

            var j = 1;

           
            foreach (var symboltemp in symbols)
            {
               
                Console.WriteLine($"Fetching Historic Data for Symbol {symboltemp} {j} out of {symbols.Count()}");
                j++;
                    var TickerInfo = new TickerInfo();
                var symbol = symboltemp.TrimEnd();
                TickerInfo.TickerSymbol = symbol;

                var historicalDataList = await yahooClient.GetHistoricalDataAsync(symbol, DataFrequency.Daily, startDate);
                if (historicalDataList != null)
                {
                    List<HistoricalData> data = new List<HistoricalData>();
                    foreach (var item in historicalDataList)
                    { data.Add(item); }
                    allData.Add(symbol, data);
                }
            }

            List<object> RegressionAnalysis = new List<object>();
            // Dictionary<string, List<double>> ydata_SYMB_Dic = new Dictionary<string, List<double>>();
            //  Dictionary<string, List<double>> ydata_SYMB_price_Dic = new Dictionary<string, List<double>>();
            j = 1;
            foreach (string symbol in allData.Keys)
            {
                Console.WriteLine($"Processing Historic Data for Symbol {symbol} {j} out of {symbols.Count()}");
                j++;
                List<double> ydata_SYMB = new List<double>();
                List<double> ydata_SYMB_price = new List<double>();
                
                dynamic thistoricalDataList = new List<object>();
                List<HistoricalData> data = allData[symbol];

                for (int i = 0; i < data.Count(); i++)
                {
                    dynamic thistoricalData = new ExpandoObject();
                    var ItemCurrent = data[i];

                    thistoricalData.Date = ItemCurrent.Date;
                    thistoricalData.AdjClose = ItemCurrent.AdjClose;
                    ydata_SYMB_price.Add(ItemCurrent.AdjClose);
                    if (i > 0)
                    {
                        var ItemPrevus = data[i - 1];
                        ydata_SYMB.Add((ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1);
                        thistoricalData.Return = (ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1;
                    }
                    else
                    {
                        thistoricalData.Return = 0;
                    }


                    thistoricalData.Space = " ";


                    var item_SP = thistoricalDataList_SP[i];


                    thistoricalData.SP_Date = item_SP.Date;
                    thistoricalData.SP_AdjClose = item_SP.AdjClose;
                    thistoricalData.SP_Return = item_SP.Return;

                    thistoricalDataList.Add(thistoricalData);
                }
                allDataProcessed.Add(symbol, thistoricalDataList);
              //  ydata_SYMB_Dic.Add(symbol, ydata_SYMB);
             //   ydata_SYMB_price_Dic.Add(symbol, ydata_SYMB_price);

                dynamic Regreession = new ExpandoObject();
                Regreession.symbol = symbol;
                if (xdata_SP != null && ydata_SYMB != null)
                { 
                    var p = Fit.Line(xdata_SP.ToArray(), ydata_SYMB.ToArray());

                double a = p.Item1; // == 10; intercept
                double b = p.Item2; // == 0.5; slope

                 Regreession.Intercept = a;
                 Regreession.Slope = b;
                 Regreession.MedianPrice = MathNet.Numerics.Statistics.Statistics.Median(ydata_SYMB_price.ToArray());
                 Regreession.AveragePrice = MathNet.Numerics.Statistics.Statistics.Mean(ydata_SYMB_price.ToArray());
                 Regreession.StandardDeviationPrice = MathNet.Numerics.Statistics.Statistics.StandardDeviation(ydata_SYMB_price.ToArray());
                 Regreession.VariancePrice = MathNet.Numerics.Statistics.Statistics.Variance(ydata_SYMB_price.ToArray());
               
                 Regreession.Return = (ydata_SYMB_price[ydata_SYMB_price.Count - 1] / ydata_SYMB_price[0]) - 1;
                }
               
                RegressionAnalysis.Add(Regreession);
             
                using (var writer = new StreamWriter($"E:\\CIIM\\Yahoo Data\\Temp\\{symbol} {DateTime.Now.Ticks.ToString()}.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(thistoricalDataList);
                }


            
            }


            Dictionary<string, double> covar = new Dictionary<string, double>();
            Dictionary<string, double> covarWorth = new Dictionary<string, double>();
            foreach (string symbol in allData.Keys)
                foreach (string symbol2 in allData.Keys)                   
                {
                    List<HistoricalData> data1 = allData[symbol];
                    List<double> PriceClose1 = new List<double>();

                    for (int i = 0; i < data1.Count(); i++)
                    {                      
                       var ItemCurrent = data1[i];
                        if (i > 0)
                        {
                            var ItemPrevus = data1[i - 1];
                            PriceClose1.Add((ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1);
                           
                        }
                        
                    }


                    List<HistoricalData> data2 = allData[symbol2];
                    List<double> PriceClose2 = new List<double>();

                    for (int i = 0; i < data2.Count(); i++)
                    {
                        var ItemCurrent = data2[i];
                        if (i > 0)
                        {
                            var ItemPrevus = data2[i - 1];
                            PriceClose2.Add((ItemCurrent.AdjClose / ItemPrevus.AdjClose) - 1);

                        }
                    }

                   var Covar= MathNet.Numerics.Statistics.Statistics.Covariance(PriceClose1, PriceClose2);
                    var PopCovar = MathNet.Numerics.Statistics.Statistics.PopulationCovariance(PriceClose1, PriceClose2);
                    covar.Add(symbol + " " + symbol2, PopCovar);
                    if (PopCovar < 0)
                    { covarWorth.Add(symbol + " " + symbol2, PopCovar);  }
                }


                    var config2 = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };
            using (var writer = new StreamWriter($"E:\\CIIM\\Yahoo Data\\Temp\\Regression Analysis {DateTime.Now.Ticks.ToString()}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(RegressionAnalysis);
            }
            return allDataProcessed;

        }




        private static async Task<bool> FetchdataAsync()
        {

          

            var startDate = DateTime.Now.AddYears(-1);
          //  var symbol = "AAPL";
          //  var fundSymbol = "VSMPX";
            var symbols = new string[] {
                "ABT ",
                "ADBE",
                "AMD ",
                "HON ",
                "AXP ",
                "AIG ",
                "AMGN",
                "AAPL",
                "VZ  ",
                "BRKb",
                "BA  ",
                "BMY ",
                "CVS ",
                "COF ",
                "CAT ",
                "JPM ",
                "CVX ",
                "CSCO",
                "KO  ",
                "CL  ",
                "DHR ",
                "TGT ",
                "DIS ",
                "DUK ",
                "EMR ",
                "XOM ",
                "NEE ",
                "FDX ",
                "GD  ",
                "GE  ",
                "GILD",
                "HD  ",
                "INTC",
                "IBM ",
                "JNJ ",
                "LLY ",
                "LMT ",
                "LOW ",
                "MCD ",
                "MDT ",
                "BK  ",
                "MSFT",
                "MMM ",
                "MS  ",
                "NKE ",
                "WFC ",
                "ORCL",
                "EXC ",
                "PEP ",
                "PFE ",
                "MO  ",
                "COP ",
                "PG  ",
                "QCOM",
                "T   ",
                "SCHW",
                "SPG ",
                "SO  ",
                "USB ",
                "SBUX",
                "TXN ",
                "TMO ",
                "C   ",
                "UNP ",
                "UNH ",
                "RTX ",
                "WMT ",
                "WBA ",
                "AMT ",
                "BAC ",
                "AMZN",
                "GS  ",
                "NVDA",
                "BKNG",
                "COST",
                "UPS ",
                "BLK ",
                "MET ",
                "F   ",
                "ACN ",
                "MDLZ",
                "NFLX",
                "CMCSA",
                "CRM ",
                "TMUS",
                "GOOGL",
                "MA  ",
                "PM  ",
                "V   ",
                "AVGO",
                "MRK ",
                "CHTR",
                "TSLA",
                "GM  ",
                "META",
                "ABBV",
                "GOOG",
                "KHC ",
                "PYPL",
                "LIN ",
                "DOW "

            };

            var TickerInfos_SummaryDetail = new List<SummaryDetail>();
            var yahooClient = new YahooClient();
            foreach (var symboltemp in symbols)
            {

                try
                {
                    var TickerInfo = new TickerInfo();
                    var symbol = symboltemp.TrimEnd();
                    TickerInfo.TickerSymbol = symbol;
                  
                    //var historicalDataList = await yahooClient.GetHistoricalDataAsync(symbolTrimmed, DataFrequency.Daily, startDate);

                    //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    //{
                    //    NewLine = Environment.NewLine,
                    //};
                    //using (var writer = new StreamWriter($"{symbolTrimmed}.csv"))
                    //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    //{
                    //    csv.WriteRecords(historicalDataList);
                    //}


                    var capitalGainList = await yahooClient.GetCapitalGainDataAsync(symbol, DataFrequency.Monthly, startDate);
                    var dividendList = await yahooClient.GetDividendDataAsync(symbol, DataFrequency.Weekly, startDate);
                    var stockSplitList = await yahooClient.GetStockSplitDataAsync(symbol, DataFrequency.Monthly, startDate);
                   
                    var recommendedList = await yahooClient.GetStockRecommendationsAsync(symbol);
                    
                    var keyStatsList = await yahooClient.GetKeyStatisticsAsync(symbol);

                    //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(keyStatsList));
                    //Console.WriteLine(keyStatsList.Beta.Fmt);

                    var summaryDetailsList = await yahooClient.GetSummaryDetailsAsync(symbol);

                    //  TickerInfo.Beta= summaryDetailsList.Beta.Fmt;


                  
                    var insiderHoldersList = await yahooClient.GetInsiderHoldersAsync(symbol);
                    var insiderTransactionsList = await yahooClient.GetInsiderTransactionsAsync(symbol);
                     var financialDataList = await yahooClient.GetFinancialDataAsync(symbol);

        
                    var institutionOwnershipList = await yahooClient.GetInstitutionOwnershipAsync(symbol);
                    var fundOwnershipList = await yahooClient.GetFundOwnershipAsync(symbol);
                    var majorDirectHoldersList = await yahooClient.GetMajorDirectHoldersAsync(symbol);
                    var secFilingsList = await yahooClient.GetSecFilingsAsync(symbol);
                   var insightsList = await yahooClient.GetInsightsAsync(symbol);

                 


                    var majorHoldersBreakdownList = await yahooClient.GetMajorHoldersBreakdownAsync(symbol);
                    var upgradeDowngradeHistoryList = await yahooClient.GetUpgradeDowngradeHistoryAsync(symbol);
                    var esgScoresList = await yahooClient.GetEsgScoresAsync(symbol);
                    var recommendationTrendList = await yahooClient.GetRecommendationTrendAsync(symbol);
                    var indexTrendList = await yahooClient.GetIndexTrendAsync(symbol);
                    var sectorTrendList = await yahooClient.GetSectorTrendAsync(symbol);
                    var earningsTrendList = await yahooClient.GetEarningsTrendAsync(symbol);
                    var assetProfileList = await yahooClient.GetAssetProfileAsync(symbol);
                    //var fundProfileList = await yahooClient.GetFundProfileAsync(fundSymbol);
                    var calendarEventsList = await yahooClient.GetCalendarEventsAsync(symbol);
                    var earningsList = await yahooClient.GetEarningsAsync(symbol);
                    var balanceSheetHistoryList = await yahooClient.GetBalanceSheetHistoryAsync(symbol);
                    var cashflowStatementHistoryList = await yahooClient.GetCashflowStatementHistoryAsync(symbol);
                    var incomeStatementHistoryList = await yahooClient.GetIncomeStatementHistoryAsync(symbol);
                    var earningsHistoryList = await yahooClient.GetEarningsHistoryAsync(symbol);
                    var quoteTypeList = await yahooClient.GetQuoteTypeAsync(symbol);
                    var priceList = await yahooClient.GetPriceInfoAsync(symbol);
                    var netSharePurchaseActivityList = await yahooClient.GetNetSharePurchaseActivityAsync(symbol);
                    var incomeStatementHistoryQuarterlyList = await yahooClient.GetIncomeStatementHistoryQuarterlyAsync(symbol);
                    var cashflowStatementHistoryQuarterlyList = await yahooClient.GetCashflowStatementHistoryQuarterlyAsync(symbol);
                    var balanceSheetHistoryQuarterlyList = await yahooClient.GetBalanceSheetHistoryQuarterlyAsync(symbol);
                    var chartInfoList = await yahooClient.GetChartInfoAsync(symbol, TimeRange._1Year, TimeInterval._1Day);
                   
                
                   


                    TickerInfos_SummaryDetail.Add(summaryDetailsList);
                }
                catch (Exception e) { }

                break;
            }

            //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    NewLine = Environment.NewLine,
            //};
            //using (var writer = new StreamWriter($"TickerInfos_SummaryDetail.csv"))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(TickerInfos_SummaryDetail);
            //}
        //    var sparkChartInfoList = await yahooClient.GetSparkChartInfoAsync(symbols, TimeRange._1Month, TimeInterval._1Day);

            var autoCompleteList = await yahooClient.GetAutoCompleteInfoAsync("Google");
            var marketSummaryList = await yahooClient.GetMarketSummaryAsync();
         //   var realTimeQuoteList = await yahooClient.GetRealTimeQuotesAsync(symbols);
            var topTrendingList = await yahooClient.GetTopTrendingStocksAsync(Country.UnitedStates, 10);
            var topGainersList = await yahooClient.GetTopGainersAsync(10);
            var topLosersList = await yahooClient.GetTopLosersAsync(10);
            var smallCapGainersList = await yahooClient.GetSmallCapGainersAsync(10);
            var mostActiveStocksList = await yahooClient.GetMostActiveStocksAsync(10);
            var aggressiveSmallCapsList = await yahooClient.GetAggressiveSmallCapStocksAsync(10);
            var conservativeForeignFundsList = await yahooClient.GetConservativeForeignFundsAsync(10);
            var growthTechStocksList = await yahooClient.GetGrowthTechnologyStocksAsync(10);
            var highYieldBondsList = await yahooClient.GetHighYieldBondsAsync(10);
            var mostShortedStocksList = await yahooClient.GetMostShortedStocksAsync(10);
            var portfolioAnchorsList = await yahooClient.GetPortfolioAnchorsAsync(10);
            var solidLargeGrowthFundsList = await yahooClient.GetSolidLargeGrowthFundsAsync(10);
            var solidMidcapGrowthFundsList = await yahooClient.GetSolidMidcapGrowthFundsAsync(10);
            var topMutualFundsList = await yahooClient.GetTopMutualFundsAsync(10);
            var undervaluedGrowthStocksList = await yahooClient.GetUndervaluedGrowthStocksAsync(10);
            var undervaluedLargeCapsList = await yahooClient.GetUndervaluedLargeCapStocksAsync(10);
            var undervaluedWideMoatStocksList = await yahooClient.GetUndervaluedWideMoatStocksAsync(10);
            var morningstarFiveStarStocksList = await yahooClient.GetMorningstarFiveStarStocksAsync(10);
            var strongUndervaluedStocksList = await yahooClient.GetStrongUndervaluedStocksAsync(10);
            var analystStrongBuyStocksList = await yahooClient.GetAnalystStrongBuyStocksAsync(10);
        //   AnalystResult an = latestAnalystUpgradedStocksList = await yahooClient.GetLatestAnalystUpgradedStocksAsync(10);
            var mostInstitutionallyBoughtLargeCapStocksList = await yahooClient.GetMostInstitutionallyBoughtLargeCapStocksAsync(10);
            var mostInstitutionallyHeldLargeCapStocksList = await yahooClient.GetMostInstitutionallyHeldLargeCapStocksAsync(10);
            var mostInstitutionallySoldLargeCapStocksList = await yahooClient.GetMostInstitutionallySoldLargeCapStocksAsync(10);
            var stocksWithMostInstitutionalBuyersList = await yahooClient.GetStocksWithMostInstitutionalBuyersAsync(10);
            var stocksWithMostInstitutionalSellersList = await yahooClient.GetStocksWithMostInstitutionalSellersAsync(10);
            var stocksMostBoughtByHedgeFundsList = await yahooClient.GetStocksMostBoughtByHedgeFundsAsync(10);
            var stocksMostBoughtByPensionFundsList = await yahooClient.GetStocksMostBoughtByPensionFundsAsync(10);
            var stocksMostBoughtByPrivateEquityList = await yahooClient.GetStocksMostBoughtByPrivateEquityAsync(10);
            var stocksMostBoughtBySovereignWealthFundsList = await yahooClient.GetStocksMostBoughtBySovereignWealthFundsAsync(10);
            var topStocksOwnedByCathieWoodList = await yahooClient.GetTopStocksOwnedByCathieWoodAsync(10);
            var topStocksOwnedByGoldmanSachsList = await yahooClient.GetTopStocksOwnedByGoldmanSachsAsync(10);
            var topStocksOwnedByWarrenBuffetList = await yahooClient.GetTopStocksOwnedByWarrenBuffetAsync(10);
            var topStocksOwnedByRayDalioList = await yahooClient.GetTopStocksOwnedByRayDalioAsync(10);
            var topBearishStocksRightNowList = await yahooClient.GetTopBearishStocksRightNowAsync(10);
            var topBullishStocksRightNowList = await yahooClient.GetTopBullishStocksRightNowAsync(10);
            var topUpsideBreakoutStocksList = await yahooClient.GetTopUpsideBreakoutStocksAsync(10);


            return true;
        }
    }
}
