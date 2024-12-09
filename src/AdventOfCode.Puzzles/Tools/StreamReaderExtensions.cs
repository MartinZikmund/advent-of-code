namespace AdventOfCode.Puzzles.Tools;

internal static class StreamReaderExtensions
{
    public static async Task<List<string>> ReadAllLinesAsync(this StreamReader reader)
    {
        List<string> lines = new();
        while (await reader.ReadLineAsync() is { } line)
        {
            lines.Add(line);
        }
        return lines;
    }
}
