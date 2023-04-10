using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Entities;
using Pronia.Entities.PlantModels;
using Pronia.Entities.SliderModels;
using Pronia.Entities.UserModels;
using System;
using Pronia.ViewModels;

namespace Pronia.DAL
{
    public class ProniaDbContext:IdentityDbContext<User>
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options):base(options)
        {
            
        }

        //SliderModel
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<LeftIcon> LeftIcons { get; set; }
        public DbSet<RightIcon> RightIcons { get; set; }

        //PlantModel
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlantDeliveryInfo> PlantDeliveryInfos { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<PlantSizeColor> PlantSizeColors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(s => s.Key).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c=>c.Name).IsUnique();
            modelBuilder.Entity<Tag>().HasIndex(t=>t.Name).IsUnique();
            base.OnModelCreating(modelBuilder);
        }

    }
}
