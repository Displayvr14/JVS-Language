# JVS Compiler

JVS is a lightweight custom language that compiles `.jvs` source files into Windows Batch (`.bat`) scripts using a C# transpiler.

---

## 📦 Installation

### Option 1: Prebuilt (Recommended)
Download the source code and ensure you have:
```
{root}\JVS-Compiler\bin\Release\net10.0\win-x64\publish\JVS.exe
{root}\JVS-Compiler\bin\Release\net10.0\win-x64\publish\JVSrun.bat (optional helper script)
```
You can place them anywhere (recommended: `C:\tools\JVS\`).

---

### Option 2: Build from source

```bash
dotnet build
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
```
The executable will appear in:
```
bin\Release\net10.0\win-x64\publish\
```

## ▶️ How to Use
### 1. Create a .jvs file
```
var(x, 5)
var(y, 10)

add(x, $y)
print($x)
```
### 2. Compile manually

Run the compiler from terminal:
```bash
JVS.exe input.jvs output.bat
```
### 3. Run the generated file

After compiling, execute the output:
```bash
output.bat
```

### ⚡ Using the Run Script (Optional)
If you included run.bat, you can simplify the workflow:
```bash
JVSrun main.jvs
```
This automatically:
1. Compiles main.jvs → main.bat
2. Executes main.bat

## 🧠 Language Syntax
### Variables
```jvs
var(variable, 9)
var(variable2, $variable) // sets variable2 to variable
```
### printing
```jvs
var(variable, 9)
print(variable) // prints "variable" to the terminal
print($variable) // prints "9" to the terminal
```
### Math operations
```jvs
var(x, 6)
var(y, 3)

add(x, $y) // x + y -> x
sub(x, $y) // x - y -> x
mul(x, $y) // x * y -> x
div(x, $y) // x / y -> x
```
## 🔁 Compilation Flow
```
.jvs file
   ↓
JVS.exe (compiler)
   ↓
.generated .bat file
   ↓
Windows executes batch script
```
## ⚠️ Notes
* JVS is a transpiler, not a runtime or VM
* Output is standard Windows Batch files
* Requires .NET runtime (if not published as self-contained)
* Designed for learning compiler fundamentals
## 🛠 Troubleshooting
### “JVS is not recognized”
* Add the folder containing JVS.exe to your system PATH.
### Nothing happens when running
Check:
* output.bat file exists
* Run compiler manually:
```
JVS.exe main.jvs main.bat
```
### Wrong run command runs something else
Use:
```
.\run.bat main.jvs
```
