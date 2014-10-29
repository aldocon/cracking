using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordCrackerCentralized
{
    class Program
    {
        public static void Main()
        {

            Cracking cracker = new Cracking();
            
           
            Parallel.For(1, 9, i => cracker.RunCracking("dictionary" + i + ".txt"));
        }
    }
}
