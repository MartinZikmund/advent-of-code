namespace AdventOfCode.Puzzles._2025._07.Part2;

public class AoC2025Day7Part2 : IPuzzleSolution
{
    private int _height;
    private int _width;
    private char[,] _map;
    private long[,] _paths;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _height = lines.Count;
        _width = lines[0].Length;
        _map = new char[lines[0].Length, lines.Count];
        _paths = new long[lines[0].Length, lines.Count];
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

        return CountPaths(startingPoint).ToString();
    }

    private long CountPaths(Point currentPoint)
    {
        if (_map[currentPoint.X, currentPoint.Y] is '.' or 'S')
        {
            if (currentPoint.Y + 1 < _height)
            {
                if (_paths[currentPoint.X, currentPoint.Y + 1] == 0)
                {
                    _paths[currentPoint.X, currentPoint.Y + 1] = CountPaths(new(currentPoint.X, currentPoint.Y + 1));
                }

                return _paths[currentPoint.X, currentPoint.Y + 1];
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (currentPoint.X - 1 >= 0 && _paths[currentPoint.X - 1, currentPoint.Y] == 0)
            {
                _paths[currentPoint.X - 1, currentPoint.Y] = CountPaths(new(currentPoint.X - 1, currentPoint.Y));
            }
            if (currentPoint.X + 1 < _width && _paths[currentPoint.X + 1, currentPoint.Y] == 0)
            {
                _paths[currentPoint.X + 1, currentPoint.Y] = CountPaths(new(currentPoint.X + 1, currentPoint.Y));
            }

            return _paths[currentPoint.X - 1, currentPoint.Y] + _paths[currentPoint.X + 1, currentPoint.Y];
        }
    }
}
