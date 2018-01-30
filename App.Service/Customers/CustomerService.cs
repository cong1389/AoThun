using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Customers;
using App.Infra.Data.UOW.Interfaces;
using App.Service.GenericAttribute;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Customers
{
    public class CustomerService : BaseService<Customer>, ICustomerService, IBaseService<Customer>, IService
    {
        private const string CACHE_CUSTOMER_KEY = "db.Customer.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly ICustomerRepository _customerRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericAttributeService _genericAttributeService;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository
            , IGenericAttributeService genericAttributeService
            , ICacheManager cacheManager) : base(unitOfWork, customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _genericAttributeService = genericAttributeService;
            _cacheManager = cacheManager;
        }

        public Customer GetById(int id, bool isCache = true)
        {
            Customer customer;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_CUSTOMER_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                customer = _cacheManager.Get<Customer>(key);
                if (customer == null)
                {
                    customer = _customerRepository.GetById(id);
                    _cacheManager.Put(key, customer);
                }
            }
            else
            {
                customer = _customerRepository.GetById(id);
            }

            return customer;
        }

        public Customer GetByGuid(Guid guid, bool isCache = true)
        {
            Customer customer;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_CUSTOMER_KEY, "GetByGuid");
                sbKey.Append(guid);

                string key = sbKey.ToString();
                customer = _cacheManager.Get<Customer>(key);
                if (customer == null)
                {
                    customer = _customerRepository.Get((Customer x) => x.CustomerGuid == guid, false);
                    _cacheManager.Put(key, customer);
                }
            }
            else
            {
                customer = _customerRepository.Get((Customer x) => x.CustomerGuid == guid, false);
            }

            //StringBuilder sbKey = new StringBuilder();
            //sbKey.AppendFormat(CACHE_CUSTOMER_KEY, "GetByGuid");
            //sbKey.Append(guid);

            //string key = sbKey.ToString();
            //Customer customer = _cacheManager.Get<Customer>(key);
            //if (customer == null)
            //{
            //    customer = _customerRepository.Get((Customer x) => x.CustomerGuid == guid, false);
            //    _cacheManager.Put(key, customer);
            //}

            return customer;
        }

        public Customer InsertGuestCusomter(Guid? customerGuid = null)
        {
            var customer = new Customer
            {
                CustomerGuid = customerGuid ?? Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            return customer = _customerRepository.Add(customer);
        }

        public IEnumerable<Customer> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._customerRepository.PagedSearchList(sortbuBuilder, page);
        }


        public virtual void ResetCheckoutData(Customer customer, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = true, bool clearShippingMethod = true,
            bool clearPaymentMethod = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (clearShippingMethod)
            {
                //_genericAttributeService.SaveGenericAttribute(customer.Id,"Customer", Contains.SelectedShippingOption,null);
            }

            if (clearPaymentMethod)
            {
                //_genericAttributeService.SaveGenericAttribute(customer.Id, "Customer", Contains.SelectedPaymentMethod, null);
            }

            Update(customer);
        }

        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);
        }

    }
}