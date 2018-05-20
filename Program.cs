using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskManager
{
    class Program
    {
        /* 
         * https://github.com/rav88/CodersLab-WarsztatyDOTNET_1 
        */

        static void Main(string[] args)
        {
            List<TaskModel> tasks = new List<TaskModel>();
            Console.ForegroundColor = ConsoleColor.White;

            string path = "Data.txt";
            LoadTasks(path, tasks);
            
            string command = "";

            do
            {
                Console.WriteLine("\nMENU:");
                Console.WriteLine("1. Dodaj zadanie");
                Console.WriteLine("2. Przeglądaj zadania ({0})", tasks.Count);
                Console.WriteLine("3. Usuń zadanie");
                Console.WriteLine("4. Zapisz listę zadań");
                Console.WriteLine("5. Usuń zadania przeterminowane");
                Console.WriteLine("'exit' by wyjść");
                Console.Write("Opcja: ");
                command = Console.ReadLine();


                if(command == "1")
                {
                    AddTask(tasks);
                }
                else if (command == "2")
                {
                    ShowTasks(tasks);
                }
                else if (command == "3")
                {
                    RemoveTask(tasks);
                }
                else if(command == "4")
                {
                    SaveTasks(tasks);
                }
                else if (command == "5")
                {
                    RemoveOutdatedTasks(tasks);
                }
                else if (command == "exit")
                {
                    break;
                }

            } while (true);
        }

        static void LoadTasks(string path, List<TaskModel> tasks)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines("Data.txt");

                foreach (var line in lines)
                {
                    string[] task = line.Split(';');

                    if (task.Length == 5)
                    {
                        string descr = task[0];
                        DateTime start = DateTime.Parse(task[1]);
                        DateTime? end;

                        if (task[2] == "")
                        {
                            end = null;
                        }
                        else
                        {
                            end = DateTime.Parse(task[2]);
                        }

                        bool important = task[3] == "Y" ? true : false;
                        bool allday = task[4] == "Y" ? true : false;

                        tasks.Add(new TaskModel(descr, start, end, important, allday));
                    }
                }

            }
        }

        static void AddTask( List<TaskModel> tasks )
        {
            ConsoleEx.Write("\nOpis zadania: ", ConsoleColor.Green);
            string descr = Console.ReadLine();

            ConsoleEx.Write("Start zadania: ", ConsoleColor.Green);
            string start = Console.ReadLine();
            DateTime date;
            bool isDateOk = DateTime.TryParse(start, out date);

            while (!isDateOk)
            {
                ConsoleEx.Write("Start zadania (rrrr-mm-dd [hh:mm]): ", ConsoleColor.Green);
                start = Console.ReadLine();
                isDateOk = DateTime.TryParse(start, out date);
            }


            ConsoleEx.Write("Ważne? Y/N: ", ConsoleColor.Green);
            string important = Console.ReadLine().ToLower();

            while (!(important == "n" || important == "y")) 
            {
                ConsoleEx.Write("Ważne? Y/N: ", ConsoleColor.Green);
                important = Console.ReadLine().ToLower();

            }


            ConsoleEx.Write("Całodzienne? Y/N: ", ConsoleColor.Green);
            string allday = Console.ReadLine().ToLower();

            while (!(allday == "n" || allday == "y"))
            {
                ConsoleEx.Write("Całodzienne? Y/N: ", ConsoleColor.Green);
                allday = Console.ReadLine().ToLower();

            }

                        

            var tm = new TaskModel(descr, date);

            if (important == "y")
            {
                tm.IsImportant = true;
            }

            if (allday == "y")
            {
                tm.IsAllday = true;
                tm.EndDate = null;
            }
            else
            {
                ConsoleEx.Write("Data zakończena zadania: ", ConsoleColor.Green);
                string end = Console.ReadLine();
                DateTime edate;
                bool iseDateOk = DateTime.TryParse(end, out edate);

                while (!iseDateOk)
                {
                    ConsoleEx.Write("Data zakończena zadania (rrrr-mm-dd [hh:mm]): ", ConsoleColor.Green);
                    end = Console.ReadLine();
                    iseDateOk = DateTime.TryParse(end, out edate);
                }

                tm.EndDate = edate;

            }

            tasks.Add(tm);
            ConsoleEx.WriteLine("Zadanie dodane.", ConsoleColor.Green);
            SaveTasks(tasks);
            
        }

        static void RemoveTask(List<TaskModel> tasks)
        {
            Console.WriteLine();
            
            for (int i = 0; i < tasks.Count; i++)
            {
                ConsoleEx.WriteLine($"[{i+1}] {tasks[i].Description} ({tasks[i].StartDate})", ConsoleColor.Red);
            }

            ConsoleEx.Write("\nKtóre zadanie usunąć? (podaj numer): ", ConsoleColor.Red);
            int index = Convert.ToInt32(Console.ReadLine());
            tasks.RemoveAt(index-1);
            ConsoleEx.WriteLine("Zadanie usunięte.", ConsoleColor.Red);
        }


        static void RemoveOutdatedTasks(List<TaskModel> tasks)
        {
            int count = 0;

            for (int i = 0; i < tasks.Count; i++)
            {
                if(tasks[i].EndDate < DateTime.Now || (tasks[i].EndDate == null && tasks[i].StartDate < DateTime.Now))
                {
                    tasks.RemoveAt(i);
                    count++;
                }
            }

            ConsoleEx.WriteLine($"Usunięte zadania: {count} ", ConsoleColor.Red);
        }

        static void ShowTasks(List<TaskModel> tasks)
        {
            ConsoleEx.Write("\nZadanie".PadRight(15), ConsoleColor.Cyan);
            ConsoleEx.Write("Start zadania".PadRight(25), ConsoleColor.Cyan);
            ConsoleEx.Write("Koniec zadania".PadRight(25), ConsoleColor.Cyan);
            ConsoleEx.Write("Ważne".PadRight(10), ConsoleColor.Cyan);
            ConsoleEx.WriteLine("Cały dzień".PadRight(10), ConsoleColor.Cyan);
            ConsoleEx.WriteLine("".PadRight(80, '-'), ConsoleColor.Cyan);

            // show important tasks
            foreach(var task in tasks)
            {
                if(task.IsImportant)
                {
                    ShowTask(task);
                }
            }

            // show others
            foreach (var task in tasks)
            {
                if (!task.IsImportant)
                {
                    ShowTask(task);
                }
            }
        }

        static void ShowTask(TaskModel task)
        {

                ConsoleEx.Write($"{task.Description}".PadRight(15), ConsoleColor.Cyan);
                ConsoleEx.Write($"{task.StartDate}".PadRight(25), ConsoleColor.Cyan);

                if (task.EndDate == null)
                {
                    ConsoleEx.Write("---".PadRight(25), ConsoleColor.Cyan);
                }
                else
                {
                    ConsoleEx.Write($"{task.EndDate}".PadRight(25), ConsoleColor.Cyan);
                }

                if (task.IsImportant)
                {
                    ConsoleEx.Write("Y".PadRight(10), ConsoleColor.Cyan);
                }
                else
                {
                    ConsoleEx.Write("N".PadRight(10), ConsoleColor.Cyan);
                }

                if(task.IsAllday)
                {
                    ConsoleEx.WriteLine("Y".PadRight(10), ConsoleColor.Cyan);
                }
                else
                {
                    ConsoleEx.WriteLine("N".PadRight(10), ConsoleColor.Cyan);
                }

                ConsoleEx.WriteLine("".PadRight(80, '-'), ConsoleColor.Cyan);
            }

        

        static void SaveTasks(List<TaskModel> tasks)
        {
            var sb = new StringBuilder();
            string line = "";

            foreach (var zadanie in tasks)
            {
                string e = zadanie.EndDate == null ? "" : zadanie.EndDate.ToString();
                string i = zadanie.IsImportant == true ? "Y" : "N";
                string a = zadanie.IsAllday == true ? "Y" : "N";

                line = $"{zadanie.Description};{zadanie.StartDate};{e};{i};{a}\n";
                sb.AppendLine(line);
            }

            File.WriteAllText("Data.txt", sb.ToString());
            ConsoleEx.WriteLine("Zapisane.", ConsoleColor.Magenta);
            
        }

    }

}
