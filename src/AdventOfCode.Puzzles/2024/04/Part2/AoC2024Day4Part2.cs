using System.Text.RegularExpressions;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._04.Part2;

public partial class AoC2024Day4Part2 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private char[,] _map;

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        _height = 0;
        _width = 0;
        List<string> lines = new List<string>();
        while (await inputReader.ReadLineAsync() is { } line)
        {
            _width = line.Length;
            lines.Add(line);
            _height++;
        }

        var map = new char[_width, _height];
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                map[x, y] = lines[y][x];
            }
        }

        _map = map;

        int foundCount = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int diagonalCount = 0;
                for (int diagonalDirectionIndex = 0; diagonalDirectionIndex < Directions.DiagonalsOnly.Length; diagonalDirectionIndex++)
                {
                    var diagonalDirection = Directions.DiagonalsOnly[diagonalDirectionIndex];
                    var oppositeDirection = diagonalDirection * -1;
                    var startingPoint = (x, y) + oppositeDirection;
                    if (SearchWord(startingPoint, diagonalDirection, "MAS"))
                    {
                        diagonalCount++;
                    }
                }

                if (diagonalCount == 2)
                {
                    foundCount++;
                }
            }
        }

        return foundCount.ToString();
    }

    public bool SearchWord(Point startingPoint, Point direction, string word)
    {
        for (var characterIndex = 0; characterIndex < word.Length; characterIndex++)
        {
            var position = startingPoint + direction * characterIndex;
            if (position.X < 0 ||
                position.X >= _width ||
                position.Y < 0 ||
                position.Y >= _height ||
                _map[position.X, position.Y] != word[characterIndex])
            {
                return false;
            }
        }

        return true;
    }
}
