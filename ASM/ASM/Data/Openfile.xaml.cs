using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Data
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Openfile : Page
    {
        public Openfile()
        {
            this.InitializeComponent();
        }

        private async void abc(object sender, RoutedEventArgs e)
        {
           // Create FileOpenPicker instance
           FileOpenPicker fileOpenPicker = new FileOpenPicker();

            // Set SuggestedStartLocation
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            // Set ViewMode
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            // Filter for file types. For example, if you want to open text files,
            // you will add .txt to the list.

            fileOpenPicker.FileTypeFilter.Clear();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");

            // Open FileOpenPicker
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Open a stream
                Windows.Storage.Streams.IRandomAccessStream fileStream =
                await file.OpenAsync(FileAccessMode.Read);

                // Create a BitmapImage and Set stream as source
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);

                // Set BitmapImage Source
                PreviewImage.Source = bitmapImage;
            }
        }
        }
    }