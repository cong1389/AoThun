using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Repairs;
using App.Infra.Data.UOW.Interfaces;
using App.Service.Repair;

namespace App.Service.Repairs
{
    public class RepairGalleryService : BaseService<RepairGallery>, IRepairGalleryService, IBaseService<RepairGallery>, IService
	{
		private readonly IRepairGalleryRepository _galleryRepository;

		private readonly IUnitOfWork _unitOfWork;

		public RepairGalleryService(IUnitOfWork unitOfWork, IRepairGalleryRepository galleryRepository) : base(unitOfWork, galleryRepository)
		{
			this._unitOfWork = unitOfWork;
			this._galleryRepository = galleryRepository;
		}
	}
}