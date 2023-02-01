using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class CustomerCreateModelValidator
        : AbstractValidator<CustomerCreateModel>
    {
        public CustomerCreateModelValidator()
        {
            #region Generated Constructor
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Hp).NotEmpty();
            RuleFor(p => p.Nik).NotEmpty();
            RuleFor(p => p.Email).NotEmpty();
            #endregion
        }

    }
}
