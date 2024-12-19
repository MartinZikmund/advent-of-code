namespace AdventOfCode.Puzzles._2024._16.Part1;

public partial class Part1 : IPuzzleSolution
{
    private int _width;
    private int _height;
    private char[,] _map;
    private Point _start;
    private Point _end;

    public record State(Point Position, Point Direction);

    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        await ReadInputAsync(inputReader);

        var lowestCost = FindLowestCost();
        return lowestCost.ToString();
    }

    private int FindLowestCost()
    {
        var states = new PriorityQueue<State, int>();
        var minimumCosts = new Dictionary<State, int>();
        var finalized = new HashSet<State>();

        var startingState = new State(_start, new(1, 0));
        states.Enqueue(startingState, 0);
        minimumCosts.Add(startingState, 0);

        while (states.Count > 0)
        {
            var state = states.Dequeue();
            finalized.Add(state);

            // We go forward
            var next = state.Position + state.Direction;
            var nextState = new State(next, state.Direction);
            AddNextIfImproved(nextState, minimumCosts[state] + 1);

            // We rotate clockwise
            nextState = new State(state.Position, new Point(-state.Direction.Y, state.Direction.X));
            AddNextIfImproved(nextState, minimumCosts[state] + 1000);

            // We rotate counter-clockwise
            nextState = new State(state.Position, new Point(state.Direction.Y, -state.Direction.X));
            AddNextIfImproved(nextState, minimumCosts[state] + 1000);

            void AddNextIfImproved(State nextState, int cost)
            {
                if (nextState.Position.X < 0 || nextState.Position.X >= _width || nextState.Position.Y < 0 || nextState.Position.Y >= _height)
                {
                    return;
                }

                if (_map[nextState.Position.X, nextState.Position.Y] == '#')
                {
                    return;
                }

                if (!finalized.Contains(nextState))
                {
                    if (!minimumCosts.TryGetValue(nextState, out var nextCost) || nextCost > cost)
                    {
                        minimumCosts[nextState] = cost;
                        states.Remove(nextState, out _, out _);
                        states.Enqueue(nextState, minimumCosts[nextState]);
                    }
                }
            }
        }

        var bestEndState = int.MaxValue;
        foreach (var direction in Directions.WithoutDiagonals)
        {
            bestEndState = Math.Min(bestEndState, minimumCosts.GetValueOrDefault(new State(_end, direction), int.MaxValue));
        }

        return bestEndState;
    }

    private async Task ReadInputAsync(StreamReader inputReader)
    {
        var lines = await inputReader.ReadAllLinesAsync();
        _width = lines[0].Length;
        _height = lines.Count;
        _map = new char[_width, _height];

        for (var y = 0; y < _height; y++)
        {
            var line = lines[y];
            for (var x = 0; x < _width; x++)
            {
                _map[x, y] = line[x];
                if (_map[x, y] == 'S')
                {
                    _start = new(x, y);
                    _map[x, y] = '.';
                }
                else if (_map[x, y] == 'E')
                {
                    _end = new(x, y);
                    _map[x, y] = '.';
                }
            }
        }
    }
}
