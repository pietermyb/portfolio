using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portfolio.API.Filters
{
    /// <summary>
    /// Validate Model Attribute
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
