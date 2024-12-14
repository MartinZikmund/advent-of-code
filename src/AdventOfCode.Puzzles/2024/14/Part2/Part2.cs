using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._14.Part2;

public partial class Part2 : IPuzzleSolution
{

    private const int MapWidth = 101;
    private const int MapHeight = 103;

    public record Robot(Point StartingPoint, Point Velocity)
    {
        public Point GetPosition(int seconds)
        {
            var finalX = (StartingPoint.X + Velocity.X * seconds) % MapWidth;
            var finalY = (StartingPoint.Y + Velocity.Y * seconds) % MapHeight;
            if (finalX < 0)
            {
                finalX += MapWidth;
            }
            if (finalY < 0)
            {
                finalY += MapHeight;
            }

            return new(finalX, finalY);
        }
    }

    private Robot[] _robots;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _robots = await ReadRobotsAsync(inputReader);

        var maxComponentSeconds = 0;
        var maxComponent = 0;

        for (var seconds = 0; seconds < 10000; seconds++)
        {
            var largestComponent = GetLargestComponent(seconds);
            if (largestComponent > maxComponent)
            {
                maxComponentSeconds = seconds;
                maxComponent = largestComponent;
                Debug.WriteLine($"Seconds: {seconds}, Largest Component: {largestComponent}");
            }
        }

        OutputMapAt(maxComponentSeconds);

        return maxComponentSeconds.ToString();
    }

    private void OutputMapAt(int seconds)
    {
        StringBuilder sb = new();
        var points = _robots.Select(r => r.GetPosition(seconds)).ToHashSet();
        for (var y = 0; y < MapHeight; y++)
        {
            for (var x = 0; x < MapWidth; x++)
            {
                sb.Append(points.Contains(new(x, y)) ? '#' : '.');
            }
            sb.AppendLine();
        }

        File.WriteAllText("output.txt", sb.ToString());
    }

    private int GetLargestComponent(int seconds)
    {
        HashSet<Point> visited = new();
        var points = _robots.Select(r => r.GetPosition(seconds)).ToHashSet();
        var largestComponent = 0;
        foreach (var point in points)
        {
            if (visited.Contains(point))
            {
                continue;
            }

            var componentSize = GetComponentSize(point, points, visited);
            if (componentSize > largestComponent)
            {
                largestComponent = componentSize;
            }
        }

        return largestComponent;
    }

    private int GetComponentSize(Point point, HashSet<Point> points, HashSet<Point> visited)
    {
        Queue<Point> queue = new();
        queue.Enqueue(point);
        visited.Add(point);
        var componentSize = 0;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            componentSize++;
            foreach (var direction in Directions.WithDiagonals)
            {
                var next = current + direction;
                if (visited.Contains(next) || !points.Contains(next))
                {
                    continue;
                }
                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return componentSize;
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
