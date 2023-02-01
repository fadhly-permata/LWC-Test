using System;
using System.Collections.Generic;

namespace Repo.Domain.Models
{
    public partial class VwRentTransReportReadModel
    {
        #region Generated Properties
        public long? CustomerId { get; set; }

        public string? Name { get; set; }

        public long? TotalRentTrans { get; set; }

        public long? TotalActiveRent { get; set; }

        public long? TotalReturned { get; set; }

        public long? TotalPaidFine { get; set; }

        #endregion

    }
}
