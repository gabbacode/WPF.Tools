using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.ComponentModel;
using FluentValidation;
using System.Linq;

namespace Ui.Wpf.Common.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IViewModel, IDataErrorInfo
    {
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public bool IsBusy { get; set; }

        public string ViewId { get; internal set; }

        
        #region CloseQuery


        public void Close()
        {
            var args=new ViewModelCloseQueryArgs();

            CloseQuery?.Invoke(this, args);

            if (args.IsCanceled)
                return;
        
            
        }


        protected internal virtual void Closed(ViewModelCloseQueryArgs args)
        {
            DisposeInternals();

        }

        protected internal virtual void Closing(ViewModelCloseQueryArgs args)
        {

        }

        public void DisposeInternals()
        {
            disposables.Dispose();
        }

        internal event EventHandler<ViewModelCloseQueryArgs> CloseQuery;

        #endregion

        protected CompositeDisposable disposables = new CompositeDisposable();

        protected IValidator validator;

        public string Error
        {
            get
            {
                if (validator != null)
                {
                    var results = validator.Validate(this);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }


        public string this[string columnName]
        {
            get
            {
                if (validator != null)
                {
                    var firstOrDefault = validator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                    if (firstOrDefault != null)
                        return validator != null ? firstOrDefault.ErrorMessage : "";
                }
                return "";
            }
        }


    }
}
