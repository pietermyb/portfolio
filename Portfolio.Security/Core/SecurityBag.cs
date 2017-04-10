using System.ComponentModel;

namespace Portfolio.Security.Core
{
    public enum ReferrerPolicy
    {
        [Description("")]
        none = 0,
        [Description("no-referrer")]
        no_referrer,
        [Description("no-referrer-when-downgrade")]
        no_referrer_when_downgrade,
        [Description("same-origin")]
        same_origin,
        [Description("origin")]
        origin,
        [Description("strict-origin")]
        strict_origin,
        [Description("origin-when-cross-origin")]
        origin_when_cross_origin,
        [Description("strict-origin-when-cross-origin")]
        strict_origin_when_cross_origin,
        [Description("unsafe-url")]
        unsafe_url
    }
    
}
