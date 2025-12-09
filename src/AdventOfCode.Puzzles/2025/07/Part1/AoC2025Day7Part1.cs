namespace AdventOfCode.Puzzles._2025._07.Part1;

public class AoC2025Day7Part1 : IPuzzleSolution
{
    private int _height;
    private int _width;
    private char[,] _map;
    private HashSet<Point> _splitters;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _height = lines.Count;
        _width = lines[0].Length;
        _map = new char[lines[0].Length, lines.Count];
        Point startingPoint = new Point();
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (var column = 0; column < line.Length; column++)
            {
                _map[column, row] = line[column];
                if (line[column] == 'S')
                {
                    startingPoint = new Point(column, row);
                }
            }
        }

        _splitters = new HashSet<Point>();

        Beam(startingPoint);

        return _splitters.Count.ToString();
    }

    private void Beam(Point currentPoint)
    {
        if (_map[currentPoint.X, currentPoint.Y] is '.' or 'S')
        {
            if (currentPoint.Y + 1 < _height)
            {
                Beam(new(currentPoint.X, currentPoint.Y + 1));
            }
        }
        else
        {
            if (_splitters.Contains(currentPoint))
            {
                return;
            }

            _splitters.Add(currentPoint);
            if (currentPoint.X - 1 >= 0)
            {
                Beam(new(currentPoint.X - 1, currentPoint.Y));
            }
            if (currentPoint.X + 1 < _width)
            {
                Beam(new(currentPoint.X + 1, currentPoint.Y));
            }
        }
    }
}
