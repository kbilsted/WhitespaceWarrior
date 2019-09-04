# WhitespaceWarrior
Remove superfluous code lines to improve readability


# Run the tool

	Usage: WhiteSpaceWarrior [options] <Path>

	Arguments:
	  Path                        The path from which to recursevely compress cs files

	Options:
	  -v|--verbosity <VERBOSITY>  Either use 0 or 1
	  -s|--skip-regions           Skip removing #region
	  -?|-h|--help                Show help information


## run as dotnet core tool

	> dotnet .\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll


## Install as a power tool 

    > dotnet tool install --add-source C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\ --tool-path tools WhiteSpaceWarrior

and then

	> .\tools\WhiteSpaceWarrior.exe --help
