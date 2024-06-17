namespace FilmsApp.Models
{
    public class Film
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Year { get; set; }
        public string? Director { get; set; }
        public string? Actors { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }     
        public string? Image { get; set; }     
    }
}
