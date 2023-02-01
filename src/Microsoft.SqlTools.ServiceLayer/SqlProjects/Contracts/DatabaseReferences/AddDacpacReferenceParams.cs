﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.SqlTools.Hosting.Protocol.Contracts;
using Microsoft.SqlTools.ServiceLayer.Utility;

namespace Microsoft.SqlTools.ServiceLayer.SqlProjects.Contracts
{
    /// <summary>
    /// Parameters for adding a Dacpac reference to a SQL project
    /// </summary>
    public class AddDacpacReferenceParams : AddDatabaseReferenceParams
    {
        /// <summary>
        /// Path to the .dacpac file
        /// </summary>
        public string DacpacPath { get; set; }

        /// <summary>
        /// SQLCMD variable name for specifying the other server this reference is to, if different from that of the current project.
        /// If this is set, DatabaseVariable must also be set.
        /// </summary>
        public string? ServerVariable { get; set; }
    }

    public class AddDacpacReferenceRequest
    {
        public static readonly RequestType<AddDacpacReferenceParams, ResultStatus> Type = RequestType<AddDacpacReferenceParams, ResultStatus>.Create("sqlprojects/addDacpacReference");
    }
}