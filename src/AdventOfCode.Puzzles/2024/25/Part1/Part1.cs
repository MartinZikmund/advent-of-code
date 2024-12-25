namespace AdventOfCode.Puzzles._2024._25.Part1;

public partial class Part1 : IPuzzleSolution
{
    private const int Width = 5;
    private const int Height = 7;

    private List<int[]> _locks = new();
    private List<int[]> _keys = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        do
        {
            var grid = new char[Height, Width];
            for (var y = 0; y < Height; y++)
            {
                var line = await inputReader.ReadLineAsync();
                for (var x = 0; x < Width; x++)
                {
                    grid[y, x] = line[x];
                }
            }

            var isLock = grid[0, 0] == '#';
            var item = new int[Width];
            for (int x = 0; x < Width; x++)
            {
                var count = 0;
                for (int y = 0; y < Height; y++)
                {
                    if (grid[y, x] == '#')
                    {
                        count++;
                    }
                }

                item[x] = count;
            }

            if (isLock)
            {
                _locks.Add(item);
            }
            else
            {
                _keys.Add(item);
            }
        }
        while (await inputReader.ReadLineAsync() is { });

        var pairCount = 0;
        foreach (var lockItem in _locks)
        {
            foreach (var keyItem in _keys)
            {
                bool isValid = true;
                for (int i = 0; i < Width; i++)
                {
                    if (lockItem[i] + keyItem[i] > 7)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    pairCount++;
                }
            }
        }

        return pairCount.ToString();
    }
}
