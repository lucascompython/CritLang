namespace CritLang
{
    internal class CommandLineArgs
    {
        /// <summary>
        /// Parse the arguments passed to the program.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string Parse(string[] args, string version)
        {
            foreach (var arg in args)
            {
                if (new[] { "-h", "--help" }.Contains(arg))
                {
                    Console.WriteLine("Usage: crit [file]");
                    Environment.Exit(0);
                }
                else if (new[] { "-v", "--version" }.Contains(arg))
                {
                    Console.WriteLine($"CritLang {version}");
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("-"))
                {
                    Console.WriteLine("Unknown option: " + arg);
                    Environment.Exit(1);
                }
                else
                {
                    return arg;
                }
            }

            return string.Empty;
        }
    }
}
