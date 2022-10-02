using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Catalog.Common.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public string Genre { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public decimal Price { get; set; }
        public int Runtime { get; set; }
        public string Classification { get; set; }
    }
}
