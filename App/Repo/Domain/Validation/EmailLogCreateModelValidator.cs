using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class EmailLogCreateModelValidator
        : AbstractValidator<EmailLogCreateModel>
    {
        public EmailLogCreateModelValidator()
        {
            #region Generated Constructor
            RuleFor(p => p.Message).NotEmpty();
            #endregion
        }

    }
}
