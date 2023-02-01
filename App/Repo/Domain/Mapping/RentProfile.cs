using System;
using AutoMapper;
using Repo.Data.Entities;
using Repo.Domain.Models;

namespace Repo.Domain.Mapping
{
    public partial class RentProfile
        : AutoMapper.Profile
    {
        public RentProfile()
        {
            CreateMap<Repo.Data.Entities.Rent, Repo.Domain.Models.RentReadModel>();

            CreateMap<Repo.Domain.Models.RentCreateModel, Repo.Data.Entities.Rent>();

            CreateMap<Repo.Data.Entities.Rent, Repo.Domain.Models.RentUpdateModel>();

            CreateMap<Repo.Domain.Models.RentUpdateModel, Repo.Data.Entities.Rent>();

            CreateMap<Repo.Domain.Models.RentReadModel, Repo.Domain.Models.RentUpdateModel>();

        }

    }
}
