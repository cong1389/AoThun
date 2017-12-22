using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.User
{
	public class LoginViewModel
	{
		[Display(Name="Password", ResourceType=typeof(FormUI))]
		public string Password
		{
			get;
			set;
		}

		[Display(Name= "Remember", ResourceType=typeof(FormUI))] 
		public bool Remember   
		{
			get;
			set;
		}

		[Display(Name="UserName", ResourceType=typeof(FormUI))]
		public string UserName
		{
			get;
			set;
		}

		public LoginViewModel()
		{
		}
	}
}