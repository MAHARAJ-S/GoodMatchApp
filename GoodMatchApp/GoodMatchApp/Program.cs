using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace GoodMatchApp
{

    class Program
    {

        static void Main(string[] args)
        {
            string firstname = "";
            string lastname = "";
            string option = "";

            while (true)
            {

                Console.WriteLine("WELCOME TO THE GOOD MATCH APPLICATION");
                Console.WriteLine("");
                Console.WriteLine("ENTER AN OPTION AND PRESS ENTER");
                Console.WriteLine("");
                Console.WriteLine("OPT 1: ENTER TWO NAMES AND GET THEIR MATCH PERCENTAGE");
                Console.WriteLine("OPT 2: ENTER THE CSV FILENAME AND MATCH MANY NAMES");
                Console.WriteLine("OPT 3: EXIT");
                Console.Write(":");
                option = Console.ReadLine();

                if (option == "1")
                {

                    while (true)
                    {
                        Console.Write("Enter the 1st name: ");
                        firstname = Console.ReadLine();

                        Console.Write("Enter the 2nd name: ");
                        lastname = Console.ReadLine();

                        string check = firstname + lastname;
                        bool result = check.All(Char.IsLetter);

                        if (!result)
                        {
                            Console.WriteLine("Invalid Input, Enter Valid Input");
                            Console.WriteLine();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    Console.WriteLine(produceOutput(firstname, lastname));



                }
                else if (option == "3")
                {
                    System.Environment.Exit(0);

                }
                else if (option == "2")
                {

                    Console.Write("Enter the CSV filename: ");
                    string fileName = Console.ReadLine();



                    using (var reader = new StreamReader(fileName))
                    {
                        List<string> names = new List<string>();
                        List<string> genders = new List<string>();

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            names.Add(values[0]);
                            genders.Add(values[1]);
                        }
                        Console.WriteLine("CSV file Read Successfully \n");
                        Console.WriteLine("");
                        Console.WriteLine("Processing Input...\n");
                        for (int i = 0; i < names.Count; i++)
                        {

                            for (int j = i + 1; j < names.Count; j++)
                            {
                                if (names[i] == names[j] && genders[i] == genders[j])
                                {


                                    names.RemoveAt(j);
                                    genders.RemoveAt(j);


                                }
                            }
                        }

                        List<string> males = new List<string>();
                        List<string> females = new List<string>();


                        for (int i = 0; i < names.Count; i++)
                        {
                            if (genders[i] == "m")
                            {
                                males.Add(names[i]);
                            }
                            else if (genders[i] == "f")
                            {
                                females.Add(names[i]);
                            }
                        }
                        for (int i = 0; i < females.Count; i++)
                        {

                        }

                        Console.WriteLine("Completed Processing Input...");

                        try
                        {
                            StreamWriter sw = new StreamWriter("Output.txt");
                            for (int i = 0; i < males.Count; i++)
                            {
                                for (int j = 0; j < females.Count; j++)
                                {

                                    sw.WriteLine(produceOutput(males[i], females[j]));

                                }

                            }

                            sw.Close();
                            Console.WriteLine("Output Written To Output.txt... \n");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: " + e.Message);
                        }

                    }


                }
                else
                {
                    Console.WriteLine("Enter A Valid Option");
                }


            }

        }
        static string calculateMatch(string sentence)
        {
            string temporary = sentence;
            string occuranceString = "";
            string sumString = "";


            while (temporary.Length > 0)
            {
                char ch = temporary[0];
                int frequency = temporary.Count(f => (f == ch));
                occuranceString = occuranceString + frequency;

                temporary = temporary.Replace(temporary[0].ToString(), String.Empty);

            }


            sumString = produceSums(occuranceString);

            return produceMatches(sumString);

        }
        static string produceOutput(string firstname, string lastname)
        {
            string name1 = firstname.ToLower();
            string name2 = lastname.ToLower();

            string check = firstname + lastname;
            bool result = check.All(Char.IsLetter);

            if (!result)
            {
                return ("Invalid Input, Enter Valid Input");
            }

            string sentence = name1 + " matches " + name2;
            sentence = String.Concat(sentence.Where(c => !Char.IsWhiteSpace(c)));
            int percentage = Int32.Parse(calculateMatch(sentence));

            if (percentage >= 80)
            {
                return (firstname + " matches " + lastname + " " + percentage + "%, good match \n");

            }
            else
            {
                return (firstname + " matches " + lastname + " " + percentage + " % \n");

            }
        }

        static string produceSums(string occuranceString)
        {
            string myString = "";

            while (occuranceString.Length > 1)
            {
                int sum = 0;
                int left = (int)Char.GetNumericValue(occuranceString[0]);
                int right = (int)Char.GetNumericValue(occuranceString[occuranceString.Length - 1]);
                sum = left + right;
                myString = myString + "" + sum.ToString();
                occuranceString = occuranceString.Remove(0, 1);
                occuranceString = occuranceString.Remove((occuranceString.Length) - 1);


                if (occuranceString.Length == 1)
                {
                    myString = myString + "" + occuranceString;

                }
            }

            return myString;
        }

        static string produceMatches(string sumString)
        {
            if (sumString.Length == 2)
            {
                return sumString;
            }
            else
            {
                return produceMatches(produceSums(sumString));
            }

        }

    }
}