using ASM.Entity;
using ASM.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SongForm : Page
    {
        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        private Song currentSong;
        public SongForm()
        {
            this.currentSong = new Song();
            this.InitializeComponent();
        }

        public object RegisterInfor { get; private set; }

        private async void SignUp(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> SongInfo = new Dictionary<string, string>();
            SongInfo.Add("singer", this.Singer.Text);
            SongInfo.Add("link", this.Link.Text);

            this.currentSong.name = this.Name.Text;
            this.currentSong.description = this.Description.Text;
            this.currentSong.singer = this.Singer.Text;
            this.currentSong.author = this.Author.Text;
            this.currentSong.thumbnail = this.Thumbnail.Text;
            this.currentSong.link = this.Link.Text;
            validatSong();
            await ApiHandle.Create_Song(this.currentSong);
            Debug.WriteLine("Action success.");

        }

        private async void validatSong()
        {
        //    HttpClient httpClient = new HttpClient();
        //    StringContent content = new StringContent(JsonConvert.SerializeObject(RegisterInfor), System.Text.Encoding.UTF8, "application/json");
        //    var response = httpClient.PostAsync(SONG_API_URL, content).Result;
        //    var responseContent = await response.Content.ReadAsStringAsync();
            
        //    Debug.WriteLine("Error: " + responseContent);

        //    ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
        //    if (errorObject != null && errorObject.error.Count > 0)
        //    {
        //        foreach (var key in errorObject.error.Keys)
        //        {
        //            var textMessage = this.FindName(key);
        //            if (textMessage == null)
        //            {
        //                continue;
        //            }
        //            TextBlock textBlock = textMessage as TextBlock;
        //            textBlock.Text = errorObject.error[key];
        //            textBlock.Visibility = Visibility.Visible;
        //        }



            if (this.Name.Text == "" && this.Description.Text == "" && this.Author.Text == "" && this.Thumbnail.Text == "" && this.Singer.Text == "" && this.Link.Text == "")
            {
                this.error_Info.Text = "Please enter full introduction";
            }
            else
            {
                this.link.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Name.Text = "";
            this.Description.Text = "";
            this.Author.Text = "";
            this.Thumbnail.Text = "";
            this.Singer.Text = "";
            this.Link.Text = "";
        }
    }
}
