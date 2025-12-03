namespace AdventOfCode.Puzzles._2025._03.Part2;

public class AoC2025Day3Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();

        var sum = 0UL;
        foreach (var line in lines)
        {
            var digits = line.Select(c => (byte)(c - '0')).ToArray();
            var best = Solve(digits);
            sum += best;
        }

        return sum.ToString();
    }

    private ulong Solve(byte[] line)
    {
        ulong[,] bestResults = new ulong[line.Length, 13];

        bestResults[0, 0] = 0;
        bestResults[0, 1] = line[0];

        for (var i = 1; i < line.Length; i++)
        {
            for (var j = 0; j <= 12; j++)
            {
                for (var previousIndex = 0; previousIndex < i; previousIndex++)
                {
                    if (j < 12)
                    {
                        var result = bestResults[previousIndex, j] * 10 + line[i];
                        if (bestResults[i, j + 1] < result)
                        {
                            bestResults[i, j + 1] = result;
                        }
                    }

                    if (bestResults[i, j] < bestResults[previousIndex, j])
                    {
                        bestResults[i, j] = bestResults[previousIndex, j];
                    }
                }
            }
        }

        return bestResults[line.Length - 1, 12];
    }
}
