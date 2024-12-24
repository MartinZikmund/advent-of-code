namespace AdventOfCode.Puzzles._2024._24.Part1;

public partial class Part1 : IPuzzleSolution
{
    private List<Connection> _connections = new();
    private HashSet<string> _gates = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var inputValues = new Dictionary<string, int>();
        while (await inputReader.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
        {
            var parts = line.Split(": ");
            var gateName = parts[0];
            var value = int.Parse(parts[1]);
            inputValues.Add(gateName, value);
            _gates.Add(gateName);
        }

        while (await inputReader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(" ");
            var connection = new Connection
            {
                Input1 = parts[0],
                Input2 = parts[2],
                Output = parts[4],
                Operation = parts[1] switch
                {
                    "AND" => Operation.And,
                    "OR" => Operation.Or,
                    "XOR" => Operation.Xor,
                    _ => throw new InvalidOperationException()
                }
            };

            _gates.Add(connection.Input1);
            _gates.Add(connection.Input2);
            _gates.Add(connection.Output);
            _connections.Add(connection);
        }
        var zGates = _gates.Where(g => g.StartsWith("z")).OrderBy(g => g).Reverse().ToList();

        Evaluate(inputValues);

        var result = GetZValue(zGates, inputValues);
        return result.ToString();
    }

    private long GetZValue(List<string> zGates, Dictionary<string, int> values)
    {
        var binary = string.Join("", zGates.Select(g => values[g]));
        return Convert.ToInt64(binary, 2);
    }

    private void Evaluate(Dictionary<string, int> values)
    {
        var gatesToProcess = new Queue<string>();
        var processedConnections = new HashSet<Connection>();
        while (values.Count < _gates.Count)
        {
            foreach (var connection in _connections)
            {
                if (!processedConnections.Contains(connection) &&
                    values.ContainsKey(connection.Input1) &&
                    values.ContainsKey(connection.Input2))
                {
                    EvaluateConnection(connection, values);
                    processedConnections.Add(connection);
                }
            }
        }

    }

    private void EvaluateConnection(Connection connection, Dictionary<string, int> values)
    {
        var input1 = values[connection.Input1];
        var input2 = values[connection.Input2];
        var output = connection.Operation switch
        {
            Operation.And => input1 & input2,
            Operation.Or => input1 | input2,
            Operation.Xor => input1 ^ input2,
            _ => throw new InvalidOperationException()
        };
        values[connection.Output] = output;
    }

    public enum Operation
    {
        And,
        Or,
        Xor
    }

    public class Connection
    {
        public string Input1 { get; set; }

        public string Input2 { get; set; }

        public string Output { get; set; }

        public Operation Operation { get; set; }
    }
}
