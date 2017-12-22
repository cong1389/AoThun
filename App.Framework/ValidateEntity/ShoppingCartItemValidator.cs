using App.FakeEntity.Location;
using App.FakeEntity.Orders;
using App.FakeEntity.Repairs;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
    public class ShoppingCartItemValidator : AbstractValidator<ShoppingCartItemViewModel>
    {
        public ShoppingCartItemValidator()
        {
            base.RuleFor<int?>((ShoppingCartItemViewModel x) => x.Quantity).NotEmpty<ShoppingCartItemViewModel, int?>().WithMessage<ShoppingCartItemViewModel, int?>("Vui lòng nhập số lượng.");
        }
    }
}