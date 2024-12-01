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

        var resourceNames = typeof(SolutionLocator).Assembly.GetManifestResourceNames(); // Get all embedded resource names

        var puzzleInfos = new List<PuzzleSolutionInfo>();

        foreach (var type in puzzleTypes.OrderBy(t => t.Namespace))
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

                if (namespaceParts.Length >= 5)
                {
                    if (namespaceParts[4].StartsWith("Part") && int.TryParse(namespaceParts[4].Substring(5), out int parsedPart))
                    {
                        part = parsedPart;
                    }
                    else if (int.TryParse(namespaceParts[4].TrimStart('_'), out parsedPart))
                    {
                        part = parsedPart;
                    }
                }

                // Search for potential test data files
                string partFolder = $"{type.Namespace}.TestData.txt";
                string dayFolder = $"{string.Join('.', namespaceParts.Take(4))}.TestData.txt";

                // Locate matching resource (priority: part folder > day folder)
                string? testDataResourceName = resourceNames.FirstOrDefault(name =>
                    name.Equals(partFolder, StringComparison.OrdinalIgnoreCase) ||
                    name.Equals(dayFolder, StringComparison.OrdinalIgnoreCase));

                puzzleInfos.Add(new PuzzleSolutionInfo(year, day, part, type, testDataResourceName));
            }
        }

        return puzzleInfos.OrderBy(p => p.Year).ThenBy(p => p.Day).ThenBy(p => p.Part);
    }
}
