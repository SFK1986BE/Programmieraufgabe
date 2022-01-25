using System;
using System.Globalization;
using System.Numerics;


class CMain
{

    static private BigInteger Fibonacci(int n)
    {
        /*
            Je größer die Zahl n wird, für die die
            Fibonacci Zahl berechnet werden soll, desto
            höher wird der Rechenaufwand bei einer recusieven
            Berechnung. Der Aufwand steigt Exponentiell.
            Deswegen ist eine iterative berechnung deutlich 
            schneller.
        */

        // a_n = 0, n = 0
        // a_n = 1, n = 1
        // a_n = a_(n-1) + a_(n-2), n >= 2

        if (n == 0) return 0;
        if (n == 1) return 1;

        BigInteger  n_minus_1 = 1;  // n=2 -> n-1 = 1
        BigInteger  n_minus_2 = 0;  // n=2 -> n-2 = 0

        for (int i = 2; i < n; i++)
        {
            BigInteger zwischenergebenis = n_minus_1 + n_minus_2;
            n_minus_2 = n_minus_1;
            n_minus_1 = zwischenergebenis;
        }

        return n_minus_1 + n_minus_2;
    } /*Fibonacci*/


    static void ParameterToNumbers(String[] args, ref List<int> vs)
    {
        int ind = 0;
        foreach (String para in args)
        {
            if (ind == 0) continue;
            try
            {
                int i = Int32.Parse(para);
                vs.Add(i);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Parameter #" + ind + ": " + para + " -> " + ex.Message);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("Parameter #" + ind + ": " + para + " -> " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Parameter #" + ind + ": " + para + " -> " + ex.Message);
            }
            ind++;
        }
    } /*ParameterToNumbers*/


    static private void GenerateRandomNumbers(ref List<int> vs)
    {
        Random rnd = new();
        for (int i = 0; i < 600; i++)
        {
            vs.Add(rnd.Next(5000 + 1));
        }
    } /*GenerateRandomNumbers*/


    static public void Main(String[] args)
    {
        List<int> vs = new();
        if (args.Length != 0)
        {
            if (args[0].Equals("numbers")) ParameterToNumbers(args, ref vs);
            if (args[0].Equals("test"   )) GenerateRandomNumbers(ref vs);
        }
        else
        {
            Console.WriteLine("Geben Sie eine Zahl ein, von der die Fibonacci Zahl berechnet werden soll:");
            String? Num = Console.ReadLine();
            if (Num != null)
            {
                vs.Add(Int32.Parse(Num.ToString()));
            }
            else Console.WriteLine("Fehlerhafte Eingabe!");
        }

        DateTime Start = DateTime.Now;
        int Number = 1;
        foreach (int i in vs)
        {
            Console.WriteLine("(" + Number + ") Die Fibonacci Zahl für " + i + " ist: " + Fibonacci(i).ToString());
            Number++;
        }
        DateTime End = DateTime.Now;
        Console.WriteLine("\nDie Laufzeit betrug: " + (End-Start).ToString(@"hh\:mm\:ss"));
    } /*Main*/
} /*CMain*/