using System.Text;
using AdventOfCode.Puzzles.Tools;

namespace AdventOfCode.Puzzles._2024._09.Part2;

public partial class Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var diskMap = await inputReader.ReadLineAsync();

        var files = new List<FileBlock>();
        var movedFiles = new List<FileBlock>();
        var freeSpace = new List<FreeBlock>();
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
                freeSpace.Add(new FreeBlock(start, length));
                output.Append(new string('.', length));
            }
            currentPosition += length;
            isFile = !isFile;
        }

        for (int fileIndex = files.Count - 1; fileIndex >= 0; fileIndex--)
        {
            var fileToMove = files[fileIndex];

            for (int freeSpaceIndex = 0; freeSpaceIndex < freeSpace.Count; freeSpaceIndex++)
            {
                var freeBlock = freeSpace[freeSpaceIndex];
                if (fileToMove.length <= freeBlock.length && fileToMove.start > freeBlock.start)
                {
                    var movedFile = new FileBlock(fileToMove.id, freeBlock.start, fileToMove.length);
                    movedFiles.Add(movedFile);
                    files.RemoveAt(fileIndex);
                    freeSpace[freeSpaceIndex] = new FreeBlock(freeBlock.start + fileToMove.length, freeBlock.length - fileToMove.length);
                    break;
                }
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
