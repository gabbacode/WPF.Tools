using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Ui.Wpf.KanbanControl.Expressions
{
    public class PropertyAccessorsExpressionCreator
    {
        public PropertyAccessorsExpressionCreator(Type type)
        {
            getters = new Dictionary<string, Func<object, object>>();
            setters = new Dictionary<string, Action<object, object>>();
            
            var propertyInfos = type.GetProperties(
                BindingFlags.Instance 
                | BindingFlags.Public 
                | BindingFlags.SetProperty);
            
            foreach (var propertyInfo in propertyInfos)
            {
                var getter = BuildGetter(propertyInfo);
                var setter = BuildSetter(propertyInfo);
                
                getters.Add(propertyInfo.Name, getter);
                setters.Add(propertyInfo.Name, setter);
            }
        }

        public Func<object, object> TakeGetterForProperty(string name)
        {
            return getters[name];
        }

        public Action<object, object> TakeSetterForProperty(string name)
        {
            return setters[name];
        }
        
        private readonly Dictionary<string, Func<object, object>> getters;
        private readonly Dictionary<string, Action<object, object>> setters;
       
        private static Action<object, object> BuildSetter(PropertyInfo propertyInfo)
        {
            var objType = propertyInfo.DeclaringType;
            Debug.Assert(objType != null);
            
            var setMethod = propertyInfo.GetSetMethod();
            
            var objExpression = Expression.Parameter(typeof(object), "obj");
            var castObjectExpression = Expression.Convert(objExpression, objType);

            var valueExpression = Expression.Parameter(typeof(object), "value");
            
            var calSetExpression = Expression.Call(
                castObjectExpression, 
                setMethod,
                Expression.Convert(valueExpression, propertyInfo.PropertyType));
            
            var lambda = Expression.Lambda<Action<object, object>>(calSetExpression, objExpression, valueExpression);

            return lambda.Compile();
        }

        private static Func<object, object> BuildGetter(PropertyInfo propertyInfo)
        {
            var objType = propertyInfo.DeclaringType;
            Debug.Assert(objType != null);

            var getMethod = propertyInfo.GetGetMethod();

            var objExpression = Expression.Parameter(typeof(object), "obj");
            var castObjectExpression = Expression.Convert(objExpression, objType);
            
            var callGetExpression = Expression.Call(castObjectExpression, getMethod);
            var castExpression = Expression.Convert(callGetExpression, typeof(object));

            var lambda = Expression.Lambda<Func<object, object>>(castExpression, objExpression);

            return lambda.Compile();
        }            
    }
}