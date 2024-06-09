using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;
using static AStar.Utilities.StringExtensions;

namespace AStar.Random.Image.Data;

/// <summary>
///
/// </summary>
public static class FilesContextExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="startingFolder"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> FilterBySearchFolder(this DbSet<FileDetail> files, string startingFolder, bool recursive)
        => startingFolder.IsNullOrWhiteSpace()
                ? []
                : GetFiles(files, startingFolder, recursive);

    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="startingFolder"></param>
    /// <param name="recursive"></param>
    /// <param name="searchType"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> GetMatchingFiles(this DbSet<FileDetail> files, string startingFolder, bool recursive, string searchType)
        => files
                .FilterBySearchFolder(startingFolder, recursive)
                .FilterImagesIfApplicable(searchType);

    private static IEnumerable<FileDetail> GetFiles(DbSet<FileDetail> files, string startingFolder, bool recursive)
        => recursive
                ? files.Where(file => file.DirectoryName.StartsWith(startingFolder))
                : (IEnumerable<FileDetail>)files.Where(file => file.DirectoryName.Equals(startingFolder));
}
