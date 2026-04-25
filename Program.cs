List<string> batchCode = new List<string>();

string resolveValue(string val)
{
    if (val.StartsWith("$"))
        return $"%{val.Substring(1)}%";

    return val;
}

string[] splitLine(string line, int lineIndex)
{
    if (!line.Contains('(') || !line.Contains(')'))
        throw new Exception($"Please close your parentheses at line {lineIndex + 1}");

    int start = line.IndexOf('(');
    int end = line.LastIndexOf(')');

    if (start == -1 || end == -1 || end <= start)
        throw new Exception($"Invalid syntax at line {lineIndex + 1}");

    string inside = line.Substring(start + 1, end - start - 1);

    string opCode = line.Split('(')[0].Trim();
    string[] operands = inside.Split(',', StringSplitOptions.RemoveEmptyEntries);

    List<string> output = new List<string> { opCode };

    foreach (string operand in operands)
        output.Add(operand.Trim());

    return output.ToArray();
}

void runLine(string line, int lineIndex)
{
    string[] code = splitLine(line, lineIndex);

    switch (code[0].ToLower())
    {
        case "var":
        {
            string val = resolveValue(code[2]);

            bool isMath = val.Contains('+') || val.Contains('-') ||
                          val.Contains('*') || val.Contains('/');

            bool isNum = int.TryParse(val, out _);

            if (isNum || isMath)
                batchCode.Add($"set /a {code[1]}={val}");
            else
                batchCode.Add($"set {code[1]}={val}");

            break;
        }

        case "print":
            batchCode.Add($"echo {resolveValue(code[1])}");
            break;

        case "add":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%+{resolveValue(code[2])}");
            break;

        case "sub":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%-{resolveValue(code[2])}");
            break;

        case "mul":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%*{resolveValue(code[2])}");
            break;

        case "div":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%/{resolveValue(code[2])}");
            break;

        case "label":
            batchCode.Add($":{code[1]}");
            break;

        case "jump":
            batchCode.Add($"goto {code[1]}");
            break;

        case "cmp":
        {
            string a = resolveValue(code[2]);
            string b = resolveValue(code[3]);

            batchCode.Add($"set {code[1]}=0");
            batchCode.Add($"if {a} GTR {b} set {code[1]}=1");
            batchCode.Add($"if {a} LSS {b} set {code[1]}=-1");

            break;
        }

        case "jumpg":
            batchCode.Add($"if defined {code[1]} if %{code[1]}%==1 goto {code[2]}");
            break;

        case "jumpe":
            batchCode.Add($"if defined {code[1]} if %{code[1]}%==0 goto {code[2]}");
            break;

        case "jumpl":
            batchCode.Add($"if defined {code[1]} if %{code[1]}%==-1 goto {code[2]}");
            break;

        default:
            throw new Exception($"Unknown opcode '{code[0]}' at line {lineIndex + 1}");
    }
}

void runFile(string path)
{
    string[] lines = File.ReadAllLines(path);

    batchCode.Add("@echo off");
    batchCode.Add("setlocal enabledelayedexpansion");

    for (int i = 0; i < lines.Length; i++)
    {
        string line = lines[i].Trim();

        if (line == "") continue;
        if (line.StartsWith("//")) continue;

        runLine(line, i);
    }
}

void writeFile(string path)
{
    File.WriteAllLines(path, batchCode);
    Console.WriteLine($"Compiled to {path}");
}

if (args.Length < 2)
{
    Console.WriteLine("Usage: JVS <input.jvs> <output.bat>");
    return;
}

runFile(args[0]);
writeFile(args[1]);