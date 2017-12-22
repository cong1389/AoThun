using App.FakeEntity.Payments;
using FluentValidation;

namespace App.Framework.ValidateEntity
{
    public class PaymentMethodValidator : AbstractValidator<PaymentMethodViewModel>
	{
		public PaymentMethodValidator()
		{
			base.RuleFor<string>((PaymentMethodViewModel x) => x.PaymentMethodSystemName).NotEmpty<PaymentMethodViewModel, string>().WithMessage<PaymentMethodViewModel, string>("Vui lòng chọn hình thức thanh toán.");
            
		}
	}
}