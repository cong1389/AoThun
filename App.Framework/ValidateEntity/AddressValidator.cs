using App.FakeEntity.Common;
using FluentValidation;

namespace App.Framework.ValidateEntity
{
    public class AddressValidator : AbstractValidator<AddressViewModel>
	{
		public AddressValidator()
		{
			base.RuleFor<string>((AddressViewModel x) => x.FirstName).NotEmpty<AddressViewModel, string>().WithMessage<AddressViewModel, string>("Vui lòng nhập họ tên.");
			base.RuleFor<string>((AddressViewModel x) => x.Address1).NotEmpty<AddressViewModel, string>().WithMessage<AddressViewModel, string>("Vui lòng nhập địa chỉ.");
            base.RuleFor<string>((AddressViewModel x) => x.PhoneNumber).NotEmpty<AddressViewModel, string>().WithMessage<AddressViewModel, string>("Vui lòng nhập số điện thoại.");
        }
	}
}