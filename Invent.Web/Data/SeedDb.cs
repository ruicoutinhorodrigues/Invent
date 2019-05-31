using Invent.Web.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext context;

        public SeedDb(DataContext context)
        {
            this.context = context;
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            if (!this.context.Suppliers.Any())
            {
                this.AddSupplier("Bicimotor", "Street A", "1000", "Lisbon", "Portugal", "123456789", "bicimotor@gmail.com", "bicimotor.com", "987654321");
                this.AddSupplier("Euromoto", "Street B", "2000", "London", "England", "123456789", "euromoto@gmail.com", "euromoto.com", "987654321");
                this.AddSupplier("Salgados", "Street C", "3000", "Lisbon", "Portugal", "123456789", "salgados@gmail.com", "salgados.com", "987654321");
                this.AddSupplier("Balgarpir", "Street D", "4000", "Lisbon", "Portugal", "123456789", "balgarpir@gmail.com", "balgarpir.com", "987654321");
                this.AddSupplier("Longitude009", "Street E", "5000", "Lisbon", "Portugal", "123456789", "longitude@gmail.com", "longitude.com", "987654321");
                this.AddSupplier("Rodabem", "Street F", "6000", "Lisbon", "Portugal", "123456789", "rodabem@gmail.com", "rodabem.com", "987654321");
                this.AddSupplier("SpeedPlease", "Street H", "7000", "Madrid", "Spain", "123456789", "speedplease@gmail.com", "speedplease.com", "987654321");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Status.Any())
            {
                this.AddStatus("Available");
                this.AddStatus("Backordered");
                this.AddStatus("Damaged");
                this.AddStatus("Discontinued");
                this.AddStatus("Lost/Stolen");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Sizes.Any())
            {
                this.AddSize("Not applicable");
                this.AddSize("XS");
                this.AddSize("S");
                this.AddSize("M");
                this.AddSize("L");
                this.AddSize("XL");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Colors.Any())
            {
                this.AddColor("Not applicable");
                this.AddColor("Blue");
                this.AddColor("Red");
                this.AddColor("White");
                this.AddColor("Black");
                this.AddColor("Green");
                this.AddColor("Grey");
                this.AddColor("Yellow");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Locations.Any())
            {
                this.AddLocation("Warehouse");
                this.AddLocation("Store");
                this.AddLocation("Transit");
                await this.context.SaveChangesAsync();
            }


            if (!this.context.Manufacturer.Any())
            {
                this.AddManufacturer("HJC");
                this.AddManufacturer("Klim");
                this.AddManufacturer("Forma");
                this.AddManufacturer("Richa");
                this.AddManufacturer("Bering");
                this.AddManufacturer("Bering");
                this.AddManufacturer("Motorex");
                this.AddManufacturer("Premier");
                this.AddManufacturer("Rukka");
                this.AddManufacturer("Bering");
                this.AddManufacturer("Givi");
                this.AddManufacturer("Warson");
                this.AddManufacturer("Sena");
                this.AddManufacturer("Interphone");
                this.AddManufacturer("Bering");
                this.AddManufacturer("Radikal");
                this.AddManufacturer("Liquimoly");

                await this.context.SaveChangesAsync();
            }

            if (!this.context.ProductModel.Any())
            {
                this.AddProductModel("CS-15", "HJC");
                this.AddProductModel("IS-17", "HJC");
                this.AddProductModel("IS-MAX II", "HJC");
                this.AddProductModel("IS-33 II", "HJC");
                this.AddProductModel("Badlands Pro", "Klim");
                this.AddProductModel("Kodiak", "Klim");
                this.AddProductModel("Latitude", "Klim");
                this.AddProductModel("Induction", "Klim");
                this.AddProductModel("Drifter", "Klim");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Categories.Any())
            {
                this.AddCategory("Jacket");
                this.AddCategory("Helmet");
                this.AddCategory("Gloves");
                this.AddCategory("Boots");
                this.AddCategory("Oils/lubricants/additives");
                this.AddCategory("Communication devices");
                this.AddCategory("Locks");
                this.AddCategory("Parts and accessories");
                await this.context.SaveChangesAsync();
            }
        }

        private void AddCategory(string name)
        {
            this.context.Categories.Add(new Category
            {
                Name = name,
                DateOfCreation = DateTime.Now
            });
        }

        private void AddManufacturer(string name)
        {
            this.context.Manufacturer.Add(new Manufacturer
            {
                Name = name,
                DateOfCreation = DateTime.Now
            });
        }

        private void AddProductModel(string name, string manufacturer)
        {
            this.context.ProductModel.Add(new ProductModel
            {
                Name = name,
                ManufacturerId = context.Manufacturer.FirstOrDefault(m => m.Name == manufacturer).Id,
                DateOfCreation = DateTime.Now
            });
        }

        private void AddLocation(string name)
        {
            this.context.Locations.Add(new Location
            {
                Name = name
            });
        }

        private void AddStatus(string name)
        {
            this.context.Status.Add(new Status
            {
                Name = name
            });
        }

        private void AddSize(string name)
        {
            this.context.Sizes.Add(new Size
            {
                Name = name
            });
        }

        private void AddColor(string name)
        {
            this.context.Colors.Add(new Color
            {
                Name = name
            });
        }

        private void AddSupplier(string name, string address, string postCode, string town, string country, string phone, string email, string website, string nipc)
        {
            this.context.Suppliers.Add(new Supplier
            {
                Name = name,
                Address = address,
                PostCode = postCode,
                Town = town,
                Country = country,
                Phone = phone,
                Email = email,
                Website = website,
                NIPC = nipc,
                DateOfCreation = DateTime.Now
            });
        }
    }
}
