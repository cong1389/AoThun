using App.Domain.Common;
using App.Domain.Entities.Menu;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace App.Infra.Data.Mapping
{
    public class AddressConfiguration : EntityTypeConfiguration<Address>
	{
		public AddressConfiguration()
		{
			base.ToTable("Address");

			base.HasKey<int>((Address x) => x.Id).Property<int>((Address x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            //base.HasMany<Customer>((Address x) => x.Customers)
            //  .WithMany((Customer x) => x.Addresses)
            //  .Map((ManyToManyAssociationMappingConfiguration x) =>
            //  {
            //      x.ToTable("CustomerAddresses");
            //      x.MapLeftKey(new string[] { "Address_Id" });
            //      x.MapRightKey(new string[] { "Customer_Id" });
            //  });

        }
	}
}