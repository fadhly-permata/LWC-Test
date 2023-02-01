using System;
using System.Collections.Generic;

namespace Repo.Data.Entities
{
    public partial class Rent
    {
        public Rent()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public long Id { get; set; }

        public long? CustomerId { get; set; }

        public long? LockerId { get; set; }

        public string? RentDate { get; set; }

        public string? ReturnDate { get; set; }

        public string? Password { get; set; }

        public long? Status { get; set; }

        public long? TotalFine { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}
