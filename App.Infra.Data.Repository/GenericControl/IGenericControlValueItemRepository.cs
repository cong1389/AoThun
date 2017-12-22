using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Repository;
using System;

namespace App.Infra.Data.Repository.GenericControl
{
	public interface IGenericControlValueItemRepository : IRepositoryBase<GenericControlValueItem>
	{
        GenericControlValueItem GetById(int id);
	}
}