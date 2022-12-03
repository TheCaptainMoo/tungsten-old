# Tungsten
**Tungsten is an Interpreted Programming Language Created Using C#.** It offers both strongly typed and loosely typed variables and has a similar structure to C# or C++. Before using Tungsten, please note the syntax is **highly strict** and **potentially buggy**, **performance** for demanding tasks is not great and errors may cause it to **crash without warning**. On a positive note, Tungsten is open source and is actively being improved and developed.

## Documentation
Documentation can be found on this page's [wiki](https://github.com/TheCaptainMoo/tungsten/wiki).

## Installation
**Currently Only Windows Is Supported**

In order for Tungsten to run correctly, the latest version of [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) must be installed. 

1. Locate and download the latest [release](https://github.com/TheCaptainMoo/tungsten/releases/latest)
2. Unzip `Tungsten.zip` and open the unzipped folder.
3. Navigate to `..\Tungsten\Interpreter\Release\net6.0-windows7.0\Tungsten Interpreter.exe`
4. Click & drag the file to be interpreted into the Console (should display the path of the file)

I recommend using a file extension of `*.tsn | *.tungsten` and setting the interpreter as the default program for easier use.

## Sample Code
```c++
$ <System>;
$ <Variables>;

/* Using Statements ^^ Are Required For Each Script */

/* Single Line Comments Are Indicated Using An Asterisk And Slash */
/* The Final Slash Is Theoretically Unnessessary But Might Break The Code So Add It :D*/

/* Print Statements Output Text Between '[' & ']' */
print [What is your favourite flavour of icecream?];
/* Output: What is your favourite flavour of icecream? */

/* Text-based User Input Can Be Achieved Using '=> OR input', Followed By The Type And Variable Name */
=> string flavour;
/* e.g. flavour: Strawberry */

/* Variables Can Also Be Included In Print Statements */
print flavour [ is my favourite too!];
/* Output: Strawberry is my favourite too! */
```

```c++
$ <System>;
$ <Variables>;

/* Variables Can Be Created Programatically */
int num: 0;

/* Variables Can Be Looped Through In A While Loop  | Spaces Between 'num', '<' & '3' Are Required */
while <num < 3>{
  print num;
  
  /* If Statements Work Similarly To While Loops */
  if <num == 2>{
    print [Number is two];
  }
  
  /* Variables Can Be Updated | update - format type (string, int, bool) - variable name - value */
  update int num: (num + 1);
}

/* Output: 0 */
/*         1 */
/*         2 */
/*         Number is two */
```
