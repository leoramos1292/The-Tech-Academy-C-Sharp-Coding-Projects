using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoResearchProject
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public string Movie { get; set; }
        public string Director { get; set; }
    }
}
