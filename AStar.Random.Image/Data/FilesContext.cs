using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace AStar.Random.Image.Data;

/// <summary>
/// The FileContext class/
/// </summary>
public class FilesContext : DbContext
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    public FilesContext(DbContextOptions<FilesContext> options) : base(options) { }

    /// <summary>
    /// The list of files in the dB.
    /// </summary>
    public DbSet<FileDetail> Files { get; set; } = null!;

    /// <summary>
    /// The list of tags to ignore.
    /// </summary>
    public DbSet<TagToIgnore> TagsToIgnore { get; set; } = null!;

    /// <summary>
    /// The list of tags to ignore completely.
    /// </summary>
    public DbSet<TagToIgnoreCompletely> TagsToIgnoreCompletely { get; set; } = null!;

    /// <summary>
    /// The overriden OnModelCreating method.
    /// </summary>
    /// <param name="modelBuilder">
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FileDetail>().HasKey(vf => new { vf.FileName, vf.DirectoryName });
        _ = modelBuilder.Entity<TagToIgnore>().HasKey(tag => tag.Value);
        _ = modelBuilder.Entity<TagToIgnoreCompletely>().HasKey(tag => tag.Value);

        _ = modelBuilder.UseCollation("NOCASE");

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(string)))
        {
            property.SetCollation("NOCASE");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _ = optionsBuilder.UseSqlite("Data Source=c:\\db\\files.db");
        }
    }
}
