namespace AdventOfCode.Puzzles._2024._17.Part1;

public partial class Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        return Execute(47792830, 0, 0);
    }

    private string Execute(long a, long b, long c)
    {
        StringBuilder output = new();

        do
        {
            b = a % 8;
            b = b ^ 5;
            c = a / (long)Math.Pow(2, b);
            b = b ^ 6;
            b = b ^ c;
            output.Append((b % 8).ToString() + ",");
            a = a / 8;
        } while (a != 0);

        return output.Remove(output.Length - 1, 1).ToString();
    }
}
