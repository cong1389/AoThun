using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.GenericControl
{
	public class GenericControlValueItemRepository : RepositoryBase<GenericControlValueItem>, IGenericControlValueItemRepository, IRepositoryBase<GenericControlValueItem>
	{
		public GenericControlValueItemRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<GenericControlValueItem> GetDefaultOrder(IQueryable<GenericControlValueItem> query)
		{
			IOrderedQueryable<GenericControlValueItem> genericControlValueItem = 
				from p in query
				orderby p.Id
				select p;

			return genericControlValueItem;
		}

		public GenericControlValueItem GetById(int id)
		{
			GenericControlValueItem GenericControlValueItem = this.FindBy((GenericControlValueItem x) => x.Id == id, false).FirstOrDefault<GenericControlValueItem>();
			return GenericControlValueItem;
		}
	}
}