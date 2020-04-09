using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using CsvHelper;
using System.Collections;

namespace ConsoleApp1
{
    class Movie
    {
        public int id;          /*Id фильма */
        public string Title;    /* Название */
        public int Year;        /* Год */
    }

    class Lab1
    {
        private string connString = "Server=127.0.0.1;Port=5432;Database=PostgresLabs;User Id=postgres;Password=k1615k0089;";

        private List<string> moviesData = new List<string>();

        public static List<Movie> Movies = new List<Movie>();


        public void Start()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            var cmd = new NpgsqlCommand();
            cmd.Connection = connection;

            ReadCSVFile();
            StructTheData();

            /* Заполнение базы данных */
            /*for (int i = 0; i < Movies.Count; i++)
            {
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO movies VALUES (@id, @year, @name)", connection);
                command.Parameters.AddWithValue("@id", Movies[i].id);
                command.Parameters.AddWithValue("@year", Movies[i].Year);
                command.Parameters.AddWithValue("@name", Movies[i].Title);
                command.ExecuteNonQuery();
            }*/

            Console.WriteLine("Таблица заполнена!");
            Console.ReadLine();

            /* Расскоментить для вывода результата */
            /*string sql = "SELECT * FROM movies";
            cmd = new NpgsqlCommand(sql, connection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                Console.WriteLine("{0}, {1}, {2}", dataReader.GetInt64(0), dataReader.GetInt64(1), dataReader.GetString(2));
            }
            Console.ReadLine();*/
        }

        private void StructTheData()
        {
            int errorsCount = 0;
            bool error = false;

            string[] ids = new string[moviesData.Count];
            string[] titles = new string [moviesData.Count];
            string[] years = new string[moviesData.Count];

            for (int i = 0; i < moviesData.Count; i++)
            {
                try
                {
                    var line = moviesData[i].Split(',');
                    ids[i] = line[0];

                    var line2 = line[1].Split('(');
                    titles[i] = line2[0].Trim();

                    var line3 = line2[1].Split(')');
                    years[i] = line3[0];


                }
                catch
                {
                    errorsCount++;
                    years[i] = "0";
                    //continue;
                }
                finally 
                {
                    Movies.Add(
                        new Movie
                        {
                            id = int.Parse(ids[i]),
                            Title = titles[i],
                            Year = int.Parse(years[i])
                        });
                }

            }

            Console.WriteLine("Структурирование данных завершено!");
            Console.WriteLine("Ошибок обноружено: " + errorsCount);
        }

        private void ReadCSVFile()
        {
            string path = "G:\\IMDB Movie Titles.csv";
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"IMDB Movie Titles.csv");

            string line;

            using (StreamReader streamReader = new StreamReader(path))
            {
                //temp = streamReader.ReadToEnd().Split(temp, StringSplitOptions.RemoveEmptyEntries);

                while ((line = streamReader.ReadLine()) != null)
                {
                    moviesData.Add(line);
                }
            }

            Console.WriteLine("Парсинг завершен");
            moviesData.RemoveAt(0);
        }
    }
}
