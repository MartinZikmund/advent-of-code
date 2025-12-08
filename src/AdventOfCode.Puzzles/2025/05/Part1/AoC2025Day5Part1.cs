
namespace AdventOfCode.Puzzles._2025._05.Part1;

public class AoC2025Day5Part1 : IPuzzleSolution
{
    public record Interval(ulong Start, ulong End);

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var freshIntervals = new List<Interval>();
        while (await inputReader.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
        {
            var parts = line.Split("-");
            var start = ulong.Parse(parts[0]);
            var end = ulong.Parse(parts[1]);
            var interval = new Interval((ulong)start, (ulong)end);
            freshIntervals.Add(interval);
        }

        var freshIngredientCount = 0;
        while (await inputReader.ReadLineAsync() is { } line)
        {
            var ingredientNumber = ulong.Parse(line);
            var containsIngredient = freshIntervals.Any(interval =>
                ingredientNumber >= interval.Start && ingredientNumber <= interval.End);
            if (containsIngredient)
            {
                freshIngredientCount++;
            }
        }

        return freshIngredientCount.ToString();
    }
}
