using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._14.Part1;

public partial class Part1 : IPuzzleSolution
{
    public record Robot(Point StartingPoint, Point Velocity);

    private const int MapWidth = 101;
    private const int MapHeight = 103;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var robots = await ReadRobotsAsync(inputReader);

        var finalRobots = new List<Point>();
        foreach (var robot in robots)
        {
            var finalX = (robot.StartingPoint.X + robot.Velocity.X * 100) % MapWidth;
            var finalY = (robot.StartingPoint.Y + robot.Velocity.Y * 100) % MapHeight;
            if (finalX < 0)
            {
                finalX += MapWidth;
            }
            if (finalY < 0)
            {
                finalY += MapHeight;
            }

            finalRobots.Add(new(finalX, finalY));
        }

        var quadrant1 = 0;
        var quadrant2 = 0;
        var quadrant3 = 0;
        var quadrant4 = 0;

        foreach (var finalRobot in finalRobots)
        {
            if (finalRobot.X < MapWidth / 2 && finalRobot.Y < MapHeight / 2)
            {
                quadrant1++;
            }
            else if (finalRobot.X > MapWidth / 2 && finalRobot.Y < MapHeight / 2)
            {
                quadrant2++;
            }
            else if (finalRobot.X < MapWidth / 2 && finalRobot.Y > MapHeight / 2)
            {
                quadrant3++;
            }
            else if (finalRobot.X > MapWidth / 2 && finalRobot.Y > MapHeight / 2)
            {
                quadrant4++;
            }
        }

        return (quadrant1 * quadrant2 * quadrant3 * quadrant4).ToString();
    }

    private async Task<Robot[]> ReadRobotsAsync(StreamReader reader)
    {
        var robots = new List<Robot>();
        while (await reader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(' ');
            var startingPoint = parts[0].Substring(2).Split(',');
            var velocity = parts[1].Substring(2).Split(',');
            robots.Add(new(
                new(int.Parse(startingPoint[0]), int.Parse(startingPoint[1])),
                new(int.Parse(velocity[0]), int.Parse(velocity[1]))));
        }

        return robots.ToArray();
    }
}
