internal class Program
{
    private static string? _word = "";
    
    public static void Main(string[] args)
    {
        // Console.WriteLine("Wpisz słowo task w ciągu 4 sekund");
        //
        // var time = Time();
        // var read = Read();
        // Task.WaitAny(Time(), Read());
        //
        // if (_word == "task")
        // {
        //     Console.WriteLine("Sukces!");
        // }
        // else
        // {
        //     Console.WriteLine("Porażka!");
        // }

        // RunDemo1();
        RunDemo2();
    }

    public static async Task Time()
    {
        await Task.Delay(4000);
    }
    
    public static async Task Read()
    {
        await Task.Delay(1);
        _word = Console.ReadLine();
    }

    public static long Factorial(int i)
    {
        if (i < 0)
        {
            throw new ArgumentException();
        }

        return i == 0 ? 1 : i * Factorial(i - 1);
    }

    public static async Task<long> FactorialAsync(int i)
    {
        Console.WriteLine("Async: before await");
        await Task.Delay(100);
        Console.WriteLine("Async: after await");
        return Factorial(i);
    }

    public static async void RunDemo1()
    {
        Console.WriteLine("Main: before calling async function.");
        var fact = FactorialAsync(13);
        fact.GetAwaiter().OnCompleted(() =>
        {
            Console.WriteLine(fact.Result);
        });
        Console.WriteLine("Main: after calling async function");
        Console.ReadLine();
    }

    public static async void ReadTextFileAsync(string? word, CancellationToken token)
    {
        int count = 0;
        var items = File.ReadLinesAsync(@"d:\rockyou.txt");
        
        await foreach (var item in items)
        {
            count++;
            
            if (count % 1000 == 0)
            {
                Console.SetCursorPosition(0, 2);
                Console.WriteLine($"{count}");
            }
            
            if (item == word)
            {
                Console.WriteLine("Found");
                Console.WriteLine(count);
                return;
            }

            if (token.IsCancellationRequested)
            {
                Console.WriteLine($"Token searched: {count}");
                Console.WriteLine("Search canceled");
                return;
            }
        }
        
        Console.WriteLine("There is no such password in the file!");
        return;
    }

    public static void RunDemo2()
    {
        CancellationTokenSource tokenSrc = new CancellationTokenSource();
        while (true)
        {
            Console.WriteLine("Wpisz szukane hasło");
            var word = Console.ReadLine();
            ReadTextFileAsync(word, tokenSrc.Token);
            var option = Console.ReadLine();
            Console.WriteLine("Wpisz: c aby anulować, n aby szukać następne słowo, q aby wyjść");
            switch (option)
            {
                case "q":
                    Environment.Exit(0);
                    break;
                case "c":
                    tokenSrc.Cancel();
                    break;
                case "n":
                    break;
            }
        }
    }
}