using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoResearchProject
{
    class Program
    {
        static void Main(string[] args)
        {
            AddMovie();
            Console.WriteLine("Press any Key");
            Console.ReadKey();

        }

        public static void AddMovie()
        {
            using (var db = GetDb())
            {
                db.Movies.Add(new Movies { Movie = "Kill Bill Vol. 1", Director = "Quentin Tarantino" });
                db.SaveChanges();
            }
        }

        public static MovieDb GetDb()
        {
            return new MovieDb();
        }
    }
}
