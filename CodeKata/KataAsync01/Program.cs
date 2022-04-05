using static System.Console;

/*
 * Note the global-using file ...\KataAsync01\obj\Debug\net6.0
 * has global using global::System.Threading.Tasks;
 * To be explicit, above we could have added the line
 * using System.Threading
 */

namespace KataAsync01;
class Program
{
    static void Main(string[] args)
    {
        DoExample();
    }

    private static void DoExample()
    {
        System.Diagnostics.Debug.WriteLine("blah");
        WriteLine($"main thread id {System.Threading.Thread.CurrentThread.ManagedThreadId}");

        var someTask = Task.Run(() => {
            WriteLine($"interior thread id {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            return "popeye the sailor man";
        });

        someTask.ContinueWith(s => {
            WriteLine($"completion thread id {System.Threading.Thread.CurrentThread.ManagedThreadId} with result from earlier thread s.Result = \"{s.Result}\"");
        },
        TaskContinuationOptions.OnlyOnRanToCompletion);

        someTask.ContinueWith(s =>
        {
            WriteLine("this line should not occur");
        }, TaskContinuationOptions.OnlyOnFaulted);

        someTask.Wait(); // here we block, but just as an example
        WriteLine($"finish main thread id {System.Threading.Thread.CurrentThread.ManagedThreadId}");
    }
}

/*
 * output
PS > dotnet run
main thread id 1
interior thread id 4
finish main thread id 1
completion thread id 6 with result from earlier thread s.Result = "popeye the sailor man"
 */
