namespace AdventOfCode.Puzzles._2024._24.Part2;

public partial class Part2 : IPuzzleSolution
{
    private List<Connection> _connections = new();
    private HashSet<string> _gates = new();

    private List<(string gateA, string gateB)> _swappedOutputs = new();

    private List<string> _sortedOutputs = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        await ReadInputAsync(inputReader);
        
        _swappedOutputs.Add(("z12", "vdc"));
        _swappedOutputs.Add(("z21", "nhn"));
        _swappedOutputs.Add(("tvb", "khg"));
        _swappedOutputs.Add(("z33", "gst"));

        int lastFaultyIndex = 33;

        if (_swappedOutputs.Count > 4)
        {
            foreach (var swap in _swappedOutputs)
            {
                SwapOutputs(swap.gateA, swap.gateB);
            }

            SortConnectionsTopologically();

            var fixes = FindFixesForFaultyConnection(lastFaultyIndex);

            foreach (var fix in fixes)
            {
                Console.WriteLine($"{fix.gate1} -> {fix.gate2} #{fix.faultIndex}");
            }
        }

        var names = string.Join(",",
            _swappedOutputs
                .SelectMany(o => new string[] { o.gateA, o.gateB })
                .OrderBy(n => n)
                .ToList());

        return names;
    }

    private (string gate1, string gate2, int faultIndex)[] FindFixesForFaultyConnection(int firstInvalidZGate)
    {
        var potentialFixes = new List<(string gate1, string gate2, int faultIndex)>();
        var confirmedGates = GetDependencies(firstInvalidZGate - 1);
        var suspectedGates = GetDependencies(firstInvalidZGate).Except(confirmedGates);
        var swapGates = _sortedOutputs.Except(confirmedGates).ToList();
        var faultIndex = _sortedOutputs.IndexOf(GetNumberedGateName('z', firstInvalidZGate));

        foreach(var suspectedGate in suspectedGates)
        {
            foreach (var swapGate in swapGates)
            {
                var newFaultIndex = TestGateSwap(suspectedGate, swapGate, firstInvalidZGate);

                if (newFaultIndex > firstInvalidZGate)
                {
                    potentialFixes.Add((suspectedGate, swapGate, newFaultIndex));
                    Console.WriteLine($"Potential fix: {suspectedGate} -> {swapGate} #{newFaultIndex}");
                }
            }
        }

        return potentialFixes.ToArray();
    }

    private HashSet<string> GetDependencies(int zGate)
    {
        HashSet<string> confirmedNodes = new();
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(GetNumberedGateName('z', zGate));

        while (queue.Count > 0)
        {
            var currentOutput = queue.Dequeue();
            confirmedNodes.Add(currentOutput);

            var connection = _connections.FirstOrDefault(c => c.Output == currentOutput);
            if (connection is null)
            {
                continue;
            }

            queue.Enqueue(connection.Input1);
            queue.Enqueue(connection.Input2);
        }

        return confirmedNodes;
    }

    private async Task ReadInputAsync(StreamReader inputReader)
    {
        while (await inputReader.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
        {
            // Skip x and y gate inputs
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
    }

    private int TestGateSwap(string gate1, string gate2, int fault)
    {
        SwapOutputs(gate1, gate2);

        var minFault = int.MaxValue;

        // Test random number pairs
        for (int i = 0; i < 20; i++)
        {
            var randomA = Random.Shared.NextInt64();
            // Keep top 45 bits only
            randomA &= 0x1FFFFFFFFFFFFF;
            var randomB = Random.Shared.NextInt64();
            randomB &= 0x1FFFFFFFFFFFFF;
            
            var faults = TestInput(randomA, randomB);
            if (faults.Length > 0)
            {
                minFault = Math.Min(faults.Min(f => f.index), minFault);
                if (minFault <= fault)
                {
                    break;
                }
            }
        }

        // Test 1 s on each position
        for (int firstIndex = 0; firstIndex < 45; firstIndex++)
        {
            var a = 1L << firstIndex;
            for (int secondIndex = 0; secondIndex < 45; secondIndex++)
            {
                var b = 1L << secondIndex;

                var faults = TestInput(a, b);
                if (faults.Length > 0)
                {
                    minFault = Math.Min(faults.Min(f => f.index), minFault);
                    if (minFault <= fault)
                    {
                        break;
                    }
                }
            }
        }

        SwapOutputs(gate1, gate2);

        return minFault;
    }

    private void SwapOutputs(string gate1, string gate2)
    {
        var connection1 = _connections.Single(c => c.Output == gate1);
        var connection2 = _connections.Single(c => c.Output == gate2);

        connection1.SwapOutput(connection2);
    }

    private void SortConnectionsTopologically()
    {
        var sortedConnections = new List<Connection>();

        var knownGates = new HashSet<string>();
        for (int i = 0; i < 45; i++)
        {
            knownGates.Add(GetNumberedGateName('x', i));
            knownGates.Add(GetNumberedGateName('y', i));
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

    private (int index, int expected, int actual)[] TestInput(long a, long b)
    {
        var aBinary = Convert.ToString(a, 2).PadLeft(45);
        var bBinary = Convert.ToString(b, 2).PadLeft(45);

        var gateValues = new Dictionary<string, int>();
        for (int i = 0; i < 45; i++)
        {
            var index = aBinary.Length - i - 1;
            gateValues[GetNumberedGateName('x', i)] = aBinary[index] == '1' ? 1 : 0;
        }

        for (int i = 0; i < 45; i++)
        {
            var index = bBinary.Length - i - 1;
            gateValues[GetNumberedGateName('y', i)] = bBinary[index] == '1' ? 1 : 0;
        }

        var expected = a + b;
        var expectedBinary = Convert.ToString(expected, 2).PadLeft(45);

        var faults = new List<(int index, int expected, int actual)>();
        if (Evaluate(gateValues))
        {
            for (int i = 0; i < 45; i++)
            {
                var actualZ = gateValues[GetNumberedGateName('z', i)];
                var expectedZ = expectedBinary[expectedBinary.Length - i - 1] == '1' ? 1 : 0;

                if (actualZ != expectedZ)
                {
                    faults.Add((i, expectedZ, actualZ));
                }
            }
        }
        else
        {
            faults.Add((-1, -1, -1));
        }

        return faults.ToArray();
    }

    private bool Evaluate(Dictionary<string, int> values)
    {
        var gatesToProcess = new Queue<string>();
        var processed = new HashSet<Connection>();
        while (processed.Count != _connections.Count)
        {
            bool changed = false;

            foreach (var connection in _connections)
            {
                if (!processed.Contains(connection) && connection.CanEvaluate(values))
                {
                    connection.Evaluate(values);
                    processed.Add(connection);
                    changed = true;
                }
            }

            if (!changed)
            {
                return false;
            }
        }

        return true;
    }

    private string GetNumberedGateName(char prefix, int number) =>
        prefix + number.ToString("00");
}
