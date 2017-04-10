namespace Portfolio.Security.Core
{
    /// <summary>
    /// Frame Options Constants
    /// </summary>
    public class FrameOptionsConstants
    {
        /// <summary>
        /// The X-Frame-Options header
        /// </summary>
        public const string Header = "X-Frame-Options";

        /// <summary>
        /// The deny value
        /// </summary>
        public const string Deny = "DENY";

        /// <summary>
        /// The same origin value
        /// </summary>
        public const string SameOrigin = "SAMEORIGIN";

        /// <summary>
        /// The allow from URI value
        /// </summary>
        public const string AllowFromUri = "ALLOW-FROM";
    }
    
    /// <summary>
    /// XssProtection Constants
    /// </summary>
    public class XssProtectionConstants
    {
        /// <summary>
        /// The X-XSS-Protection header
        /// </summary>
        public const string Header = "X-XSS-Protection";

        /// <summary>
        /// The deny value
        /// </summary>
        public const string Block = "1; mode=block";
    }

    /// <summary>
    /// ContentTypeOptions Constants
    /// </summary>
    public class ContentTypeOptionsConstants
    {
        /// <summary>
        /// The X-Content-Type-Options header
        /// </summary>
        public const string Header = "X-Content-Type-Options";

        /// <summary>
        /// The deny value
        /// </summary>
        public const string NoSniff = "nosniff";
    }

    /// <summary>
    /// StrictTransportSecurity Constants
    /// </summary>
    public class StrictTransportSecurityConstants
    {
        /// <summary>
        /// The Strict-Transport-Security header
        /// </summary>
        public const string Header = "Strict-Transport-Security";

        /// <summary>
        /// The deny value
        /// </summary>
        public const string MaxAge = "max-age=31536000";
    }

    /// <summary>
    /// Server Constants
    /// </summary>
    public class ServerConstants
    {

        /// <summary>
        /// The Server header
        /// </summary>
        public const string Header = "Server";
        
    }

    /// <summary>
    /// Powered By Constants
    /// </summary>
    public class PoweredByConstants
    {

        /// <summary>
        /// The X-Powered-By header
        /// </summary>
        public const string Header = "X-Powered-By";

    }
}
