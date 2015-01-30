using System;

namespace MaasOne.Finance.Yahoo.Web
{

    /// <summary>
    /// Provides properties for a webservice query.
    /// </summary>
    public interface ILookupQuery
    {
        /// <summary>
        /// The query text.
        /// </summary>
        string LookupText { get; set; }
    }


    /// <summary>
    /// Provides properties to set the start index and count number for a query in results queue.
    /// </summary>
    public interface IResultIndexSettings
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }

}
