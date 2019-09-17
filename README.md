# WhitespaceWarrior

![Logo](whitespacewarrior.png)

_Remove whitespace and cruft from source code, maing it easier to read and absorb._


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

# Other language support

At the moment only C# is supported. However, I'm very interested in working toghether with people wanting whitespacewarrior-support for their favorite language!

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


## run as dotnet core tool

	> dotnet .\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll


## Install as a power tool 

    > dotnet tool install --add-source C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\ --tool-path tools WhiteSpaceWarrior

and then

	> .\tools\WhiteSpaceWarrior.exe --help
