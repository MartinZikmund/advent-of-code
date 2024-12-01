namespace AdventOfCode.Puzzles;

public record PuzzleSolutionInfo(int Year, int Day, int Part, Type PuzzleType)
{
    public override string ToString() => $"Year: {Year}, Day: {Day}, Part: {Part}";
}

