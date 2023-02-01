using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class EmailLogUpdateModelValidator
        : AbstractValidator<EmailLogUpdateModel>
    {
        public EmailLogUpdateModelValidator()
        {
            #region Generated Constructor
            RuleFor(p => p.Message).NotEmpty();
            #endregion
        }

    }
}
