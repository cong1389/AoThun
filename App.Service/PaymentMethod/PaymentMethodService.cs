using App.Core.Caching;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Entities.Payments;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.PaymentMethodes;
using App.Infra.Data.UOW.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace App.Service.PaymentMethodes
{
    public class PaymentMethodService : BaseService<PaymentMethod>, IPaymentMethodService, IBaseService<PaymentMethod>, IService
    {
        private const string CACHE_PAYMENTMETHOD_KEY = "db.PaymentMethod.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IPaymentMethodRepository _paymentMethodRepository;

        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodService(IUnitOfWork unitOfWork, IPaymentMethodRepository paymentMethodRepository
            , ICacheManager cacheManager) : base(unitOfWork, paymentMethodRepository)
        {
            this._unitOfWork = unitOfWork;
            this._paymentMethodRepository = paymentMethodRepository;
            _cacheManager = cacheManager;
        }

        public PaymentMethod GetById(int id)
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_PAYMENTMETHOD_KEY, "GetBySeoUrl");
            sbKey.AppendFormat("-{0}", id);

            string key = sbKey.ToString();
            PaymentMethod paymentMethod = _cacheManager.Get<PaymentMethod>(key);
            if (paymentMethod == null)
            {
                paymentMethod = _paymentMethodRepository.GetById(id);
                _cacheManager.Put(key, paymentMethod);
            }

            return paymentMethod;
        }
        public PaymentMethod GetBySystemName(string systemName)
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_PAYMENTMETHOD_KEY, "GetBySystemName");

            if (systemName.HasValue())
                sbKey.AppendFormat("-{0}", systemName);

            string key = sbKey.ToString();
            PaymentMethod paymentMethod = _cacheManager.Get<PaymentMethod>(key);
            if (paymentMethod == null)
            {
                paymentMethod = _paymentMethodRepository.Get((PaymentMethod x) => x.PaymentMethodSystemName == systemName);
                _cacheManager.Put(key, paymentMethod);
            }

            return paymentMethod;
        }

        public IEnumerable<PaymentMethod> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._paymentMethodRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}