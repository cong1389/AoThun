using AutoMapper;
using System;
using System.Runtime.CompilerServices;

namespace App.Framework.Mappings
{
	public class AutoMapperConfiguration
	{
		public AutoMapperConfiguration()
		{
		}

		public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<DomainToViewModelMappingProfile>();
                cfg.AddProfile<ViewModelToDomainMappingProfile>();
            });


            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<DomainToViewModelMappingProfile>();
            //    cfg.AddProfile<ViewModelToDomainMappingProfile>();
            //});

   //         Mapper.Initialize((IMapperConfiguration x) => {
			//	x.AddProfile<DomainToViewModelMappingProfile>();
			//	x.AddProfile<ViewModelToDomainMappingProfile>();
			//});
		}
	}
}