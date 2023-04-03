using Microsoft.EntityFrameworkCore;
using Pronia.Entities;
using Pronia.Entities.PlantModels;
using Pronia.Entities.SliderModels;
using System;

namespace Pronia.DAL
{
    public class ProniaDbContext:DbContext
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(s => s.Key).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
