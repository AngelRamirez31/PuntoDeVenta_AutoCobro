using CsvHelper;
using Microsoft.EntityFrameworkCore;
using PuntoDeVentaAutoCobroFinal.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PuntoDeVentaAutoCobroFinal.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                
                context.Database.EnsureCreated();

                
                if (context.Productos.Any())
                {
                    return;
                }

                var csvFilePath = Path.Combine(AppContext.BaseDirectory, "catalogo_productos_mx.csv");

                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>();
                    foreach (var record in records)
                    {
                        
                        
                        
                        var recordDict = (IDictionary<string, object>)record;

                        var product = new Product
                        {
                            Marca = (string)recordDict["Marca"],
                            Nombre = (string)recordDict["Nombre"],
                            Categoria = (string)recordDict["Categoría"],
                            
                            CodigoDeBarras = long.Parse((string)recordDict["Código de Barras"]),
                            Precio = ParsePrice((string)recordDict["Precio"])
                        };
                        

                        context.Productos.Add(product);
                    }
                }
                context.SaveChanges();
            }
        }

        private static decimal ParsePrice(string priceStr)
        {
            var match = Regex.Match(priceStr, @"\d+\.?\d*");
            if (match.Success)
            {
                if (decimal.TryParse(match.Value, CultureInfo.InvariantCulture, out decimal price))
                {
                    return price;
                }
            }
            return 0;
        }
    }
}