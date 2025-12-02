namespace AdventOfCode.Puzzles._2025._02.Part2;

public class AoC2025Day1Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var line = await inputReader.ReadLineAsync();
        var parts = line!.Split(',');
        var total = 0UL;
        foreach (var part in parts)
        {
            var bounds = part.Split('-');
            var lowerBound = ulong.Parse(bounds[0]);
            var upperBound = ulong.Parse(bounds[1]);
            var invalidSum = SumInvalidIds(lowerBound, upperBound);
            total += invalidSum;
        }

        return total.ToString();
    }

    private ulong SumInvalidIds(ulong lowerBound, ulong upperBound)
    {
        ulong sum = 0;
        for (var currentNumber = lowerBound; currentNumber <= upperBound; currentNumber++)
        {
            if (IsInvalid(currentNumber))
            {
                sum += currentNumber;
            }
        }
        return sum;
    }

    private bool IsInvalid(ulong currentNumber)
    {
        var numberAsString = currentNumber.ToString();

        for (var subsequenceLength = 1; subsequenceLength <= numberAsString.Length / 2; subsequenceLength++)
        {
            var firstSubsequence = numberAsString.Substring(0, subsequenceLength);
            var repeatedSubsequence = string.Concat(Enumerable.Repeat(firstSubsequence, numberAsString.Length / subsequenceLength));
            if (repeatedSubsequence == numberAsString)
            {
                return true;
            }
        }

        return false;
    }
}
