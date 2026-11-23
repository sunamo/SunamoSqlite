namespace SunamoSqlite._public.SunamoData.Data;

/// <summary>
/// Represents a collection of AB (key-value pair) items.
/// </summary>
public class ABC : List<AB>
{
    /// <summary>
    /// Gets an empty ABC instance.
    /// </summary>
    public static ABC Empty { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ABC"/> class.
    /// </summary>
    public ABC()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABC"/> class with the specified capacity filled with null values.
    /// </summary>
    /// <param name="capacity">The number of null elements to pre-allocate.</param>
    public ABC(int capacity) : base(capacity)
    {
        for (var i = 0; i < capacity; i++) Add(null!);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABC"/> class from various input types.
    /// Supports AB objects, ABC collections, or name-value pairs.
    /// </summary>
    /// <param name="setsNameValue">Variable number of parameters that can be AB objects, ABC collections, or alternating name-value pairs.</param>
    public ABC(params object[] setsNameValue)
    {
        if (setsNameValue.Length == 0) return;
        var firstElement = setsNameValue[0];
        var elementType = firstElement.GetType();
        var actualType = elementType;
        if (firstElement is IList)
        {
            var list = firstElement as IList;
            var firstListElement = (list != null && list.Count != 0) ? list[0] : null;
            actualType = firstListElement?.GetType() ?? actualType;
        }

        if (actualType == typeof(AB))
        {
            for (var i = 0; i < setsNameValue.Length; i++)
            {
                var currentElement = setsNameValue[i];
                actualType = currentElement.GetType();
                if (actualType == AB.Type)
                {
                    Add((AB)currentElement);
                }
                else
                {
                    var list = (IList)currentElement;
                    foreach (var item in list)
                    {
                        var abElement = (AB)item;
                        Add(abElement);
                    }
                }
            }
        }
        else if (elementType == typeof(ABC))
        {
            var collection = (ABC)firstElement;
            AddRange(collection);
        }
        else
        {
            for (var i = 0; i < setsNameValue.Length; i++) Add(AB.Get(setsNameValue[i]?.ToString() ?? string.Empty, setsNameValue[++i]));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABC"/> class from an array of AB objects.
    /// </summary>
    /// <param name="abc">Array of AB objects to add to the collection.</param>
    public ABC(params AB[] abc)
    {
        AddRange(abc);
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    public int Length => Count;

    /// <summary>
    /// Returns a string representation of all elements in comma-separated format.
    /// </summary>
    /// <returns>A string containing all elements separated by commas.</returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.Append(item + ",");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns an array containing only the values (B part) from all AB pairs.
    /// Must be an array due to SQL requirements.
    /// See: https://stackoverflow.com/questions/9149919/no-mapping-exists-from-object-type-system-collections-generic-list-when-executin
    /// </summary>
    /// <returns>An object array containing all values.</returns>
    public object[] OnlyBs()
    {
        return OnlyBsList().ToArray();
    }

    /// <summary>
    /// Returns a list containing only the values (B part) from all AB pairs.
    /// </summary>
    /// <returns>A list of objects containing all values.</returns>
    public List<object> OnlyBsList()
    {
        var result = new List<object>(Count);
        for (var i = 0; i < Count; i++) result.Add(this[i].B);
        return result;
    }

    /// <summary>
    /// Returns a list containing only the keys (A part) from all AB pairs.
    /// </summary>
    /// <returns>A list of strings containing all keys.</returns>
    public List<string> OnlyAs()
    {
        var result = new List<string>(Count);
        for (var i = 0; i < Count; i++) result.Add(this[i].A);
        return result;
    }

    /// <summary>
    /// Extracts only the values (B part) from a list of AB pairs.
    /// </summary>
    /// <param name="list">The list of AB pairs to extract values from.</param>
    /// <returns>A list of objects containing all values.</returns>
    public static List<object> OnlyBs(List<AB> list)
    {
        return list.Select(element => element.B).ToList();
    }
}