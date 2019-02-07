using System.Threading.Tasks;

namespace CookieClijkstra
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        private const int target_cookies = 1000;

        public static async Task MainAsync(string[] args)
        {
            var graph = new Graph(target_cookies);

            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            while (!graph.Solved)
            {
                graph.Step();
            }
            stopwatch.Stop();

            Logger.Log($"Calculated in {stopwatch.Elapsed.TotalSeconds:N2}s with {graph.StepCount} steps.");

            float previousCost = 0;
            foreach (Vertex vertex in graph.GetSolution())
            {
                if (vertex.Previous == null)
                {
                    Logger.Log("0.00s (+0.00s) => source");
                }
                else
                {
                    Logger.Log($"{vertex.MinimumCost:N2}s (+{vertex.MinimumCost - previousCost:N2}s) => {vertex.PreviousName}");
                    previousCost = vertex.MinimumCost;
                }
            }

            await Task.Delay(-1);
        }
    }
}
