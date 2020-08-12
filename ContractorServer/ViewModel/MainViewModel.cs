using ContractorServer.Commands;
using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace ContractorServer.ViewModel
{
    public class MainViewModel
    {
        const string url = "api/Users/";
        static readonly HttpClient client = new HttpClient();
        private RelayCommand addUserCommand;
        public ObservableCollection<User> Users { get; set; }
        public MainViewModel()
        {
            client.BaseAddress = new Uri("https://localhost:44386/");
            Users = GetUsersAsync();
        }

        public static ObservableCollection<User> GetUsersAsync()
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            ObservableCollection<User> users = null;
            if (response.IsSuccessStatusCode)
            {
                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(response.Content.ReadAsStringAsync().Result);
            }
            return users;
        }

        public void PostUsers(User user)
        {
            var response = client.PostAsJsonAsync(url, user).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Пользователь добавлен");
                Users.Add(JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result));
            } else
            {
                MessageBox.Show("Возникла ошибка при добавлении нового пользователя");
            }
        }
        public RelayCommand AddUserCommand
        {
            get
            {
                return addUserCommand ??
                    (addUserCommand = new RelayCommand(obj =>
                    {
                        Users.Add(new User());
                    }));
            }
        }
    }
}
