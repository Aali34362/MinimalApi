using System.Text;

namespace FormatDemo.Model;

public class EmployeeModel : IFormattable
{

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }

    public override string ToString()
    {
        return this.ToString(null, null);
    }

    public string ToString(string? format)
    {
        return this.ToString(format, null);
    }
    public void dateformator()
    {
        DateTime dateTime = DateTime.Now;
        Console.WriteLine(dateTime.ToString("ddMMyyyy"));
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrWhiteSpace(format) || format == "G") { return $"{FirstName} {LastName}"; }
        return format switch
        {
            "L" or
            "Login" => $"{FirstName?.Substring(0, 1)} {LastName}",
            "S" or
            "SORTABLE" => $"{LastName}, {FirstName} {MiddleName}",
            _ => $"{FirstName} {LastName}"
        };
    }
}









/*
 * https://learn.microsoft.com/en-us/dotnet/api/system.iformattable?view=net-8.0
 * IFormattable : Provides functionality to format the value of an object into a string representation.
 * The IFormattable interface converts an object to its string representation based on a format string and a format provider.
 * eg dateTime.ToString("ddMMyyyy")
 * 
 */


////////////////GET SET INIT////////////////////////////
/*
{ get; set; }: 
This is the most common property declaration. 
It creates a property with a getter and a setter. 
This means that the property can be read from and written to from outside 
the class or struct.
public int MyProperty { get; set; }

{ init; }: 
This is a new feature introduced in C# 9.0. 
It creates a property with a getter and an initializer, but no setter. 
This means that the property can be set only once, typically during object 
initialization, and cannot be changed afterwards.
public int MyProperty { get; init; }

{ private set; }: 
This creates a property with a public getter and a private setter. 
This means that the property can be read from outside the class or struct,
but can only be set from within the class or struct.
public int MyProperty { get; private set; }
Here's a brief comparison of these property declarations:

{ get; set; }: Readable and writable property.
{ init; }: Readable and writable property, but only settable during initialization.
{ private set; }: Readable property with a private setter, allowing controlled
modification within the class or struct.
These property declarations provide flexibility in controlling access to data 
within your classes and structs, allowing you to enforce encapsulation and 
immutability where necessary.

Read-Write Property ({ get; set; }):
public int MyProperty { get; set; }
This form creates a property with both a getter and a setter,
allowing both reading and writing of the property's value.

Read-Only Property ({ get; }):
public int MyProperty { get; }
This form creates a read-only property with a getter but no setter, 
allowing the value to be read but not modified.

Write-Only Property ({ set; }):
public int MyProperty { set; }
This form creates a write-only property with a setter but no getter, 
allowing the value to be modified but not read.

Property with Initializer ({ init; }):
public int MyProperty { get; init; }
Introduced in C# 9.0, this form creates a property with a getter and an 
initializer but no setter, allowing the value to be set only during object 
initialization.

Property with Access Modifier ({ get; private set; }):
public int MyProperty { get; private set; }
This form creates a property with a public getter and a private setter, 
allowing the value to be read from anywhere but set only from within the
class itself.

Property with Custom Logic (using a backing field):
private int _myField;
public int MyProperty
{
    get { return _myField; }
    set { _myField = value; }
}
This form defines a property with custom logic using a private backing field.
It allows for additional logic to be executed when getting or setting the 
property value.

These are some of the common forms of property declarations in C#. 
Depending on your requirements, you can choose the appropriate form to 
control the accessibility, mutability, and behavior of your properties. 
 */
