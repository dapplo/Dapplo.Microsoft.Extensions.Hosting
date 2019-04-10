//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// This is a helper to implement STA "Threading", which is a modified version of <see href="https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/">this</see>
    /// The class was found <see href="https://github.com/nunit/nunit/issues/1200#issuecomment-312120851">here</see>.
    /// </summary>
    public sealed class SingleThreadedSynchronizationContext : SynchronizationContext
    {
        private readonly BlockingCollection<(SendOrPostCallback d, object state)> _queue = new BlockingCollection<(SendOrPostCallback, object)>();

        /// <inheritdoc />
        public override void Post(SendOrPostCallback d, object state)
        {
            _queue.Add((d, state));
        }

        /// <summary>
        /// This implements the await
        /// </summary>
        /// <param name="taskInvoker">Func returning Task</param>
        public static void Await(Func<Task> taskInvoker)
        {
            var originalContext = Current;
            try
            {
                var context = new SingleThreadedSynchronizationContext();
                SetSynchronizationContext(context);

                var task = taskInvoker.Invoke();
                task.ContinueWith(_ => context._queue.CompleteAdding());

                while (context._queue.TryTake(out var work, Timeout.Infinite))
                {
                    work.d.Invoke(work.state);
                }

                task.GetAwaiter().GetResult();
            }
            finally
            {
                SetSynchronizationContext(originalContext);
            }
        }
    }
}
