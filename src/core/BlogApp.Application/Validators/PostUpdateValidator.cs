using BlogApp.Application.Requests;
using FluentValidation;

namespace BlogApp.Application.Validators;

public class PostUpdateValidator : PostValidator<PostUpdateRequest>
{
    public PostUpdateValidator()
    {
        RuleFor(prop => prop.Id)
            .Must(guid => guid != Guid.Empty).WithMessage("Post [Id] must not be empty.")
            .NotEmpty().WithMessage("Post [Id] is required.");
    }
}
