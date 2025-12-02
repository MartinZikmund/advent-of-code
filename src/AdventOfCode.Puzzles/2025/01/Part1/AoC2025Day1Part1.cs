namespace AdventOfCode.Puzzles._2025._01.Part1;

public class AoC2025Day1Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();

        var counter = 0;
        var currentLocation = 50;

        foreach (var line in lines)
        {
            var direction = line[0] == 'L' ? -1 : 1;
            var distance = int.Parse(line[1..]);

            currentLocation += direction * distance;
            currentLocation %= 100;

            if (currentLocation == 0)
            {
                counter++;
            }
        }

        return counter.ToString();
    }
}
