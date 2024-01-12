﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//


using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlTools.ServiceLayer.Management;

namespace Microsoft.SqlTools.ServiceLayer.Admin
{
    /// <summary>
    /// Database properties for SqlServer 2016
    /// </summary>
    internal class DatabasePrototype130 : DatabasePrototype110
    {
        // Properties that doen't support secondary value updates
        // More info here: https://learn.microsoft.com/en-us/sql/t-sql/statements/alter-database-scoped-configuration-transact-sql?view=sql-server-ver16
        // The secondaryValUnsupportedPropsSet containst the configuration Ids of the below properties
        // IDENTITY_CACHE(6)
        // ELEVATE_ONLINE(11)
        // ELEVATE_RESUMABLE(12)
        // GLOBAL_TEMPORARY_TABLE_AUTO_DROP(21)
        // PAUSED_RESUMABLE_INDEX_ABORT_DURATION_MINUTES(25)
        private static readonly HashSet<int> secondaryValUnsupportedPropsSet = new HashSet<int> { 6, 11, 12, 21, 25 };

        /// <summary>
        /// Database properties for SqlServer 2016 class constructor
        /// </summary>
        public DatabasePrototype130(CDataContainer context)
            : base(context)
        {
        }

        [Category("Category_DatabaseScopedConfigurations")]
        public DatabaseScopedConfigurationCollection DatabaseScopedConfiguration
        {
            get
            {
                return this.currentState.databaseScopedConfigurations;
            }
            set
            {
                this.currentState.databaseScopedConfigurations = value;
                this.NotifyObservers();
            }
        }

        [Category("Category_QueryStoreOptions")]
        public QueryStoreOptions QueryStoreOptions
        {
            get
            {
                return this.currentState.queryStoreOptions;
            }
            set
            {
                this.currentState.queryStoreOptions = value;
                this.NotifyObservers();
            }
        }

        protected override void SaveProperties(Database db)
        {
            base.SaveProperties(db);

            for (int i = 0; i < db.DatabaseScopedConfigurations.Count; i++)
            {
                if (db.DatabaseScopedConfigurations[i].Value != this.currentState.databaseScopedConfigurations[i].Value)
                {
                    db.DatabaseScopedConfigurations[i].Value = this.currentState.databaseScopedConfigurations[i].Value;
                }

                // Configurations that are not allowed secondary replicas are excluded.
                if (db.DatabaseScopedConfigurations[i].ValueForSecondary != this.currentState.databaseScopedConfigurations[i].ValueForSecondary
                    && !secondaryValUnsupportedPropsSet.Contains(db.DatabaseScopedConfigurations[i].Id))
                {
                    db.DatabaseScopedConfigurations[i].ValueForSecondary = this.currentState.databaseScopedConfigurations[i].ValueForSecondary;
                }
            }

            if (db.IsSupportedObject<QueryStoreOptions>() && this.currentState.queryStoreOptions != null)
            {
                if (!this.Exists || (db.QueryStoreOptions.DesiredState != this.currentState.queryStoreOptions.DesiredState))
                {
                    db.QueryStoreOptions.DesiredState = this.currentState.queryStoreOptions.DesiredState;
                }

                if (this.currentState.queryStoreOptions.DesiredState != QueryStoreOperationMode.Off)
                {
                    if (!this.Exists || (db.QueryStoreOptions.DataFlushIntervalInSeconds != this.currentState.queryStoreOptions.DataFlushIntervalInSeconds))
                    {
                        db.QueryStoreOptions.DataFlushIntervalInSeconds = this.currentState.queryStoreOptions.DataFlushIntervalInSeconds;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.StatisticsCollectionIntervalInMinutes != this.currentState.queryStoreOptions.StatisticsCollectionIntervalInMinutes))
                    {
                        db.QueryStoreOptions.StatisticsCollectionIntervalInMinutes = this.currentState.queryStoreOptions.StatisticsCollectionIntervalInMinutes;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.MaxPlansPerQuery != this.currentState.queryStoreOptions.MaxPlansPerQuery))
                    {
                        db.QueryStoreOptions.MaxPlansPerQuery = this.currentState.queryStoreOptions.MaxPlansPerQuery;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.MaxStorageSizeInMB != this.currentState.queryStoreOptions.MaxStorageSizeInMB))
                    {
                        db.QueryStoreOptions.MaxStorageSizeInMB = this.currentState.queryStoreOptions.MaxStorageSizeInMB;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.QueryCaptureMode != this.currentState.queryStoreOptions.QueryCaptureMode))
                    {
                        db.QueryStoreOptions.QueryCaptureMode = this.currentState.queryStoreOptions.QueryCaptureMode;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.SizeBasedCleanupMode != this.currentState.queryStoreOptions.SizeBasedCleanupMode))
                    {
                        db.QueryStoreOptions.SizeBasedCleanupMode = this.currentState.queryStoreOptions.SizeBasedCleanupMode;
                    }

                    if (!this.Exists || (db.QueryStoreOptions.StaleQueryThresholdInDays != this.currentState.queryStoreOptions.StaleQueryThresholdInDays))
                    {
                        db.QueryStoreOptions.StaleQueryThresholdInDays = this.currentState.queryStoreOptions.StaleQueryThresholdInDays;
                    }
                }
            }
        }
    }
}
