namespace AdventOfCode.Puzzles;

public interface IPuzzleSolution
{
    Task<string> SolveAsync(StreamReader inputReader);
}
