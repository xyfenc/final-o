namespace CPMTools;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Env.Load();
        LocalDb.Init();
        Application.Run(new Form1());
    }
}
