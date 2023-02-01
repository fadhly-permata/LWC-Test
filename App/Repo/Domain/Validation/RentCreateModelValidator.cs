using System;
using FluentValidation;
using Repo.Domain.Models;

namespace Repo.Domain.Validation
{
    public partial class RentCreateModelValidator
        : AbstractValidator<RentCreateModel>
    {
        public RentCreateModelValidator()
        {
            #region Generated Constructor
            #endregion
        }

    }
}
