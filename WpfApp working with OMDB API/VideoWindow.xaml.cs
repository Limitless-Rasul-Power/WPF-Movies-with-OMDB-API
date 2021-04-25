using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp_working_with_OMDB_API
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {      
        public VideoWindow()
        {
            InitializeComponent();
        }

        public string VideoSource
        {
            get
            {
                return FrmVideo.Source.ToString();
            }
            set
            {
                string[] movieWords = value.Split(' ');

                StringBuilder fullPath = new StringBuilder();

                for (int i = 0; i < movieWords.Length; i++)
                {
                    fullPath.Append(movieWords[i]);

                    if (i + 1 < movieWords.Length)
                        fullPath.Append('+');
                }

                FrmVideo.Source = new Uri($@"https://www.youtube.com/results?search_query={fullPath}+trailer");
            }
        }
      
    }
}
