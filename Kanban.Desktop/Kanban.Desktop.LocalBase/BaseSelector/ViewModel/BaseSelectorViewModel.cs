using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    class BaseSelectorViewModel : IBaseSelectorViewModel
    {

        public string BaseName { get; set; } = "dsa";
        public string Title { get; set; } = "sad";
        public ReactiveCommand NewDbCommand { get; set; }
        private readonly IBaseSelectorViewModel _model;


        public BaseSelectorViewModel(IBaseSelectorViewModel model)
        {
            _model = model;
        }

        public string CreateDataBase()
        {
            var saveDialog = new SaveFileDialog()
            {
                Filter = "SQLite DataBase | *.db", Title = "Создание базы"
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
