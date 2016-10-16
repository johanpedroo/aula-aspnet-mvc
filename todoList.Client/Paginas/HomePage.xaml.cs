using todoList.Client.Logic;
using todoList.Client.Model;
using Newtonsoft.Json;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace todoList.Client.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private Usuario usuario;
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var usuarioSalvo = (string)MyLocalStorage.GetFromLocalStorage(@"usuario");

            if (string.IsNullOrWhiteSpace(usuarioSalvo))
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                usuario = JsonConvert.DeserializeObject<Usuario>(usuarioSalvo);

                txbTitulo.Text = $@"Olá {usuario.Login.Split('@')[0]}!";

                frmConteudo.Navigate(typeof(TarefasPage));
            }
        }

        private void btnHamburguer_Click(object sender, RoutedEventArgs e)
        {
            spvMenu.IsPaneOpen = !spvMenu.IsPaneOpen;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frmConteudo.CanGoBack)
            {
                frmConteudo.GoBack();
            }
            else
            {
                btnBack.Visibility = Visibility.Collapsed;
                lbiHome.IsSelected = true;
            }
        }

        private void lstMenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbiLogOff.IsSelected)
            {
                MyLocalStorage.RemoveFromLocalStorage(@"usuario");
                Frame.Navigate(typeof(MainPage));
            }
            else if (!lbiLogOff.IsSelected && !lbiHome.IsSelected)
            {
                btnBack.Visibility = Visibility.Visible;

                if (lbiTarefas.IsSelected)
                {
                    frmConteudo.Navigate(typeof(NovaTarefaPage));
                }
            }
            else
            {
                btnBack.Visibility = Visibility.Collapsed;
                frmConteudo.Navigate(typeof(TarefasPage));
            }
        }
    }
}
