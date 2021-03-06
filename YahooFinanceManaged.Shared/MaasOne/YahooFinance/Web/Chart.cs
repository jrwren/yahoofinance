#region "License"
// *********************************************************************************************
// **                                                                                         **
// **  Yahoo! Finance Managed                                                                 **
// **                                                                                         **
// **  Copyright (c) Marius Häusler 2009-2015                                                 **
// **                                                                                         **
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).  **
// **                                                                                         **
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt                        **
// **                                                                                         **
// **  Project: https://yahoofinance.codeplex.com/                                            **
// **                                                                                         **
// **  Contact: maasone@live.com                                                              **
// **                                                                                         **
// *********************************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using MaasOne.Net;
using MaasOne.YahooFinance;

namespace MaasOne.YahooFinance.Web
{
    /// <summary>
    /// Provides properties for setting options of chart download.
    /// </summary>
    /// <remarks>Inherits from Query&lt;ChartResult&gt;.</remarks>
    public class ChartQuery : Query<ChartResult>
    {
        /// <summary>
        /// Gets the ID list of all compared stocks/indices.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] ComparingIDs { get; set; }

        /// <summary>
        /// Gets or sets the used culture for scale descriptions. Can only be used with Server [USA].
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Culture Culture { get; set; }

        /// <summary>
        /// Gets the list of exponential moving average indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartMovingAverageInterval[] ExponentialMovingAverages { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo! Finance ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets the height of the image
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Gets the size of the image (only available if SimplifiedImage = FALSE)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartImageSize ImageSize { get; set; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ImageWidth { get; set; }

        /// <summary>
        /// Gets the list of moving average indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartMovingAverageInterval[] MovingAverages { get; set; }

        /// <summary>
        /// Gets the scaling of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartScale Scale { get; set; }

        /// <summary>
        /// Gets a bool value if the image is simplified (1 day period; only ImageWidth, ImageHeight and Culture options available; Other options will be ignored)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SimplifiedImage { get; set; }

        /// <summary>
        /// Gets the list of technical indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartTechnicalIndicator[] TechnicalIndicators { get; set; }

        /// <summary>
        /// Gets the span of the reviewed period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartTimeSpan TimeSpan { get; set; }

        /// <summary>
        /// Gets the chart type of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartType Type { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ChartQuery()
        {
            this.ImageWidth = 300;
            this.ImageHeight = 180;
            this.SimplifiedImage = false;
            this.ImageSize = ChartImageSize.Middle;
            this.TimeSpan = ChartTimeSpan.c1D;
            this.Type = ChartType.Line;
            this.Scale = ChartScale.Logarithmic;
            this.MovingAverages = new ChartMovingAverageInterval[] { };
            this.ExponentialMovingAverages = new ChartMovingAverageInterval[] { };
            this.TechnicalIndicators = new ChartTechnicalIndicator[] { };
            this.ComparingIDs = new string[] { };
            this.Culture = Cultures.UnitedStates_English;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public ChartQuery(string id) : this() { this.ID = id; }


        public override QueryBase Clone()
        {
            return new ChartQuery(this.ID)
            {
                SimplifiedImage = this.SimplifiedImage,
                ImageWidth = this.ImageWidth,
                ImageHeight = this.ImageHeight,
                ImageSize = this.ImageSize,
                TimeSpan = this.TimeSpan,
                Type = this.Type,
                Scale = this.Scale,
                Culture = this.Culture,
                MovingAverages = (ChartMovingAverageInterval[])this.MovingAverages.Clone(),
                ExponentialMovingAverages = (ChartMovingAverageInterval[])this.ExponentialMovingAverages.Clone(),
                TechnicalIndicators = (ChartTechnicalIndicator[])this.TechnicalIndicators.Clone(),
                ComparingIDs = (string[])this.ComparingIDs.Clone(),
            };
        }


        protected override ChartResult ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            return new ChartResult() { Chart = MyHelper.CopyStream(stream) };
        }

        protected override string CreateUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            url.Append("http://");
            url.Append("chart.finance.yahoo.com/");

            if (this.SimplifiedImage) { url.Append("t?"); }
            else if (this.ImageSize == ChartImageSize.Small) { url.Append("h?"); }
            else { url.Append("z?"); }

            url.Append("s=");
            url.Append(Uri.EscapeDataString(this.ID));

            if (this.SimplifiedImage)
            {
                url.Append("&width=" + this.ImageWidth.ToString());
                url.Append("&height=" + this.ImageHeight.ToString());
            }
            else if (this.ImageSize != ChartImageSize.Small)
            {
                url.Append("&t=");
                url.Append(this.GetChartTimeSpan(this.TimeSpan));
                url.Append("&z=");
                url.Append(this.GetChartImageSize(this.ImageSize));
                url.Append("&q=");
                url.Append(this.GetChartType(this.Type));
                url.Append("&l=");
                url.Append(this.GetChartScale(this.Scale));
                if (this.MovingAverages.Length > 0 | this.ExponentialMovingAverages.Length > 0 | this.TechnicalIndicators.Length > 0)
                {
                    url.Append("&p=");
                    foreach (ChartMovingAverageInterval ma in this.MovingAverages)
                    {
                        url.Append('m');
                        url.Append(this.GetMovingAverageInterval(ma));
                        url.Append(',');
                    }
                    foreach (ChartMovingAverageInterval ma in this.ExponentialMovingAverages)
                    {
                        url.Append('e');
                        url.Append(this.GetMovingAverageInterval(ma));
                        url.Append(',');
                    }
                    foreach (ChartTechnicalIndicator ti in this.TechnicalIndicators)
                    {
                        url.Append(this.GetTechnicalIndicatorsI(ti));
                    }
                }
                if (this.TechnicalIndicators.Length > 0)
                {
                    url.Append("&a=");
                    foreach (ChartTechnicalIndicator ti in this.TechnicalIndicators)
                    {
                        url.Append(this.GetTechnicalIndicatorsII(ti));
                    }
                }
                if (this.ComparingIDs.Length > 0)
                {
                    url.Append("&c=");
                    foreach (string csid in this.ComparingIDs)
                    {
                        url.Append(YFHelper.CleanYqlParam(csid));
                        url.Append(',');
                    }
                }
                if (this.Culture == null)
                {
                    this.Culture = Cultures.UnitedStates_English;
                }
            }
            url.Append(YFHelper.CultureToParameters(this.Culture));
            return url.ToString();
        }

        protected override void Validate(ValidationResult result)
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                result.Success = false;
                result.Info.Add("ID", "ID is empty.");
            }
        }


        private string GetChartImageSize(ChartImageSize value)
        {
            return value.ToString().Substring(0, 1).ToLower();
        }

        private string GetChartTimeSpan(ChartTimeSpan value)
        {
            if (value == ChartTimeSpan.cMax)
            {
                return "my";
            }
            else
            {
                return value.ToString().Replace("c", "").ToLower();
            }
        }

        private string GetChartType(ChartType value)
        {
            return value.ToString().Substring(0, 1).ToLower();
        }

        private string GetChartScale(ChartScale value)
        {
            if (value == ChartScale.Arithmetic)
            {
                return "off";
            }
            else
            {
                return "on";
            }
        }

        private string GetMovingAverageInterval(ChartMovingAverageInterval value)
        {
            return value.ToString().Replace("m", "");
        }

        private string GetTechnicalIndicatorsI(ChartTechnicalIndicator value)
        {
            switch (value)
            {
                case ChartTechnicalIndicator.Bollinger_Bands:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case ChartTechnicalIndicator.Parabolic_SAR:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case ChartTechnicalIndicator.Splits:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case ChartTechnicalIndicator.Volume:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                default:
                    return string.Empty;
            }
        }

        private string GetTechnicalIndicatorsII(ChartTechnicalIndicator value)
        {
            switch (value)
            {
                case ChartTechnicalIndicator.MACD:
                    return "m26-12-9,";
                case ChartTechnicalIndicator.MFI:
                    return "f14,";
                case ChartTechnicalIndicator.ROC:
                    return "p12,";
                case ChartTechnicalIndicator.RSI:
                    return "r14,";
                case ChartTechnicalIndicator.Slow_Stoch:
                    return "ss,";
                case ChartTechnicalIndicator.Fast_Stoch:
                    return "fs,";
                case ChartTechnicalIndicator.Vol:
                    return "v,";
                case ChartTechnicalIndicator.Vol_MA:
                    return "vm,";
                case ChartTechnicalIndicator.W_R:
                    return "w14,";
                default:
                    return string.Empty;
            }
        }
    }


    /// <summary>
    /// Class for storing the downloaded chart.
    /// </summary>
    public class ChartResult : ResultBase
    {
        public System.IO.Stream Chart { get; internal set; }


        internal ChartResult() { }
    }
}
