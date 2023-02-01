using System;
using System.Collections.Generic;

namespace Repo.Data.Entities
{
    public partial class VwRentTransReport
    {
        public VwRentTransReport()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public long? CustomerId { get; set; }

        public string? Name { get; set; }

        public long TotalRentTrans { get; set; }

        public long TotalActiveRent { get; set; }

        public long TotalReturned { get; set; }

        public long TotalPaidFine { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}
