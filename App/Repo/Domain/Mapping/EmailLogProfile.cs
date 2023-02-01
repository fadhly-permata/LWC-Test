using System;
using AutoMapper;
using Repo.Data.Entities;
using Repo.Domain.Models;

namespace Repo.Domain.Mapping
{
    public partial class EmailLogProfile
        : AutoMapper.Profile
    {
        public EmailLogProfile()
        {
            CreateMap<Repo.Data.Entities.EmailLog, Repo.Domain.Models.EmailLogReadModel>();

            CreateMap<Repo.Domain.Models.EmailLogCreateModel, Repo.Data.Entities.EmailLog>();

            CreateMap<Repo.Data.Entities.EmailLog, Repo.Domain.Models.EmailLogUpdateModel>();

            CreateMap<Repo.Domain.Models.EmailLogUpdateModel, Repo.Data.Entities.EmailLog>();

            CreateMap<Repo.Domain.Models.EmailLogReadModel, Repo.Domain.Models.EmailLogUpdateModel>();

        }

    }
}
