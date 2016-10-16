

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using System;
using System.Text.RegularExpressions;
using todoList.Client.Logic;
using todoList.Client.Model;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace todoList.Client.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NovoCadastro : Page
    {


        public NovoCadastro()
        {
            this.InitializeComponent();
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            var tudoPreenchido =
                            !(
                                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                                string.IsNullOrWhiteSpace(txtConfirmacao.Password)
                            );

            if (!tudoPreenchido)
            {
                var dialog = new MessageDialog(
                    "Todos os campos são obrigatórios!",
                    "Atenção!!!"
                    );
                await dialog.ShowAsync();
            }
            else
            {
                if (!ValidarEmail(txtEmail.Text))
                {
                    var dialog = new MessageDialog(
                    "Email inválido!",
                    "Atenção!!!"
                    );
                    await dialog.ShowAsync();
                }
                else
                {
                    if (txtPassword.Password.Length < 6)
                    {
                        var dialog = new MessageDialog(
                        "A senha tem que ter no mínimo 6 caracteres!",
                        "Atenção!!!"
                        );
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        if (txtPassword.Password != txtConfirmacao.Password)
                        {
                            var dialog = new MessageDialog(
                            "Senha e Confirmação devem ser iguais!",
                            "Atenção!!!"
                            );
                            await dialog.ShowAsync();
                        }
                        else
                        {
                            try
                            {
                                var coisa = await NovoUsuarioRequestApi.Cadastrar(
                                    txtEmail.Text,
                                    txtPassword.Password,
                                    txtConfirmacao.Password
                                    );

                                if (coisa)
                                {
                                    var token = await LoginRequestApi.Login(
                                        txtEmail.Text,
                                        txtPassword.Password
                                        );

                                    var usuario = new Usuario
                                    {
                                        Login = txtEmail.Text,
                                        Senha = txtPassword.Password
                                    };

                                    var json = JsonConvert.SerializeObject(usuario);

                                    MyLocalStorage.SaveToLocalStorage("usuario", json);

                                    MyLocalStorage.SaveToLocalStorage("token", token);

                                    Frame.Navigate(typeof(HomePage));
                                }
                            }
                            catch (Exception ex)
                            {
                                var dialog = new MessageDialog(
                                TratarException.ErrorMessage(ex),
                                "Atenção!!!"
                                );
                                await dialog.ShowAsync();
                            }
                        }
                    }
                }
            }
        }

        private static bool ValidarEmail(string email)
        {
            var regExpEmail = new Regex("^[A-Za-z0-9](([_.-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([.-]?[a-zA-Z0-9]+)*)([.][A-Za-z]{2,4})$");

            return regExpEmail.Match(email).Success;
        }
    }
}
