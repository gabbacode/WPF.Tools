using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ui.Wpf.KanbanControl.Expressions
{
    public class PropertyAccessorsExpressionCreator
    {
        public PropertyAccessorsExpressionCreator(Type type, bool isNullPropositionEnabled = true)
        {
            nullProposition = isNullPropositionEnabled;
            getters = new Dictionary<string, Func<object, object>>();
            setters = new Dictionary<string, Action<object, object>>();

            InitByType(type);
        }

        public PropertyAccessorsExpressionCreator(IEnumerable items, bool isNullPropositionEnabled = true)
        {
            nullProposition = isNullPropositionEnabled;
            getters = new Dictionary<string, Func<object, object>>();
            setters = new Dictionary<string, Action<object, object>>();

            var type = GetElementTypeOfEnumerable(items);

            Debug.Assert(type != null);

            InitByType(type);
        }
        
        private void InitByType(Type type)
        {
            var processed = new HashSet<ExpressionPathMember>();
            var toWalk = new Queue<ExpressionPathMember>();
            var rootParameterExpression = Expression.Parameter(typeof(object), "root");
            var valueExpression = Expression.Parameter(typeof(object), "value");


            toWalk.Enqueue(new ExpressionPathMember
            {
                Path = null, Name = "", OwnerType = type
            });

            while (toWalk.Any())
            {
                var pathMember = toWalk.Dequeue();
                if (!processed.Add(pathMember))
                    // check if we walked element with same type and property type
                    continue;

                var props = pathMember.OwnerType.GetProperties(
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.SetProperty
                    | BindingFlags.GetProperty);

                foreach (var propertyInfo in props)
                {
                    if (!propertyApplicable(propertyInfo))
                        continue;

                    var path = pathMember.Path != null
                        ? $"{pathMember.Path}.{propertyInfo.Name}"
                        : propertyInfo.Name;

                    var callGetExpr = BuildGetExpression(
                        rootParameterExpression,
                        pathMember.GetMemberExpression,
                        propertyInfo,
                        nullProposition);

                    AddGetter(rootParameterExpression, path, callGetExpr);

                    AddSetter(rootParameterExpression, valueExpression, pathMember, propertyInfo, path);

                    toWalk.Enqueue(new ExpressionPathMember
                    {
                        Path = path,
                        Name = propertyInfo.Name,
                        OwnerType = propertyInfo.PropertyType,
                        GetMemberExpression = callGetExpr
                    });
                }
            }
        }

        public Func<object, object> TakeGetterForProperty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            getters.TryGetValue(name, out var func);

            return func;
        }

        public Action<object, object> TakeSetterForProperty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            setters.TryGetValue(name, out var action);

            return action;
        }

        private readonly bool nullProposition;
        private readonly Dictionary<string, Func<object, object>> getters;
        private readonly Dictionary<string, Action<object, object>> setters;

        private void AddGetter(ParameterExpression rootParameterExpression, string path, Expression callGetExpr)
        {
            var castExpression = Expression.Convert(callGetExpr, typeof(object));
            var getter = Expression.Lambda<Func<object, object>>(castExpression, rootParameterExpression)
                .Compile();

            getters.Add(path, getter);
        }

        private void AddSetter(
            ParameterExpression rootParameterExpression,
            ParameterExpression valueExpression,
            ExpressionPathMember pathMember,
            PropertyInfo propertyInfo,
            string path)
        {
            var setter = BuildSetter(
                rootParameterExpression,
                valueExpression,
                pathMember.GetMemberExpression,
                propertyInfo);

            setters.Add(path, setter);
        }

        private static Action<object, object> BuildSetter(
            ParameterExpression rootParameterExpression,
            ParameterExpression valueParamererExpression,
            Expression getMemberExpression,
            PropertyInfo propertyInfo)
        {
            var objType = propertyInfo.DeclaringType;
            Debug.Assert(objType != null);
            var setMethod = propertyInfo.GetSetMethod();

            if (getMemberExpression == null)
            {
                var castObjectExpression = Expression.Convert(rootParameterExpression, objType);

                var calSetExpression = Expression.Call(
                    castObjectExpression,
                    setMethod,
                    Expression.Convert(valueParamererExpression, propertyInfo.PropertyType));

                var setterLlambda = Expression.Lambda<Action<object, object>>(
                    calSetExpression, 
                    rootParameterExpression, 
                    valueParamererExpression);

                return setterLlambda.Compile();
            }
            else
            {
                var calSetExpression = Expression.Call(
                    getMemberExpression,
                    setMethod,
                    Expression.Convert(valueParamererExpression, propertyInfo.PropertyType));

                var setterLlambda = Expression.Lambda<Action<object, object>>(
                    calSetExpression,
                    rootParameterExpression,
                    valueParamererExpression);

                return setterLlambda.Compile();
            }
        }

        private static Expression BuildGetExpression(
            ParameterExpression rootParameter,
            Expression currentObjectExpression, 
            PropertyInfo propertyInfo,
            bool nullProposition)
        {
            var objType = propertyInfo.DeclaringType;
            Debug.Assert(objType != null);
            var getMethod = propertyInfo.GetGetMethod();

            if (currentObjectExpression == null)
            {
                var castObjectExpression = Expression.Convert(rootParameter, objType);

                if (nullProposition && !rootParameter.Type.IsValueType)
                {
                    var callExpression = Expression.Call(castObjectExpression, getMethod);
                    return AddNullCheckedExpression(rootParameter, callExpression);
                }

                return Expression.Call(castObjectExpression, getMethod);
            }
            else
            {
                if (nullProposition && !currentObjectExpression.Type.IsValueType)
                {
                    var callExpression = Expression.Call(currentObjectExpression, getMethod);
                    return AddNullCheckedExpression(
                        checkExpression: currentObjectExpression, 
                        callExpression: callExpression);
                }

                return Expression.Call(currentObjectExpression, getMethod);
            }
        }

        private static ConditionalExpression AddNullCheckedExpression(
            Expression checkExpression,
            Expression callExpression)
        {
            var nullValueExpression = Expression.Constant(null);
            var nullTestExpression = Expression.Equal(checkExpression, nullValueExpression);

            var nullCheckedCallExpression = Expression.Condition(
                nullTestExpression,
                Expression.Default(callExpression.Type),
                callExpression);

            return nullCheckedCallExpression;
        }

        private static Type GetElementTypeOfEnumerable(object o)
        {
            var enumerable = o as IEnumerable;
            if (enumerable == null)
                return null;

            Type[] interfaces = enumerable.GetType().GetInterfaces();

            var elementType = interfaces
                .Where(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();

            return elementType;
        }

        private static bool propertyApplicable(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetMethod == null)
                return false;

            if (propertyInfo.GetMethod.GetParameters().Length > 0)
                return false;

            if (propertyInfo.SetMethod == null)
                return false;

            return true;
        }
    }
}