namespace AdventOfCode.Puzzles;

public static class SolutionLocator
{
    public static IEnumerable<PuzzleSolutionInfo> GetPuzzleSolutions()
    {
        var puzzleTypes = typeof(SolutionLocator).Assembly.GetTypes()
            .Where(type => 
                typeof(IPuzzleSolution).IsAssignableFrom(type) &&
                !type.IsInterface &&
                !type.IsAbstract);

        var puzzleInfos = new List<PuzzleSolutionInfo>();

        foreach (var type in puzzleTypes)
        {
            // Split namespace into parts ("AdventOfCode.Puzzles._2024._01._Part1")
            var namespaceParts = type.Namespace?.Split('.') ?? Array.Empty<string>();
            if (namespaceParts.Length < 4)
            {
                throw new InvalidOperationException($"Invalid namespace for type {type.FullName}");
            }

            // Namespace structure: AdventOfCode.Puzzles._<Year>._<Day>._Part<Part>
            if (int.TryParse(namespaceParts[2].TrimStart('_'), out int year) &&
                int.TryParse(namespaceParts[3].TrimStart('_'), out int day))
            {
                // Default to Part 1 if no part is specified
                int part = 1;

                if (namespaceParts.Length >= 5 && namespaceParts[4].StartsWith("_Part") &&
                    int.TryParse(namespaceParts[4].Substring(5), out int parsedPart))
                {
                    part = parsedPart;
                }

                puzzleInfos.Add(new PuzzleSolutionInfo(year, day, part, type));
            }
        }

        return puzzleInfos.OrderBy(p => p.Year).ThenBy(p => p.Day).ThenBy(p => p.Part);
    }
}
