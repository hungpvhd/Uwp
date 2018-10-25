using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using ASM.Entity;
using ASM.Service;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using ASM;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class SignUp : Page  
    {
        private Member currentMember;
        private static StorageFile file;
        private static string UploadUrl;
        private static string API_Register = "https://2-dot-backup-server-002.appspot.com/_api/v2/members";
        public SignUp()
        {
            GetUploadUrl();
            this.currentMember = new Member();
            this.InitializeComponent();
        }

        private async void Capture_Photo(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null)
            {
                // User cancelled photo capture
                return;
            }
            HttpUploadFile(UploadUrl, "myFile", "image/png");
        }

        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            //write file.
            
            Stream fileStream = await file.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
               rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                Debug.WriteLine(u.AbsoluteUri);
                ImageUrl.Text = u.AbsoluteUri;
                MyAvatar.Source = new BitmapImage(u);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        private async void Handle_Signup(object sender, RoutedEventArgs e)
        {
            this.currentMember.firstName = this.FirstName.Text;
            this.currentMember.lastName = this.LastName.Text;
            this.currentMember.email = this.Email.Text;
            this.currentMember.password = this.Password.Password.ToString();
            this.currentMember.avatar = this.ImageUrl.Text;
            this.currentMember.phone = this.Phone.Text;
            this.currentMember.address = this.Address.Text;
            this.currentMember.introduction = this.Introduction.Text;
            validateRegister();
            await ApiHandle.Sign_Up(this.currentMember);
        }

        private async void validateRegister()
        {
            Dictionary<String, String> RegisterInfor = new Dictionary<string, string>();
            RegisterInfor.Add("email", this.Email.Text);
            RegisterInfor.Add("firstName", this.FirstName.Text);
            RegisterInfor.Add("lastName", this.LastName.Text);
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(RegisterInfor), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_Register, content).Result;
            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Action success.");

            ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
            if (errorObject != null && errorObject.error.Count > 0)
            {
                foreach (var key in errorObject.error.Keys)
                {
                    var textMessage = this.FindName(key);
                    if (textMessage == null)
                    {
                        continue;
                    }
                    TextBlock textBlock = textMessage as TextBlock;
                    textBlock.Text = errorObject.error[key];
                    textBlock.Visibility = Visibility.Visible;
                }
            }

            if (this.Email.Text == "")
            {

                this.error_Email.Text = "Please enter introduction";
                this.email.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.email.Visibility = Visibility.Visible;
            };

            if (this.Password.ToString() == "")
            {

                this.error_Password.Text = "Please enter introduction";
            }
            if (this.FirstName.Text == "")
            {

                this.error_Fname.Text = "Please enter introduction";
                this.firstName.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.firstName.Visibility = Visibility.Visible;
            };
            if (this.LastName.Text == "")
            {

                this.error_Lname.Text = "Please enter introduction";
                this.lastName.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.lastName.Visibility = Visibility.Visible;
            }
            if (this.Introduction.Text == "")
            {

                this.error_Intro.Text = "Please enter introduction";
            }
            if (this.Address.Text == "")
            {

                this.error_Add.Text = "Please enter introduction";
            }
            if (this.Phone.Text == "")
            {

                this.error_Phone.Text = "Please enter introduction";
            }
        }

        //image,cam
        private static async void GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://2-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            Debug.WriteLine(httpResponseBody);
            UploadUrl = httpResponseBody;
        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
            Debug.WriteLine(this.currentMember.gender);
        }

        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private async void Select_Photo(object sender, RoutedEventArgs e)
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
            StorageFile fileLocal = await fileOpenPicker.PickSingleFileAsync();
            if (fileLocal != null)
            {
                // Open a stream
                Windows.Storage.Streams.IRandomAccessStream fileStream =
                await fileLocal.OpenAsync(FileAccessMode.Read);

                // Create a BitmapImage and Set stream as source
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);

                // Set BitmapImage Source
                MyAvatar.Source = bitmapImage;
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(UploadUrl);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";

                Stream rs = await wr.GetRequestStreamAsync();
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", "myFile", "path_file", "image / png");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                Stream fileStreamLocal = await fileLocal.OpenStreamForReadAsync();
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStreamLocal.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);

                WebResponse wresp = null;
                try
                {
                    wresp = await wr.GetResponseAsync();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                    //string imgUrl = reader2.ReadToEnd();
                    Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                    Debug.WriteLine(u.AbsoluteUri);
                    ImageUrl.Text = u.AbsoluteUri;
                    this.MyAvatar.Source = new BitmapImage(u);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error uploading file", ex.StackTrace);
                    Debug.WriteLine("Error uploading file", ex.InnerException);
                    if (wresp != null)
                    {
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }
            }
        }

        private void TapToLogin(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Email.Text = "";
            this.Password.Password = "";
            this.FirstName.Text = "";
            this.LastName.Text = "";
            this.ImageUrl.Text = "";
            this.Introduction.Text = "";
            this.Address.Text = "";
            this.Phone.Text = "";

        }
    }
}
            

        
