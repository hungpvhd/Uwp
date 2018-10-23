using ASM.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ASM.Service
{
    class ApiHandle
    {
        private static string API_Register = "https://2-dot-backup-server-002.appspot.com/_api/v2/members";
        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        private static string GetMySongAPI = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine";


        public async static Task<string> Sign_Up(Member member)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(member), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_Register, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }

        public async static Task<string> Create_Song(Song song)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            HttpClient httpClient = new HttpClient();
            StorageFile file = await folder.GetFileAsync("token.txt");
            var Content = await FileIO.ReadTextAsync(file);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Content);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }

        
    }
}
