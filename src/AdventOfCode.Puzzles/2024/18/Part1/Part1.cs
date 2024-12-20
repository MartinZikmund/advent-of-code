namespace AdventOfCode.Puzzles._2024._18.Part1;

public partial class Part1 : IPuzzleSolution
{
    private const int Width = 71;
    private const int Height = 71;
    private const int ByteCount = 1024;

    private char[,] _map = new char[Width, Height];

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _map = new char[Width, Height];
        for (int i = 0; i < ByteCount; i++)
        {
            var line = await inputReader.ReadLineAsync();
            var parts = line.Split(",");
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);

            _map[x, y] = '#';
        }

        var length = FindPath((0,0), (Width - 1, Height - 1));

        return length.ToString();
    }

    private int FindPath(Point start, Point end)
    {
        var queue = new Queue<(Point position, int steps)>();
        queue.Enqueue((start, 0));
        var visited = new HashSet<Point>();
        visited.Add(start);
        while (queue.Count > 0)
        {
            var (position, steps) = queue.Dequeue();
            if (position == end)
            {
                return steps;
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
                queue.Enqueue((next, steps + 1));
            }
        }

        return -1;
    }
}
