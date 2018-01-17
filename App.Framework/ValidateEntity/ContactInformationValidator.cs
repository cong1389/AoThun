using App.FakeEntity.ContactInformation;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace App.Framework.ValidateEntity
{
    public class ContactInformationValidator : AbstractValidator<ContactInformationViewModel>
	{
		public ContactInformationValidator()
		{
            RuleFor((ContactInformationViewModel x) => x.Title).NotEmpty().WithMessage("Vui lòng nhập tiêu đề.");
            RuleFor((ContactInformationViewModel x) => x.Address).NotEmpty().WithMessage("Vui lòng nhập địa chỉ.");
            RuleFor((ContactInformationViewModel x) => x.Email).Must(new Func<string, bool>(IsValidEmail)).WithMessage("Email không đúng định dạng");
            RuleFor((ContactInformationViewModel x) => x.OrderDisplay).NotEmpty().WithMessage("Vui lòng nhập vị trí hiển thị.");
            RuleFor((ContactInformationViewModel x) => x.OrderDisplay).GreaterThanOrEqualTo(0).WithMessage("Vị trí hiển thị phải là số và lớn hơn 0.");
		}

		public static bool IsValidEmail(string email)
		{
			bool flag;
			flag = ((new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")).IsMatch(email) ? true : false);
			return flag;
		}
	}
}