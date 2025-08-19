
using FluentValidation;

namespace Music.API.Api.EndpointFilters;

public class ValidationFilter<TDto> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<TDto>().FirstOrDefault();
        if (model is null)
            return Results.BadRequest();

        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<TDto>>();
        var result = validator.Validate(model);
        if (!result.IsValid)
        {
            var errors = ValidationErrorBuilder(result.Errors);
            return Results.BadRequest(errors);
        }
        return await next(context);
    }
    private Dictionary<string, string> ValidationErrorBuilder(List<FluentValidation.Results.ValidationFailure> errors)
    {
        var dict = new Dictionary<string, string>();
        errors.ForEach(e => dict.Add(e.PropertyName, e.ErrorMessage));
        return dict;
    }
}
