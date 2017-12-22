using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
    public class RepairGallery : Entity<int>
    {
        public string ImagePath
        {
            get;
            set;
        }

        [ForeignKey("RepairId")]
        public virtual Repair Repairs
        {
            get;
            set;
        }

        public int RepairId
        {
            get;
            set;
        }

        public RepairGallery()
        {
        }
    }
}