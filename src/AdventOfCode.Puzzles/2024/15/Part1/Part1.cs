using System.Diagnostics;
using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._15.Part1;

public partial class Part1 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private char[,] _map;
    private string _instructions;
    private Point _robotPosition;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var mapLines = new List<string>();
        while (await inputReader.ReadLineAsync() is { } line && !string.IsNullOrWhiteSpace(line))
        {
            mapLines.Add(line);
        }

        _width = mapLines[0].Length;
        _height = mapLines.Count;
        _map = new char[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _map[x, y] = mapLines[y][x];

                if (_map[x, y] == '@')
                {
                    _robotPosition = new Point(x, y);
                }
            }
        }

        StringBuilder instructions = new();
        while (await inputReader.ReadLineAsync() is { } line)
        {
            instructions.Append(line);
        }

        _instructions = instructions.ToString();

        foreach (var instruction in _instructions)
        {
            MoveRobot(instruction);
        }

        var total = 0;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_map[x, y] == 'O')
                {
                    total += y * 100 + x;
                }
            }
        }

        return total.ToString();
    }

    private void MoveRobot(char instruction)
    {
        var direction = instruction switch
        {
            '^' => new Point(0, -1),
            '>' => new Point(1, 0),
            'v' => new Point(0, 1),
            '<' => new Point(-1, 0),
            _ => throw new InvalidOperationException()
        };

        if (CanMove(_robotPosition, direction))
        {
            Move(_robotPosition, direction);
            _robotPosition += direction;
        }        
    }

    private bool CanMove(Point position, Point direction)
    {
        var newPosition = position + direction;

        var newTile = _map[newPosition.X, newPosition.Y];
        if (newTile == '#')
        {
            return false;
        }
        else if (newTile == 'O')
        {
            return CanMove(newPosition, direction);
        }
        else if (newTile == '.')
        {
            return true;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private void Move(Point position, Point direction)
    {
        var newPosition = position + direction;
        var newTile = _map[newPosition.X, newPosition.Y];
        if (newTile != '.')
        {
            Move(newPosition, direction);
        }

        _map[newPosition.X, newPosition.Y] = _map[position.X, position.Y];
        _map[position.X, position.Y] = '.';
    }
}
