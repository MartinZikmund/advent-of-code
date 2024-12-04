using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2024._03.Part1;

public partial class AoC2024Day3Part1 : IPuzzleSolution
{
    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    public static partial Regex MulRegex();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var input = await inputReader.ReadToEndAsync();

        var matches = MulRegex().Matches(input);

        var total = 0;
        foreach (Match match in matches)
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            total += x * y;
        }

        return total.ToString();
    }
}
