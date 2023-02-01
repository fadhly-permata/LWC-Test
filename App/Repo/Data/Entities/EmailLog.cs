using System;
using System.Collections.Generic;

namespace Repo.Data.Entities
{
    public partial class EmailLog
    {
        public EmailLog()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public long Id { get; set; }

        public long CustomerId { get; set; }

        public long LockerId { get; set; }

        public string Message { get; set; } = null!;

        public long Status { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}
