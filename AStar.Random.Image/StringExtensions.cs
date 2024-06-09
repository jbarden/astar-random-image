using System.Text.Json;

namespace AStar.Utilities;

/// <summary>
/// The <see cref="StringExtensions" /> class contains some useful methods to enable checks to be
/// performed in a more fluid, English sentence, style.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// The IsNull method, as you might expect, checks whether the string is, in fact, null.
    /// </summary>
    /// <param name="value">The string to check for being null.</param>
    /// <returns>True if the string is null, False otherwise.</returns>
    public static bool IsNull(this string? value) => value is null;

    /// <summary>
    /// The IsNotNull method, as you might expect, checks whether the string is not null.
    /// </summary>
    /// <param name="value">The string to check for being not null.</param>
    /// <returns>True if the string is not null, False otherwise.</returns>
    public static bool IsNotNull(this string? value) => !value.IsNull();

    /// <summary>
    /// The IsNullOrWhiteSpace method, as you might expect, checks whether the string is, in fact, null, empty or whitespace.
    /// </summary>
    /// <param name="value">The string to check for being null, empty or whitespace.</param>
    /// <returns>True if the string is null, empty or whitespace, False otherwise.</returns>
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// The IsNotNullOrWhiteSpace method, as you might expect, checks whether the string is not null, empty or whitespace.
    /// </summary>
    /// <param name="value">The string to check for being not null, empty or whitespace.</param>
    /// <returns>True if the string is not null, empty or whitespace, False otherwise.</returns>
    public static bool IsNotNullOrWhiteSpace(this string? value) => !value.IsNullOrWhiteSpace();

    /// <summary>
    /// The FromJson method, as you might expect, converts the supplied JSON to the specified object.
    /// </summary>
    /// <typeparam name="T">The required type of the object to deserialise to.</typeparam>
    /// <param name="json">The JSON representation of the object.</param>
    /// <returns>A deserialised object based on the original JSON.</returns>
    public static T FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json)!;

    /// <summary>
    /// The FromJson method, as you might expect, converts the supplied JSON to the specified object.
    /// </summary>
    /// <typeparam name="T">The required type of the object to deserialise to.</typeparam>
    /// <param name="json">The JSON representation of the object.</param>
    /// <param name="options">Allows the specific <see href="JsonSerializerOptions">options</see> to be set to control deserialisation.</param>
    /// <returns>A deserialised object based on the original JSON.</returns>
    public static T FromJson<T>(this string json, JsonSerializerOptions options) => JsonSerializer.Deserialize<T>(json, options)!;
}
