using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Api.Model;

namespace Api.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("WebApp.Model.Estate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Area");

                    b.Property<int>("LocationId");

                    b.Property<int>("Price");

                    b.Property<string>("Title");

                    b.Property<int>("TotalSurface");

                    b.Property<int>("UsableSurface");

                    b.HasKey("Id");

                    b.ToTable("Estates");
                });

            modelBuilder.Entity("WebApp.Model.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EstateId");

                    b.Property<string>("Link");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("EstateId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("WebApp.Model.Image", b =>
                {
                    b.HasOne("WebApp.Model.Estate")
                        .WithMany("Images")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
