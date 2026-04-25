List<string> batchCode = new List<string>();
Dictionary<string, bool> variables = new Dictionary<string, bool>();

string resolveValue(string val){
    return val.StartsWith("$") ? $"%{val.Substring(1)}%" : val;
}

string[] splitLine(string line, int lineIndex)
{
    if (!line.Contains('(') || !line.Contains(')'))
        throw new Exception($"Please close your parentheses at line {lineIndex + 1}");

    int start     = line.IndexOf('(') + 1;
    int end       = line.IndexOf(')');
    string inside = line.Substring(start, end - start);

    string opCode     = line.Split('(')[0].Trim();
    string[] operands = inside.Split(',', StringSplitOptions.RemoveEmptyEntries);

    List<string> output = new List<string>();
    output.Add(opCode);
    foreach(string operand in operands)
        output.Add(operand.Trim());

    return output.ToArray();
}

void runLine(string line, int lineIndex)
{
    string[] code = splitLine(line, lineIndex);

    switch (code[0].ToLower())
    {
        case "var":
            string val = resolveValue(code[2]);
            bool isMath = val.Contains('+') || val.Contains('-') || 
                        val.Contains('*') || val.Contains('/');
            bool isNum  = int.TryParse(val, out _);
            
            if(isNum || isMath)
                batchCode.Add($"set /a {code[1]}={val}");
            else
                batchCode.Add($"set {code[1]}={val}");
    break;
        case "print":
            batchCode.Add($"echo {resolveValue(code[1])}");
            break;
        case "add":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%+{resolveValue(code[2])}");
            break;
        case "mul":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%*{resolveValue(code[2])}");
            break;
        case "div":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%/{resolveValue(code[2])}");
            break;
        case "sub":
            batchCode.Add($"set /a {code[1]}=%{code[1]}%-{resolveValue(code[2])}");
            break;
        default:
            throw new Exception($"Unknown opcode '{code[0]}' at line {lineIndex + 1}");
    }
}
void runFile(string path)
{
    string[] lines = File.ReadAllLines(path);
    batchCode.Add("@echo off");
    for(int i = 0; i < lines.Length; i++)
    {
        string line = lines[i].Trim();
        if(line == "") continue;
        if(line.StartsWith("//")) continue;
        runLine(line, i);
    }
}
void writeFile(string path)
{
    File.WriteAllLines(path, batchCode);
    Console.WriteLine($"Compiled to {path}");
}

runFile(args[0]);
writeFile(args[1]);