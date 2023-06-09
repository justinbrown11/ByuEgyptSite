using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ByuEgyptSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ByuEgyptSite.Data
{
    public class ApplicationDbContext : IdentityDbContext // Inherit from AspNetCore Identity for authentication
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Declare db sets for all models (tables)
        public DbSet<Burial> Burials { get; set; }
        public DbSet<Textile> Textiles { get; set; }
        public DbSet<BurialTextile> BurialTextiles { get; set;}
        public DbSet<Structure> Structures { get; set; }
        public DbSet<StructureTextile> StructureTextiles { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ColorTextile> ColorTextiles { get; set; }
        public DbSet<TextileFunction> TextileFunctions { get; set; }
        public DbSet<TextileFunctionTextile> TextileFunctionTextiles { get; set; }
        public DbSet<YarnManipulation> YarnManipulation { get; set;}
        public DbSet<YarnManipulationTextile> YarnManipulationTextiles { get;set; }
        public DbSet<Dimension> Dimensions { get; set; }
        public DbSet<DimensionTextile> DimensionTextiles { get; set; }
        public DbSet<Decoration> Decorations { get; set; }
        public DbSet<DecorationTextile> DecorationTextiles { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<AnalysisTextile> AnalysisTextiles { get; set; }
        public DbSet<PhotoData> PhotoData { get; set; }
        public DbSet<PhotoDataTextile> PhotoDataTextiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Set key relationships for association tables/models (many to many relationships)
            builder.Entity<BurialTextile>()
                .HasKey(at => new { at.TextileId, at.BurialId });
            builder.Entity<StructureTextile>()
                .HasKey(at => new { at.TextileId, at.StructureId });
            builder.Entity<ColorTextile>()
                .HasKey(at => new { at.TextileId, at.ColorId });
            builder.Entity<TextileFunctionTextile>()
                .HasKey(at => new { at.TextileId, at.TextileFunctionId });
            builder.Entity<YarnManipulationTextile>()
                .HasKey(at => new { at.TextileId, at.YarnManipulationId });
            builder.Entity<DimensionTextile>()
                .HasKey(at => new { at.TextileId, at.DimensionId });
            builder.Entity<DecorationTextile>()
                .HasKey(at => new { at.TextileId, at.DecorationId });
            builder.Entity<AnalysisTextile>()
                .HasKey(at => new { at.TextileId, at.AnalysisId });
            builder.Entity<PhotoDataTextile>()
                .HasKey(at => new { at.TextileId, at.PhotoDataId });

            // Set foreign key relationship for body analysis and burials
            builder.Entity<BodyAnalysis>()
                .HasOne(d => d.Burial)
                .WithMany(p => p.bodyAnalyses)
                .HasPrincipalKey(p => p.burialmainid)
                .HasForeignKey(d => d.burialmainid)
                .HasConstraintName("burialmainfk");


            base.OnModelCreating(builder);
        }
    }
}