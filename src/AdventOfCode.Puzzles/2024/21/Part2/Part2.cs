namespace AdventOfCode.Puzzles._2024._21.Part2;

public partial class Part2 : IPuzzleSolution
{
    private char[,] _numericKeypad = new char[,]
    {
        { '7', '8', '9' },
        { '4', '5', '6' },
        { '1', '2', '3' },
        { '#', '0', 'A' }
    };

    private char[,] _directionalKeypad = new char[,]
    {
        { '#', '^', 'A' },
        { '<', 'v', '>' }
    };

    private Dictionary<(char from, char to), List<string>> _shortestSequences = new();

    private Dictionary<(string code, int level), ulong> _shortestSubsequenceLengths = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        CacheAllShortestSequences();

        ulong result = 0;
        var codes = await inputReader.ReadAllLinesAsync();
        foreach (var code in codes)
        {
            var numericCode = int.Parse(string.Join("", code.Where(char.IsDigit)));
            var shortestSequenceLength = GetShortestSequenceLength(code, 0);
            result += shortestSequenceLength * (ulong)numericCode;
        }

        return result.ToString();
    }

    private ulong GetShortestSequenceLength(string code, int layer)
    {
        if (_shortestSubsequenceLengths.TryGetValue((code, layer), out var cached))
        {
            return cached;
        }

        if (layer == 26)
        {
            _shortestSubsequenceLengths[(code, layer)] = (ulong)code.Length;
            return (ulong)code.Length;
        }

        ulong best = 0;
        char previous = 'A';
        for (int codeIndex = 0; codeIndex < code.Length; codeIndex++)
        {
            var current = code[codeIndex];

            var keypad = layer == 0 ? _numericKeypad : _directionalKeypad;
            var paths = _shortestSequences;

            var shortestPaths = paths[(previous, current)];

            ulong currentBest = ulong.MaxValue;
            foreach (var path in shortestPaths)
            {
                var length = GetShortestSequenceLength(path, layer + 1);
                if (currentBest > length)
                {
                    currentBest = length;
                }
            }

            best += currentBest;

            previous = current;
        }

        _shortestSubsequenceLengths[(code, layer)] = best;
        return best;
    }

    private void CacheAllShortestSequences()
    {
        CacheAllShortestSequences(_numericKeypad);
        CacheAllShortestSequences(_directionalKeypad);
    }

    private void CacheAllShortestSequences(char[,] keypad)
    {
        foreach (var character in keypad)
        {
            foreach (var otherCharacter in keypad)
            {
                _shortestSequences[(character, otherCharacter)] = new List<string>();

                if (character == otherCharacter)
                {
                    _shortestSequences[(character, otherCharacter)].Add("A");
                    continue;
                }

                CacheShortestSequence(character, otherCharacter, keypad);
            }
        }
    }

    private void CacheShortestSequence(char from, char to, char[,] keypad)
    {
        var fromPoint = FindCharacter(from, keypad);
        var toPoint = FindCharacter(to, keypad);

        var shortestPathLength = Math.Abs(fromPoint.X - toPoint.X) + Math.Abs(fromPoint.Y - toPoint.Y);

        Queue<(Point point, string sequence)> queue = new();
        queue.Enqueue((fromPoint, ""));
        while (queue.Count > 0)
        {
            var (point, sequence) = queue.Dequeue();

            if (point == toPoint && sequence.Length == shortestPathLength)
            {
                _shortestSequences[(from, to)].Add(sequence + "A");
                continue;
            }

            if (sequence.Length >= shortestPathLength)
            {
                continue;
            }

            foreach (var direction in Directions.WithoutDiagonals)
            {
                var nextPoint = point + direction;
                if (nextPoint.X >= 0 && nextPoint.X < keypad.GetLength(1) &&
                    nextPoint.Y >= 0 && nextPoint.Y < keypad.GetLength(0) &&
                    keypad[nextPoint.Y, nextPoint.X] != '#')
                {
                    var directionalCharater = direction switch
                    {
                        (0, 1) => 'v',
                        (1, 0) => '>',
                        (0, -1) => '^',
                        (-1, 0) => '<',
                        _ => throw new InvalidOperationException("Invalid direction.")
                    };
                    queue.Enqueue((nextPoint, sequence + directionalCharater));
                }
            }
        }
    }

    private Point FindCharacter(char character, char[,] keypad)
    {
        for (int y = 0; y < keypad.GetLength(0); y++)
        {
            for (int x = 0; x < keypad.GetLength(1); x++)
            {
                if (keypad[y, x] == character)
                {
                    return new Point(x, y);
                }
            }
        }
        throw new InvalidOperationException($"Character '{character}' not found in keypad.");
    }
}
