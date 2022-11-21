## 0.2.0 -> 0.2.1
This update mainly brings dictionaries but it also brings a lot of bug fixes and improvements.  
Fixed the syntax warnings.  
Optimized arrays.  
Fixed the bug that made that you coudn't use a declared variable as a value to an array.  
Added multi line comments E.g /* */.  
Removed unused code.  
Improved the way `Write` and `WriteLine` print arrays and now dictionaries.





## 0.1.11 -> 0.2.0
Updated from .Net 6 to [.Net 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).  
Updated from [Antlrcs4.6.6](https://github.com/tunnelvisionlabs/antlr4cs/releases/tag/v4.6.6) to [Antlr4.11.1](https://github.com/antlr/antlr4/releases/tag/4.11.1).  
No more dynamic types used inside the interpreter, this only happened because of the new Generic maths added in .NET 7 and for now, basically all numbers are converted to doubles but this needs urgent change.  
The compiled executable is about 5.5 times smaller and the execution time is about 4.5 times faster, this is only possible with the new [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) introduced in .Net7.<br> Unfortunately it only works on win-x64 and Linux-x64 (it also works for win-arm64 and linux-arm64, but I don't own any ARM machines to compile it to those targets) for now, the other platforms still use [ReadyToRun](https://learn.microsoft.com/en-us/dotnet/core/deploying/ready-to-run).