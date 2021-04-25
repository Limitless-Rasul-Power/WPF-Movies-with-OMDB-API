using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_working_with_OMDB_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient http = new HttpClient();
        private readonly List<Movie> _movies = new List<Movie>();
        private int _currentNumber = default;
        private bool _isMovieFind = default;

        public MainWindow()
        {
            InitializeComponent();
            TxtBxMovieName.Focus();
        }

        private void ImgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Search();
        }

        private void TxtBxMovieName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void BtnWatch_Click(object sender, RoutedEventArgs e)
        {
            if (_isMovieFind)
            {
                this.Hide();

                VideoWindow videoWindow = new VideoWindow
                {
                    VideoSource = _movies[_currentNumber].Title.ToLower()
                };

                try
                {
                    videoWindow.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBox.Show($"\"{_movies[_currentNumber].Title}\" movie did not found.");
                }
                finally
                {
                    videoWindow.Close();
                    videoWindow.VideoSource = $@"https://www.youtube.com";
                }

                this.Show();
            }


        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNumber - 1 >= 0 && _isMovieFind)
            {
                _currentNumber -= 1;
                SetDataForMovie();

                if (_currentNumber - 1 < 0)
                    BtnPrevious.Visibility = Visibility.Collapsed;

                BtnNext.Visibility = Visibility.Visible;
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNumber + 1 < _movies.Count && _isMovieFind)
            {
                _currentNumber += 1;
                SetDataForMovie();

                if (_currentNumber + 1 >= _movies.Count)
                    BtnNext.Visibility = Visibility.Collapsed;

                BtnPrevious.Visibility = Visibility.Visible;
            }
        }


        private void SetDataForMovie()
        {
            try
            {
                ImgMovie.Source = new BitmapImage(new Uri(_movies[_currentNumber].Poster));
            }
            catch (Exception)
            {
                ImgMovie.Source = new BitmapImage(new Uri(@"pack://application:,,,/img/no image.png"));
            }

            TxtBlckTitle.Text = $"{_movies[_currentNumber].Title}, IMDB Rating {_movies[_currentNumber].ImdbRating}";
            TxtBlckPlot.Text = _movies[_currentNumber].Plot;
            TxtBlckGenre.Text = _movies[_currentNumber].Genre;
        }


        private void Search()
        {
            try
            {
                this.Cursor = Cursors.Wait;

                _movies.Clear();
                _currentNumber = default;

                const string APIKEY = "e0c9d947";

                HttpResponseMessage response = new HttpResponseMessage();
                response = http.GetAsync($@"http://www.omdbapi.com/?s={TxtBxMovieName.Text.Trim()}&apikey={APIKEY}&plot=full").Result;

                string dataSetsFromMovieName = response.Content.ReadAsStringAsync().Result;

                dynamic firstData = JsonConvert.DeserializeObject(dataSetsFromMovieName);


                for (int i = 0; i < firstData.Search.Count; i++)
                {

                    response = http.GetAsync($@"http://www.omdbapi.com/?i={firstData.Search[i].imdbID}&apikey={APIKEY}&plot=full").Result;

                    string dataSetFromMovieImdbID = response.Content.ReadAsStringAsync().Result;
                    dynamic movie = JsonConvert.DeserializeObject(dataSetFromMovieImdbID);

                    _movies.Add(new Movie(movie.Title.ToString(), movie.Plot.ToString(), movie.Poster.ToString(), Convert.ToDouble(movie.imdbRating), movie.Genre.ToString()));
                }

                _isMovieFind = true;

                BtnPrevious.Visibility = Visibility.Collapsed;
                BtnNext.Visibility = (_movies.Count == 1) ? Visibility.Collapsed : Visibility.Visible;

                SetDataForMovie();

                this.Cursor = Cursors.Hand;
            }
            catch (Exception caption)
            {
                MessageBox.Show(caption.Message, "Movie Life..");
                this.Cursor = Cursors.Hand;
            }

        }

    }
}