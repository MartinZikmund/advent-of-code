using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._07.Part2;

public partial class Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        ulong total = 0;

        while (await inputReader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(":");
            var expected = ulong.Parse(parts[0].Trim());
            var equation = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var numbers = equation.Select(ulong.Parse).ToArray();
            if (CanSolve(expected, numbers))
            {
                total += expected;
            }
        }

        return total.ToString();
    }

    private bool CanSolve(ulong expected, ulong[] numbers)
    {
        return Solve(numbers[0], 0, expected, numbers);
    }

    private bool Solve(ulong currentResult, int currentIndex, ulong expected, ulong[] numbers)
    {
        if (currentResult > expected)
        {
            return false;
        }

        if (currentIndex == numbers.Length - 1)
        {
            return currentResult == expected;
        }

        if (Solve(currentResult + numbers[currentIndex + 1], currentIndex + 1, expected, numbers))
        {
            return true;
        }

        if (Solve(currentResult * numbers[currentIndex + 1], currentIndex + 1, expected, numbers))
        {
            return true;
        }

        if (Solve(ulong.Parse(currentResult.ToString() + numbers[currentIndex + 1].ToString()), currentIndex + 1, expected, numbers))
        {
            return true;
        }

        return false;
    }
}
