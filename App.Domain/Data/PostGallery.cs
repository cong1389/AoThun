using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
    public class PostGallery : Entity<int>
    {      

        //[ForeignKey("PostId")]
        //public virtual Post Post
        //{
        //    get;
        //    set;
        //}

        public int PostId
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int OrderDisplay
        {
            get;
            set;
        }

        public string ImageSmallSize
        {
            get;
            set;
        }
        public string ImageBigSize
        {
            get;
            set;
        }

        public string ImageMediumSize
        {
            get;
            set;
        }

        public PostGallery()
        {
        }
    }
}