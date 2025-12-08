namespace AdventOfCode.Puzzles._2025._05.Part2;

public class AoC2025Day5Part2 : IPuzzleSolution
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

        var orderedIntervals = freshIntervals
            .OrderBy(i => i.Start)
            .ThenBy(i => i.End)
            .ToList();

        var numberOfFreshIngredients = 0UL;
        var currentStart = orderedIntervals[0].Start;
        var currentEnd = orderedIntervals[0].End;
        foreach (var interval in orderedIntervals.Skip(1))
        {
            if (interval.End <= currentEnd)
            {
                continue;
            }

            if (interval.Start > currentEnd + 1)
            {
                numberOfFreshIngredients += currentEnd - currentStart + 1;

                currentStart = interval.Start;
            }

            currentEnd = interval.End;
        }

        numberOfFreshIngredients += currentEnd - currentStart + 1;

        return numberOfFreshIngredients.ToString();
    }
}
