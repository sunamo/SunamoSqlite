namespace SunamoSqlite._sunamo.SunamoCSharp;

/// <summary>
/// String builder with a delimiter between items.
/// </summary>
public class InstantSB
{
    public StringBuilder StringBuilder { get; set; } = new StringBuilder();
    private string _tokensDelimiter;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public InstantSB(string delimiter)
    {
        _tokensDelimiter = delimiter;
    }

    /// <summary>
    /// Returns the generated text.
    /// </summary>
    public override string ToString()
    {
        string result = StringBuilder.ToString();
        return result;
    }

    /// <summary>
    /// Adds the item followed by the delimiter.
    /// </summary>
    public void AddItem(string value)
    {
        string text = value.ToString();
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text + _tokensDelimiter);
        }
    }

    /// <summary>
    /// Adds the raw content.
    /// </summary>
    public void AddRaw(object content)
    {
        StringBuilder.Append(content.ToString());
    }

    /// <summary>
    /// Ends the line with the content.
    /// </summary>
    public void EndLine(object content)
    {
        string text = content.ToString()!;
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text);
        }
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine(string text)
    {
        EndLine(text + Environment.NewLine);
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine()
    {
        EndLine(Environment.NewLine);
    }

    /// <summary>
    /// Removes the delimiter from the end.
    /// </summary>
    public void RemoveEndDelimiter()
    {
        StringBuilder.Remove(StringBuilder.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }
}