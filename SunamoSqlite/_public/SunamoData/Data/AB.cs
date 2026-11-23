namespace SunamoSqlite._public.SunamoData.Data;

/// <summary>
/// Represents a key-value pair where the key is a string and the value is an object.
/// </summary>
public class AB
{
    /// <summary>
    /// Gets the type of this class.
    /// </summary>
    public static Type Type { get; } = typeof(AB);

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string A { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public object B { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AB"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public AB(string key, object value)
    {
        A = key;
        B = value;
    }

    /// <summary>
    /// Creates a new AB instance using a type's full name as the key.
    /// </summary>
    /// <param name="type">The type whose full name will be used as the key.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new AB instance.</returns>
    public static AB Get(Type type, object value)
    {
        return new AB(type.FullName ?? string.Empty, value);
    }

    /// <summary>
    /// Creates a new AB instance with the specified key and value.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new AB instance.</returns>
    public static AB Get(string key, object value)
    {
        return new AB(key, value);
    }

    /// <summary>
    /// Returns a string that represents the current object in "Key:Value" format.
    /// </summary>
    /// <returns>A string representation of the key-value pair.</returns>
    public override string ToString()
    {
        return A + ":" + B;
    }
}