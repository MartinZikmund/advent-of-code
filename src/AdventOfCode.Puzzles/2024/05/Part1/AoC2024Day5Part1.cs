namespace AdventOfCode.Puzzles._2024._05.Part1;

public partial class AoC2024Day5Part1 : IPuzzleSolution
{
    private readonly List<(int pageLeft, int pageRight)> _rules = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        // Reading rules
        while (await inputReader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var parts = line.Split("|");
            var left = int.Parse(parts[0]);
            var right = int.Parse(parts[1]);
            _rules.Add((left, right));
        }

        int total = 0;
        // Read updates
        while (await inputReader.ReadLineAsync() is { } line)
        {
            var update = line.Split(",").Select(int.Parse).ToArray();
            if (IsUpdateValid(update))
            {
                total += update[update.Length / 2];
            }
        }

        return total.ToString();
    }

    private bool IsUpdateValid(int[] update)
    {
        for (int i = 0; i < update.Length; i++)
        {
            var currentNumber = update[i];
            foreach (var rule in _rules)
            {
                var (left, right) = rule;
                var leftIndex = Array.IndexOf(update, left);
                var rightIndex = Array.IndexOf(update, right);
                if (currentNumber == left && rightIndex != -1 && rightIndex < i)
                {
                    return false;
                }
                else if (currentNumber == right && leftIndex != -1 && Array.IndexOf(update, left) > i)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
