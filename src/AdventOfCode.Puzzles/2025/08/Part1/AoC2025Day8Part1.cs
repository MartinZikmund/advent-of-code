namespace AdventOfCode.Puzzles._2025._08.Part1;

public class AoC2025Day8Part1 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        int targetConnections = lines.Count < 50 ? 10 : 1000;

        var boxes = new List<Point3d>();
        foreach (var line in lines)
        {
            var parts = line.Split(',', StringSplitOptions.TrimEntries);
            boxes.Add(new Point3d
            {
                X = int.Parse(parts[0]),
                Y = int.Parse(parts[1]),
                Z = int.Parse(parts[2]),
            });
        }

        var distinctDistances = new List<(Point3d box1, Point3d box2, double distance)>();

        var distances = new Dictionary<(Point3d, Point3d), double>();
        for (int box1 = 0; box1 < boxes.Count; box1++)
        {
            for (int box2 = box1 + 1; box2 < boxes.Count; box2++)
            {
                var distance = boxes[box1].Distance(boxes[box2]);
                distances[(boxes[box1], boxes[box2])] = distance;
                distances[(boxes[box2], boxes[box1])] = distance;
                distinctDistances.Add((boxes[box1], boxes[box2], distance));
            }
        }

        distinctDistances = distinctDistances
            .OrderBy(t => t.distance)
            .ToList();

        var circuits = new Dictionary<Point3d, List<Point3d>>();
        foreach (var box in boxes)
        {
            var circuit = new List<Point3d> { box };
            circuits.Add(box, circuit);
        }

        for (var distanceId = 0; distanceId < targetConnections; distanceId++)
        {
            var (box1, box2, distance) = distinctDistances[distanceId];

            var circuit1 = circuits[box1];
            var circuit2 = circuits[box2];
            if (circuit1 != circuit2)
            {
                // Merge circuits
                circuit1.AddRange(circuit2);
                foreach (var box in circuit2)
                {
                    circuits[box] = circuit1;
                }
            }
        }

        var largestCircuits = circuits
            .Select(c => c.Value)
            .Distinct()
            .OrderByDescending(c => c.Count)
            .Select(c => c.Count)
            .Take(3)
            .ToArray();
        return (largestCircuits[0] * largestCircuits[1] * largestCircuits[2]).ToString();
    }
}
