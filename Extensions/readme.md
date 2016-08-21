# Extensions Library

A .NET Library filled with Extension Methods for popular classes i work with, and other useful namespaces and classes. The Extension Methods include the following:

### Bitmap Extensions

This contains extension methods neccessary if you're working on image manipulation. I used to find myself googling *"how to convert byte array to bitmap"* each time i needed to do that. With these extension methods, i could just do 
  ```cs
  (new Bitmap(150, 150)).ToByte()
  ```
and voila! i have my byte array filled with 0s. Okay, let's see what this class has to offer ...

- `static Byte[] ToBytes(this Bitmap b)`
  Convert a bitmap tp a byte array easily with this extension method as shown below:
  ```cs
  Bitmap b = Bitmap.FromFile(@"photo.jpg");
  byte[] bArray = b.ToBytes();
  ```
  
- `static Bitmap ToBitmap(this Byte[] b)`
  Convert a byte array to a bitmap as shown below:
  ```cs
  byte[] bArray = Bitmap.FromFile(@"photo.jpg").ToBytes();
  bArray.ToBitmap();
  ```
  
- `static Bitmap ToBitmap(this WebBrowser browser)`
  Take a snapshot of a WebBrowser control ... If used correctly, you could use this to take snapshot of web pages.
  
- `static Stream ToStream(this Byte[] b)`
  Convert a byte array into a memory stream
  
- `static byte[] ToBytes(this Stream ms)`
  Convert a stream into a byte array
  
- `static Bitmap Scale(this Bitmap source, int x, int y)`
  Resize a Bitmap Image to width *x* and height *y*
  
- `static Bitmap Scale(this Bitmap source, int size)`
  Scale the width and height of a bitmap image to size. 

- `static Bitmap GetSpectrum(this Bitmap b, ColorSpectrum cs, bool actualspectrum = false)`
  Retrieve the red, green or blue spectrum from a bitmap image. Such as:
  ```cs
  Bitmap b = Bitmap.FromFile(@"photo.jpg");
  b.GetSpectrum(ColorSpectrum.Red); // the image appears red.
  b.GetSpectrum(ColorSpectrum.Red, true); // only the red part of each pixel is shown
  ```

- `static Bitmap DrawRectangle(this Bitmap b, Rectangle r, Pen p)`
  Draws a rectangle on the Image in any pen of your choosing. The Pen determines properties of the rectangle such as the stroke width, pen color, etc.
  

### Boolean Extensions
You know how you can do this in csharp?
```cs
(1 < 2) ? true_value : falsey_value;
```
Can't remember what it's called, but i had no idea how to do that in vb.net, or if it was even possible ... So, I came up with these extension methods that take care of that for me. I do this instead:
```cs
(1 < 2).If("true_value").Else("falsey_value").Resolve();
```
Ok, i admit this is perhaps a little redundant.

### Html Extensions
I am quite proud of this one. `GetElementsByClassName` has always been a huge part of HTML Manipulation for most people. I had to add this to HTMLDocument, HTMLElement, and HTMLElementCollection

### List and IEnumerable Extensions
There are many extensions methods that you will find useful ...

- `static IEnumerable<T> First<T>(this IEnumerable<T> myarray, int count)`
  The First method already exists, and returns the first item in an Enumerable such as a List or an Array if it exists. If it doesn't, it throws an Error. This is different, though. With this extension method, you can return the first *count* items in the enumerable. If the enumerable is empty, an empty enumerable if returned. if the value of count is larger than the number of items in the enumerable, all the values in the enumerable are returned.
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
- ``
### Number Extensions

### Object Extensions

### Rectangle Extensions

### String Extensions
