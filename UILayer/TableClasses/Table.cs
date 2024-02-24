using System.Text;
using Extensions;

namespace UILayer.TableClasses;

/// <summary>
/// Represents a table for displaying data with customizable alignment.
/// </summary>
public class Table
{
    private readonly AlignMode[] _columnsAlign;
    private readonly AlignMode _tableAlign;
    private readonly int[] _columnSizes;

    public int ColumnCount => _columnSizes.Length;

    public Table(int[] columnSizes, AlignMode tableAlign, AlignMode[] columnsAlign)
    {
        if (columnSizes.Length != columnsAlign.Length)
            throw new ArgumentException("columnSizes and columnsAlign must be the same length");
        
        _columnSizes = columnSizes;
        _tableAlign = tableAlign;
        _columnsAlign = columnsAlign;
    }

    public Table() { }

    /// <summary>
    /// Formats the items of a row according to the column alignments and sizes.
    /// </summary>
    /// <param name="rowItems">An array of strings representing the items in the row.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when AlignMode does match supported ones.</exception>
    /// <returns>A formatted string representing the row.</returns>
    /// <exception cref="ArgumentException">Thrown when the length of <paramref name="rowItems"/> does not match the number of columns.</exception>
    public string FormatRowItems(string[] rowItems)
    {
        if (rowItems.Length != _columnSizes.Length)
            throw new ArgumentException($"There must be {ColumnCount} items in rowItems");
        
        var sb = new StringBuilder();
        for (int i = 0; i < rowItems.Length; i++)
        {
            switch (_columnsAlign[i])
            {
                case AlignMode.Left:
                    sb.Append(rowItems[i].AlignLeft(_columnSizes[i]));
                    break;
                case AlignMode.Right:
                    sb.Append(rowItems[i].AlignRight(_columnSizes[i]));
                    break;
                case AlignMode.Center:
                    sb.Append(rowItems[i].AlignCenter(_columnSizes[i]));
                    break;
                default:
                    throw new Exception("Supported only Left, Right and Center alignment.");
            }
        }

        return _tableAlign switch
        {
            AlignMode.Left => sb.ToString().AlignLeft(Console.WindowWidth),
            AlignMode.Right => sb.ToString().AlignRight(Console.WindowWidth),
            AlignMode.Center => sb.ToString().AlignCenter(Console.WindowWidth),
            _ => ""
        };
    }
    
}