# WhitespaceWarrior

`WhiteSpaceWarrior` is a tool that make scanning code significantly faster by improving the _signal to noice ratio of the code_ 
- i.e. removing all the stuff you get anoyed about when you speed-read code such as empty lines, empty comments, short meaningles comments.

![Logo](whitespacewarrior.png)




# Installation

    > dotnet tool install --global WhiteSpaceWarrior 

Then you can use the command `WhiteSpaceWarrior` in your shell.

Or visit the releases at https://github.com/kbilsted/WhitespaceWarrior/releases



# C# Example

Let's illustrate the effect of a code base filled with cruft and white space. Of course the example is rigged in the sense
that it contains example of all the things `whitespacewarrior` is catering for. But you'll notice that with all the stuff
thats in the way, focus is harder to attain.

The example samples code pieced I've experienced in my career.

```cs
/// <summary>
/// </summary>
public class Calculator
{

    #region Properties
    /// <summary>
    /// Usage count
    /// </summary>
    private int CalculationCount
    {
        get;
        set;
    }
    #endregion

    #region Methods
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name=""T""></typeparam>
    /// <param name=""a""></param>
    /// <param name=""b""></param>
    /// <returns></returns>
    public int Add<T>(int a, int b)
    {

        CalculationCount++;
        return a + b;

    }

    ///////////////////////////////////////////////

    /// <summary>
    /// Minus impl
    /// </summary>
    /// <param name=""a"">A number</param>
    /// <param name=""b"">A number</param>
    /// <returns></returns>
    public int Minus(int a, int b)
    {

        CalculationCount++;
        return a - b;
    }

    #endregion


}
```

Reformatting it with the whitespacewarrior it becomes

```cs
public class Calculator
{
    private int CalculationCount { get; set; }

    public int Add<T>(int a, int b)
    {
        CalculationCount++;
        return a + b;
    }

    public int Minus(int a, int b)
    {
        CalculationCount++;
        return a - b;
    }
}
```

Of course the example is a show case of the powers of the white space warrior. That being said
it is still a reduction from _49_ lines to just _16_ lines! Making the code *much* easier to read and absorb. 

Here is a short summary of what kind of whitespace that is cleaned up:

* Empty lines around `{` and `}`
* Empty or short `<summary>`,`<param name="..">`, `<returns>`, ...
* Shortening short `<summary>` down to 1 line from the standard 3 lines
* Regions `#region` and `#endregion`
* Newlines around properties (getters, setters) and also reordering of properties so `get` is placed before `set`.




# Code readability and catering for it

One thing that strikes me, is the different between reading a book and reading code.
A book has a narative, emotional characters, page numbers and switches beteen building and releasing tension.
Reading code, on the other hand, is about _absorbing_ intent, building mental models in your mind. Often with 
great frustration when the code gets too complex.

When we read code, we use techniques ranging from _scanning_ (fast-pased reading), to 
slow-pased _scrutiny_. 


              (code reading spectrum)
 
         fast                        slow 
       <---------------------------------->
    Scannning                     Scrutinizing


When discussing code readability, it often from the perspective slower-pased reading. I believe we do a great deal of _scanning_. My belief is rooted in a lot of vague facts

  * Predominant use of Mono-spaced fonts (contrary to books!)
  * Individual preferences with regard to Indent-size and file/folder organization
  * Predominant use of colors when reading and editing code 
  * Mild infuriation and/or diffculty in keeping concentration and attaining focus when faced with large portions of text, that do not contribute meaning.

This is important to understand, since when we discuss _code readability_ and deciding on initiatives to improve it, we can not cater for the whole spectrum. Obviously, reducing the 
amount of characters in a source code, makes scanning more effective, whereas code scrutiny is much less affected. I've compiled a set articles that deal with improving code readability here
http://firstclassthoughts.co.uk/Tags/Code_Readability.html 

So faced with a large code base, how do you improve its readability? _Effort_ is a key word here. Some code-improving actions require a lot of effort, others not so much.
Of course, the same goes for the effect on the code base. An example of a low effort initiaitive is code formatting making the code base look simiar in terms of overall structure. 
An example of a large effort initiative is ensuring test coverage, applying SRP, other design patterns, introducing service orchestration and so on.

To visualize this we need two graphs. One for each end of the spectrum.


              (scanning)                                                (scrutinizing)                 
                                          
         ^  Effort                                                   ^  Effort                           
         |                                                           |                                   
         |   o TDD                                                   |                        o TDD              
         |                                                           |                                    
         |   o Design patterns                                       |               o Design patterns           
         |                                                           |                                   
         |   o SRP                                                   |                    o SRP                  
         |                                                           |                                   
         |               o reformat                                  |   o reformat                      
         |                   o whitespacewarrior                     | o whitespacewarrior               
    -----+-------------------------->                           -----+-------------------------->  
         |                     Yield                                 |                     Yield           
         |                                                           |                                   
                      


`WhiteSpaceWarrior` is a tool that make scanning code significantly faster by improving the _signal to noice ratio of the code_ 
- i.e. removing all the stuff you get anoyed about when you speed-read code such as empty lines, empty comments, short meaningles comments.



# Language support

At the moment only C# is supported. 

However, I'm very interested in working toghether with people wanting whitespacewarrior-support for their favorite language!


# Running the tool

    Usage: WhiteSpaceWarrior [options] <Path>

    Arguments:
      Path                                                                  The path from which to recursively compress cs files

    Options:
      --version                                                             Show version information
      -v|--verbosity <VERBOSITY>                                            Set verbosity level
      -rpn|--remove-param-name-upto-nwords <REMOVE_PARAM_NAME_UPTO_NWORDS>  Remove <param name=".."> descriptions up to N words long. A low number such as "2" captures many useless comments.
      -rsn|--remove-summary-upto-nwords <REMOVE_SUMMARY_UPTO_NWORDS>        Remove <summary> descriptions up to N words long. A low number such as "2" captures many useless comments.
      -rr|--remove-regions                                                  Remove #region
      -rt|--remove-tags <REMOVE_TAGS>                                       Remove <tag> in ///-sections. Can be specified multiple times
      -nl|--no-logo                                                         Don't show logo
      -?|-h|--help                                                          Show help information



# Testing 

Install as a power tool locally from a local build

    > dotnet tool install --add-source C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\ --tool-path tools WhiteSpaceWarrior

and then

    > .\tools\WhiteSpaceWarrior.exe --help
