# BetterResources

<a id="what"></a>
## What is it
This is a tool which can be used in place of the default code generator for
.resx resource files in C# projects. When a format string is found in the
resources, a method will be generated accepting values to format the resource.

If your .resx file contains a string resource named `AString` with the value
`"{0}, {1}"`, then the following code will be generated for it:
```CSharp
public static string AStringRaw
{
    get { return ResourceManager.GetString("AString", Culture); }
}

public static string AString(object arg0, object arg1)
{
    return string.Format(AStringRaw, arg0, arg1);
}
```

It is also possible to provide more specific type information for resources by
using the resource's comment. If the resources comment was changed to start with
`{int x, double y}`, then the function would instead be generated as:
```CSharp
public static string AString(int x, double y)
{
    return string.Format(AStringRaw, x, y);
}
```
In this way, if you have any format specifiers in your resources (such as
`{0:X}` to format an integer as hexadecimal), you can have that be type-checked
by the compiler, at build time, so as to avoid unexpected runtime errors trying
to format values incorrectly.
<br/>
<br/>

<a id="how"></a>
## How to use it

#### Setting Up The Tool
The .vsix package registers two new tools with Visual Studio:
  1. `InternalBetterResourcesGenerator` which generates the resource class with `internal` accessibility, and
  2. `PublicBetterResourcesGenerator` which generates it with `public` accessibility.

You can do use one of these by right-clicking your .resx file in Visual Studio's
Solution Explorer, clicking "Properties" to show the Properties panel. One of
the properties on the .resx file will be "Custom Tool". If the file was created
by Visual Studio, this will likely be either `ResXFileCodeGenerator` or
`PublicResXFileCodeGenerator`. Replacing this value with one of the tools 
above will cause the better resources tool to be used instead.

This can also be changed directly in your MSBuild project file (`.csproj`) by
finding the `<EmbeddedResource>` element for your .resx file, and changing
the `<Generator>` under that to one of the tools above:
```XML
<Project>
  <!-- snip -->
  <ItemGroup>
    <EmbeddedResource Include="Resources.resx">
      <Generator>PublicBetterResourcesGenerator</Generator>
      <SubType>Designer</SubType>
      <LastGenOutput>Resources.Designer.cs</LastGenOutput>
    </EmbeddedResource>
  </ItemGroup>
  <!-- snip -->
</Project>
```
<br/>

#### Typed Format Arguments

This tool allows you to give format arguments declared types by editing the
resource's comment field. When the comment starts with a parameter list inside
a pair of curly-braces, similar to `{int x, double y}`, then that will be
used as the parameters to the resource method.

If a format string resource does not have a comment providing a parameter list,
then one is auto-generated using arguments of type `object`.
