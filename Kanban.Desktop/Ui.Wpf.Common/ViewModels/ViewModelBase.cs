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

        internal bool isBusinesscall { get; set; } = false;

        #region CloseQuery


        public void Close()
        {
            isBusinesscall = true;
            var args=new ViewModelCloseQueryArgs();
            args.IsCanceled = false;
            
            Closing(args);
            CloseQuery?.Invoke(this, args);

            if (args.IsCanceled)
            {
                isBusinesscall = false;
                return;
            }

            Closed(args);
            DisposeInternals();

        }


        
        
        
        protected internal virtual void Closed(ViewModelCloseQueryArgs args)
        {
            
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
