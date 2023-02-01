using System;
using AutoMapper;
using Repo.Data.Entities;
using Repo.Domain.Models;

namespace Repo.Domain.Mapping
{
    public partial class LockerProfile
        : AutoMapper.Profile
    {
        public LockerProfile()
        {
            CreateMap<Repo.Data.Entities.Locker, Repo.Domain.Models.LockerReadModel>();

            CreateMap<Repo.Domain.Models.LockerCreateModel, Repo.Data.Entities.Locker>();

            CreateMap<Repo.Data.Entities.Locker, Repo.Domain.Models.LockerUpdateModel>();

            CreateMap<Repo.Domain.Models.LockerUpdateModel, Repo.Data.Entities.Locker>();

            CreateMap<Repo.Domain.Models.LockerReadModel, Repo.Domain.Models.LockerUpdateModel>();

        }

    }
}
