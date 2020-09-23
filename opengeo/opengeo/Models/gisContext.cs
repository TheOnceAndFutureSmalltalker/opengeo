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
        public virtual DbSet<GeojsonFeature> GeojsonFeature { get; set; }
        public virtual DbSet<GeojsonLayer> GeojsonLayer { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Layer> Layer { get; set; }
        public virtual DbSet<LayerStyles> LayerStyles { get; set; }
        public virtual DbSet<Map> Map { get; set; }

        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MSI;Database=gis;Trusted_Connection=True;", x => x.UseNetTopologySuite());
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
                    .HasMaxLength(100);

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
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<GeojsonFeature>(entity =>
            {
                entity.ToTable("geojson_feature");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GeojsonLayerId).HasColumnName("geojson_layer_id");

                entity.Property(e => e.Geom)
                    .HasColumnName("geom")
                    .HasColumnType("geometry");

                entity.Property(e => e.Properties).HasColumnName("properties");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20);

                entity.HasOne(d => d.GeojsonLayer)
                    .WithMany(p => p.GeojsonFeature)
                    .HasForeignKey(d => d.GeojsonLayerId)
                    .HasConstraintName("FK_geojson_feature_geojson_layer");
            });

      modelBuilder.Entity<User>(entity => {
        entity.ToTable("user");
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);
        entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);
        entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(30);
        entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);
      }
      
      );

            modelBuilder.Entity<GeojsonLayer>(entity =>
            {
                entity.ToTable("geojson_layer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Crs)
                    .HasColumnName("crs")
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.ContentType)
                    .HasColumnName("content_type")
                    .HasMaxLength(10);

                entity.Property(e => e.Guid).HasColumnName("guid");

                entity.Property(e => e.MapName)
                    .HasColumnName("map_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.Pathname)
                    .HasColumnName("pathname")
                    .HasMaxLength(200);

                entity.Property(e => e.QgsFid).HasColumnName("qgs_fid");
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.ToTable("layer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasColumnName("format")
                    .HasMaxLength(20);

                entity.Property(e => e.GeometryType)
                    .HasColumnName("geometry_type")
                    .HasMaxLength(20);

                entity.Property(e => e.Group)
                    .HasColumnName("group")
                    .HasMaxLength(100);

                entity.Property(e => e.GroupNumber).HasColumnName("group_number");

                entity.Property(e => e.IsBasemap).HasColumnName("is_basemap");

                entity.Property(e => e.IsWfs).HasColumnName("is_wfs");

                entity.Property(e => e.Layer1)
                    .HasColumnName("layer")
                    .HasMaxLength(50);

                entity.Property(e => e.LayerNumber).HasColumnName("layer_number");

                entity.Property(e => e.Layers)
                    .IsRequired()
                    .HasColumnName("layers")
                    .HasMaxLength(100);

                entity.Property(e => e.LegendUrl)
                    .HasColumnName("legend_url")
                    .HasMaxLength(200);

                entity.Property(e => e.MapId).HasColumnName("map_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Namespace)
                    .HasColumnName("namespace")
                    .HasMaxLength(50);

                entity.Property(e => e.StylesUrl)
                    .HasColumnName("styles_url")
                    .HasMaxLength(200);

                entity.Property(e => e.Transparent)
                    .IsRequired()
                    .HasColumnName("transparent")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.WfsUrl)
                    .HasColumnName("wfs_url")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Layer)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_layer_map");
            });

            modelBuilder.Entity<LayerStyles>(entity =>
            {
                entity.ToTable("layer_styles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplyRule).HasColumnName("applyRule");

                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasMaxLength(12);

                entity.Property(e => e.IconUrl)
                    .HasColumnName("iconUrl")
                    .HasMaxLength(200);

                entity.Property(e => e.LayerId).HasColumnName("layer_id");

                entity.Property(e => e.Opacity).HasColumnName("opacity");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Layer)
                    .WithMany(p => p.LayerStyles)
                    .HasForeignKey(d => d.LayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_layer_styles_layer");
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
                    .HasMaxLength(100);

                entity.Property(e => e.Zoom).HasColumnName("zoom");

                entity.HasOne(d => d.Basemap)
                    .WithMany(p => p.Map)
                    .HasForeignKey(d => d.BasemapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_map_basemap");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
