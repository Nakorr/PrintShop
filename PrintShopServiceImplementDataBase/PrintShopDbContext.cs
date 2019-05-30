using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PrintShopModel;

namespace PrintShopServiceImplementDataBase
{
   public class PrintShopDbContext : DbContext
    {
        public PrintShopDbContext() : base("AbstractDatabase")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied =
           System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Indent> Indents { get; set; }
        public virtual DbSet<Print> Prints { get; set; }
        public virtual DbSet<PrintIngredient> PrintIngredients { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockIngredient> StockIngredients { get; set; }
        public virtual DbSet<Implementer> Implementers { get; set; }
        public virtual DbSet<MessageInfo> MessageInfos { get; set; }

    }
}
