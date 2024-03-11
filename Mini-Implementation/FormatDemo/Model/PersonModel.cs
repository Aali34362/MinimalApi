using System.Globalization;
using System.Text;

namespace FormatDemo.Model;

public class PersonModel : IFormattable
{
    public string? Title { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }

    public override string ToString()
    {
        return this.ToString("G");
    }

    public string ToString(string? format)
    {

        return this.ToString(format,CultureInfo.CurrentCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format = format?.ToUpper();
        if (string.IsNullOrWhiteSpace(format) || format == "G")
        {
            return $"{FirstName} {LastName}";
        }

        // "T FF M. LL"
        StringBuilder output = new();
        
        for(int i = 0; i<format.Length; i++)
        {
            char c = format[i];
            string token = c.ToString();
            while (i + 1 < format.Length && c == format[i+1])
            {
                i++;
                token += c;
            }
            switch(token)
            {
                case "T":
                    output.Append(Title);
                    break;
                case "F":
                    output.Append(FirstName.Substring(0, 1));
                    break;
                case "FF":
                    output.Append(FirstName);
                    break;
                case "M":
                    output.Append(MiddleName?.Substring(0, 1));
                    break;
                case "MM":
                    output.Append(MiddleName);
                    break;
                case "L":
                    output.Append(LastName.Substring(0, 1));
                    break;
                case "LL":
                    output.Append(LastName);
                    break;
                default:
                    output.Append(token);
                    break;
            }
        }

        return output.ToString();
    }
}
