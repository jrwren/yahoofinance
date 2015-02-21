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

namespace MaasOne.Net
{
    public interface IQueryDownload
    {
        event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;



        DownloadOperationCollection ActiveOperations { get; }

        QueryBase DefaultQuery { get; set; }

#if !(SILVERLIGHT)
        System.Net.IWebProxy Proxy { get; set; }
#endif

        int Timeout { get; set; }

        

#if !(NETFX_CORE || SILVERLIGHT)
        IQueryResponse Download();

        IQueryResponse Download(QueryBase query);
#endif

        void DownloadAsync(object userArgs);

        void DownloadAsync(QueryBase query, object userArgs);

#if !(NET20 || NET35 || NET40 || SILVERLIGHT)
        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync();

        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync(QueryBase query);
#endif

        bool IsCorrespondingType(Type queryType);

        bool IsCorrespondingType(QueryBase queryObject);
    }
}
