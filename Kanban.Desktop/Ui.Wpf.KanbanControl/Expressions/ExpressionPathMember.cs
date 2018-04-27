using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ui.Wpf.KanbanControl.Expressions
{
    internal class ExpressionPathMember : IEquatable<ExpressionPathMember>
    {
        public Type OwnerType { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
        
        public Expression<Func<object, object>> CurrentExpression { get; set; }

        #region [ IEquatable ]
        public override bool Equals(object obj)
        {
            return Equals(obj as ExpressionPathMember);
        }

        public bool Equals(ExpressionPathMember other)
        {
            return other != null 
                && EqualityComparer<Type>.Default.Equals(OwnerType, other.OwnerType) 
                && Name == other.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = -157323742;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(OwnerType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
        #endregion
    }
}
