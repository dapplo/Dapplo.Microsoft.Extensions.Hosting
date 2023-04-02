// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;

#if NET472
using System.Security.AccessControl;
using System.Security.Principal;
#endif

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    ///     This protects your resources or application from running more than once
    ///     Simplifies the usage of the Mutex class, as described here:
    ///     https://msdn.microsoft.com/en-us/library/System.Threading.Mutex.aspx
    /// </summary>
    public sealed class ResourceMutex : IDisposable
    {
        private readonly ILogger logger;

        private readonly string mutexId;
        private readonly string resourceName;
        private Mutex applicationMutex;

        /// <summary>
        ///     Private constructor
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="mutexId">string with a unique Mutex ID</param>
        /// <param name="resourceName">optional name for the resource</param>
        private ResourceMutex(ILogger logger, string mutexId, string resourceName = null)
        {
            this.logger = logger;
            this.mutexId = mutexId;
            this.resourceName = resourceName ?? mutexId;
        }

        /// <summary>
        ///     Test if the Mutex was created and locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        ///     Create a ResourceMutex for the specified mutex id and resource-name
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="mutexId">ID of the mutex, preferably a Guid as string</param>
        /// <param name="resourceName">Name of the resource to lock, e.g your application name, useful for logs</param>
        /// <param name="global">true to have a global mutex see: https://msdn.microsoft.com/en-us/library/bwe34f1k.aspx</param>
        public static ResourceMutex Create(ILogger logger, string mutexId, string resourceName = null, bool global = false)
        {
            if (mutexId == null)
            {
                throw new ArgumentNullException(nameof(mutexId));
            }
            logger ??= new LoggerFactory().CreateLogger<ResourceMutex>();
            var applicationMutex = new ResourceMutex(logger, (global ? @"Global\" : @"Local\") + mutexId, resourceName);
            applicationMutex.Lock();
            return applicationMutex;
        }

        /// <summary>
        ///     This tries to get the Mutex, which takes care of having multiple instances running
        /// </summary>
        /// <returns>true if it worked, false if another instance is already running or something went wrong</returns>
        public bool Lock()
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("{0} is trying to get Mutex {1}", this.resourceName, this.mutexId);
            }

            IsLocked = true;
            // check whether there's an local instance running already, but use local so this works in a multi-user environment
            try
            {
#if NET472
                // Added Mutex Security, hopefully this prevents the UnauthorizedAccessException more gracefully
                var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                var mutexsecurity = new MutexSecurity();
                mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
                mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.ChangePermissions, AccessControlType.Deny));
                mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.Delete, AccessControlType.Deny));

                // 1) Create Mutex
                this.applicationMutex = new Mutex(true, this.mutexId, out var createdNew, mutexsecurity);
#else
                // 1) Create Mutex
                this.applicationMutex = new Mutex(true, this.mutexId, out var createdNew);
#endif
                // 2) if the mutex wasn't created new get the right to it, this returns false if it's already locked
                if (!createdNew)
                {
                    IsLocked = this.applicationMutex.WaitOne(2000, false);
                    if (!IsLocked)
                    {
                        this.logger.LogWarning("Mutex {0} is already in use and couldn't be locked for the caller {1}", this.mutexId, this.resourceName);
                        // Clean up
                        this.applicationMutex.Dispose();
                        this.applicationMutex = null;
                    }
                    else
                    {
                        this.logger.LogInformation("{0} has claimed the mutex {1}", this.resourceName, this.mutexId);
                    }
                }
                else
                {
                    this.logger.LogInformation("{0} has created & claimed the mutex {1}", this.resourceName, this.mutexId);
                }
            }
            catch (AbandonedMutexException e)
            {
                // Another instance didn't cleanup correctly!
                // we can ignore the exception, it happened on the "WaitOne" but still the mutex belongs to us
                this.logger.LogWarning(e, "{resourceName} didn't cleanup correctly, but we got the mutex {mutexId}.", this.resourceName, this.mutexId);
            }
            catch (UnauthorizedAccessException e)
            {
                this.logger.LogError(e, "{resourceName} is most likely already running for a different user in the same session, can't create/get mutex {mutexId} due to error.",
                    this.resourceName, this.mutexId);
                IsLocked = false;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Problem obtaining the Mutex {mutexId} for {resourceName}, assuming it was already taken!", this.resourceName, this.mutexId);
                IsLocked = false;
            }
            return IsLocked;
        }

        //  To detect redundant Dispose calls
        private bool disposedValue;

        /// <summary>
        ///     Dispose the application mutex
        /// </summary>
        public void Dispose()
        {
            if (this.disposedValue)
            {
                return;
            }
            this.disposedValue = true;
            if (this.applicationMutex == null)
            {
                return;
            }
            try
            {
                if (IsLocked)
                {
                    this.applicationMutex.ReleaseMutex();
                    IsLocked = false;
                    this.logger.LogInformation("Released Mutex {0} for {1}", this.mutexId, this.resourceName);
                }
                this.applicationMutex.Dispose();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error releasing Mutex {0} for {1}", this.mutexId, this.resourceName);
            }
            this.applicationMutex = null;
        }
    }
}
