
# Markdown To Html Converter

Here you will find a few tools written in C# that convert Markdown files into HTML files.

## Motivation

I choose to write this library for a couple of reasons.

Firstly I started writing some articles in Markdown to post on my own website and, naturally, they needed to be converted to HTML before displaying on the website. Ideally this process should be automated (or automatable) so any changes I make to the Markdown will (more or less) immediately be reflected on the website. I didn't find any satisfactory, existing tools to do this.

Secondly it was a learning exercise for myself both for C# and particularly for web servers in C#.

Thirdly I had some ideas in mind for extensions/custom functionality that I wanted to add to the Markdown (i.e. custom elements types) so I wanted to create a package that makes this straightforward to do.

## Projects

* **MarkdownToHtml** - the core library which provides Markdown parsing and HTML conversion tools

* **MarkdownToHtml.Tests** - tests of core functionality in the MarkdownToHtml library

* **MdToHtml** - command line application to convert a Markdown file to a HTML file

* **MdToHtml.Tests** - test for a few core utilities used in the command line application MdToHtml

* **MdToHtmlServer** - a simple HTTP server that provides a single endpoint for converting a Markdown string to a HTML string

## Current Functionality / TODOs

The functionality of the main parsing library is intended to match as closely as possible that displayed by [Markdown dingus](https://daringfireball.net/projects/markdown/dingus).

Not all functionality expected of basic Markdown is yet included in this implementation, and some additional features over those shown on [Markdown dingus](https://daringfireball.net/projects/markdown/dingus) are offered.

### "Missing" Functionality

* Nested lists (untested)
* 4 space indented block code sections

Also the functionality to include custom extensions/elements has not yet been added.

### "Extra" functionality

* Double tilde (~~) as ~~strikethrough~~

# MdToHtml

A command line application that uses the MarkdownToHtml library to convert a Markdown file to a HTML file.

Usage:

`MdToHtml -i input_file_path -o output_file_path`

The input file will be read as a string and treated as Markdown regardless of what it contains. Both the input file path and the output file path are mandatory.

If a file already exists at the output file path, then the application by default will not overwrite it and will just throw an error. To overwrite the output file the flag `-f` must be provided.

To view the help information use the usual flag `-h`.

# MdToHtmlServer

A simple web server application which provides a single endpoint that accepts some Markdown as input and returns the HTML into which it was converted by the MarkdownToHtml library.

The endpoint is at `/mdtohtml/convert`. 

It requires a POST request and expects JSON in the body. The request JSON should contain the key `Markdown` and only this key.

It will respond with the usual JSON error messages provided by the ASP.NET MVC framework if an error occurs. On success JSON will be returned with a single key `html`, whose value is a string containing the HTML.

# MarkdownToHtml

## Element parent classes

There are four abstract parent classes which are inherited by the classes described below which represent particular elements of the Markdown document. Each different markdown element contains some combination of the following data items: a *type* (with corresponding html tag), some inner *content* (as other element objects), some *attributes* (as string:string key:value pairs). The four parent classes thus define four different combinations of these data items, and the `ToHtml` methods in each case.

* `MarkdownElementBase` - *type* only
* `MarkdownElementWithContent` - *type* + *content*
* `MarkdownElementWithAttributes` - *type* + *attributes*
* `MarkdownElementFull` - *type* + *content* + *attributes*

## Element classes

The biggest group of files/glasses in the project are those that represent "Markdown elements". Broadly speaking these are some specific sub-part of a Markdown document, such as a code block, a heading or a emphasised piece of text. These are the classes prefixed with Markdown, with the exception of the following: `MarkdownElementBase`, `MarkdownElementFull`, `MarkdownElementWithAttributes`, `MarkdownElementWithContent` `MarkdownElementType`, `MarkdownParser`, 

These classes are responsible for detecting whether an element of that type can be parsed from a line or set of lines, and actually parsing that element out. They all provide two static methods to do this - `CanParseFrom` and `ParseFrom`.

`CanParseFrom` takes a `ParseInput` object (explained below) and detects whether the element of the type is present at the start of the line(s) in the `ParseInput` object, returning this as a boolean.

`ParseFrom` takes a `ParseInput` object and creates a new object of this element type from the start of the line(s) in the `ParseInput` provided (if possible). A `ParseResult` is returned indicating whether the parsing was successful and including the object of this type that was created. For elements which appear within a line (i.e. don't take up a whole line or don't span multiple lines, e.g. inline code) the updated line with the text used to create this new object removed may be added to the `ParseResult`.

Most element types will contain a `tag` string, which is the HTML tag type corresponding to the element - for example, `strong` for the `MarkdownStrong` element type.

Many element types can contain nested elements. Generally these elements will hold a `IHtmlable` array called contents, which will hold these nested elements.

All element types contain a `Type` property of type `MarkdownElementType`, which can indicate to external classes which element type it is.

All Markdown element classes should implement `IHtmlable`. In almost all cases, however, the ToHtml will be inherited from a parent class (described in the previous section).

## IHtmlable

This interface can be applied to any class whose objects have a meaningful HTML representation. It requires a `ToHtml` method which returns the HTML respresentation of the object as a string.

Many of the Markdown elements contain nested elements. For these elements typically the `ToHtml` method will call `ToHtml` on all of the elements it contains, string the results together and wrap them in the appropriate tags for the element.

## MarkdownParser

This is the main controlling class of the application. Any code which uses this library can parse a Markdown document, represented as an array of strings (the lines from the Markdown file), and convert this to HTML in string form.

To utilise this class a new instance can be created passing the array of strings as the sole constructor argument. This array will be automatically parsed and converted to a set of Markdown elements. This class implements `IHtmlable`, so to get the HTML string call `ToHtml` on the object.

## MarkdownElementType

An enum which contains named constants for all element types supported by the library. Note that some "utility" types are included in this library which do not correspond to elements of the Markdown specification directly. For example MarkdownLinebreak is just used to introduce a line break element `<br />` in to the HTML output.

## Input/Output classes

The below pair of classes is passed in as input to and returned as output respectively from `CanParseFrom`/`ParseFrom` methods.

### ParseInput

`ParseInput` contains the complete array of string lines from the markdown supplied to the parser. Additionally it contains an array of `ReferencedUrl` objects (explained below).

The class actually includes fields (`startIndex` and `elements`) which specify which lines the `ParseFrom` method is supposed to look at when it starts parsing - recall that `ParseFrom` always starts from the start of the first indicated line. When a method attempts to get the set of lines via the Lines method, the object will return an `ArraySegment` whose start line and size is set by these fields.

There are also a few helper methods that allow the startIndex and number of elements to be updated, either by specifying the new values manually (`Slice`) or by advancing a certain number of lines (one line -> `NextLine`; many lines -> `JumpLines`).

The value of using an object for these inputs instead of just an `ArraySegment` is to make it easier to pass the array of `Urls` through the hierarchy of Markdown elements during parsing, without having to pass two arguments (`ArraySegment` and array of urls) to each.

### ParseResult

A `ParseFrom` method returns a `ParseResult` object. This includes properties which indicate the success/failure of parsing (`Success`), and also (on success) a list of the parsed content as Markdown elements (returned as an array of `IHtmlable`s from `GetContent`).

There is the option to set a `Line` property, which is used to hold the updated value of the line currently being worked on (i.e. with the parsed text removed) for a Markdown element which lives either on a single line or within a line. Types that span multiple lines directly modify the array of lines in the `ParseInput` object and thus do not need to return such updated content. Thus in the future the former described behaviour and the `Line` property may be altered to match the latter behaviour for consistency.

## ReferencedUrl / ReferencedUrlType

These classes are used to represent urls which are provided in the "footnote" format, like `[1]: url`. These must be parsed before all other content since they will be needed when parsing `MarkdownLink` and `MarkdownImage` elements to check that they have valid urls.

### ReferencedUrl

This class can parse a url in the footnote format, like `[1]: url` for a link or `[1]: src "title"` for an image. 

It presents `CanParseFrom` and `ParseFrom` methods similar to described above, except these take `ArraySegment`s directly instead of `ParseInput` objects, since the `ReferencedUrl` array in `ParseInput` clearly isn't required here (equally obviously, it will in fact not have been fully constructed yet).

The object offers the following properties:

* `ReferencedUrlType` - see below for a description
* `Reference` - the text used to refer to this url ("reference" in the example)
* `Url` - the href/src for the link/image ("url" in the example)
* `Title` - a title, if supplied, for images ("title" in the example); empty string for links and if not supplied for images

Example: `[reference]: url "title"`.

### ReferencedUrlType

An enum with two constants that indicates (strictly) whether the parsed reference contains a title or not. References which do not include a title are set to type Link, references which do included a title are set to type Image.

## Utils

This class is intended to contain set of "utility" methods whose functionality is not specific to a certain class/element type, and whose functionality is used across multiple other classes in the library.

Currently there is only one method included, but this is expected to increase after common functionality is identified and refactored out.

### LinkedListToArray

A convenience method that captures the steps needed to copy a LinkedList of any type into a new array of that type.
