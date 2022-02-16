using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            var autocompleter = new AutoCompleter();
            autocompleter.AddToSearch(new List<FullName> {
                new FullName { Name = "Анастасия" },
                new FullName { Name = "Ан" },
                new FullName { Name = "Анна" },
                new FullName { Name = "Богдан" },
                new FullName { Name = "Боря" }
            });

            var result = autocompleter.Search("Ан");
            foreach (var str in result)
                Console.WriteLine(str);

            Console.WriteLine();

            result = autocompleter.Search("Бо");
            foreach (var str in result)
                Console.WriteLine(str);

            Console.WriteLine();

            autocompleter = new AutoCompleter();
            autocompleter.AddToSearch(new List<FullName> {
                new FullName { Surname = "  Хордин  ", Name = " Анатолий  ", Patronymic = "  Михайлович   " },
                new FullName { Surname = "   Ходайкин  ", Name = "    Миша    ", Patronymic = "   Петрович" },
                new FullName { Surname = "Арбузова", Name = " Анна  ", Patronymic = "  Павловна   " },
            });

            result = autocompleter.Search("Хо");
            foreach (var str in result)
                Console.WriteLine(str);
        }
    }
}
