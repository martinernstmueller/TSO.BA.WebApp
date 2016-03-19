using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace TSO.BA.WebApp.Models
{
    public interface IFloorContext
    {
        DbSet<Floor> Floors { get; set; }
        DbSet<BuildingState> BuildingStates { get; set; }
        IConfigurationRoot Configuration { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }

    public class FloorContext : DbContext, IFloorContext
    {
        public DbSet<Floor> Floors { get; set; }
        public DbSet<BuildingState> BuildingStates { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public FloorContext(IHostingEnvironment argEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{argEnv.EnvironmentName}.json", optional: true);

            if (argEnv.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder argBuilder)
        {
            argBuilder.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
        }
    }
}
