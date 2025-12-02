namespace AdventOfCode.Puzzles._2025._01.Part2;

public class AoC2025Day1Part2 : IPuzzleSolution
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

            var fullRotations = distance / 100;
            counter += fullRotations;

            var startLocation = currentLocation;

            currentLocation += direction * (distance % 100);

            if (startLocation != 0 && (currentLocation < 0 || currentLocation > 100))
            {
                counter++;
            }
            
            currentLocation %= 100;
            if (currentLocation < 0)
            { 
                currentLocation += 100;
            }

            if (startLocation != 0 && currentLocation == 0)
            {
                counter++;
            }
        }

        return counter.ToString();
    }
}
