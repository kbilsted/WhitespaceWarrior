`WhiteSpaceWarrior` is a tool that make reading code significantly faster by improving the _signal to noise ratio of the code_. It 
 removes all the stuff you get anoyed about when you speed-read code, such as empty lines, empty comments, and short meaningles comments. In other words,
 it removes _noise_.

![Logo](whitespacewarrior.png)



# The impact on code readbility when removing noise from the code

Let's illustrate the effect of a noisy code base. The example is perhaps not a 100% accurate representation of all code bases, however, I've accumulated all the different kinds
of code noise I've encountered in my career. It also show cases the capabilities of `whitespacewarrior`. 

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

Let's reformat with `whitespacewarrior`. Notice once all the noise is gone, how much easier it is to attain and keep focus.
The code is reduced from _49_ lines to just _16_ lines! 

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


Here is a short summary of what kind of whitespace that is cleaned up:

* Empty lines around `{` and `}`
* Empty or short `<summary>`,`<param name="..">`, `<returns>`, ...
* Shortening short `<summary>` down to 1 line from the standard 3 lines
* Regions `#region` and `#endregion`
* Newlines around properties (getters, setters) and also reordering of properties so `get` is placed before `set`.



# Installation

    > dotnet tool install --global WhiteSpaceWarrior 

Then you can use the command `WhiteSpaceWarrior` in your shell.

Or visit the releases at https://github.com/kbilsted/WhitespaceWarrior/releases



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


When discussing code readability, it often from the perspective slower-pased reading. 
I believe we do a great deal of _scanning_. My belief is rooted in a lot of facts

  * Only source code is typeset with Mono-spaced fonts. Books, magazines , newspapers, etc. all use proportional fonts
  * Most programmers have an oppinion and preference with regards to indentation size 
  * Most programmers prefer syntax highlighting with colours both in editors/IDE's and when reading code-oriented web pages
  * People express diffculty in keeping concentration and focus when faced with code bases with large portions of text, that do not contribute meaning.


So faced with a large code base, how do you improve its readability? I think _Effort_ is a key word here. Some code-improving actions require a lot of effort, others not so much. 
With a tool like `WhiteSpaceWarrior` we can process a very large code base and review all the changes within an hour! That will beat most other initiatives by magnitutes.
Of course, the same goes for the effect on the code base. An example of a low effort initiaitive is code formatting making the code base look simiar in terms of overall structure. 
An example of a large effort initiative is ensuring test coverage, applying SRP, SOLID, FailFast, design patterns, introducing service orchestration and so on.

To visualize this we need two graphs. One for each end of the spectrum of our code reading strategies.


              (scanning)                                                 (scrutinizing)                 
                                          
         ^  Effort                                                   ^  Effort                           
         |                                                           |                                   
         |   o TDD                                                   |                       o TDD              
         |                                                           |                                    
         |   o Design patterns                                       |               o Design patterns           
         |                                                           |                                   
         |               o SRP                                       |                        o SRP                  
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                                                           |                                   
         |                   o whitespacewarrior                     | o whitespacewarrior               
    -----+-------------------------->                           -----+-------------------------->  
         |                     Readability                           |                     Readability
         |                                                           |                                   
                      

Obviously, reducing the amount of characters in a source code, makes scanning more effective, whereas code scrutiny is much less affected. 
I've compiled a set articles that deal with improving code readability here http://firstclassthoughts.co.uk/Tags/Code_Readability.html 


# Language support

At the moment only C# is supported. 

However, I'm very interested in working toghether with people wanting whitespacewarrior-support for their favorite language!


# Configuration

WhiteSpaceWarrior is very configurable


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
