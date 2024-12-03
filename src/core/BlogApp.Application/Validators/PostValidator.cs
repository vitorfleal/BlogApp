using BlogApp.Application.Requests;
using FluentValidation;
using System.Linq.Expressions;

namespace BlogApp.Application.Validators;

public class PostValidator<T> : AbstractValidator<T> where T : PostCreateRequest
{
    public PostValidator()
    {
        RuleRequiredFor(prop => prop.Title, "Title");
        RuleRequiredFor(prop => prop.Content, "Content");
        RuleRequiredFor(prop => prop.Title, "Title");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"Post [{label}] is required.");
    }
}
