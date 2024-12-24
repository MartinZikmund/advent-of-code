namespace AdventOfCode.Puzzles._2024._24.Part2;

public partial class Part2 : IPuzzleSolution
{
    private List<Connection> _connections = new();
    private HashSet<string> _gates = new();

    private List<string> _sortedOutputs = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        while (await inputReader.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
        {
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

        SwapOutputs("z12", "vdc");
        SwapOutputs("z21", "nhn");
        //SwapOutputs("z25", "z45");
        //SwapOutputs("z33", "vnr");
        //SwapOutputs("z38", "kmm");

        var names = string.Join(",", new List<string> { "z12", "z21", "z33", "z38", "vdc", "nhn", "vnr", "kmm" }.OrderBy(n => n).ToList());
        Console.WriteLine(names);
        SortConnectionsTopologically();

        int confirmedSolution = 25;
        bool performSwap = false;
        var faultIndex = _sortedOutputs.IndexOf("z25");
        for (var gateToSwap1 = faultIndex; gateToSwap1 >= 0; gateToSwap1--)
        {
            for (var gateToSwap2 = gateToSwap1 + 1; gateToSwap2 < _sortedOutputs.Count; gateToSwap2++)
            {
                var gate1 = _sortedOutputs[gateToSwap1];
                var gate2 = _sortedOutputs[gateToSwap2];

                var minFault = int.MaxValue;

                for (int i = 0; i < 10000; i++)
                {
                    var randomA = Random.Shared.NextInt64();
                    // Keep top 45 bits only
                    randomA &= 0x1FFFFFFFFFFFFF;
                    var randomB = Random.Shared.NextInt64();
                    randomB &= 0x1FFFFFFFFFFFFF;
                    try
                    {
                        if (!performSwap)
                        {
                            gate1 = "";
                            gate2 = "";
                        }
                        var faults = TestInput(randomA, randomB, zGates, gate1, gate2);
                        if (faults.Length == 0)
                        {
                            // Solved!
                            break;
                        }

                        minFault = Math.Min(faults.Min(f => f.index), minFault);
                    }
                    catch (InvalidOperationException)
                    {
                        minFault = 0;
                        break;
                    }
                    //foreach (var fault in faults)
                    //{
                    //    Console.WriteLine($"{fault.index}: Expected {fault.expected}, got {fault.actual}");
                    //}
                    //Console.WriteLine();
                }

                if (minFault <= confirmedSolution && minFault < int.MaxValue)
                {
                    Console.WriteLine("Correct swap " + gate1 + " for " + gate2);
                }
            }
        }
        return "";
    }

    private void SwapOutputs(string gate1, string gate2)
    {
        foreach (var connection in _connections)
        {
            if (connection.Output == gate1)
            {
                connection.Output = gate2;
            }
            else if (connection.Output == gate2)
            {
                connection.Output = gate1;
            }
        }
    }

    private void SortConnectionsTopologically()
    {
        var sortedConnections = new List<Connection>();

        var knownGates = new HashSet<string>();
        for (int i = 0; i < 45; i++)
        {
            knownGates.Add("x" + i.ToString("00"));
            knownGates.Add("y" + i.ToString("00"));
        }

        while (_connections.Count > 0)
        {
            foreach (var connection in _connections)
            {
                if (knownGates.Contains(connection.Input1) && knownGates.Contains(connection.Input2))
                {
                    sortedConnections.Add(connection);
                    knownGates.Add(connection.Output);
                    _sortedOutputs.Add(connection.Output);
                    break;
                }
            }

            _connections.Remove(sortedConnections[sortedConnections.Count - 1]);
        }

        _connections = sortedConnections;
    }

    private (int index, int expected, int actual)[] TestInput(long a, long b, List<string> zGates, string gateSwap1, string gateSwap2)
    {
        var aBinary = Convert.ToString(a, 2).PadLeft(45);
        var bBinary = Convert.ToString(b, 2).PadLeft(45);

        var inputValues = new Dictionary<string, int>();
        for (int i = 0; i < 45; i++)
        {
            var index = aBinary.Length - i - 1;
            inputValues["x" + i.ToString("00")] = aBinary[index] == '1' ? 1 : 0;
        }

        for (int i = 0; i < 45; i++)
        {
            var index = bBinary.Length - i - 1;
            inputValues["y" + i.ToString("00")] = bBinary[index] == '1' ? 1 : 0;
        }

        var expected = a + b;
        var expectedBinary = Convert.ToString(expected, 2).PadLeft(45);

        Evaluate(inputValues, gateSwap1, gateSwap2);

        var faults = new List<(int index, int expected, int actual)>();
        for (int i = 0; i < 45; i++)
        {
            var actualZ = inputValues["z" + i.ToString("00")];
            var expectedZ = expectedBinary[expectedBinary.Length - i - 1] == '1' ? 1 : 0;

            if (actualZ != expectedZ)
            {
                faults.Add((i, expectedZ, actualZ));
            }
        }

        return faults.ToArray();
    }

    private long GetZValue(List<string> zGates, Dictionary<string, int> values)
    {
        var binary = string.Join("", zGates.Select(g => values[g]));
        return Convert.ToInt64(binary, 2);
    }

    private void Evaluate(Dictionary<string, int> values, string gateSwap1, string gateSwap2)
    {
        var gatesToProcess = new Queue<string>();
        var processedConnections = new HashSet<Connection>();
        while (values.Count < _gates.Count)
        {
            int processed = 0;

            foreach (var connection in _connections)
            {
                if (!processedConnections.Contains(connection) &&
                    values.ContainsKey(connection.Input1) &&
                    values.ContainsKey(connection.Input2))
                {
                    EvaluateConnection(connection, values, gateSwap1, gateSwap2);
                    processedConnections.Add(connection);
                    processed++;
                }
            }

            if (processed == 0)
            {
                throw new InvalidOperationException("No progress made");
            }
        }

    }

    private void EvaluateConnection(Connection connection, Dictionary<string, int> values, string gateSwap1, string gateSwap2)
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

        if (connection.Output == gateSwap1)
        {
            values[gateSwap2] = output;
        }
        else if (connection.Output == gateSwap2)
        {
            values[gateSwap1] = output;
        }
        else
        {
            values[connection.Output] = output;
        }
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
