using App.Core.Utils;
using App.Domain.Interfaces.Services;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;

namespace App.Service.Customers
{
    public interface ICustomerService : IBaseService<Customer>, IService
    {
        Customer GetById(int id, bool isCache = true);

        Customer GetByGuid(Guid guid, bool isCache = true);

        Customer InsertGuestCusomter(Guid? customerGuid = null);

        IEnumerable<Customer> PagedList(SortingPagingBuilder sortBuider, Paging page);

        void ResetCheckoutData(Customer customer, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = true, bool clearShippingMethod = true,
            bool clearPaymentMethod = true);

        void UpdateCustomer(Customer customer);

    }
}