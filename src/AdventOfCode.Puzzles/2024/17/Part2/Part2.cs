using System.Xml;

namespace AdventOfCode.Puzzles._2024._17.Part2;

public partial class Part2 : IPuzzleSolution
{
    private ulong _best = ulong.MaxValue;
    private string _expectedOutput = "2415751643550330";

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        // 2,4,1,5,7,5,1,6,4,3,5,5,0,3,3,0

        // A0 = 0-7
        // A1 = A0 * 8 - A0 * 8 + 7

        Solve(0UL, _expectedOutput.Length - 1);
        var output = Execute(107416732707226);
        return _best.ToString();
    }

    private void Solve(ulong currentA, int index)
    {
        if (index == -1)
        {
            _best = Math.Min(_best, currentA);
            return;
        }

        var next = _expectedOutput[index];
        for (int remainder = 0; remainder < 8; remainder++)
        {
            var nextA = currentA * 8 + (ulong)remainder;
            var result = Execute(nextA);
            if (_expectedOutput.EndsWith(result))
            {
                Solve(nextA, index - 1);
            }
        }
    }

    private string Execute(ulong a)
    {
        StringBuilder output = new();
        do
        {
            ulong b = a % 8;
            b = b ^ 5;
            ulong c = a / (ulong)Math.Pow(2, b);
            b = b ^ 6;
            b = b ^ c;
            output.Append((b % 8).ToString());
            a = a / 8;
        } while (a != 0);

        return output.ToString();
    }
}
