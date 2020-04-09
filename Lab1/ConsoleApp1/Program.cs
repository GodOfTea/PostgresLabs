using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Lab1 lab1 = new Lab1();

            /* Заполнение базы данных */
            lab1.Start();

            Search search = new Search();

            search.StartSearch();
        }
    }
}
