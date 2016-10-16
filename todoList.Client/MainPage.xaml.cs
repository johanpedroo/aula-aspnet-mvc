using Newtonsoft.Json;
using todoList.Client.Logic;
using todoList.Client.Model;
using todoList.Client.Paginas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace todoList.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var usuario = (string)MyLocalStorage.GetFromLocalStorage("usuario");

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                Frame.Navigate(typeof(HomePage));
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                var dialog = new MessageDialog(@"O Login e a Senha são obrigatórios!", "Atenção!!");
                await dialog.ShowAsync();
            }
            else
            {
                var usuario = new Usuario
                {
                    Login = txtLogin.Text,
                    Senha = txtPassword.Password
                };

                try
                {
                    var token = await LoginRequestApi.Login(txtLogin.Text, txtPassword.Password);

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var json = JsonConvert.SerializeObject(usuario);

                        MyLocalStorage.SaveToLocalStorage(@"usuario", json);

                        MyLocalStorage.SaveToLocalStorage("token", token);

                        Frame.Navigate(typeof(HomePage));
                    }
                    else
                    {
                        var dialog = new MessageDialog(@"Não foi possível fazer o login... Tente novamente", "Atenção!!");
                        await dialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    var dialog = new MessageDialog(TratarException.ErrorMessage(ex), "Atenção!!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NovoCadastro));
        }
    }
}