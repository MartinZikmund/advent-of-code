using System.Numerics;

namespace AdventOfCode.Puzzles._2024._11.Part1;

public partial class Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var input = await inputReader.ReadToEndAsync();
        var numbers = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currentState = numbers.ToList();
        for (int i = 0; i < 25; i++)
        {
            var newState = new List<string>();
            foreach (var number in currentState)
            {
                ApplyRules(number, newState);
            }

            currentState = newState;
        }

        return currentState.Count.ToString();
    }

    private void ApplyRules(string number, List<string> next)
    {
        if (number == "0")
        {
            next.Add("1");
        }
        else if (number.Length % 2 == 0)
        {
            var half = number.Length / 2;
            var left = number.Substring(0, half);
            var right = number.Substring(half).TrimStart('0');
            if (right == "")
            {
                right = "0";
            }

            next.Add(left);
            next.Add(right);
        }
        else
        {
            var bigNumber = BigInteger.Parse(number);
            var multiple = bigNumber * 2024;
            next.Add(multiple.ToString());
        }
    }
}
