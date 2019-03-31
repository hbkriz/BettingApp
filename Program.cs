using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BettingApp
{
    class Program
    {
        static object   Locker      = new object();
        static Random   RNG         = new Random();
        static int[]    Deck1       = { 1, 2, 3 };
        static int[]    Deck2       = { 1, 2, 3 };
        static long[]   MyMethod    = new long[322];
        static long[]   FyMethod    = new long[322];
        static long     Trials      = 0;


        static void Main(string[] args)
        {
            // double[] results = MonteCarlo.simulate(2, new double[] { 11, 12 }, new double[] { 12.5, 13 }, new double[] { 17, 15 });
            // Console.Write("Simulation Results<br/>");
            // foreach (double player in results)
            // {
            //     Console.Write("Wins" + player + "<br/>");
            // }

            // //show probability (TEST ONLY)
            // double total = results[0] + results[1];
            // double player1 = results[0] / total * 100;
            // double player2 = results[1] / total * 100;
            // Console.Write("Player1 win probability " + player1);
            // Console.Write("<br/>  - Player2 win probability" + player2);

            
            
            //Console.WriteLine(Markov.Simulate("alice_oz.txt", 3, 200));
        

            Thread t1 = new Thread(RunSimulation);
            Thread t2 = new Thread(Monitor);

            t1.Start();
            t2.Start();

            t2.Join();

        }

        

        public static double triangular(double  Min,double  Mode,double  Max)
        { 
            //   Declarations
            double  R=0.0;
            //   Initialise
            Random r = new Random();
            R = r.NextDouble();       //between 0.0 and 1.0 gaussian
            //    Triangular
            if ( R == (( Mode -  Min) / ( Max -  Min)))
            {
                return  Mode;
            }
            else if ( R < (( Mode -  Min) / ( Max -  Min)))
            {
                return  Min + Math.Sqrt( R * ( Max -  Min) * ( Mode -  Min));
            }
            else
            {
                return  Max - Math.Sqrt((1 -  R) * ( Max -  Min) * ( Max -  Mode));
            }
        }

         static void RunSimulation()
        {
            while (true)
            {
                // MY METHOD

                for (int x = 0; x < 3; x++)
                {
                    int r    = RNG.Next(3);
                    int t    = Deck1[x];
                    Deck1[x] = Deck1[r];
                    Deck1[r] = t;
                }

                // FISHER-YATES METHOD

                for (int x = 2; x > 0; x--)
                {
                    int r    = RNG.Next(x + 1);
                    int t    = Deck2[x];
                    Deck2[x] = Deck2[r];
                    Deck2[r] = t;
                }

                // UPDATE RESULTS

                lock (Locker)
                {
                    MyMethod[(Deck1[0] * 100) + (Deck1[1] * 10) + Deck1[2]]++;
                    FyMethod[(Deck2[0] * 100) + (Deck2[1] * 10) + Deck2[2]]++;
                    Trials++;
                }
            }
        }

        static void Monitor()
        {
            while (true)
            {
                lock (Locker)
                {
                    Console.Clear();
                    Console.WriteLine("PERM\tMY METHOD\tFY METHOD");
                    Console.WriteLine();
                    Console.WriteLine("123\t" + MyMethod[123] + "\t" + FyMethod[123]);
                    Console.WriteLine("132\t" + MyMethod[132] + "\t" + FyMethod[132]);
                    Console.WriteLine("213\t" + MyMethod[213] + "\t" + FyMethod[213]);
                    Console.WriteLine("231\t" + MyMethod[231] + "\t" + FyMethod[231]);
                    Console.WriteLine("312\t" + MyMethod[312] + "\t" + FyMethod[312]);
                    Console.WriteLine("321\t" + MyMethod[321] + "\t" + FyMethod[321]);
                    Console.WriteLine();
                    Console.WriteLine("TRIALS\t" + Trials);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
