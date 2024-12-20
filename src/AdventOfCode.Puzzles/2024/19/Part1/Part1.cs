namespace AdventOfCode.Puzzles._2024._19.Part1;

public partial class Part1 : IPuzzleSolution
{
    private string[] _patterns;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _patterns = (await inputReader.ReadLineAsync()).Split(", ");
        await inputReader.ReadLineAsync();
        int possibleCount = 0;
        while (await inputReader.ReadLineAsync() is { } towel)
        {
            if (IsPossible("", towel))
            {
                possibleCount++;
            }
        }

        return possibleCount.ToString();
    }

    private bool IsPossible(string current, string towel)
    {
        if (current == towel)
        {
            return true;
        }

        for (int i = 0; i < _patterns.Length; i++)
        {
            var newStart = current + _patterns[i];
            if (towel.StartsWith(newStart) && IsPossible(newStart, towel))
            {
                return true;
            }
        }

        return false;
    }
}
