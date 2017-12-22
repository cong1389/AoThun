using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using Domain.Entities.Customers;

namespace App.Service.Common
{
    public interface IWorkContext 
    {
        Customer CurrentCustomer { get; set; }

        App.Domain.Entities.Language.Language WorkingLanguage { get; set; }
    }
}