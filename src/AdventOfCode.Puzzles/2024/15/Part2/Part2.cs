using System.Diagnostics;
using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._15.Part2;

public partial class Part2 : IPuzzleSolution
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

        _width = mapLines[0].Length * 2;
        _height = mapLines.Count;
        _map = new char[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < mapLines[0].Length; x++)
            {
                var character = mapLines[y][x];

                if (character == '@')
                {
                    _robotPosition = new Point(x * 2, y);
                    _map[x * 2, y] = '@';
                    _map[x * 2 + 1, y] = '.';
                }
                else if (character == '#')
                {
                    _map[x * 2, y] = '#';
                    _map[x * 2 + 1, y] = '#';
                }
                else if (character == '.')
                {
                    _map[x * 2, y] = '.';
                    _map[x * 2 + 1, y] = '.';
                }
                else if (character == 'O')
                {
                    _map[x * 2, y] = '[';
                    _map[x * 2 + 1, y] = ']';
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
                if (_map[x, y] == '[')
                {
                    total += y * 100 + x;
                }
            }
        }

        return total.ToString();
    }

    private void OutputMap()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Debug.Write(_map[x, y]);
            }
            Debug.WriteLine("");
        }
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
            //OutputMap();
            Move(_robotPosition, direction);
            _robotPosition += direction;
        }
    }

    private bool CanMove(Point position, Point direction)
    {
        var newPosition = position + direction;
        var currentTile = _map[position.X, position.Y];
        var newTile = _map[newPosition.X, newPosition.Y];

        if (currentTile == '.')
        {
            return true;
        }

        if (currentTile == '#')
        {
            return false;
        }

        if (currentTile == '[' || currentTile == ']')
        {
            if ((currentTile == ']' && direction == (-1, 0)) ||
                (currentTile == '[' && direction == (1, 0)))
            {
                return CanMove(newPosition, direction);
            }

            if (currentTile == ']' && (direction == (0, -1) || direction == (0, 1)))
            {
                return CanMove(newPosition, direction) && CanMove(newPosition + (-1, 0), direction);
            }

            if (currentTile == '[' && (direction == (0, -1) || direction == (0, 1)))
            {
                return CanMove(newPosition, direction) && CanMove(newPosition + (1, 0), direction);
            }

            return CanMove(newPosition, direction);
        }
               
        if (newTile == '.')
        {
            return true;
        }

        if (currentTile == '@')
        {
            return CanMove(newPosition, direction);
        }

        throw new InvalidOperationException();
    }

    private void Move(Point position, Point direction)
    {
        var newPosition = position + direction;

        var currentTile = _map[position.X, position.Y];
        var newTile = _map[newPosition.X, newPosition.Y];

        if (direction == (-1, 0) || direction == (1, 0))
        {
            if (newTile != '.')
            {
                Move(newPosition, direction);
            }
        }
        else
        {
            if (newTile == '[')
            {
                Move(newPosition + (1, 0), direction);
                Move(newPosition, direction);
            }
            else if (newTile == ']')
            {
                Move(newPosition + (-1, 0), direction);
                Move(newPosition, direction);
            }
            else if (newTile != '.')
            {
                Move(newPosition, direction);
            }
        }

        _map[newPosition.X, newPosition.Y] = _map[position.X, position.Y];
        _map[position.X, position.Y] = '.';
    }
}
