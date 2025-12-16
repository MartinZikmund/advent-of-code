namespace AdventOfCode.Puzzles._2025._09.Part1;

public class Solution : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();

        var points = new List<Point>();

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            points.Add(new Point(x, y));
        }

        var maxArea = 0L;
        foreach (var point1 in points)
        {
            foreach (var point2 in points)
            {
                // Area
                var area = (long)(Math.Abs(point1.X - point2.X) + 1) * (Math.Abs(point1.Y - point2.Y) + 1);
                if (maxArea < area)
                {
                    maxArea = area;
                }
            }
        }

        return maxArea.ToString();
    }
}
