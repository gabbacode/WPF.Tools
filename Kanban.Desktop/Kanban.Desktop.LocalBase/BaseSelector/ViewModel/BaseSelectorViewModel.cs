using ReactiveUI;
using System.Reactive;
using MahApps.Metro.Controls.Dialogs;
using Kanban.Desktop.LocalBase.BaseSelector.Model;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    public class BaseSelectorViewModel : IBaseSelectorViewModel
    {

        public string BasePath { get; set; } = null;
        public ReactiveList<string> BaseList { get; set; }
        public string Title { get; set; } = "BaseSelector";
        public ReactiveCommand NewDbCommand { get; set; }
        public ReactiveCommand OpenDbCommand { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly IBaseSelectorModel _model;

        public BaseSelectorViewModel(IBaseSelectorModel model)
        {
            _model = model;

            BaseList = new ReactiveList<string>();
            var list = _model.GetBaseList();
            foreach (var addr in list) BaseList.Add(addr);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
              {
                  var t = _model.CheckRecentBase(basePath);
                  if (t) BasePath = BasePath;
                  else
                  {
                      BaseList.Remove(basePath);
                      DialogCoordinator.Instance
                          .ShowMessageAsync(this, "Ошибка", "База была удалена или перемещена из данной папки");
                  }
              });

            NewDbCommand = ReactiveCommand.Create(() => BasePath = _model.CreateDatabase());

            OpenDbCommand = ReactiveCommand.Create(() => BasePath = _model.OpenDatabase());
        }
    }
}
