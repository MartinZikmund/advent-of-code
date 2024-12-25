namespace AdventOfCode.Puzzles._2024._24;

public class Connection
{
    private Connection(string input1, string input2, string output, Operation operation)
    {
        Input1 = input1;
        Input2 = input2;
        Output = output;
        Operation = operation;
    }

    public string Input1 { get; }

    public string Input2 { get; }

    public string Output { get; private set; }

    public Operation Operation { get; }

    public bool CanEvaluate(Dictionary<string, int> knownGates) =>
        knownGates.ContainsKey(Input1) && knownGates.ContainsKey(Input2);

    public void Evaluate(Dictionary<string, int> knownGates)
    {
        if (!CanEvaluate(knownGates))
        {
            throw new InvalidOperationException("Gate inputs not known.");
        }

        var input1 = knownGates[Input1];
        var input2 = knownGates[Input2];
        var output = Operation switch
        {
            Operation.And => input1 & input2,
            Operation.Or => input1 | input2,
            Operation.Xor => input1 ^ input2,
            _ => throw new InvalidOperationException()
        };

        knownGates[Output] = output;
    }

    public static Connection ParseConnection(string line)
    {
        var parts = line.Split(" ");
        var input1 = parts[0];
        var input2 = parts[2];
        var output = parts[4];
        var operation = parts[1] switch
        {
            "AND" => Operation.And,
            "OR" => Operation.Or,
            "XOR" => Operation.Xor,
            _ => throw new InvalidOperationException()
        };

        return new Connection(input1, input2, output, operation);
    }

    public void SwapOutput(Connection otherConnection)
    {
        var temp = Output;
        Output = otherConnection.Output;
        otherConnection.Output = temp;
    }
}
