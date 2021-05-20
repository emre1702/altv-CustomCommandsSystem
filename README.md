<p align="center">
  <h2 align="center">RAGE:MP Custom Command System</h2>
  <p align="center">A custom command system for RAGE:MP, written in C#.</p>
</p>


## Features

* Register commands in different assemblies
* Unregister commands in specific assemblies
* Add custom parameter converters (with Task support)
* Add custom requirement checkers to command methods
* Execute commands manually
* Configure settings (like error messages)
* Add aliases to command methods
* Remaining text support
* Default values for command parameters support  
* You can help decide what to implement next.  
  
You want more? Add an [issue](https://github.com/emre1702/RAGEMP-CustomCommandSystem/issues) and help make that system better!
  
  
## How do I use it?

1. Install the NuGet package (url coming soon)
2. [Use this code at clientside](https://github.com/emre1702/RAGEMP-CustomCommandSystem/blob/master/Integration_Client/CommandFetcher.cs).  
The code there cancels the default command and triggers the custom command.
3. Implement it. Use the [Wiki](https://github.com/emre1702/RAGEMP-CustomCommandSystem/wiki) for information.


