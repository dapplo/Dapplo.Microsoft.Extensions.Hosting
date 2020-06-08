using System;
using System.Diagnostics;
using System.Reactive;
using NuGet.Protocol.Core.Types;
using ReactiveUI;

namespace Dapplo.Hosting.Sample.ReactiveDemo
{
    // This class wraps out NuGet model object into a ViewModel and allows
    // us to have a ReactiveCommand to open the NuGet package URL.
    public class NugetDetailsViewModel : ReactiveObject
    {
        private readonly IPackageSearchMetadata metadata;
        private readonly Uri defaultUrl;

        public NugetDetailsViewModel(IPackageSearchMetadata metadata)
        {
            this.metadata = metadata;
            this.defaultUrl = new Uri("https://git.io/fAlfh");

            var startInfo = new ProcessStartInfo(ProjectUrl?.ToString() ?? this.defaultUrl.ToString())
            {
                UseShellExecute = true
            };
            OpenPage = ReactiveCommand.Create( () => { Process.Start(startInfo); });
        }

        public Uri IconUrl => this.metadata.IconUrl ?? this.defaultUrl;
        public string Description => this.metadata.Description;
        public Uri ProjectUrl => this.metadata.ProjectUrl;
        public string Title => this.metadata.Title;

        // ReactiveCommand allows us to execute logic without exposing any of the
        // implementation details with the View. The generic parameters are the
        // input into the command and it's output. In our case we don't have any
        // input or output so we use Unit which in Reactive speak means a void type.
        public ReactiveCommand<Unit, Unit> OpenPage { get; }
    }
}
