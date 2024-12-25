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

        // Read connections
        while (await inputReader.ReadLineAsync() is { } line)
        {
            var connection = Connection.ParseConnection(line);

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
                    connection.CanEvaluate(values))
                {
                    connection.Evaluate(values);
                    processedConnections.Add(connection);
                }
            }
        }

    }
}
