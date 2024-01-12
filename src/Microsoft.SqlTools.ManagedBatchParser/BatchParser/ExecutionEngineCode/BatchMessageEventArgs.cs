//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Data.SqlClient;

namespace Microsoft.SqlTools.ServiceLayer.BatchParser.ExecutionEngineCode
{
    /// <summary>
    /// Event args for notification about non-error message
    /// </summary>
    public class BatchMessageEventArgs : EventArgs
    {
        private readonly string message = string.Empty;
        private readonly string detailedMessage = string.Empty;
        private readonly SqlError error;

        private BatchMessageEventArgs()
        {
        }

        internal BatchMessageEventArgs(string msg)
            : this(string.Empty, msg)
        {
        }

        internal BatchMessageEventArgs(string detailedMsg, string msg) : this(detailedMsg, msg, null)
        {
        }
        internal BatchMessageEventArgs(string detailedMsg, string msg, SqlError error)
        {
            message = msg;
            detailedMessage = detailedMsg;
            this.error = error;
        }

        public string Message
        {
            get
            {
                return message;
            }
        }

        public string DetailedMessage
        {
            get
            {
                return detailedMessage;
            }
        }

        public SqlError Error { get { return error; } }

    }
}
