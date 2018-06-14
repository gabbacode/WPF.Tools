using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Ui.Wpf.Common.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IViewModel
    {
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public bool IsBusy { get; private set; }

        #region CloseQuery

        public void Close()
        {
            var args = new ViewModelCloseQueryArgs();

            Closing(args);
            CloseQuery?.Invoke(this, args);

            if (args.IsCanceled)
                return;

            Closed(args);
            DisposeInternals();
        }

        protected virtual void Closed(ViewModelCloseQueryArgs args)
        {
        }

        protected virtual void Closing(ViewModelCloseQueryArgs args)
        {
        }

        public void DisposeInternals()
        {
            disposables.Dispose();
        }

        internal event EventHandler<ViewModelCloseQueryArgs> CloseQuery;

        #endregion

        protected CompositeDisposable disposables = new CompositeDisposable();
    }
}
