namespace AdventOfCode.Puzzles._2024._23.Part1;

public partial class Part1 : IPuzzleSolution
{
    private Dictionary<string, HashSet<string>> _graph = new();

    private HashSet<string> _vertices = new();

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var edges = await inputReader.ReadAllLinesAsync();

        foreach (var edge in edges)
        {
            var neighbors = edge.Split("-");
            foreach (var neighbor in neighbors)
            {
                if (!_graph.ContainsKey(neighbor))
                {
                    _graph[neighbor] = new HashSet<string>();
                }

                if (!_vertices.Contains(neighbor))
                {
                    _vertices.Add(neighbor);
                }
            }

            _graph[neighbors[0]].Add(neighbors[1]);
            _graph[neighbors[1]].Add(neighbors[0]);
        }

        var triangles = GetTrianglesWithT();

        return triangles.Count.ToString();
    }

    private HashSet<string> GetTrianglesWithT()
    {
        var results = new HashSet<string>();
        foreach (var vertex in _vertices.Where(v => v.StartsWith("t")))
        {
            foreach (var secondVertex in _vertices)
            {
                foreach (var thirdVertex in _vertices)
                {
                    if (_graph[vertex].Contains(secondVertex) &&
                        _graph[secondVertex].Contains(thirdVertex) &&
                        _graph[thirdVertex].Contains(vertex))
                    {
                        results.Add(string.Join(",", new[] { vertex, secondVertex, thirdVertex }.OrderBy(v => v)));
                    }
                }
            }
        }

        return results;
    }
}
