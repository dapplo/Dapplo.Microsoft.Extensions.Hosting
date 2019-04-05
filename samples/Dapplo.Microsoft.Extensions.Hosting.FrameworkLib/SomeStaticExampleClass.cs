using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.FrameworkLib
{
    /// <summary>
    /// A bad example, but it will demonstrate if the framework lib is correctly loaded
    /// </summary>
    public static class SomeStaticExampleClass
    {
        /// <summary>
        /// Every plugin can register itself here
        /// </summary>
        public static IList<string> RegisteredServices { get; } = new List<string>();
    }
}