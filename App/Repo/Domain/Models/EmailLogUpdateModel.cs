using System;
using System.Collections.Generic;

namespace Repo.Domain.Models
{
    public partial class EmailLogUpdateModel
    {
        #region Generated Properties
        public long Id { get; set; }

        public long CustomerId { get; set; }

        public long LockerId { get; set; }

        public string Message { get; set; } = null!;

        public long Status { get; set; }

        #endregion

    }
}
