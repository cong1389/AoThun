using System;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Repairs
{
    public class RepairGalleryViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public int RepairId
        {
            get;
            set;
        }

        public RepairGalleryViewModel()
        {
        }
    }
}