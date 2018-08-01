using System.IO;
using System.Linq;
using FluentValidation;
using Kanban.Desktop.LocalBase.ViewModels;

namespace Kanban.Desktop.LocalBase.WpfResources
{
    public class WizardValidator : AbstractValidator<WizardViewModel>
    {
        public WizardValidator()
        {
            RuleFor(wiz => wiz.BoardName)
                .NotNull().Length(5, 25)
                .WithMessage("This field must be between 5 and 25 chars length");

            //boardname-check if exists

            RuleFor(wiz => wiz.FolderName)
                .Must(Directory.Exists).WithMessage("Can't find such directory");

            RuleFor(wiz => wiz.FileName)
                .NotNull().Must(IsValidDataBaseName)
                .WithMessage(
                    "File name must have .db extension and can't contain any specific chars");

            RuleFor(wiz => wiz.ColumnList)
                .Must(list => list.Select(col=>col.Name).ToList().Count
                              == list.Select(col => col.Name).ToList().Distinct().Count())
                .WithMessage("Table can not contain duplicate columns");

            RuleFor(wiz => wiz.RowList)
                .Must(list => list.Count == list.Distinct().Count())
                .WithMessage("Table can not contain duplicate rows");

            RuleForEach(wiz => wiz.ColumnList)
                .Must(col => !string.IsNullOrEmpty(col.Name) && col.Name.Length < 30)
                .WithMessage("Column name length must be between 1 and 30 chars");

            RuleForEach(wiz => wiz.RowList)
                .Must(row => !string.IsNullOrEmpty(row.Name) && row.Name.Length < 30)
                .WithMessage("Row name length must be between 1 and 30 chars");
        }

        private bool IsValidDataBaseName(string name)
        {
            char[] separators =
            {
                '+', '=', '[', ']', ':', ';', '"', ',', '/', '?', ' ',
                '\\', '*', '<', '>', '|'
            };

            return name.Count(s => s == '.') == 1 && !separators.Any(name.Contains) &&
                   Path.GetExtension(name)   == ".db";
        }
    }
}
