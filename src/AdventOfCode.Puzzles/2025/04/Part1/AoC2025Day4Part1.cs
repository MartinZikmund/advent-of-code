
namespace AdventOfCode.Puzzles._2025._04.Part1;

public class AoC2025Day4Part1 : IPuzzleSolution
{
    private const char Roll = '@';
    private char[,] _map;
    private int _width;
    private int _height;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;
        _map = new char[lines[0].Length, lines.Count];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _map[x, y] = lines[y][x];
            }
        }

        int movable = 0;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_map[x, y] == Roll && IsMovable(x, y))
                {
                    movable++;
                }
            }
        }

        return movable.ToString();
    }

    private bool IsMovable(int x, int y)
    {
        var adjacentRolls = 0;
        foreach (var direction in Directions.WithDiagonals)
        {
            var neighborPosition = new Point(x + direction.X, y + direction.Y);
            if (neighborPosition.X < 0 || neighborPosition.X >= _width ||
                neighborPosition.Y < 0 || neighborPosition.Y >= _height)
            {
                continue;
            }

            if (_map[neighborPosition.X, neighborPosition.Y] == Roll)
            {
                adjacentRolls++;
            }
        }

        return adjacentRolls < 4;
    }
}
