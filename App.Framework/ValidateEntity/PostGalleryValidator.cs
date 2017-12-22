using App.FakeEntity.Repairs;
using App.FakeEntity.Post;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
    public class PostGalleryValidator : AbstractValidator<PostGalleryViewModel>
    {
        public PostGalleryValidator()
        {
            base.RuleFor<string>((PostGalleryViewModel x) => x.Title).NotEmpty<PostGalleryViewModel, string>().WithMessage<PostGalleryViewModel, string>("Vui lòng nhập Tên.");            
        }
    }
}