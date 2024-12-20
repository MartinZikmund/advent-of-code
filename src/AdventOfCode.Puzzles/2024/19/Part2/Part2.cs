namespace AdventOfCode.Puzzles._2024._19.Part2;

public partial class Part2 : IPuzzleSolution
{
    private HashSet<string> _patterns;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _patterns = new HashSet<string>((await inputReader.ReadLineAsync()).Split(", "));
        await inputReader.ReadLineAsync();
        ulong possibleCount = 0;
        while (await inputReader.ReadLineAsync() is { } towel)
        {
            possibleCount += CountPossible(towel);
        }

        return possibleCount.ToString();
    }

    private ulong CountPossible(string towel)
    {
        ulong[] ways = new ulong[towel.Length + 1];
        ways[0] = 1;

        for (var length = 1; length <= towel.Length; length++)
        {
            for (int previousLength = 0; previousLength < length; previousLength++)
            {
                if (ways[previousLength] == 0)
                {
                    continue;
                }

                var current = towel.Substring(previousLength, length - previousLength);
                if (_patterns.Contains(current))
                {
                    ways[length] += ways[previousLength];
                }
            }
        }

        return ways[towel.Length];
    }
}
