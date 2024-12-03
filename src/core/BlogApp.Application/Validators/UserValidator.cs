using BlogApp.Application.Requests;
using FluentValidation;
using System.Linq.Expressions;

namespace BlogApp.Application.Validators;

public class UserValidator<T> : AbstractValidator<T> where T : UserCreateRequest
{
    public UserValidator()
    {
        RuleRequiredFor(prop => prop.Name, "Name");
        RuleRequiredFor(prop => prop.Username, "Username");
        RuleRequiredFor(prop => prop.Password, "Password");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"User [{label}] is required.");
    }
}
