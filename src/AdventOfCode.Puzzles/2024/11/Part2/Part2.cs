using System.Numerics;
using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._11.Part2;

public partial class Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var input = await inputReader.ReadToEndAsync();
        var numbers = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, long> currentState = new();
        numbers.GroupBy(x => x).ToList().ForEach(x => currentState.Add(x.Key, x.Count()));

        for (int i = 0; i < 75; i++)
        {
            var newState = new Dictionary<string, long>();
            foreach (var (number, count) in currentState)
            {
                ApplyRules(number, count, newState);
            }

            currentState = newState;
        }

        return currentState.Sum(n => n.Value).ToString();
    }

    private void ApplyRules(string number, long count, Dictionary<string, long> next)
    {
        void AddOrUpdate(string key, long value)
        {
            if (next.ContainsKey(key))
            {
                next[key] += value;
            }
            else
            {
                next.Add(key, value);
            }
        }

        if (number == "0")
        {
            AddOrUpdate("1", count);
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

            AddOrUpdate(left, count);
            AddOrUpdate(right, count);
        }
        else
        {
            var bigNumber = BigInteger.Parse(number);
            var multiple = bigNumber * 2024;
            AddOrUpdate(multiple.ToString(), count);
        }
    }
}
