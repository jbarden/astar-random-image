using AStar.Web.Domain;

namespace AStar.Random.Image;

/// <summary>
///
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="searchType"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> FilterImagesIfApplicable(this IEnumerable<FileDetail> files, string searchType)
        => searchType != "Images" ? files : files.Where(file => file.IsImage2);

    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public static int GroupDuplicates(this IEnumerable<FileDetail> files)
    {
        var duplicatesBySize = files.AsEnumerable()
                .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

        return duplicatesBySize.Length;
    }
}
