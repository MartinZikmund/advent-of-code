using System.Diagnostics;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._12.Part1;

public partial class Part1 : IPuzzleSolution
{
    private int _width;
    private int _height;

    private char[,] _map;
    private HashSet<Point> _visited = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;

        _map = new char[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _map[x, y] = lines[y][x];
            }
        }

        int total = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (!_visited.Contains(new Point(x, y)))
                {
                    var (area, perimeter) = FindGroup((x, y));

                    total += area * perimeter;
                }
            }
        }

        return total.ToString();
    }

    private (int area, int perimeter) FindGroup(Point startingPoint)
    {
        var totalPerimeter = 0;
        HashSet<Point> shapePoints = new HashSet<Point>();

        Queue<Point> queue = new();
        _visited.Add(startingPoint);
        queue.Enqueue(startingPoint);
        while (queue.Count > 0)
        {
            var point = queue.Dequeue();

            shapePoints.Add(point);
            int pointPerimeter = 0;
            foreach (var direction in Directions.WithoutDiagonals)
            {
                var neighbor = point + direction;
                if (IsInBounds(neighbor))
                {
                    var type = _map[neighbor.X, neighbor.Y];
                    if (type == _map[point.X, point.Y])
                    {
                        if (!_visited.Contains(neighbor))
                        {
                            _visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                    else
                    {
                        pointPerimeter++;
                    }
                }
                else
                {
                    pointPerimeter++;
                }
            }
            totalPerimeter += pointPerimeter;
        }

        return (shapePoints.Count, totalPerimeter);
    }

    private bool IsInBounds(Point point) => point.X >= 0 && point.X < _width && point.Y >= 0 && point.Y < _height;
}
