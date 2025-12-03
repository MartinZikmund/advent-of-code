namespace AdventOfCode.Puzzles._2025._03.Part1;

public class AoC2025Day3Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();

        var sum = 0;
        foreach (var line in lines)
        {
            var digits = line.Select(c => c - '0').ToArray();
            var max = digits.Take(digits.Length - 1).Max();
            var index = Array.IndexOf(digits, max);
            var nextHighest = digits.Skip(index + 1).Max();
            sum += max * 10 + nextHighest;
        }

        return sum.ToString();
    }
}
