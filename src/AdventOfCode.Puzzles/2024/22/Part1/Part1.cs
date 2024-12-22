namespace AdventOfCode.Puzzles._2024._22.Part1;

public partial class Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        ulong sum = 0;
        foreach (var line in await inputReader.ReadAllLinesAsync())
        {
            ulong secretNumber = ulong.Parse(line);
            for (int i = 0; i < 2000; i++)
            {
                secretNumber = Evolve(secretNumber);
            }

            sum += secretNumber;
        }

        return sum.ToString();
    }

    private ulong Mix(ulong a, ulong b) => a ^ b;

    private ulong Prune(ulong a) => a % 16777216;

    private ulong Evolve(ulong secretNumber)
    {
        var multiple = secretNumber * 64UL;
        secretNumber = Mix(secretNumber, multiple);
        secretNumber = Prune(secretNumber);

        var divide = secretNumber / 32Ul;
        secretNumber = Mix(secretNumber, divide);
        secretNumber = Prune(secretNumber);

        multiple = secretNumber * 2048;
        secretNumber = Mix(secretNumber, multiple);
        secretNumber = Prune(secretNumber);

        return secretNumber;
    }
}
