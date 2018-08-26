# DotNet2HTML
C# (.Net) to HTML generator. Enjoy typesafe HTML generation.

This project is heavily inspired by [j2html](http://j2html.com).

# Getting started
## Downlaod from Nuget
### Package Manager
```
PM> Install-Package DotNet2HTML -Version 1.0.0
```
### .NET CLI
```
> dotnet add package DotNet2HTML --version 1.0.0
```

## Import the TagCreator and start building HTML
```cs
using DotNet2HTML.Tags;
using static DotNet2HTML.TagCreator;

namespace Program {  
  class Program {
    public static void Main(string[] args) {
      Body(
        H1("Hello, World!"),
        Img().WithSrc("/img/hello.png")
      ).Render();
    }    
  }
}
```
The above C# will result in the following HTML:
```html
<body>
    <h1>Hello, World!</h1>
    <img src="/img/hello.png">
</body>
```
