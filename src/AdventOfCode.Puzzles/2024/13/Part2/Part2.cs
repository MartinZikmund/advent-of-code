using System.Diagnostics;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._13.Part2;

public partial class Part2 : IPuzzleSolution
{
    public Task<string> SolveAsync(StreamReader inputReader)
    {
        long total = 0;
        while (TryReadClawMachine(inputReader, out var clawMachine))
        {
            var price = SolveClawMachine(clawMachine);
            if (price > 0)
            {
                total += price;
            }
        }

        return Task.FromResult(total.ToString());
    }

    private long SolveClawMachine(ClawMachine clawMachine)
    {
        var buttonA = clawMachine.ButtonA;
        var buttonB = clawMachine.ButtonB;
        var prize = clawMachine.Prize;

        // a*ax + b*bx = prizeX
        // a*ay + b*by = prizeY
        // a = (prizeX - b*bx) / ax
        // ((prizeX - b*bx) / ax) * ay + b*by = prizeY
        // ((prizeX / ax) - (b*bx / ax)) * ay + b*by = prizeY
        // (prizeX / ax) * ay - (b*bx / ax) * ay + b*by = prizeY
        // - (b * bx / ax) * ay + b * by = prizeY - (prizeX / ax) * ay
        // b * by - (b* bx / ax) * ay = prizeY - (prizeX / ax) * ay
        // b * (by - (bx / ax) * ay) = prizeY - (prizeX / ax) * ay
        // b = (prizeY - (prizeX / ax) * ay) / (by - (bx / ax) * ay)

        var prizeX = 10000000000000 + (double)prize.X;
        var prizeY = 10000000000000 + (double)prize.Y;
        var buttonAX = (double)buttonA.X;
        var buttonAY = (double)buttonA.Y;
        var buttonBX = (double)buttonB.X;
        var buttonBY = (double)buttonB.Y;

        long b = (long)Math.Round((prizeY - (prizeX / buttonAX) * buttonAY) / (buttonBY - (buttonBX / buttonAX) * buttonAY));
        long a = (long)Math.Round((prizeX - b * buttonBX) / buttonAX);

        var actualX = a * buttonAX + b * buttonBX;
        var actualY = a * buttonAY + b * buttonBY;
        if (actualX == prizeX && actualY == prizeY && a >= 0 && b >= 0)
        {
            return a * 3 + b;
        }

        return -1;
    }

    private bool TryReadClawMachine(StreamReader reader, out ClawMachine clawMachine)
    {
        clawMachine = default;
        var line = reader.ReadLine();
        if (line == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            line = reader.ReadLine();
        }

        var buttonALine = line;
        var buttonBLine = reader.ReadLine();
        var prizeLine = reader.ReadLine();

        var parts = buttonALine.Split(":")[1].Split(',');
        var buttonAX = int.Parse(parts[0].Trim().Substring(2));
        var buttonAY = int.Parse(parts[1].Trim().Substring(2));

        parts = buttonBLine.Split(":")[1].Split(',');
        var buttonBX = int.Parse(parts[0].Trim().Substring(2));
        var buttonBY = int.Parse(parts[1].Trim().Substring(2));

        parts = prizeLine.Split(":")[1].Split(',');
        var prizeX = int.Parse(parts[0].Trim().Substring(2));
        var prizeY = int.Parse(parts[1].Trim().Substring(2));

        clawMachine = new ClawMachine(
            (buttonAX, buttonAY),
            (buttonBX, buttonBY),
            (prizeX, prizeY)
        );

        return true;
    }

    public record ClawMachine(Point ButtonA, Point ButtonB, Point Prize);
}
