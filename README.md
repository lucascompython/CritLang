# Crit (the programming language)

## What is Crit?
Crit is a interpreted dynamic programming language made with C# and [ANTLR4](https://www.antlr.org/) made just because I can.
This language is still very experimental and is no were to be used for production.
Why the name? Because in League of Legends, **no crit = no bitches**.
The syntax is somewhat similar to Golang's syntax.


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
You can look [here](CritLang/primes.crit) for more "real world" example.

## Tips and Tricks
You can have a else block after a while loop declaration to avoid a if statement.
To have syntax highligting you can set the language to Golang or Rust, I've tested both and they look fine to me.


## TODOs

- [ ] Add arrays, python-like dictionaries and some more basic data structures.
- [ ] Add a special block of code to query with [Nanql](https://github.com/lucascompython/NANQL).
- [ ] Add the hability of making functions.
- [ ] Make a proper std lib.
- [ ] Add for loops.
- [ ] Add local variables.
- [ ] Add a proper break keyword.
- [ ] Add readable error messages.
- [ ] Optimizing the interpreter.
- [ ] Make a compiler.


## How to get it

You can get it from just cloning this repository and then running it (`dotnet run`).<br />
Or you can clone this repository and then install it globally as a dotnet package (`./install_globally.ps1`) and then just use `crit`<br />
Or download the executable [here](https://github.com/lucascompython/CritLang/releases) and then you can just use that file.




## Contributions 
Please feel free to help because I don't really know what im doing!


## Known Bugs
Sometimes for some reason semicolon are not needed.


## License
This project is licensed under the GPL3 license.
