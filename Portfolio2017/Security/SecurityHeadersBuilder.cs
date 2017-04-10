namespace Portfolio.API.Security
{
    /// <summary>
    /// Security Headers Builder
    /// </summary>
    public class SecurityHeadersBuilder
    {
        private readonly SecurityHeadersPolicy _policy = new SecurityHeadersPolicy();

        /// <summary>
        /// Adds the default secure policy.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddDefaultSecurePolicy()
        {
            AddFrameOptionsDeny();
            AddXssProtectionBlock();
            AddContentTypeOptionsNoSniff();
            AddStrictTransportSecurityMaxAge();
            RemovePoweredByHeader();
            RemoveServerHeader();

            return this;
        }

        #region FrameOptions
        
        /// <summary>
        /// Adds the frame options deny.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddFrameOptionsDeny()
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.Deny;
            return this;
        }

        /// <summary>
        /// Adds the frame options same origin.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddFrameOptionsSameOrigin()
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.SameOrigin;
            return this;
        }

        /// <summary>
        /// Adds the frame options same origin.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public SecurityHeadersBuilder AddFrameOptionsSameOrigin(string uri)
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = string.Format(FrameOptionsConstants.AllowFromUri, uri);
            return this;
        }

        #endregion

        #region XssProtection

        /// <summary>
        /// Adds the XSS protection block.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddXssProtectionBlock()
        {
            _policy.SetHeaders[XssProtectionConstants.Header] = XssProtectionConstants.Block;
            return this;
        }

        #endregion

        #region ContentTypeOptions

        /// <summary>
        /// Adds the content type options no sniff.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddContentTypeOptionsNoSniff()
        {
            _policy.SetHeaders[ContentTypeOptionsConstants.Header] = ContentTypeOptionsConstants.NoSniff;
            return this;
        }

        #endregion

        #region StrictTransportSecurity

        /// <summary>
        /// Adds the strict transport security maximum age.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddStrictTransportSecurityMaxAge()
        {
            _policy.SetHeaders[StrictTransportSecurityConstants.Header] = StrictTransportSecurityConstants.MaxAge;
            return this;
        }

        #endregion

        /// <summary>
        /// Removes the ASP net header.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder RemovePoweredByHeader()
        {
            _policy.RemoveHeaders.Add(PoweredByConstants.Header);
            return this;
        }

        /// <summary>
        /// Removes the server header.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder RemoveServerHeader()
        {
            _policy.RemoveHeaders.Add(ServerConstants.Header);
            return this;
        }

        /// <summary>
        /// Adds the custom header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SecurityHeadersBuilder AddCustomHeader(string header, string value)
        {
            _policy.SetHeaders[header] = value;
            return this;
        }

        /// <summary>
        /// Removes the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public SecurityHeadersBuilder RemoveHeader(string header)
        {
            _policy.RemoveHeaders.Add(header);
            return this;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersPolicy Build()
        {
            return _policy;
        }
    }
}
