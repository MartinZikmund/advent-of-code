namespace AdventOfCode.Puzzles._2024._01.Part2;

public class AoC2024Day1Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var numbers1 = new List<int>();
        var counts2 = new Dictionary<int, int>();
        while (await inputReader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            numbers1.Add(int.Parse(parts[0]));

            var number2 = int.Parse(parts[1]);
            if (counts2.ContainsKey(number2))
            {
                counts2[number2]++;
            }
            else
            {
                counts2[number2] = 1;
            }
        }

        var result = 0;
        for (var i = 0; i < numbers1.Count; i++)
        {
            if (!counts2.ContainsKey(numbers1[i]))
            {
                continue;
            }
            result += numbers1[i] * counts2[numbers1[i]];
        }

        return result.ToString();
    }
}
