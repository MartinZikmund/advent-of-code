namespace AdventOfCode.Puzzles._2025._11.Part1;

public class Solution : IPuzzleSolution
{
    private Dictionary<string, string[]> _targets;

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

        return CountPaths("you").ToString();
    }

    private int CountPaths(string current)
    {
        if (current == "out")
        {
            return 1;
        }

        var targets = _targets[current];
        var total = 0;
        foreach (var target in targets)
        {
            total += CountPaths(target);
        }

        return total;
    }
}
