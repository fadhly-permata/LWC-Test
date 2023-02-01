using System;
using System.Collections.Generic;

namespace Repo.Data.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Hp { get; set; } = null!;

        public string Nik { get; set; } = null!;

        public string Email { get; set; } = null!;

        #endregion

        #region Generated Relationships
        #endregion

    }
}
