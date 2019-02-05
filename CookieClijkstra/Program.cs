using System.Threading.Tasks;

namespace CookieClijkstra
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        private const int target_cookies = 1000000;

        public static async Task MainAsync(string[] args)
        {
            var graph = new Graph(target_cookies);

            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            while (!graph.Solved)
            {
                graph.Step();

                if (graph.StepCount % 100000 == 0)
                {
                    Logger.Log($"Completed step {graph.StepCount:n0}.");
                }
            }
            stopwatch.Stop();

            Logger.Log($"Calculated in {stopwatch.Elapsed.TotalSeconds:N2}s with {graph.StepCount} steps.");

            double previousDistance = 0;
            foreach (Vertex vertex in graph.GetSolution())
            {
                if (vertex.Previous == null)
                {
                    Logger.Log("0.00s (+0.00s) => source");
                }
                else
                {
                    Logger.Log($"{vertex.DistanceFromSource:N2}s (+{vertex.DistanceFromSource - previousDistance:N2}s) => {vertex.PreviousName}");
                    previousDistance = vertex.DistanceFromSource;
                }
            }

            await Task.Delay(-1);
        }
    }
}
