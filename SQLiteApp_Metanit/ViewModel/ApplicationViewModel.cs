using Microsoft.EntityFrameworkCore;
using SQLiteApp_Metanit.Model;
using SQLiteApp_Metanit.Veiw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteApp_Metanit.ViewModel
{
    internal class ApplicationViewModel
    {
        ApplicationContext db = new ApplicationContext();
        RelayCommand? addCommand;
        RelayCommand? editCommand;
        RelayCommand? deleteCommand;

        public ObservableCollection<User> Users { get; set; }

        public ApplicationViewModel()
        {
            db.Database.EnsureCreated();
            db.Users.Load();
            Users = db.Users.Local.ToObservableCollection();
        }
        // команда добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      UserWindow userWindow = new UserWindow(new User());
                      if (userWindow.ShowDialog() == true)
                      {
                          User user = userWindow.User;
                          db.Users.Add(user);
                          db.SaveChanges();
                      }
                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      // получаем выделенный объект
                      User? user = selectedItem as User;
                      if (user == null) return;

                      User vm = new User
                      {
                          Id = user.Id,
                          Name = user.Name,
                          Age = user.Age
                      };
                      UserWindow userWindow = new UserWindow(vm);


                      if (userWindow.ShowDialog() == true)
                      {
                          user.Name = userWindow.User.Name;
                          user.Age = userWindow.User.Age;
                          db.Entry(user).State = EntityState.Modified;
                          db.SaveChanges();
                      }
                  }));
            }
        }
        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      // получаем выделенный объект
                      User? user = selectedItem as User;
                      if (user == null) return;
                      db.Users.Remove(user);
                      db.SaveChanges();
                  }));
            }
        }
    }
}
