namespace AdventOfCode.Puzzles._2025._06.Part2;

public class AoC2025Day6Part2 : IPuzzleSolution
{
    public async Task<string> SolveAsync(StreamReader inputReader)
    {
        var problems = new List<string[]>();
        var lines = await inputReader.ReadAllLinesAsync();
        
        var operatorsLine = lines[lines.Count - 1];
        ulong gradTotal = 0;

        var startIndex = 0;
        do
        {
            var multiply = operatorsLine[startIndex] == '*';
            ulong currentTotal = multiply ? 1UL : 0; 

            var endIndex = operatorsLine.IndexOfAny(new[] { '+', '*' }, startIndex + 1);
            if (endIndex == -1)
            {
                endIndex = operatorsLine.Length + 1;
            }

            for (var currentColumn = endIndex - 2; currentColumn >= startIndex; currentColumn--)
            {
                var currentNumber = 0UL;
                for (var currentRow = 0; currentRow < lines.Count - 1; currentRow++)
                {
                    var character = lines[currentRow][currentColumn];
                    if (char.IsDigit(character))
                    {
                        var number = (ulong)(character - '0');
                        currentNumber *= 10;
                        currentNumber += number;
                    }
                }

                if (multiply)
                {
                    currentTotal *= currentNumber;
                }
                else
                {
                    currentTotal += currentNumber;
                }
            }
            gradTotal += currentTotal;
            startIndex = endIndex;
        } while (startIndex < operatorsLine.Length);

        return gradTotal.ToString();
    }
}
