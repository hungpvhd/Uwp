using ASM.Entity;
using ASM.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private Song currentSong;
        public SongForm()
        {
            this.currentSong = new Song();
            this.InitializeComponent();
        }

        private async void SignUp(object sender, RoutedEventArgs e)
        {
            this.currentSong.name = this.Name.Text;
            this.currentSong.description = this.Description.Text;
            this.currentSong.singer = this.Singer.Text;
            this.currentSong.author = this.Author.Text;
            this.currentSong.thumbnail = this.Thumbnail.Text;
            this.currentSong.link = this.Link.Text;
            await ApiHandle.Create_Song(this.currentSong);
            Debug.WriteLine("Action success.");

            if (this.Name.Text == "" && this.Description.Text == "" && this.Name.Text == "" && this.Name.Text == "" && this.Name.Text == "" && this.Name.Text == "")
            {
                this.error_Info.Text = "Please enter full introduction";
            }
        }
    }
}
