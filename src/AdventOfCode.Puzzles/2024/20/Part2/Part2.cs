using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Puzzles._2024._20.Part2;

public partial class Part2 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private int[,] _map;
    private Point _start;
    private Point _end;

    private const int WantToSave = 100;
    private const int CheatTime = 20;

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

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var distance = Math.Abs(start.X - x) + Math.Abs(start.Y - y);
                if (distance > CheatTime)
                {
                    continue;
                }

                var saved = _map[x, y] - _map[start.X, start.Y] - distance;
                if (saved >= WantToSave)
                {
                    targets.Add(new Point(x, y));
                }
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
