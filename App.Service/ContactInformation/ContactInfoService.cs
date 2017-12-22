using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.ContactInformation;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.ContactInformation
{
    public class ContactInfoService : BaseService<Domain.Entities.GlobalSetting.ContactInformation>, IContactInfoService, IBaseService<Domain.Entities.GlobalSetting.ContactInformation>, IService
    {
        private const string CACHE_CONTACTINFO_KEY = "db.ContactInfo.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IContactInfoRepository _contactInfoRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ContactInfoService(IUnitOfWork unitOfWork, IContactInfoRepository contactInfoRepository
            , ICacheManager cacheManager) : base(unitOfWork, contactInfoRepository)
        {
            this._unitOfWork = unitOfWork;
            this._contactInfoRepository = contactInfoRepository;
            _cacheManager = cacheManager;
        }

        public Domain.Entities.GlobalSetting.ContactInformation GetById(int id, bool isCache = true)
        {
            Domain.Entities.GlobalSetting.ContactInformation contactInformation;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_CONTACTINFO_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                contactInformation = _cacheManager.Get<Domain.Entities.GlobalSetting.ContactInformation>(key);
                if (contactInformation == null)
                {
                    contactInformation = _contactInfoRepository.GetById(id);
                    _cacheManager.Put(key, contactInformation);
                }
            }
            else
            {
                contactInformation = _contactInfoRepository.GetById(id);
            }


            return contactInformation;
        }

        public IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._contactInfoRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int Save()
        {
            return this._unitOfWork.Commit();
        }
    }
}