namespace AdventOfCode.Puzzles._2024._18.Part2;

public partial class Part2 : IPuzzleSolution
{
    private const int Width = 71;
    private const int Height = 71;
    private const int ByteCount = 1024;

    private char[,] _map = new char[Width, Height];

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _map = new char[Width, Height];
        HashSet<Point> lastPath = new();
        for (int i = 0; ; i++)
        {
            var line = await inputReader.ReadLineAsync();
            var parts = line.Split(",");
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);

            _map[x, y] = '#';
            if (lastPath.Count == 0 || lastPath.Contains((x, y)))
            {
                (var length, lastPath) = FindPath((0, 0), (Width - 1, Height - 1));

                if (length == -1)
                {
                    return x + "," + y;
                }
            }
        }
    }

    private (int length, HashSet<Point> points) FindPath(Point start, Point end)
    {
        var queue = new Queue<(Point position, int steps)>();
        queue.Enqueue((start, 0));
        var visited = new HashSet<Point>();
        var previous = new Dictionary<Point, Point>();
        visited.Add(start);
        while (queue.Count > 0)
        {
            var (position, steps) = queue.Dequeue();
            if (position == end)
            {
                var path = new HashSet<Point>();
                var current = position;
                while (current != start)
                {
                    path.Add(current);
                    current = previous[current];
                }

                return (steps, path);
            }

            foreach (var direction in Directions.WithoutDiagonals)
            {
                var next = position + direction;
                if ((next.X < 0 || next.X >= Width || next.Y < 0 || next.Y >= Height) ||
                    (_map[next.X, next.Y] == '#') ||
                    visited.Contains(next))
                {
                    continue;
                }

                visited.Add(next);
                previous[next] = position;
                queue.Enqueue((next, steps + 1));
            }
        }

        return (-1, null);
    }
}
