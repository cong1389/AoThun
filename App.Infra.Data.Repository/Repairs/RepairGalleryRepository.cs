using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System.Linq;

namespace App.Infra.Data.Repository.Repairs
{
    public class RepairGalleryRepository : RepositoryBase<RepairGallery>, IRepairGalleryRepository, IRepositoryBase<RepairGallery>
	{
		public RepairGalleryRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

        protected override IOrderedQueryable<RepairGallery> GetDefaultOrder(IQueryable<RepairGallery> query)
        {
            IOrderedQueryable<RepairGallery> repairGalleries =
                from p in query
                orderby p.Id
                select p;
            return repairGalleries;
        }
    }
}