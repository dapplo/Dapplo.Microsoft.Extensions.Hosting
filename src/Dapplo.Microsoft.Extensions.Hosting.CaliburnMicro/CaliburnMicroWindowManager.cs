// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro
{
    /// <summary>
    ///     This extends the WindowManager to add some additional settings
    /// </summary>
    public class CaliburnMicroWindowManager : WindowManager
    {
        /// <summary>
        /// These imports make it possible to configure every window that is created
        /// </summary>
        protected IEnumerable<IConfigureWindowViews> ConfigureWindows { get; }

        /// <summary>
        /// These imports make it possible to configure every dialog that is created
        /// </summary>
        protected IEnumerable<IConfigureDialogViews> ConfigureDialogs { get; }

        /// <inheritdoc />
        public CaliburnMicroWindowManager(
            IEnumerable<IConfigureWindowViews> configureWindows,
            IEnumerable<IConfigureDialogViews> configureDialogs
            )
        {
            ConfigureWindows = configureWindows;
            ConfigureDialogs = configureDialogs;
        }

        /// <inheritdoc />
        public override void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null && rootModel != null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            base.ShowPopup(rootModel, context, settings);
        }

        /// <inheritdoc />
        public override bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.ShowDialog(rootModel, context, settings);
        }

        /// <inheritdoc />
        public override void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            base.ShowWindow(rootModel, context, settings);
        }

        /// <inheritdoc />
        public override Page CreatePage(object rootModel, object context, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreatePage(rootModel, context, settings);
        }

        /// <inheritdoc />
        protected override Popup CreatePopup(object rootModel, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreatePopup(rootModel, settings);
        }

        /// <inheritdoc />
        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreateWindow(rootModel, isDialog, context, settings);
        }

        /// <summary>
        ///     Create the window type for this window manager
        /// </summary>
        /// <param name="model">object with the model</param>
        /// <param name="view">object with the view</param>
        /// <param name="isDialog">specifies if this is a dialog</param>
        /// <returns>Window</returns>
        protected virtual Window CreateCustomWindow(object model, object view, bool isDialog)
        {
            var window = view as Window ?? new Window
            {
                Content = view
            };
            return window;
        }

        /// <inheritdoc />
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = CreateCustomWindow(model, view, isDialog);

            // Make dialogs possible
            window.SetValue(View.IsGeneratedProperty, true);

            var inferOwnerOf = InferOwnerOf(window);
            if (inferOwnerOf != null && isDialog)
            {
                window.Owner = inferOwnerOf;

                // Make sure the configurations, coming from elsewhere, are applied
                if (ConfigureDialogs != null)
                {
                    foreach (var configureWindow in ConfigureDialogs)
                    {
                        configureWindow.ConfigureDialogView(window, model);
                    }
                }
            }
            else if (ConfigureWindows != null)
            {
                // Make sure the configurations, coming from elsewhere, are applied
                foreach (var configureWindow in ConfigureWindows)
                {
                    configureWindow.ConfigureWindowView(window, model);
                }
            }

            // Just in case, make sure it's activated
            window.Activate();
            return window;
        }
    }
}