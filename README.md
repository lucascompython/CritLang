# Crit (the programming language)

## What is Crit?

Crit is an interpreted dynamic programming language made with C# and [ANTLR4](https://www.antlr.org/).  
This language is still very experimental and is no were to be used for production.  
Why the name? Because in League of Legends, **no crit = no bitches**.  

The language is [turing complete](https://en.wikipedia.org/wiki/Turing_completeness) (I think) so theoretically you can solve any computational problem with Crit!  

The syntax is somewhat similar to Golang's syntax.  
Check the Change log [here](CHANGELOG.MD).

## Code Preview

```rust
num = 5;
#Comment
if num > 2 {
    Write(num + " is bigger than 2.");
} #else if work as expected
else {
    WriteLine(num + " is smaller than 2.");
}

while num < 10 { 
    WriteLine(num);
    num = num + 1;
}
else {
    WriteLine("num was already bigger than 10.");
}
```

You can look [here](https://github.com/lucascompython/CritLang/tree/master/Examples) for more an implemantion of the [Sieve of Eratosthenes](https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes) in crit!

## Documentation

Check [here](https://github.com/lucascompython/CritLang/wiki/Language-Defenition) for the language definition.

## Tips and Tricks

You can have a else block after a while loop declaration to avoid a if statement.  
To have syntax highlighting you can set the language to Golang or Rust, I've tested both and they look fine to me.  
This language also has a `until` keyword, which is just like the `while` keyword but with the opposite condition.

```rust
until num > 10 {
    WriteLine(num);
    num = num + 1;
}
```

## Run It
Download the binary [here](https://github.com/lucascompython/CritLang/tags).  
OR  
Build it
```powershell
git clone https://github.com/lucascompython/CritLang.git
cd CritLang/CritLang
./build.ps1 -help
```

## TODOs

- [X] Documentation.
- [X] Automatically detect the length of a number and assign to it the corret type (int, long, float and double)
- [X] Add Python like dictionaries.
- [ ] Integrate [NANQL](https://github.com/lucascompython/NANQL) with Crit's dictionaries and arrays.
- [ ] Add the hability of making functions.
- [ ] Seperate code into different files.
- [ ] Add Crit's own types & remove most object types.
- [ ] Add the hability of importing other files.
- [ ] Add interactive mode (with a REPL).
- [ ] Make a proper std lib.
- [ ] Add for loops.
- [ ] Add a proper break keyword.
- [ ] Add readable error messages.
- [ ] Optimizing the interpreter.
- [ ] Make a compiler.

<!--## How to get it

You can get it from just cloning this repository and then running it (`dotnet run`).<br />
Or you can clone this repository and then install it globally as a dotnet package (`./install_globally.ps1`) and then just use `crit`.<br />
Or download the executable [here](https://github.com/lucascompython/CritLang/releases) and then you can just use that file.-->

## Contributions

Please feel free to help.  
All help is appreciated!

## Known Bugs

Right now you can't use arrays inside arrays nor inside dictionaries and vice versa. This will be fixed in the next update.

## License

This project is licensed under the GPL3 license.
