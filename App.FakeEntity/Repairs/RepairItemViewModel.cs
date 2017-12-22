using App.FakeEntity.Repairs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Repairs
{
    public class RepairItemViewModel
    {
        [Display(Name = "Hạng mục sửa chữa")]
        public string Category
        {
            get;
            set;
        }

        [Display(Name = "Chi phí")]
        public decimal? FixedFee
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string Index
        {
            get;
            set;
        }

        public RepairViewModel Repair
        {
            get;
            set;
        }

        public int RepairId
        {
            get;
            set;
        }

        public DateTime? WarrantyFrom
        {
            get;
            set;
        }

        public DateTime? WarrantyTo
        {
            get;
            set;
        }

        public RepairItemViewModel()
        {
        }
    }
}