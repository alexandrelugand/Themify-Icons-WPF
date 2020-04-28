# Themify-Icons-WPF

WPF controls for the iconic font and CSS toolkit Themify Icons.

Themify Icons: https://github.com/lykmapipo/themify-icons
- Current Version: v0.1.2

## Getting started

### Install

To install Themify.Icons.WPF, run the following command in the Package Manager Console:
```
PM> Install-Package Themify.Icons.WPF
```

Or search & install the package via the NuGet Package Manager.


### Usage XAML

```
<Window x:Class="Example.ThemifyIcons.WPF.Single"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ti="http://schemas.ThemifyIcons.io/icons/"
        Title="Single" Height="300" Width="300">
    <Grid  Margin="20">
        <ti:ImageThemify Icon="Flag" VerticalAlignment="Center" HorizontalAlignment="Center" />
    </Grid>
</Window>
```

You can also use the TextBlock based control.
```
<ti:ThemifyIcons Icon="Flag" />
```

> The Image based `ImageThemify` control is useful when you need to fill an entire space. Whereas the TextBlock base `ThemifyIcons` is useful when you need a certain FontSize. 

You can also work with existing ContentControl based controls, like Button, without having to go back to the Themify Icons cheatsheet and look for that Unicode sequence. Use the `Themify.Content` attached property and select an icon enum value through IntelliSense. Do not forget to set the FontFamily on element or in its style.  

```xaml
<Button ti:Themify.Content="Flag" 
        TextElement.FontFamily="pack://application:,,,/ThemifyIcons.WPF;component/#Themify"/>
```

> VS2013 XAML Designer has issues when using fonts embedded in another assembly (like this scenario), which prevents it to dispaly the glyph properly.  
You could either grab a copy of TTF font file and include it in you Project as a Resource, so to use it in FontFamily, or you could follow the advices proposed in this [StackOverflow thread](http://stackoverflow.com/questions/29615572/visual-studio-designer-isnt-displaying-embedded-font/29636373#29636373). 

#### Binding

The Icon Property is a DependencyProperty so it can be used with-in a {Binding}. There is an example in the example project.


### Usage Code-Behind

If you want to create an Image from Code-Behind (e.g. setting the Window.Icon):

```C#
Icon = ImageThemify.CreateImageSource(ThemifyIconsIcon.Flag, Brushes.Black);
```

## WPF Example

![alt text](/doc/screen-example.png "Example")

Can be found in /example-wpf/ folder.

## Spinning Icons

![](http://i.stack.imgur.com/1w1cC.gif)

```
<ti:ImageThemify Icon="Reload" Spin="True" SpinDuration="10" />
```

Using the `ImageThemify` control for spinning icons is recommended, as it uses the correct centre point for rotation.

## Rotated / Flipped
```
<ti:ImageThemify Icon="Spinner" FlipOrientation="Horizontal" Rotation="90" />
```
## Icons

All icons including their aliases are generated from ThemifyIconss' icons.yaml. 

```C#
public enum ThemifyIconsIcon {
....
///<summary>Flag</summary>
[Description("Flag"),IconId("flag"),IconCategory("Themify Icons")]
Flag = 0xe63a,
....
}
```

Following meta data is added:
* Icon
	* XML-Doc <summary> from name with created reference.
	* XML-DOC <see /> for direct link to icon web page.
	* IconCategory Attributes, one per category
	* Description Attribute, name
* Alias
	* XML-Doc <summary> Alias of: referencing icon
	* XML-Doc <see /> to referencing field (to reduce code file length)
	* IconAlias Attribute
	
## Converters

The converters are also MarkupExtension, so than can be used in-place without creating a StaticRessource.

### CssClassNameConverter

Say you have (or need) the css class name of a themify icon (e.g. "flag"), this converter takes the ThemifyIconsIcon enum and translates its value (using reflection) to its css class name (or vice-versa).

There are two modes:
* FromStringToIcon (Default mode. Expects a string and converts to a ThemifyIconsIcon.)
* FromIconToString (Expects a ThemifyIconsIcon and converts it to a string.)

Example usage:
```
<TextBlock Text="{Binding Path=ThemifyIconsIcon, Converter={ti:CssClassNameConverter Mode=FromIconToString}}" Grid.Column="1" Grid.Row="1" />
```

### ImageSourceConverter

Use this converter if you require an ImageSource of a ThemifyIconsIcon. Use the ConverterParameter to pass the Brush (default is a solid black brush). This is useful for existing wpf controls.

Example usage:
```
<SolidColorBrush x:Key="ImageBrush"  Color="LightBlue" />
...
<Image Source="{Binding Path=ThemifyIconsIcon, Converter={ti:ImageSourceConverter}, ConverterParameter={StaticResource ImageBrush}}" />
```	
## License

The MIT License (MIT)

Copyright (c) 2017 alexandrelugand

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

