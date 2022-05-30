/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using System.Collections.Generic;

namespace cAlgo
{

    #region Extensions

    public static class SymbolExtensions
    {

        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }

        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }

    public static class BarsExtensions
    {

        public static int GetIndexByDate(this Bars MyBars, DateTime MyTime)
        {

            for (int i = MyBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (MyTime == MyBars.OpenTimes[i])
                    return i;

            }

            return -1;

        }

    }

    #endregion

    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    [Levels(0)]
    public class MarketWatch : Indicator
    {

        #region Enums

        public class SymbolData
        {

            public string Name = string.Empty;
            public double Pips = double.NaN;
            public double Price = double.NaN;

        }

        #endregion

        #region Identity

        public const string NAME = "Market Watch";

        public const string VERSION = "1.0.5";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+market+watch")]
        public string ProductInfo { get; set; }

        [Parameter("Period", Group = "EMA", DefaultValue = 500)]
        public int EMAPeriod { get; set; }

        [Parameter("1° Symbol", Group = "Symbols", DefaultValue = "EURUSD")]
        public string SymbolCode1 { get; set; }

        [Parameter("2° Symbol", Group = "Symbols", DefaultValue = "USDJPY")]
        public string SymbolCode2 { get; set; }

        [Parameter("3° Symbol", Group = "Symbols", DefaultValue = "USDCAD")]
        public string SymbolCode3 { get; set; }

        [Parameter("4° Symbol", Group = "Symbols", DefaultValue = "AUDUSD")]
        public string SymbolCode4 { get; set; }

        [Parameter("5° Symbol", Group = "Symbols", DefaultValue = "GBPUSD")]
        public string SymbolCode5 { get; set; }

        [Parameter("6° Symbol", Group = "Symbols", DefaultValue = "NZDUSD")]
        public string SymbolCode6 { get; set; }

        [Parameter("7° Symbol", Group = "Symbols", DefaultValue = "EURGBP")]
        public string SymbolCode7 { get; set; }

        [Parameter("8° Symbol", Group = "Symbols", DefaultValue = "EURJPY")]
        public string SymbolCode8 { get; set; }

        [Parameter("9° Symbol", Group = "Symbols", DefaultValue = "AUDCAD")]
        public string SymbolCode9 { get; set; }

        [Parameter("10° Symbol", Group = "Symbols", DefaultValue = "GBPJPY")]
        public string SymbolCode10 { get; set; }

        [Parameter("Show Labels?", Group = "Styles", DefaultValue = true)]
        public bool ShowLabels { get; set; }

        [Output("1° Symbol", LineColor = "#fe5f55")]
        public IndicatorDataSeries Symbol1 { get; set; }

        [Output("2° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol2 { get; set; }

        [Output("3° Symbol", LineColor = "#4f6367")]
        public IndicatorDataSeries Symbol3 { get; set; }

        [Output("4° Symbol", LineColor = "#7a9e9f")]
        public IndicatorDataSeries Symbol4 { get; set; }

        [Output("5° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol5 { get; set; }

        [Output("6° Symbol", LineColor = "#967aa1")]
        public IndicatorDataSeries Symbol6 { get; set; }

        [Output("7° Symbol", LineColor = "#192a51")]
        public IndicatorDataSeries Symbol7 { get; set; }

        [Output("8° Symbol", LineColor = "#9d44b5")]
        public IndicatorDataSeries Symbol8 { get; set; }

        [Output("9° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol9 { get; set; }

        [Output("10° Symbol", LineColor = "#1c1c1c")]
        public IndicatorDataSeries Symbol10 { get; set; }

        #endregion

        #region Property

        readonly List<SymbolData> AllSymbols = new List<SymbolData>();

        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

            SymbolCode1 = SymbolCode1.Trim().ToUpper();
            SymbolCode2 = SymbolCode2.Trim().ToUpper();
            SymbolCode3 = SymbolCode3.Trim().ToUpper();
            SymbolCode4 = SymbolCode4.Trim().ToUpper();
            SymbolCode5 = SymbolCode5.Trim().ToUpper();
            SymbolCode6 = SymbolCode6.Trim().ToUpper();
            SymbolCode7 = SymbolCode7.Trim().ToUpper();
            SymbolCode8 = SymbolCode8.Trim().ToUpper();
            SymbolCode9 = SymbolCode9.Trim().ToUpper();
            SymbolCode10 = SymbolCode10.Trim().ToUpper();

        }

        public override void Calculate(int index)
        {

            AllSymbols.RemoveAll(item => true);

            AllSymbols.Add(SetValue(SymbolCode1, index, Symbol1));

            AllSymbols.Add(SetValue(SymbolCode2, index, Symbol2));

            AllSymbols.Add(SetValue(SymbolCode3, index, Symbol3));

            AllSymbols.Add(SetValue(SymbolCode4, index, Symbol4));

            AllSymbols.Add(SetValue(SymbolCode5, index, Symbol5));

            AllSymbols.Add(SetValue(SymbolCode6, index, Symbol6));

            AllSymbols.Add(SetValue(SymbolCode7, index, Symbol7));

            AllSymbols.Add(SetValue(SymbolCode8, index, Symbol8));

            AllSymbols.Add(SetValue(SymbolCode9, index, Symbol9));

            AllSymbols.Add(SetValue(SymbolCode10, index, Symbol10));

        }

        #endregion

        #region Private Methods

        private SymbolData SetValue(string MySymbol, int index, IndicatorDataSeries Result)
        {

            if (!Symbols.Exists(MySymbol))
                return new SymbolData();


            Symbol CROSS = Symbols.GetSymbol(MySymbol);
            Bars CROSS_Bars = MarketData.GetBars(TimeFrame, CROSS.Name);

            int CROSS_Index = CROSS_Bars.GetIndexByDate(Bars.OpenTimes[index]);
            if (CROSS_Index < 0)
                return new SymbolData();

            ExponentialMovingAverage CROSS_ema = Indicators.ExponentialMovingAverage(CROSS_Bars.ClosePrices, EMAPeriod);

            double CROSSpips = CROSS.DigitsToPips(Math.Round(CROSS_Bars.ClosePrices[CROSS_Index] - CROSS_ema.Result[CROSS_Index], CROSS.Digits));
            Result[index] = CROSSpips;

            if (ShowLabels)
            {

                string CROSStext = string.Format("  ‹ {0} {1:0.00} ({2:0.00000})", MySymbol, CROSSpips, CROSS_Bars.ClosePrices[CROSS_Index]);
                ChartText ThisLabel = Chart.IndicatorAreas[0].DrawText(MySymbol, CROSStext, Bars.OpenTimes.LastValue, CROSSpips, Color.Gray);
                ThisLabel.VerticalAlignment = VerticalAlignment.Center;

            }


            SymbolData response = new SymbolData 
            {
                Name = MySymbol,
                Price = Result[index],
                Pips = CROSSpips
            };

            return response;

        }

        #endregion

    }

}
