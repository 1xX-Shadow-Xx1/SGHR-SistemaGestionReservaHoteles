using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using WPFapp.Models;

namespace WPFapp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5020/") 
            };
        }
        private async void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Llamada al endpoint GET: api/Usuario
                var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("api/Usuario/Get-Usuarios");

                dgUsuarios.ItemsSource = usuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuarios: {ex.Message}");
            }
        }
    }
}
