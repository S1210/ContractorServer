using ContractorServer.Commands;
using ContractorServer.CustomElements;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ContractorServer.ViewModel
{
    public class MainViewModel
    {
        const string host = "https://localhost:44386/";
        const string url = "api/Users/";
        readonly HttpClient client = new HttpClient();
        private RelayCommand addUserCommand;
        private RelayCommand deleteUserCommand;
        private RelayCommand saveCommand;
        public ObservableCollection<User> Users { get; set; }
        public User SelectUser { get; set; }
        public MainViewModel()
        {
            client.BaseAddress = new Uri(host);
            Users = GetUsersAsync();

        }

        private ObservableCollection<User> GetUsersAsync()
        {
            var response = client.GetAsync(url).Result;
            ObservableCollection<User> users = null;
            if (response.IsSuccessStatusCode)
            {
                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(response.Content.ReadAsStringAsync().Result);
            } else
            {
                CustomMessageBox.Show("Сервер не отвечает");
            }
            return users;
        }

        public RelayCommand AddUserCommand
        {
            get
            {
                return addUserCommand ??
                    (addUserCommand = new RelayCommand(obj =>
                    {
                        var response = client.PostAsJsonAsync(url, new User()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Application.Current.Dispatcher.Invoke(delegate 
                        {
                            Users.Add(JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result));                                
                        }); 
                        }
                        else
                        {
                            MessageBox.Show("Возникла ошибка при добавлении нового пользователя");
                        }
                    }));
            }
        }
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ??
                    (deleteUserCommand = new RelayCommand(obj =>
                    {
                        User user = obj as User;
                        if (user != null)
                        {
                            CustomMessageBox.ShowYesNo($"Вы действительно хотите удалить пользователя {user.Login}?");
                            if (CustomMessageBox.RESULT == CustomMessageBox.YES)
                            {
                                var response = client.DeleteAsync(url + Convert.ToString(user.Id)).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    Application.Current.Dispatcher.Invoke(delegate
                                    {
                                        Users.Remove(user);
                                    });
                                }
                                else
                                {
                                    MessageBox.Show("Возникла ошибка при удалении пользователя");
                                }
                            }
                        }
                    }));
            }
        }
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        foreach(User user in Users)
                        {
                            var response = client.PutAsJsonAsync(url + Convert.ToString(user.Id), user).Result;
                            if (!response.IsSuccessStatusCode)
                            {
                                CustomMessageBox.Show($"Возникла ошибка при сохранении изменений пользователя {user.Login}");
                            }
                        }
                        Snackbar snackbar = obj as Snackbar;
                        snackbar.Dispatcher.Invoke(delegate
                        {
                            var messageQueue = snackbar.MessageQueue;
                            var message = "Сохранено";
                            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                        });
                    }));
            }
        }
    }
}
