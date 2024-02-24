using System.Text;
using Extensions;

namespace UILayer.TableClasses;

/// <summary>
/// Represents a table for displaying data with customizable alignment.
/// </summary>
public class Table
{
    private readonly AlignMode[] _columnAlign;
    private readonly AlignMode _tableAlign;
    private readonly int[] _columnSizes;

    public int ColumnCount => _columnSizes.Length;

    public Table(int[] columnSizes, AlignMode tableAlign, AlignMode[] columnAlign)
    {
        if (columnSizes.Length != columnAlign.Length)
            throw new ArgumentException($"columnSizes and columnAlign must be the same length");
        
        _columnSizes = columnSizes;
        _tableAlign = tableAlign;
        _columnAlign = columnAlign;
    }

    /// <summary>
    /// Formats the items of a row according to the column alignments and sizes.
    /// </summary>
    /// <param name="rowItems">An array of strings representing the items in the row.</param>
    /// <returns>A formatted string representing the row.</returns>
    /// <exception cref="ArgumentException">Thrown when the length of <paramref name="rowItems"/> does not match the number of columns.</exception>
    public string FormatRowItems(string[] rowItems)
    {
        if (rowItems.Length != _columnSizes.Length)
            throw new ArgumentException($"There must be {ColumnCount} items in rowItems");
        
        var sb = new StringBuilder();
        for (int i = 0; i < rowItems.Length; i++)
        {
            if (_columnAlign[i] == AlignMode.Left)
                sb.Append(rowItems[i].AlignLeft(_columnSizes[i]));
            else if (_columnAlign[i] == AlignMode.Right)
                sb.Append(rowItems[i].AlignRight(_columnSizes[i]));
            else if (_columnAlign[i] == AlignMode.Center) 
                sb.Append(rowItems[i].AlignCenter(_columnSizes[i]));
        }

        
        if (_tableAlign == AlignMode.Left)
            return sb.ToString().AlignLeft(Console.WindowWidth);
        if (_tableAlign == AlignMode.Right)
            return sb.ToString().AlignRight(Console.WindowWidth);
        if (_tableAlign == AlignMode.Center) 
            return sb.ToString().AlignCenter(Console.WindowWidth);

        return "";
    }
    
}