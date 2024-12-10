using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._10.Part2;

public partial class Part2 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private int[,] _map;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;

        List<Point> zeroes = new List<Point>();

        _map = new int[_width, _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _map[x, y] = lines[y][x] - '0';

                if (_map[x, y] == 0)
                {
                    zeroes.Add(new Point(x, y));
                }
            }
        }

        var total = 0;
        foreach (var zero in zeroes)
        {
            total += GetScore(zero);
        }

        return total.ToString();
    }

    private int GetScore(Point start)
    {
        var score = 0;
        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var value = _map[current.X, current.Y];
            if (value == 9)
            {
                score++;
                continue;
            }

            foreach (var direction in Directions.WithoutDiagonals)
            {
                var next = current + direction;
                if ((next.X >= 0 && next.X < _width && next.Y >= 0 && next.Y < _height) &&
                    _map[next.X, next.Y] == (value + 1))
                {
                    queue.Enqueue(next);
                }
            }
        }

        return score;
    }
}
