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
            
            Thread thread1 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary1.txt")));
            Thread thread2 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary2.txt")));
            Thread thread3 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary3.txt")));
            Thread thread4 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary4.txt")));
            Thread thread5 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary5.txt")));
            Thread thread6 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary6.txt")));
            Thread thread7 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary7.txt")));
            Thread thread8 = new Thread(new ThreadStart(() => cracker.RunCracking("dictionary8.txt")));

            Parallel.Invoke(thread1.Start, thread2.Start, thread3.Start, thread4.Start, thread5.Start, thread6.Start, thread7.Start, thread8.Start);
            /*, thread3.Start, thread4.Start, thread5.Start, thread6.Start, thread7.Start, thread8.Start
             */
        }
    }
}
