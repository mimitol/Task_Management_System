partial class Program
{
    private static void Main(string[] args)
    {
        Welcome5176();
        Welcome5571();
        Console.ReadKey();
    }

    private static void Welcome5176()
    {
        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first console application", name);
    }
    private static void Welcome5571() { }
}