using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2025._11.Part2;

public class Solution : IPuzzleSolution
{
    private Dictionary<string, string[]> _targets;
    private Dictionary<(string node, bool fft, bool dac), ulong> _pathCounts = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();


        _targets = new Dictionary<string, string[]>();
        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var key = parts[0];
            var values = parts[1].Split(" ", StringSplitOptions.TrimEntries).ToArray();
            _targets[key] = values;
        }

        var visited = new HashSet<string>();
        visited.Add("svr");
        return CountPaths("svr", false, false, visited).ToString();
    }

    private ulong CountPaths(string current, bool fft, bool dac, HashSet<string> visited)
    {
        if (current == "dac")
        {
            dac = true;
        }

        if (current == "fft")
        {
            fft = true;
        }

        if (current == "out")
        {
            _pathCounts[("out", fft, dac)] = fft && dac ? 1UL : 0UL;
            return _pathCounts[("out", fft, dac)];
        }

        if (!_targets.ContainsKey(current))
        {
            return 0;
        }

        var targets = _targets[current];
        var total = 0UL;

        foreach (var target in targets.Where(t => !visited.Contains(t)))
        {
            visited.Add(target);
            if (_pathCounts.TryGetValue((target, fft, dac), out var cachedPaths))
            {
                total += cachedPaths;
            }
            else
            {
                var pathsForNode = CountPaths(target, fft, dac, visited);
                _pathCounts[(target, fft, dac)] = pathsForNode;
                total += pathsForNode;
            }
            visited.Remove(target);
        }

        return total;
    }
}
