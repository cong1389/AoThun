using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.GenericControl
{
    public interface IGenericControlValueItemService : IBaseService<GenericControlValueItem>, IService
	{
		GenericControlValueItem GetById(int id, bool isCache = true);

        IEnumerable<GenericControlValueItem> GetByOption(int? genericControlValueId = null
           , int? entity = null
           , int status = 1, bool isCache = true);

    }
}