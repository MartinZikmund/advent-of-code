namespace AdventOfCode.Puzzles._2024._23.Part2;

public partial class Part2 : IPuzzleSolution
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

        var triangles = GetTriangles();
        var largestSet = FindLargestSet(triangles);
        return largestSet;
    }

    private string FindLargestSet(HashSet<string> triangles)
    {
        var sortedVertices = _vertices.OrderBy(v => v).ToArray();

        var currentSets = triangles;

        var newSets = currentSets;
        do
        {
            currentSets = newSets;
            newSets = new HashSet<string>();

            foreach (var currentSet in currentSets)
            {
                var verticesInSet = currentSet.Split(",");
                foreach (var vertex in sortedVertices)
                {
                    if (!currentSet.Contains(vertex))
                    {
                        if (verticesInSet.All(v => _graph[v].Contains(vertex)))
                        {
                            var newSet = string.Join(",", verticesInSet.Append(vertex).OrderBy(v => v));
                            newSets.Add(newSet);
                        }
                    }
                }
            }
        } while (newSets.Count != 0);

        var firstSet = currentSets.First();
        return firstSet;
    }

    private HashSet<string> GetTriangles()
    {
        var results = new HashSet<string>();
        foreach (var vertex in _vertices)
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
