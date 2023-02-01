using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class LockerUpdateModelValidator
        : AbstractValidator<LockerUpdateModel>
    {
        public LockerUpdateModelValidator()
        {
            #region Generated Constructor
            RuleFor(p => p.Number).NotEmpty();
            #endregion
        }

    }
}
