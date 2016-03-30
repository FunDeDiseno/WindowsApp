using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace federacionHemofiliaPacienteApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WebRequest request;
        public MainPage()
        {
            this.InitializeComponent();
            request = (HttpWebRequest)WebRequest.Create("http://localhost:5000/paciente/login");
            request.Method = "POST";
            request.ContentType = "application/json";

        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                var loginVm = new LoginVM
                {
                    Email = email.Text,
                    Password = passwordBox.Password,
                    RememberMe = true
                };
                string json = JsonConvert.SerializeObject(loginVm);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Dispose();
            }

            var httpResponse = await request.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var deserialize = JsonConvert.DeserializeObject(result);
                if ((string)deserialize != "locked" || (string)deserialize != "something went wrong")
                {
                    this.Frame.Navigate(typeof(RegisterApp), deserialize);
                }
            }
        }
    }

    class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
