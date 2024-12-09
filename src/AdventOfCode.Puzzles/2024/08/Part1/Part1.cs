using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._08.Part1;

public partial class Part1 : IPuzzleSolution
{
    private int _width;
    private int _height;

    private Dictionary<char, List<Point>> _antennas = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;

        var grid = new char[_width, _height];
        for (var y = 0; y < _height; y++)
        {
            var line = lines[y];
            for (var x = 0; x < _width; x++)
            {
                var character = line[x];
                grid[x, y] = character;

                if (character != '.')
                {
                    if (!_antennas.TryGetValue(character, out var pointList))
                    {
                        pointList = new List<Point>();
                        _antennas[character] = pointList;
                    }

                    pointList.Add((x, y));
                }
            }
        }

        HashSet<Point> antinodes = new();

        foreach (var antennaType in _antennas)
        {
            var points = antennaType.Value;
            for (var antenna1 = 0; antenna1 < points.Count; antenna1++)
            {
                for (int antenna2 = antenna1 + 1; antenna2 < points.Count; antenna2++)
                {
                    var diff = points[antenna2] - points[antenna1];

                    antinodes.Add(points[antenna1] - diff);
                    antinodes.Add(points[antenna2] + diff);
                }
            }
        }

        return antinodes.Where(p => IsInBounds(p)).Count().ToString();
    }

    private bool IsInBounds(Point p)
    {
        return p.X >= 0 && p.X < _width && p.Y >= 0 && p.Y < _height;
    }
}
