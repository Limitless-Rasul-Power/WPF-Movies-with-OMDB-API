namespace WpfApp_working_with_OMDB_API
{
    public class Movie
    {
        public Movie(string title, string plot, string poster, double imdbRating, string genre)
        {
            Title = title;
            Plot = plot;
            Poster = poster;
            ImdbRating = imdbRating;
            Genre = genre;
        }

        public string Title { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public double ImdbRating { get; set; }
        public string Genre { get; set; }
    }
}
