using App.FakeEntity.Language;
using App.Service.Language;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.FakeEntity.Menu
{
	public class MenuLinkViewModel: ILocalizedModel<MenuLinkLocalesViewModel>
    {
		public string CurrentVirtualId
		{
			get;
			set;
		}

        [Display(Name= "DisplayOnHomePage", ResourceType=typeof(Resources.FormUI))]
		public bool DisplayOnHomePage
		{
			get;
			set;
		}

		[Display(Name="DisplayOnMenu", ResourceType=typeof(FormUI))]
		public bool DisplayOnMenu
		{
			get;
			set;
		}

		[Display(Name= "DisplayOnSearch", ResourceType = typeof(FormUI))]
		public bool DisplayOnSearch
		{
			get;
			set;
		}

        [Display(Name = "Icon1", ResourceType = typeof(FormUI))]
        public string Icon1
		{
			get;
			set;
		}

        [Display(Name = "Icon2", ResourceType = typeof(FormUI))]
        public string Icon2
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		[Display(Name= "ImageSelect", ResourceType=typeof(FormUI))]
		public HttpPostedFileBase Image
		{
			get;
			set;
		}

		[Display(Name="Icon1")]
		public HttpPostedFileBase ImageIcon1
		{
			get;
			set;
		}

		[Display(Name="Icon2")]
		public HttpPostedFileBase ImageIcon2
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public string Language
		{
			get;
			set;
		}

		public MenuLinkViewModel MenuLink
		{
			get;
			set;
		}

		[Display(Name="MenuName", ResourceType=typeof(FormUI))]
		public string MenuName
		{
			get;
			set;
		}

		[Display(Name="MetaDescription", ResourceType=typeof(FormUI))]
		public string MetaDescription
		{
			get;
			set;
		}

		[Display(Name="MetaKeywords", ResourceType=typeof(FormUI))]
		public string MetaKeywords
		{
			get;
			set;
		}

		[Display(Name="MetaTitle", ResourceType=typeof(FormUI))]
		public string MetaTitle
		{
			get;
			set;
		}

		[Display(Name="OrderDisplay", ResourceType=typeof(FormUI))]
		public int OrderDisplay
		{
			get;
			set;
		}

		[Display(Name="ParentMenu", ResourceType=typeof(FormUI))]
		public int? ParentId
		{
			get;
			set;
		}

		[Display(Name="Position", ResourceType=typeof(FormUI))]
		public int Position
		{
			get;
			set;
		}

		[Display(Name="SeoUrl", ResourceType=typeof(FormUI))]
		public string SeoUrl
		{
			get;
			set;
		}

		[Display(Name="SourceLink", ResourceType=typeof(FormUI))]
		public string SourceLink
		{
			get;
			set;
		}

		[Display(Name="Status", ResourceType=typeof(FormUI))]
		public int Status
		{
			get;
			set;
		}

		[Display(Name="TemplateType", ResourceType=typeof(FormUI))]
		public int TemplateType
		{
			get;
			set;
		}

		[Display(Name="TypeMenu", ResourceType=typeof(FormUI))]
		public int TypeMenu
		{
			get;
			set;
		}

		public string VirtualId
		{
			get;
			set;
		}

		public string VirtualSeoUrl
		{
			get;
			set;
		}

        public bool Selected
        {
            get; set;
        }
        public ICollection<Domain.Entities.GenericControl.GenericControl> GenericControls { get; set; }
        public IList<MenuLinkLocalesViewModel> Locales { get; set; }

        public MenuLinkViewModel()
		{           
            this.Locales = new List<MenuLinkLocalesViewModel>();
            this.GenericControls = new List<Domain.Entities.GenericControl.GenericControl>();
        }
	}

    public class MenuLinkLocalesViewModel: ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public int LocalesId { get; set; }

        public string CurrentVirtualId
        {
            get;
            set;
        }

        [Display(Name = "DisplayOnHomePage", ResourceType = typeof(FormUI))]
        public bool DisplayOnHomePage
        {
            get;
            set;
        }

        [Display(Name = "DisplayOnMenu", ResourceType = typeof(FormUI))]
        public bool DisplayOnMenu
        {
            get;
            set;
        }

        [Display(Name = "DisplayOnSearch", ResourceType = typeof(FormUI))]
        public bool DisplayOnSearch
        {
            get;
            set;
        }

        [Display(Name = "Icon1", ResourceType = typeof(FormUI))]
        public string Icon1
        {
            get;
            set;
        }

        [Display(Name = "Icon2", ResourceType = typeof(FormUI))]
        public string Icon2
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        [Display(Name = "ImageSelect", ResourceType = typeof(FormUI))]
        public HttpPostedFileBase Image
        {
            get;
            set;
        }

        [Display(Name = "Icon1")]
        public HttpPostedFileBase ImageIcon1
        {
            get;
            set;
        }

        [Display(Name = "Icon2")]
        public HttpPostedFileBase ImageIcon2
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }
        
        [Display(Name = "MenuName", ResourceType = typeof(FormUI))]
        public string MenuName
        {
            get;
            set;
        }

        [Display(Name = "MetaDescription", ResourceType = typeof(FormUI))]
        public string MetaDescription
        {
            get;
            set;
        }

        [Display(Name = "MetaKeywords", ResourceType = typeof(FormUI))]
        public string MetaKeywords
        {
            get;
            set;
        }

        [Display(Name = "MetaTitle", ResourceType = typeof(FormUI))]
        public string MetaTitle
        {
            get;
            set;
        }

        [Display(Name = "OrderDisplay", ResourceType = typeof(FormUI))]
        public int OrderDisplay
        {
            get;
            set;
        }

        [Display(Name = "ParentMenu", ResourceType = typeof(FormUI))]
        public int? ParentId
        {
            get;
            set;
        }

        [Display(Name = "Position", ResourceType = typeof(FormUI))]
        public int Position
        {
            get;
            set;
        }

        [Display(Name = "SeoUrl", ResourceType = typeof(FormUI))]
        public string SeoUrl
        {
            get;
            set;
        }

        [Display(Name = "SourceLink", ResourceType = typeof(FormUI))]
        public string SourceLink
        {
            get;
            set;
        }

        [Display(Name = "Status", ResourceType = typeof(FormUI))]
        public int Status
        {
            get;
            set;
        }

        [Display(Name = "TemplateType", ResourceType = typeof(FormUI))]
        public int TemplateType
        {
            get;
            set;
        }

        [Display(Name = "TypeMenu", ResourceType = typeof(FormUI))]
        public int TypeMenu
        {
            get;
            set;
        }

        public string VirtualId
        {
            get;
            set;
        }

        public string VirtualSeoUrl
        {
            get;
            set;
        }
        public bool Selected
        {
            get; set;
        }
        public ICollection<Domain.Entities.GenericControl.GenericControl> GenericControls { get; set; }
    }
}