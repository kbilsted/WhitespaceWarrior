# WhitespaceWarrior
Remove superfluous code lines to improve readability


# Run the tool

	Usage: WhiteSpaceWarrior [options] <Path>

	Arguments:
	  Path                             The path from which to recursevely compress cs files

	Options:
	  --version                        Show version information
	  -v|--verbosity <VERBOSITY>       Set verbosity level
	  -rr|--remove-regions             Remove #region
	  -rt|--remove-tags <REMOVE_TAGS>  Remove <tag> in /// sections. Can be specified multiple times
	  -?|-h|--help                     Show help information

## run as dotnet core tool

	> dotnet .\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll


## Install as a power tool 

    > dotnet tool install --add-source C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\ --tool-path tools WhiteSpaceWarrior

and then

	> .\tools\WhiteSpaceWarrior.exe --help
