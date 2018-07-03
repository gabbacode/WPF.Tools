using Microsoft.Win32;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI.Fody.Helpers;
using Kanban.Desktop.LocalBase.BaseSelector.Model;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    public class BaseSelectorViewModel : IBaseSelectorViewModel
    {

        public string BaseName { get; set; } = "dsa";
        public string Title { get; set; } = "sad";
        public ReactiveCommand<Unit, string> NewDbCommand { get; set; }
        public ReactiveCommand<Unit, string> OpenDbCommand { get; set; }
        private readonly IBaseSelectorModel _model;

        public BaseSelectorViewModel(IBaseSelectorModel model)
        {
            _model = model;
            NewDbCommand = ReactiveCommand.Create(CreateDataBase);
            OpenDbCommand = ReactiveCommand.Create(OpenDataBase);
        }

        public string CreateDataBase()
        {
                  var saveDialog = new SaveFileDialog()
                  {
                      Filter = "SQLite DataBase | *.db",
                      Title = "Создание базы"
                  };
                  if ((bool)saveDialog.ShowDialog())
                      return saveDialog.FileName;

                  else return null;
            // var cols = database.GetColumns();
        }

        public string OpenDataBase()
        {
            var openDialog = new OpenFileDialog()
            {
                Filter = "SQLite DataBase | *.db"
            };

            if ((bool)openDialog.ShowDialog())
                return openDialog.FileName;

            else return null;
        }
    }
}
