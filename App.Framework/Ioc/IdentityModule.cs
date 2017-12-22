using App.Domain.Entities.Identity;
using App.Service.Account;
using Autofac;
using Autofac.Builder;
using Microsoft.AspNet.Identity;
using System;

namespace App.Framework.Ioc
{
	public class IdentityModule : Module
	{
		public IdentityModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserManager<IdentityUser, Guid>>().As<UserManager<IdentityUser, Guid>>().InstancePerRequest(new object[0]);
			builder.RegisterType<UserStoreService>().As<IUserStore<IdentityUser, Guid>>().InstancePerRequest(new object[0]);
			builder.RegisterType<RoleManager<IdentityRole, Guid>>().As<RoleManager<IdentityRole, Guid>>().InstancePerRequest(new object[0]);
			builder.RegisterType<RoleStoreService>().As<IRoleStore<IdentityRole, Guid>>().InstancePerRequest(new object[0]);
		}
	}
}