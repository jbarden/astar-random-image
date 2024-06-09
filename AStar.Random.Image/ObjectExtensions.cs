using System.Text.Json;

namespace AStar.Utilities;

/// <summary>
/// The <see cref="ObjectExtensions" /> class contains some useful methods to enable various tasks
/// to be performed in a more fluid, English sentence, style.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// The ToJson method, as you might expect, converts the supplied object to its JSON equivalent.
    /// </summary>
    /// <param name="object">The object to convert to JSON.</param>
    /// <returns>The JSON string of the object supplied.</returns>
    public static string ToJson<T>(this T @object) => JsonSerializer.Serialize(@object);
}
