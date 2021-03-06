using System.Collections.Generic;
using System.Reflection;

namespace HCF.BAL.Interfaces
{
    public interface IAssemblyProvider
    {
        /// <summary>
        ///     Discovers and then gets the discovered assemblies.
        /// </summary>
        /// <param name="path">
        ///     The extensions path of a web application. Might be used or ignored
        ///     by an implementation of the <see cref="IAssemblyProvider">IAssemblyProvider</see> interface.
        /// </param>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies(string path);

        /// <summary>
        ///     Discovers and then gets the discovered assemblies.
        /// </summary>
        /// <param name="paths">
        ///     Takes in multiple paths
        /// </param>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies(IEnumerable<string> paths);
    }
}
