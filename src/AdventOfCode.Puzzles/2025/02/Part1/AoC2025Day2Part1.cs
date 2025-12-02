namespace AdventOfCode.Puzzles._2025._02.Part1;

public class AoC2025Day2Part1 : IPuzzleSolution
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
        var count = 0UL;
        var numberAsString = lowerBound.ToString();
        var length = numberAsString.Length;
        if (length % 2 != 0)
        {
            numberAsString = "0" + numberAsString;
        }

        var firstHalf = numberAsString[..(numberAsString.Length / 2)];

        var currentNumber = ulong.Parse(firstHalf);

        while (true)
        {
            var fullNumber = currentNumber.ToString() + currentNumber.ToString();

            var fullNumberAsUlong = ulong.Parse(fullNumber);

            if (fullNumberAsUlong >= lowerBound && fullNumberAsUlong <= upperBound)
            {
                count += fullNumberAsUlong;
            }

            if (fullNumberAsUlong > upperBound)
            {
                return count;
            }

            currentNumber++;
        }
    }
}
