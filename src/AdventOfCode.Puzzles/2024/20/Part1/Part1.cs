namespace AdventOfCode.Puzzles._2024._20.Part1;

public partial class Part1 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private int[,] _map;
    private Point _start;
    private Point _end;

    private const int WantToSave = 100;
    private const int CheatTime = 2;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _height = lines.Count;
        _width = lines[0].Length;

        _map = new int[_width, _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _map[x, y] = lines[y][x] == '#' ? -1 : 0;
                if (lines[y][x] == 'S')
                {
                    _start = new Point(x, y);
                }
                else if (lines[y][x] == 'E')
                {
                    _end = new Point(x, y);
                }
            }
        }

        FindDistances(_start, _end);

        return CountCheats().ToString();
    }

    private int CountCheats()
    {
        var cheats = 0;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_map[x, y] == -1)
                {
                    continue;
                }
                var start = new Point(x, y);
                cheats += CountCheats(start);
            }
        }

        return cheats;
    }

    private int CountCheats(Point start)
    {
        HashSet<Point> targets = new HashSet<Point>();
        Queue<(Point position, int steps)> queue = new();
        queue.Enqueue((start, 0));
        while (queue.Count > 0)
        {
            var (position, steps) = queue.Dequeue();

            if (steps == CheatTime && _map[position.X, position.Y] != -1)
            {
                var saved = _map[position.X, position.Y] - _map[start.X, start.Y] - CheatTime;
                if (saved >= WantToSave)
                {
                    targets.Add(position);
                }
            }

            if (steps >= CheatTime)
            {
                continue;
            }

            foreach (var direction in Directions.WithoutDiagonals)
            {
                var next = position + direction;
                if ((next.X < 0 || next.X >= _width || next.Y < 0 || next.Y >= _height))
                {
                    continue;
                }

                queue.Enqueue((next, steps + 1));
            }
        }

        return targets.Count;
    }

    private void FindDistances(Point start, Point end)
    {
        var currentPoint = start;

        while (currentPoint != end)
        {
            foreach (var direction in Directions.WithoutDiagonals)
            {
                var next = currentPoint + direction;
                if (_map[next.X, next.Y] == 0 && next != start)
                {
                    _map[next.X, next.Y] = _map[currentPoint.X, currentPoint.Y] + 1;
                    currentPoint = next;
                    break;
                }
            }
        }
    }
}
