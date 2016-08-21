# Extensions Library

In object-oriented computer programming, an [extension method](https://en.wikipedia.org/wiki/Extension_method) is a method added to an object after the original object was compiled. The modified object is often a class, a prototype or a type. Extension methods are features of some object-oriented programming languages.

This is a .NET Library filled with Extension Methods for popular classes i work with, and other useful namespaces and classes. The Extension Methods include the following:

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

- `static IEnumerable<T> Last<T>(this IEnumerable<T> myarray, int count)`
  The Last method also exists for Lists and Arrays (i am not so sure about Arrays), but like the First method given above, this overloaded function lets you specify the count. So, it returns the last [count] items in the collection.

- `static List<T> Fill<T>(this List<T> myarray, int count, T value = default(T))`
  This lets you fill up a List with a default or specified value

- `static List<T> Push<T>(this List<T> myarray, T obj)`
  Push works the same way as the Add method, but it returns the resultant List, so you can chain Push methods together.

- `static List<T> PushRange<T>(this List<T> myarray, List<T> arr)`
  This works just like AddRange, but also returns the resultant list to make chaining extension methods possible.

- `static T Random<T>(this IEnumerable<T> l)`
  This returns a random member of the list, unless of course, the list is empty. In that case, it returns the default value of the List Member Type, which is usually null.

- `static List<T> Sort<T>(this List<T> myarray, string propertyname, string type = "asc")`
  For fast sorts using a property of the list member objects, use `myList.Sort("property name")`

- `static List<X> Select<T, X>(this List<T> myarray, string propertyname)`
  Perhaps not the most efficient, but this Select statement lets you select properties  from each member of the list. 
  ```cs
  myList.Select<Person, string>("Name"); // get a list of string of Names
  myList.Select<Person, string>("Name", "asc") //get a list of string of Names in descending order
  ```
  
- `string Join<T>(this IEnumerable<T> myarray, string concatenator = "")`
  This is much closer to the String.prototype.join method offered in JavaScript. It lets you join elements in a collection with a specified string. E.g. `myArray.Join(",")`

- `static List<T> From<T>(this List<T> myarray, int index)`
  Get Elements in a List starting from a specified index. If such an index is not available, returns an empty list.

- `static List<T> Where<T>(this List<T> myarray, string prop, dynamic val)`
  Returns Elements in a List whose specified property has the same value as the specified value.

- `static IEnumerable<T> Backwards<T>(this IEnumerable<T> myarray)`
  I was just learning to use yield return when i did this. Silly me! I should have learned this sooner. A lot of the methods here would not have been written so shabbily. This works the same way as `myList.Reverse()`

- `static bool IsEmpty<T>(this IEnumerable<T> myarray)`
  Returns a boolean that is true when the collection is empty and false when it isn't

- `static List<X> Flatten<X>(this IEnumerable<IEnumerable<X>> myarray)`
  Do you have a list of lists? Or a collection of collections? Flatten that monstrousity with this extension method.

- `static bool Contains<X> (this List<X> myarray, X val, string propname = "")`
  Find out if any member of your list has a property with a specific value with `myList.Contains("Name", "Michael");`

- `static List<X> Distinct<X> (this List<X> myarray, string propname = "")`
  Get Distinct Elements based on a particular property such as `myList.Distinct("Name")` will return members with distinct Name property values

- `static ICollection<X> AddIf<X>(this ICollection<X> myarray, X val, Func<X, X, bool> check)`
  Add an object to the collection if a condition is satisfied

- `static List<X> Sync<X> (this List<X> myarray, List<X> vals, string propname = "")`
  Synchronise two Lists using a specific property such as `myList.Sync(myOtherList)`

- `static List<List<X>> Paginate<X> (this List<X> myarray, int width = 5)`
  The Opposite of the Flatten method described above. This turns a list into a list of lists. I intend to make this work with Enumrables instead, and perhaps yield the returned values to save memory. So, you want to send a large byte array in chunks somewhere? This could be a time saver.

- `static void AddSafe(this IDictionary<string, dynamic> mydict, string key, dynamic value)`
  Well, well, well ... Ever experienced this?
  ```cs
  Dictionary<string, int> people = new Dictionary<string, int>();
  people.Add("michael", 1005);
  people.Add("michael", 1062); //throws an error
  ```
  I always found that annoying. I didn't want to have to check if a key "michael" already existed to replace its value. I wanted to either add the key "michael" or replace its value. That's what this method does for you. It lets you add the key and value safely. Without throwing an error. So, with this instead:
  ```cs
  Dictionary<string, int> people = new Dictionary<string, int>();
  people.AddSafe("michael", 1005);
  people.AddSafe("michael", 1062);
  people["michael"] // returns 1062
  ```
  Life is full of smiles
  
- `static void AddSafe<K, V>(this IDictionary<K, V> mydict, K key, V value)`
  Works just like the function above, but adds generics instead of using dynamic type-ing

- `static void AddOnce<K, V>(this IDictionary<K, V> mydict, K key, V value)`
  Want to add a key and value to a Dicitionary just once ever? Use this method.

- `static T FirstOrDefault<T>(this IEnumerable<T> myarray, T default_value)`
  Works just like the normal FirstOrDefault, but you can specify a default value to return instead instead of the traditional system-specified default value (usually null);

- `static T LastOrDefault<T>(this IEnumerable<T> myarray, T default_value)`
  Works just like the normal LastOrDefault, but you can specify a default value to return instead instead of the traditional system-specified default value (usually null);
  
- `static void AddSafe(this System.Web.SessionState.HttpSessionState session, string key, dynamic value)`
  Works just like AddSafe for Dictionary, but this time, on Session objects

- `static void AddOnce(this System.Web.SessionState.HttpSessionState session, string key, dynamic value)`
  Works just like AddOnce for Dictionary, but this time, on Session objects
  
### Number Extensions
  This contains extension methods for numeric-types in .NET 
  
- `object.IsNumber` lets you know if an unidentified object is numeric
- `string.ToInteger` converts a string to an integer
- `int.Pad(count)` returns a string representing the padded integer. The Integer is padded with preceding zeros (0s)

### Object Extensions
This contains general methods that sometimes extend objects in .NET, such that almost every object can use them. They include:

- `static T Clone<T>(this T obj)`
  Make clones of your objects easily with this methods

- `static Y Copy<X, Y>(this Y obj1, X obj2)`
  Copy the properties of one object into another. They do not have to be of the same type/class.

- `static int IndexOfProperty(this PropertyInfo[] info, string prop)`
  Find the Index of a Particular Property in an Object. This is very useful during reflection

- `static Y GetValue<X,Y>(this X obj, string prop)`
  Get the value of particular property in an object. Useful when dealing with dynamic objects, where you cannot simply say `dynamic_obj.property_name`. Rather, use `dynamic_obj.GetValue("property_name")`

- `static void SetValue<X, Y>(this X obj, string prop, Y value)`
  Sets the value of a specified property in an object. This is also useful when working with dynamic objects and late-runtime binding. Use as `dynamic_obj.SetValue("property_name", "property_value")`

- `static byte[] ToBytes<T>(this T obj)`
  Convert any Object to bytes using Serialization. If using a custom class, you might want to mark it as serializable.

- `static XElement ToXElement<T>(this T obj)`
  Convert any Object to XElement. I find this particularly useful when saving to MsSQL database XML fields. `obj.ToXElement()` always does the trick.

- `static void SaveToFile(this byte[] arr, string f)`
  Want to Save a byte array to file without going through the rigor or creating a filestream and stuff?
  ```cs
  byte[] arr = new byte[] {0, 5, 23, 67, 89};
  arr.SaveToFile("mykeels.dat");
  ```
  When combined with the ToBytes extension method, you can easily save an object to disk such as:
  ```cs
  Person p = new Person("Michael");
  p.ToBytes().SaveToFile(p.Name.ToLower() + ".dat");
  ```
  
- `static T ToObject<T>(this byte[] arr)`
  Convert a byte array into an object of a specified type. `myPersonBytes.ToObject<Person>()`

- `static T ToObject<T>(this XElement xml)`
  Convert an XElement into an object of a specified type.  `myPersonXElem.ToObject<Person>()`

### Rectangle Extensions
I worked with rectangles when doing work on an object recognition software. I had to make sure overlapping rectangles were combined into a single rectangle at some point. I finally had this class to show for my work done.

- `static bool EqualsTo(this Rectangle rect1, Rectangle rect2)`
  Are these two Rectangles equal? Do they have the same size and start point? Use this extension method to find out.

- `static Rectangle Overlap(this Rectangle rect1, Rectangle rect2)`
  Does this rectangle overlap with this other rectangle? This is the extension method for the job.

- `static Rectangle Inflate(this Rectangle rect, int size)`
  I want to inflate this rectangle by this number of pixels (defined by the size variable)

- `static bool CloseTo(this Rectangle rect1, Rectangle rect2, int threshold)`
  How close are these two rectangles? Is the value within a defined threshold? Find out with this method.

- `static bool Contains(this IEnumerable<Rectangle> rects, Rectangle rect)`
  Does this rectangle contain the other rectangle? This is the extension method for you

- `static IEnumerable<Rectangle> Distinct(this IEnumerable<Rectangle> rects)`
  Returns distinct rectangles from a collection
  
### String Extensions
What would we do without text? There are probably more efficient methods to perform the functions that this class offers, but till i know what they are, these are the best i can offer.

- `static string ToSentenceCase(this string s)`
  Returns a string with the first letter capitalized.
  ```cs
  "i am ikechi michael".ToSentenceCase(); // will return 'I am ikechi michael'
  ```

- `static string CapitaliseEachWord(this string s)`
  Returns a string with each word capitalized
  ```cs
  "i am ikechi michael".CapitaliseEachWord(); // will return 'I Am Ikechi Michael'
  ```

- `static string EncodeURI(this string s1, List<char> exempt = null)`
  Removes all special characters and separates a string using hyphens such as used in clean urls. 
  ```cs
  "Magic Moments".EncodeURI(); // returns 'magic-moments'
  ```

- `static bool HasSpecial(this string str)`
  Returns a boolean indicating whether a string contains special characters or not

- `static List<char> GetSpecials(this string str)`
  Returns a List of special characters that a string contains

- `static string DecodeURI(this string s1)`
  Converts a string from clean url format e.g. `'magic-moments'` into Word Capitalized form e.g. `"Magic Moments"`

- `static string Shuffle(this string s)`
  Shuffles a string randomly e.g. `"Michael".Shuffle()` could become `"hMileac"`

- `static bool HasLower(this string str)`
  Returns a boolean indicating whether the string has lowercase characters

- `static bool HasUpper(this string str)`
  Returns a boolean indicating whether the string has uppercase characters

- `static bool IsString(this object value)`
  Is this dynamic object a string or not? This is your extension method for the job

- `static double ContainPercentage(this string s, string f)`
  Gives the percentage correlation of a string to another ... Useful when comparing spell-error-prone strings

- `static FoundText Find(this string s, string f)`
  Returns the Start and End indices of a string in another

- `static List<FoundText> FindAll(this string s, string f)`
  Returns a List of all Start and End Indices of a string in another

- `static string Truncate(this string s, int length, bool allowword = true)`
  Truncates a string based on a specified text length. You could specify that it breaks a word if [allowword] is set to false

- `static string Encrypt(this string s, string password = null)`
  Use AES Encryption on your strings easily

- `static string Decrypt(this string s, string password = null)`
  Decrypt your AES encrypted strings easily

- `static string ToJson<T>(this T obj, bool useIndent = false)`
  Convert any object into its Json Equivalent

- `static void SaveToFile(this string txt, string f)`
  Save any string to Disk

- `static string First(this string s, int count = 1)`
  Returns a string containing the first [count] characters in the parent string

- `static string Last(this string s, int count = 1)`
  Returns a string containing the last [count] characters in the parent string
  


