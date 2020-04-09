using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Search
    {
        public void StartSearch()
        {
            HelloMessage();

            bool endProgram = false;

            while (endProgram == false)
            {
                Console.WriteLine("-----------------");
                Console.WriteLine("Введите поисковой запрос или команду выхода exit:");
                var request = Console.ReadLine();

                if (request == "exit")
                {
                    endProgram = true;
                    continue;
                }
                else
                {
                    SearchSystem(request);

                    Console.WriteLine();
                }
            }
            Console.WriteLine("Спасибо, что воспользовались нашей поисковой системой, до скорой встречи");
            Console.ReadKey();

        }

        private void HelloMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Поисковая система запущена");
        }

        private void SearchSystem(string request)
        {
            string connString = "Server=127.0.0.1;Port=5432;Database=PostgresLabs;User Id=postgres;Password=k1615k0089;";
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            var cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            string sql = "SELECT * FROM movies WHERE ";

            int year = 0;
            string title = "";
            string[] line = request.Split();

            /* В запросе есть год? */
            bool isYear = false;
            for (int i=0; i<line.Length; i++)
            {
                if (line[i].Length == 4 && int.TryParse(line[i], out year))
                {
                    isYear = true;
                    title = request.Replace(line[i], "");
                }
            }
            

            /* Поиск по имени и году */
            if (line.Length > 1 && isYear == true)
            {
                sql += ("name LIKE '%' || \'"+ title + "\' || '%' and year ="+ year.ToString() + " LIMIT 10");
            }
            /* Поиск только по году */
            else if (isYear == true)
            {
                sql += ("year ="+ year.ToString() + " LIMIT 10");
            }
            /* Поиск только по имени */
            else
            {
                sql += ("name LIKE '%' || \'"+ request + "\' || '%' LIMIT 10");
            }


            cmd = new NpgsqlCommand(sql, connection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();

            Console.WriteLine("-----------------");
            Console.WriteLine("Результат поиска:");
            while (dataReader.Read())
            {
                Console.WriteLine("Название фильма: {2} | Год выпуска: {1} | id: {0}", dataReader.GetInt64(0), dataReader.GetInt64(1), dataReader.GetString(2));
            }
            Console.ReadKey();
        }
    }
}
