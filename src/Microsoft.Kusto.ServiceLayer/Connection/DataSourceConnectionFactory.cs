﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Composition;
using Microsoft.Kusto.ServiceLayer.Connection.Contracts;
using Microsoft.Kusto.ServiceLayer.DataSource;
using Microsoft.SqlTools.ServiceLayer.Connection.ReliableConnection;

namespace Microsoft.Kusto.ServiceLayer.Connection
{
    /// <summary>
    /// Factory class to create SqlClientConnections
    /// The purpose of the factory is to make it easier to mock out the database
    /// in 'offline' unit test scenarios.
    /// </summary>
    [Export(typeof(IDataSourceConnectionFactory))]
    public class DataSourceConnectionFactory : IDataSourceConnectionFactory
    {
        private readonly IDataSourceFactory _dataSourceFactory;

        [ImportingConstructor]
        public DataSourceConnectionFactory(IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        /// <summary>
        /// Creates a new SqlConnection object
        /// </summary>
        public ReliableDataSourceConnection CreateDataSourceConnection(ConnectionDetails connectionDetails, string ownerUri)
        {
            RetryPolicy connectionRetryPolicy = RetryPolicyFactory.CreateDefaultConnectionRetryPolicy();
            RetryPolicy commandRetryPolicy = RetryPolicyFactory.CreateDefaultConnectionRetryPolicy();
            return new ReliableDataSourceConnection(connectionDetails, connectionRetryPolicy, commandRetryPolicy, _dataSourceFactory, ownerUri);
        }
    }
}
