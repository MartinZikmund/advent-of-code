
namespace AdventOfCode.Puzzles._2024._01._Part1;

public class AoC2024Day1Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var numbers1 = new List<int>();
        var numbers2 = new List<int>();
        while (await inputReader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            numbers1.Add(int.Parse(parts[0]));
            numbers2.Add(int.Parse(parts[1]));
        }

        numbers1.Sort();
        numbers2.Sort();

        var result = 0;
        for (var i = 0; i < numbers1.Count; i++)
        {
            result += Math.Abs(numbers1[i] - numbers2[i]);
        }

        return result.ToString();
    }
}
