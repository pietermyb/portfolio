using System.Collections.Generic;

namespace Portfolio.Security
{
    /// <summary>
    /// Security Headers Policy
    /// </summary>
    public class SecurityHeadersPolicy
    {
        /// <summary>
        /// Gets the set headers.
        /// </summary>
        /// <value>
        /// The set headers.
        /// </value>
        public IDictionary<string, string> SetHeaders { get; }
             = new Dictionary<string, string>();

        /// <summary>
        /// Gets the remove headers.
        /// </summary>
        /// <value>
        /// The remove headers.
        /// </value>
        public ISet<string> RemoveHeaders { get; }
            = new HashSet<string>();
    }
}
