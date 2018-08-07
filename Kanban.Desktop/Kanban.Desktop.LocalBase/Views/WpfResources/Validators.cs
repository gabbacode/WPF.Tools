using System.IO;
using System.Linq;
using System.Security.AccessControl;
using FluentValidation;
using Kanban.Desktop.LocalBase.ViewModels;

namespace Kanban.Desktop.LocalBase.Views.WpfResources
{
    public class WizardValidator : AbstractValidator<WizardViewModel>
    {
        public WizardValidator()
        {
            
            RuleFor(wiz => wiz.BoardName)
                .NotNull().Length(5, 25)
                .WithMessage("This field must be between 5 and 25 chars length");

            RuleFor(wiz => wiz.FolderName)
                .Must(Directory.Exists)
                .WithMessage("Can't find such directory")
                .Must(HasUserRightsInDirectory)
                .WithMessage("You have no rights in this directory...");

            RuleFor(wiz => wiz.FileName)
                .NotNull().Must(IsValidDataBaseName)
                .WithMessage(
                    "File name must have .db extension and can't contain any specific chars");
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

        private bool HasUserRightsInDirectory(string path)
        {
            var savePermission = false;
            var saveBlock = false;

            if (!Directory.Exists(path)) return true;

            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.CreateFiles & rule.FileSystemRights) != FileSystemRights.CreateFiles)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    savePermission = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    saveBlock = true;
            }

            return savePermission && !saveBlock;
        }
    }

    public class LocalDimensionValidator : AbstractValidator<WizardViewModel.LocalDimension>
    {
        public LocalDimensionValidator()
        {
            RuleFor(dim => dim.IsDuplicate)
                .Must(isd => isd == false)
                .WithMessage("Table can not contain duplicate rows");

            RuleFor(dim => dim.Name)
                .Must(name => !string.IsNullOrEmpty(name) && name.Length < 30)
                .WithMessage("Column name length must be between 1 and 30 chars");
        }
    }
}
