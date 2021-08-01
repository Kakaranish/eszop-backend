using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Utilities.Extensions
{
    public static class GenericExtensions
    {
        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value)
            => model.Bind<T, object>(expression, value);

        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression,
            object value)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var modelType = model.GetType();
            var fieldAttributes = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            var allFields = modelType.GetFields(fieldAttributes).Concat(modelType.BaseType.GetFields(fieldAttributes));

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var concreteField = allFields.SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (concreteField == null)
            {
                return model;
            }

            concreteField.SetValue(model, value);

            return model;
        }
    }
}
