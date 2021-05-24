using System;

namespace JOSE
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================== JWS EXAMPLE ====================================");
            JwsExample.Run();
            Console.WriteLine();
            Console.WriteLine("==================================== JWE EXAMPLE ====================================");
            JweExample.Run();
        }
    }
}
