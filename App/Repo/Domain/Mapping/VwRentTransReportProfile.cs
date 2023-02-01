using System;
using AutoMapper;
using Repo.Data.Entities;
using Repo.Domain.Models;

namespace Repo.Domain.Mapping
{
    public partial class VwRentTransReportProfile
        : AutoMapper.Profile
    {
        public VwRentTransReportProfile()
        {
            CreateMap<Repo.Data.Entities.VwRentTransReport, Repo.Domain.Models.VwRentTransReportReadModel>();

            CreateMap<Repo.Domain.Models.VwRentTransReportCreateModel, Repo.Data.Entities.VwRentTransReport>();

            CreateMap<Repo.Data.Entities.VwRentTransReport, Repo.Domain.Models.VwRentTransReportUpdateModel>();

            CreateMap<Repo.Domain.Models.VwRentTransReportUpdateModel, Repo.Data.Entities.VwRentTransReport>();

            CreateMap<Repo.Domain.Models.VwRentTransReportReadModel, Repo.Domain.Models.VwRentTransReportUpdateModel>();

        }

    }
}
