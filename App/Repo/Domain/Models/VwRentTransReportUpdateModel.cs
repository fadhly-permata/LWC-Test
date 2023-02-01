using System;
using System.Collections.Generic;

namespace Repo.Domain.Models
{
    public partial class VwRentTransReportUpdateModel
    {
        #region Generated Properties
        public long? CustomerId { get; set; }

        public string? Name { get; set; }

        public Byte[]? TotalRentTrans { get; set; }

        public Byte[]? TotalActiveRent { get; set; }

        public Byte[]? TotalReturned { get; set; }

        public Byte[]? TotalPaidFine { get; set; }

        #endregion

    }
}
