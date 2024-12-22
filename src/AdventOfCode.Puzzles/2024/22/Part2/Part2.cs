namespace AdventOfCode.Puzzles._2024._22.Part2;

public partial class Part2 : IPuzzleSolution
{
    private Dictionary<string, int> _sequenceSums = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        foreach (var line in await inputReader.ReadAllLinesAsync())
        {
            var currentSums = new Dictionary<string, int>();
            var lastDiffs = new LinkedList<int>();

            ulong secretNumber = ulong.Parse(line);
            var previousLastDigit = GetLastDigit(secretNumber);
            for (int i = 0; i < 1999; i++)
            {
                secretNumber = Evolve(secretNumber);
                var newLastDigit = GetLastDigit(secretNumber);

                var diff = (int)(newLastDigit - previousLastDigit);
                lastDiffs.AddLast(diff);
                if (lastDiffs.Count == 4)
                {
                    var diffString = string.Join(",", lastDiffs.Select(d => d.ToString()));
                    if (!currentSums.ContainsKey(diffString))
                    {
                        currentSums[diffString] = newLastDigit;
                    }
                    lastDiffs.RemoveFirst();
                }

                previousLastDigit = newLastDigit;
            }

            foreach (var (key, value) in currentSums)
            {
                if (!_sequenceSums.ContainsKey(key))
                {
                    _sequenceSums[key] = value;
                }
                else
                {
                    _sequenceSums[key] += value;
                }
            }
        }

        return _sequenceSums.Values.Max().ToString();
    }

    private int GetLastDigit(ulong number) => (int)(number % 10);

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
