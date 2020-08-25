using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace opengeo.Models
{
    public partial class gisContext : DbContext
    {
        public gisContext()
        {
        }

        public gisContext(DbContextOptions<gisContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Basemap> Basemap { get; set; }
        public virtual DbSet<Layer> Layer { get; set; }
        public virtual DbSet<Map> Map { get; set; }

        public virtual DbSet<Image> Image { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MSI;Database=gis;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basemap>(entity =>
            {
                entity.ToTable("basemap");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasColumnName("format")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('image/png')");

                entity.Property(e => e.Layers)
                    .IsRequired()
                    .HasColumnName("layers")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasColumnName("service")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('NONE')");

                entity.Property(e => e.Transparent)
                    .IsRequired()
                    .HasColumnName("transparent")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.ToTable("layer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasColumnName("format")
                    .HasMaxLength(20);

                entity.Property(e => e.Group)
                    .HasColumnName("group")
                    .HasMaxLength(100);

                entity.Property(e => e.GroupNumber).HasColumnName("group_number");

                entity.Property(e => e.IsBasemap).HasColumnName("is_basemap");

                entity.Property(e => e.LayerNumber).HasColumnName("layer_number");

                entity.Property(e => e.Layers)
                    .IsRequired()
                    .HasColumnName("layers")
                    .HasMaxLength(100);

                entity.Property(e => e.MapId).HasColumnName("map_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.Transparent)
                    .IsRequired()
                    .HasColumnName("transparent")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.StylesUrl)
                    .HasColumnName("styles_url")
                    .HasMaxLength(200);

              entity.Property(e => e.LegendUrl)
                  .HasColumnName("legend_url")
                  .HasMaxLength(200);

              entity.Property(e => e.IsWFS)
                  .HasColumnName("is_wfs")
                  .HasDefaultValueSql("((0))");

              entity.Property(e => e.WFSUrl)
                    .HasColumnName("wfs_url")
                    .HasMaxLength(200);

              entity.Property(e => e.Namespace)
                    .HasColumnName("namespace")
                    .HasMaxLength(50);

              entity.Property(e => e.LayerId)
                    .HasColumnName("layer")
                    .HasMaxLength(50);

              entity.Property(e => e.GeometryType)
                    .HasColumnName("geometry_type")
                    .HasMaxLength(20);

              entity.HasOne(d => d.Map)
                    .WithMany(p => p.Layer)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_layer_map");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("map");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BasemapId).HasColumnName("basemap_id");

                entity.Property(e => e.CenterLat).HasColumnName("center_lat");

                entity.Property(e => e.CenterLong).HasColumnName("center_long");

                entity.Property(e => e.Crs)
                    .IsRequired()
                    .HasColumnName("crs")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.Zoom).HasColumnName("zoom");

                entity.HasOne(d => d.Basemap)
                    .WithMany(p => p.Map)
                    .HasForeignKey(d => d.BasemapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_map_basemap");
            });

            modelBuilder.Entity<Image>(entity =>
            {
              entity.ToTable("image");

              entity.Property(e => e.Id).HasColumnName("id");

              entity.Property(e => e.Guid).HasColumnName("guid");

              entity.Property(e => e.Pathname).HasColumnName("pathname");

              entity.Property(e => e.Name).HasColumnName("name");

              entity.Property(e => e.ContentType).HasColumnName("content_type");

              entity.Property(e => e.Content).HasColumnName("content");

              entity.Property(e => e.QgsFid).HasColumnName("qgs_fid");

              entity.Property(e => e.MapName).HasColumnName("map_name");

            });

      OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
