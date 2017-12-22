using App.FakeEntity.Repairs;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
    public class RepairValidator : AbstractValidator<RepairViewModel>
    {
        public RepairValidator()
        {
            base.RuleFor<string>((RepairViewModel x) => x.CustomerName).NotEmpty().WithMessage<RepairViewModel, string>("Vui lòng nhập tên bạn.");
            base.RuleFor<int>((RepairViewModel x) => x.BrandId).NotEmpty().WithMessage("Vui lòng chọn thương hiệu.");
            base.RuleFor<string>((RepairViewModel x) => x.PhoneNumber).NotEmpty().WithMessage<RepairViewModel, string>("Vui lòng nhập số điện thoại.");
            base.RuleFor<string>((RepairViewModel x) => x.ModelBrand).NotEmpty().WithMessage<RepairViewModel, string>("Vui lòng nhập dòng máy.");
        }
    }
}