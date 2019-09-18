# WhitespaceWarrior

_Remove whitespace and other "empty" content from source code, making it easier to read/browse._

![Logo](whitespacewarrior.png)


Code is not read like a book. It is not like a fairy tale, where momentum is built up and suddenly you find yourself 
unable to put down the book. 

Reading code is about _absorbing_ intent and building mental models in your mind.
I find that I tend to use strategies ranging from reading fast-pased scanning to slower scrutiny of sections. 


         fast                        slow 
       <---------------------------------->
    Scannning                     Scrutinizing


This is important to understand, since when we discuss _code readability_ and decide on initiatives to improve readability, we like only cater for a part of the spectrum. Obviously, reducing the 
amount of characters in a source code, makes scanning more effective, whereas code scrutiny is much less affected. Here code intentions have bigger effects. See 
http://firstclassthoughts.co.uk/Tags/Code_Readability.html for advice on improving code readability in general.

So faced with a large code base, how do you improve its readability? It is a balance between providing business value and cleaning up the code. 
_Effort_ is a key word here. Some code-improving actions require a lot of effort and other much less. Naturally, different actions also yield diffent amount of effects
on the code base. A low effort initiaitive is code formatting making. A large effort initiative is ensuring test coverage, applying SRP or other design patterns.

We visualize this idea by two graphs. One for each end of the spectrum:


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
         |                   Yield                                   |                   Yield           
         |                                                           |                                   
                      

So which do you choose? The activity with a high yield requiring a large effort, or do you start with the very low effort activities with a much lesser yield?
If I can choose an automated tool to do _some_ clean up for me, I'd prefer that any day over a manual effort. And since my tools are automatic work, 
I can soon after focus on the more laborious initiaitives.

`WhiteSpaceWarrior` is a tool that make scanning code significantly faster by improving the _signal to noice ratio of the code_ 
- i.e. removing all the stuff you get anoyed about when you speed-read code such as empty lines, empty comments, short meaningles comments.


# Installation

    > dotnet tool install --global WhiteSpaceWarrior 

Then you can use the command `WhiteSpaceWarrior` in your shell.


# C# Example

Before going into the details. Let's have a look at a code sample that 
well represents much of the code that I've experienced in my career.
You'll notice there is a lot of weird stuff in there. 

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
