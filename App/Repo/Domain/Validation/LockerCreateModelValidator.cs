using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class LockerCreateModelValidator
        : AbstractValidator<LockerCreateModel>
    {
        public LockerCreateModelValidator()
        {
            #region Generated Constructor
            RuleFor(p => p.Number).NotEmpty();
            #endregion
        }

    }
}
