
namespace AdventOfCode.Puzzles._2025._06.Part1;

public class AoC2025Day6Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var problems = new List<string[]>();
        var lines = await inputReader.ReadAllLinesAsync();
        foreach (var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            problems.Add(parts);
        }

        ulong total = 0;
        for (int i = 0; i < problems[0].Length; i++)
        {
            var multiply = problems[problems.Count - 1][i] == "*";
            var currentResult = multiply ? 1UL : 0;
            for (int line = 0; line < problems.Count - 1; line++)
            {
                var number = ulong.Parse(problems[line][i]);
                if (multiply)
                {
                    currentResult *= number;
                }
                else
                {
                    currentResult += number;
                }
            }

            total += currentResult;
        }

        return total.ToString();
    }
}
