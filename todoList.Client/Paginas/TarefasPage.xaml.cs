using System;
using System.Collections.Generic;
using todoList.Client.Logic;
using todoList.Client.Model;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace todoList.Client.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TarefasPage : Page
    {

        private List<Tarefa> lista;
        public TarefasPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*lista = new List<Tarefa>();
            for (int i = 0; i < 10; i++)
            {
                var t = new Tarefa
                {
                    Id = i,
                    Titulo = "Tarefa Nº " + i,
                    Descricao = "Descricao da minha tarefa um pouco maior por que eu preciso validar quebra de texto " + i,
                    Concluido = i % 2 == 0,
                    DataLimite = DateTime.Now,
                    Username = ""
                };

                lista.Add(t);
            }*/

            try
            {
                lista = await TarefasRequestApi.ListarAsync();

                lstDados.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                var msg = TratarException.ErrorMessage(ex);

                var dialog = new MessageDialog(msg, "ooopsss...");
                await dialog.ShowAsync();
            }

            lstDados.ItemsSource = lista;
        }

        private void lstDados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            var tarefa = listBox.SelectedItem as Tarefa;

            Frame.Navigate(typeof(NovaTarefaPage), tarefa);
        }
    }
}
