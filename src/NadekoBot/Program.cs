using NadekoBot.Core.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NadekoBot
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var pid = Process.GetCurrentProcess().Id;
            System.Console.WriteLine($"Pid: {pid}");
            if (args.Length == 2
                && int.TryParse(args[0], out int shardId)
                && int.TryParse(args[1], out int parentProcessId))
            {
                await new NadekoBot(shardId, parentProcessId == 0 ? pid : parentProcessId)
                    .RunAndBlockAsync();
            }
            else
            {
                await new ShardsCoordinator()
                    .RunAsync()
                    .ConfigureAwait(false);
#if DEBUG
                await new NadekoBot(0, pid)
                    .RunAndBlockAsync();
#else
                await Task.Delay(-1);
#endif
            }
        }
    }
}
