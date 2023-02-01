using System;
using AutoMapper;
using Repo.Data.Entities;
using Repo.Domain.Models;

namespace Repo.Domain.Mapping
{
    public partial class CustomerProfile
        : AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Repo.Data.Entities.Customer, Repo.Domain.Models.CustomerReadModel>();

            CreateMap<Repo.Domain.Models.CustomerCreateModel, Repo.Data.Entities.Customer>();

            CreateMap<Repo.Data.Entities.Customer, Repo.Domain.Models.CustomerUpdateModel>();

            CreateMap<Repo.Domain.Models.CustomerUpdateModel, Repo.Data.Entities.Customer>();

            CreateMap<Repo.Domain.Models.CustomerReadModel, Repo.Domain.Models.CustomerUpdateModel>();

        }

    }
}
