using System;
using System.Collections.Generic;
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
using System.Net;
using Newtonsoft.Json;

namespace federacionHemofiliaPacienteApp
{
    public sealed partial class RegisterApp : Page
    {
        private string Id;
        private WebRequest request;
        public RegisterApp()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Id = (string)e.Parameter;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["UserId"] = Id;
            base.OnNavigatedTo(e);
        }

        private async void PostUser()
        {
            request = (HttpWebRequest)WebRequest.Create("http://localhost:5000/paciente/registerApliacion/");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                var aplicacion = new AplicacionVM
                {
                    nuevaAplicacion = new Aplicacion
                    {
                        cantidad = Convert.ToInt16(cantidad.Text),
                        fecha = DateTime.Now
                    },
                    userId = Id
                };

                string json = JsonConvert.SerializeObject(aplicacion);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Dispose();
            }

            var httpResponse = await request.GetResponseAsync();
        }

        private void appBtn_Click(object sender, RoutedEventArgs e)
        {
            PostUser();
        }

        private void closeSession_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["UserId"] = null;
            this.Frame.Navigate(typeof(MainPage), null);
        }
    }

    class AplicacionVM
    {
        public Aplicacion nuevaAplicacion { get; set; }
        public string userId { get; set; }
    }

    class Aplicacion
    {
        public int cantidad { get; set; }
        public DateTime fecha { get; set; }
    }
}
