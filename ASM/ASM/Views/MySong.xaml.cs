using ASM.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySong : Page
    {
        private static string GetMySongAPI = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine";
        private bool isPlaying = false;
        private ObservableCollection<Song> listSong;

        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }

        private async void GetSongs()
        {
            this.ListSong = new ObservableCollection<Song>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await folder.GetFileAsync("token.txt");
            var Content = await FileIO.ReadTextAsync(file);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Content);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
            HttpResponseMessage responseMessage = httpClient.GetAsync(GetMySongAPI).Result;
            string content = responseMessage.Content.ReadAsStringAsync().Result;
            var responseContent = responseMessage.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(file);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                ObservableCollection<Song> GetSong = JsonConvert.DeserializeObject<ObservableCollection<Song>>(content);
                foreach (var song in GetSong)
                {
                    this.ListSong.Add(song);
                }
                Debug.WriteLine("Done!");
            }
            else
            {
                ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                foreach (var key in errorObject.error.Keys)
                {
                    if (this.FindName(key) is TextBlock textBlock)
                    {
                        textBlock.Text = errorObject.error[key];
                    }
                }
            }
        }
        public MySong()
        {
            GetSongs();
            this.ListSong.Add(new Song()
            {
                name = "Hello Viet Nam",
                singer = "Unknow",
                thumbnail = "https://file.tinnhac.com/resize/600x-/music/2017/07/04/19554480101556946929-b89c.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui935/XinChaoVietNamViolinCover-JmiKo-4736247.mp3?st=HjtU7F5zduJ_7vPYQDCB9g&e=1540060130&t=1539973736378"
            });
            

            this.InitializeComponent();
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            Song selectedSong = panel.Tag as Song;
            LoadSong(selectedSong);
            PlaySong();
            Debug.WriteLine(MediaPlayer.Source);
        }

        private void PlaySong()
        {
            MediaPlayer.Play();
            isPlaying = true;
        }

        private void LoadSong(Entity.Song Song)
        {
            MediaPlayer.Source = new Uri(Song.link);

        }
    }
}
