﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
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
        public string FullTitle { get; set; }

        [Reactive]
        public bool IsBusy { get; set; }

        [Reactive]
        public bool IsEnabled { get; set; } = true;

        [Reactive]
        public bool CanHide { get; set; }

        [Reactive]
        public bool CanClose { get; set; }


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
            Disposables.Dispose();
        }

        internal event EventHandler<ViewModelCloseQueryArgs> CloseQuery;

        #endregion

        internal CompositeDisposable Disposables { get; set; }  = new CompositeDisposable();

        protected IValidator validator;

        [Reactive] protected ReactiveList<KeyValuePair<string,string>> AllErrors { get; set; } =
            new ReactiveList<KeyValuePair<string, string>>();

        public string Error
        {
            get
            {
                if (validator != null)
                {
                    var results = validator.Validate(this);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine,
                            results.Errors.Select(x => x.ErrorMessage).ToArray());
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
                    var errs = validator.Validate(this).Errors;
                    AllErrors.Clear();
                    AllErrors.AddRange(errs.Select(e=>
                        new KeyValuePair<string, string>(e.PropertyName,e.ErrorMessage)));

                    var firstOrDefault = errs.FirstOrDefault(lol => lol.PropertyName == columnName);
                    if (firstOrDefault != null)
                        return validator != null ? firstOrDefault.ErrorMessage : "";
                }

                return "";
            }
        }

    }
}
