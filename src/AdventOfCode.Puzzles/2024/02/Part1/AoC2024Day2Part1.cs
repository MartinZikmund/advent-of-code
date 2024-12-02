namespace AdventOfCode.Puzzles._2024._02.Part1;

public class AoC2024Day2Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var safeCount = 0;

        while(await inputReader.ReadLineAsync() is { } line)
        {
            var report = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (IsSafe(report))
            {
                safeCount++;
            }
        }

        return safeCount.ToString();
    }

    public bool IsSafe(List<int> report)
    {
        if (report.Count < 2)
        {
            return true;
        }

        var firstDiff = report[1] - report[0];

        if (firstDiff == 0 || Math.Abs(firstDiff) > 3)
        {
            return false;
        }

        var expectedSgn = firstDiff / Math.Abs(firstDiff);

        for (int i = 1; i < report.Count - 1; i++)
        {
            var diff = report[i + 1] - report[i];
            if (diff == 0 || Math.Abs(diff) > 3)
            {
                return false;
            }

            var sgn = diff / Math.Abs(diff);
            if (sgn != expectedSgn)
            {
                return false;
            }
        }

        return true;
    }
}
