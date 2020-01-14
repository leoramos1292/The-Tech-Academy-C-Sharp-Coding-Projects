namespace LeoResearchProject
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MovieDb : DbContext
    {
        
        public MovieDb()
            : base("name=MovieDb")
        {
        }

        public virtual DbSet<Movies> Movies { get; set; }

    }

  
}