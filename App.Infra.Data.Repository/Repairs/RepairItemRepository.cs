using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using App.Infra.Data.Repository.Repairs;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Repairs
{
    public class RepairItemRepository : RepositoryBase<RepairItem>, IRepairItemRepository, IRepositoryBase<RepairItem>
    {
        public RepairItemRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        protected override IOrderedQueryable<RepairItem> GetDefaultOrder(IQueryable<RepairItem> query)
        {
            IOrderedQueryable<RepairItem> orderItems =
                from p in query
                orderby p.Id
                select p;
            return orderItems;
        }
    }
}