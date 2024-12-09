using System.Text;

namespace AdventOfCode.Puzzles._2024._09.Part1;

public partial class Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var diskMap = await inputReader.ReadLineAsync();

        var files = new List<FileBlock>();
        var movedFiles = new List<FileBlock>();
        var freeSpace = new Queue<FreeBlock>();
        StringBuilder output = new StringBuilder();
        bool isFile = true;
        var currentPosition = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            var length = diskMap[i] - '0';
            if (isFile)
            {
                var fileId = i / 2;
                var start = currentPosition;
                files.Add(new FileBlock(fileId, start, length));
                output.Append(new string(fileId.ToString()[0], length));
            }
            else
            {
                var start = currentPosition;
                freeSpace.Enqueue(new FreeBlock(start, length));
                output.Append(new string('.', length));
            }
            currentPosition += length;
            isFile = !isFile;
        }

        while (freeSpace.Count > 0)
        {
            var firstSpace = freeSpace.Dequeue();

            if (firstSpace.start > files.Last().start)
            {
                break;
            }

            var currentStart = firstSpace.start;
            var remainingSpace = firstSpace.length;
            while (remainingSpace > 0)
            {
                var lastFile = files.Last();
                var toMove = Math.Min(remainingSpace, lastFile.length);
                var movedFile = new FileBlock(lastFile.id, currentStart, toMove);
                currentStart += toMove;
                movedFiles.Add(movedFile);
                if (toMove == lastFile.length)
                {
                    files.RemoveAt(files.Count - 1);
                }
                else
                {
                    files[^1] = new FileBlock(lastFile.id, lastFile.start, lastFile.length - toMove);
                }

                remainingSpace -= toMove;
            }
        }

        var allFiles = files.Concat(movedFiles).OrderBy(f => f.start).ToList();
        var totalSum = 0L;
        for (int i = 0; i < allFiles.Count; i++)
        {
            var file = allFiles[i];
            for (int blockId = 0; blockId < file.length; blockId++)
            {
                var diskPosition = file.start + blockId;
                totalSum += diskPosition * file.id;
            }
        }

        return totalSum.ToString();
    }

    public record FileBlock(int id, int start, int length);

    public record FreeBlock(int start, int length);
}
