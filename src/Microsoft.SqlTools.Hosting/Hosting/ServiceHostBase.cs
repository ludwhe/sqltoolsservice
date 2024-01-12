//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Threading.Tasks;
using Microsoft.SqlTools.Hosting.Contracts;
using Microsoft.SqlTools.Hosting.Protocol;
using Microsoft.SqlTools.Hosting.Protocol.Channel;

namespace Microsoft.SqlTools.Hosting
{
    public abstract class ServiceHostBase : ProtocolEndpoint
    {
        private TaskCompletionSource<bool> serverExitedTask;

        protected ServiceHostBase(ChannelBase serverChannel) :
            base(serverChannel, MessageProtocolType.LanguageServer)
        {
        }

        protected override Task OnStart()
        {
            // Register handlers for server lifetime messages

            this.SetEventHandler(ExitNotification.Type, this.HandleExitNotification);
            this.serverExitedTask = new TaskCompletionSource<bool>();

            return Task.FromResult(true);
        }

        private async Task HandleExitNotification(
            object exitParams,
            EventContext eventContext)
        {
            // Stop the server channel
            await this.Stop();

            // Notify any waiter that the server has exited
            if (this.serverExitedTask != null)
            {
                this.serverExitedTask.SetResult(true);
            }
        }
    }
}

