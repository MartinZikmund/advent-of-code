using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2024._03.Part2;

public partial class AoC2024Day3Part2 : IPuzzleSolution
{
    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    public static partial Regex MulRegex();

    [GeneratedRegex("do\\(\\)")]
    public static partial Regex DoRegex();

    [GeneratedRegex("don't\\(\\)")]
    public static partial Regex DontRegex();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var input = await inputReader.ReadToEndAsync();

        var matches = MulRegex().Matches(input);
        var isEnabled = true;

        var doMatches = DoRegex().Matches(input);
        var dontMatches = DontRegex().Matches(input);

        var doIndicies = doMatches.Select(m => m.Index).ToList();
        var dontIndicies = dontMatches.Select(m => m.Index).ToList();

        var mergedIndicies = doIndicies.Concat(dontIndicies).OrderBy(i => i).ToArray();
        var nextIndex = 0;

        var total = 0;
        foreach (Match match in matches)
        {
            var matchIndex = match.Index;
            if (nextIndex < mergedIndicies.Length && matchIndex > mergedIndicies[nextIndex])
            {
                var isDo = doIndicies.Contains(mergedIndicies[nextIndex]);
                isEnabled = isDo;
                nextIndex++;
            }

            if (isEnabled)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                total += x * y;
            }
        }

        return total.ToString();
    }
}
