dotnet C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll --no-logo .
git add .
"Visually compress comments and clear out blank lines and blank comments"
start gitextensions commit -wait

dotnet C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll --no-logo -rr .
git add .
"Remove regions The use of regions is a code smell indicating the class/method/... is too big"
start gitextensions commit -wait

dotnet C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll --no-logo  -rpn 1 -rsn 1 -rrn 1 .
git add .
"Remove 1-word documentation"
start gitextensions commit -wait

dotnet C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll --no-logo  -rpn 2 -rsn 2 -rrn 2 .
git add .
"Remove 2-word documentation"
start gitextensions commit -wait

dotnet C:\src\WhitespaceWarrior\WhiteSpaceWarrior\bin\Debug\netcoreapp2.1\WhiteSpaceWarrior.dll --no-logo  -rpn 3 -rsn 3 -rrn 3 .
git add .
"Remove 3-word documentation"
start gitextensions commit -wait