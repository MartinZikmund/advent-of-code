using System.Diagnostics;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._12.Part2;

public partial class Part2 : IPuzzleSolution
{
    private int _width;
    private int _height;

    private char[,] _map;
    private int[,] _shapes;
    Dictionary<int, int> _shapeAreas = new Dictionary<int, int>();
    Dictionary<int, int> _shapeSides = new Dictionary<int, int>();
    Dictionary<int, char> _shapeNames = new Dictionary<int, char>();

    private int _shapeId;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;

        _map = new char[_width, _height];
        _shapes = new int[_width, _height];
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
                if (_shapes[x, y] == 0)
                {
                    var (area, perimeter) = FindGroup((x, y));
                    _shapeAreas[_shapes[x, y]] = area;
                }
            }
        }

        foreach (var shape in _shapeAreas.Keys)
        {
            _shapeSides[shape] = 0;
        }

        CountSides();

        foreach (var shape in _shapeAreas.Keys)
        {
            total += _shapeSides[shape] * _shapeAreas[shape];
        }

        return total.ToString();
    }

    private void CountSides()
    {
        foreach (var direction in new Point[] { (1, 0), (-1, 0) })
        {
            for (int x = 0; x < _width; x++)
            {
                var lastShapeId = 0;
                var sideLength = 0;
                for (int y = 0; y < _height; y++)
                {
                    (lastShapeId, sideLength) = TrackSides(new Point(x, y), direction, lastShapeId, sideLength);
                }

                if (sideLength != 0)
                {
                    _shapeSides[lastShapeId]++;
                }
            }
        }

        foreach (var direction in new Point[] { (0, 1), (0, -1) })
        {
            for (int y = 0; y < _height; y++)
            {
                var lastShapeId = 0;
                var sideLength = 0;
                for (int x = 0; x < _width; x++)
                {
                    (lastShapeId, sideLength) = TrackSides(new Point(x, y), direction, lastShapeId, sideLength);
                }

                if (sideLength != 0)
                {
                    _shapeSides[lastShapeId]++;
                }
            }
        }

        (int lastShapeId, int sideLength) TrackSides(Point currentPoint, Point neighborDirection, int lastShapeId, int sideLength)
        {
            if (lastShapeId != _shapes[currentPoint.X, currentPoint.Y] && lastShapeId != 0 && sideLength != 0)
            {
                _shapeSides[lastShapeId]++;
                sideLength = 0;
            }

            lastShapeId = _shapes[currentPoint.X, currentPoint.Y];
            var neighbor = currentPoint + neighborDirection;
            if (!IsInBounds(neighbor) || _shapes[neighbor.X, neighbor.Y] != lastShapeId)
            {
                sideLength++;
                return (lastShapeId, sideLength);
            }
            else if (sideLength != 0)
            {
                _shapeSides[lastShapeId]++;
                sideLength = 0;
            }

            return (lastShapeId, sideLength);
        }
    }

    private (int area, int perimeter) FindGroup(Point startingPoint)
    {
        var shapeId = ++_shapeId;
        _shapeNames[shapeId] = _map[startingPoint.X, startingPoint.Y];
        var totalPerimeter = 0;
        HashSet<Point> shapePoints = new HashSet<Point>();

        Queue<Point> queue = new();
        _shapes[startingPoint.X, startingPoint.Y] = shapeId;

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
                        if (_shapes[neighbor.X, neighbor.Y] == 0)
                        {
                            _shapes[neighbor.X, neighbor.Y] = shapeId;
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
