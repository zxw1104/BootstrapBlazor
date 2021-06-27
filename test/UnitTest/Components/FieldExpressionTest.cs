// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Components
{
    public class FieldExpressionTest
    {
        [Fact]
        public void CreateFieldIdentifier_Test()
        {
            User user = new User();
            var nameExp = GetExp(user, "name", typeof(string));
            var del = nameExp.Compile();
            var name = del.DynamicInvoke();
            int a = 1;
        }

        private LambdaExpression GetExp(IDynamicType dModel, string fieldName, Type fieldType)
        {
            var methodName = nameof(IDynamicType.GetValue);
            var constExp = Expression.Constant(dModel);
            Assert.Equal(constExp.Type, dModel.GetType());
            var getValueExp = Expression.Call(constExp, dModel.GetType().GetMethod(methodName), Expression.Constant(fieldName));
            var convertExp = Expression.Convert(getValueExp, fieldType);
            var tDelegate = typeof(Func<>).MakeGenericType(fieldType);

            return Expression.Lambda(tDelegate, convertExp);
        }

        private static void ParseAccessor<T>(Expression<Func<T>> accessor, out object model, out string fieldName)
        {
            var accessorBody = accessor.Body;

            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");
            }

            // Identify the field name. We don't mind whether it's a property or field, or even something else.
            fieldName = memberExpression.Member.Name;

            // Get a reference to the model object
            // i.e., given an value like "(something).MemberName", determine the runtime value of "(something)",
            if (memberExpression.Expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Value is null)
                {
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                }
                model = constantExpression.Value;
            }
            else if (memberExpression.Expression != null)
            {
                // It would be great to cache this somehow, but it's unclear there's a reasonable way to do
                // so, given that it embeds captured values such as "this". We could consider special-casing
                // for "() => something.Member" and building a cache keyed by "something.GetType()" with values
                // of type Func<object, object> so we can cheaply map from "something" to "something.Member".
                var modelLambda = Expression.Lambda(memberExpression.Expression);
                var modelLambdaCompiled = (Func<object?>)modelLambda.Compile();
                var result = modelLambdaCompiled();
                if (result is null)
                {
                    throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                }
                model = result;
            }
            else
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");
            }
        }
    }

    public class User : IDynamicType
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void CopyFrom(IDynamicType other)
        {
            throw new NotImplementedException();
        }

        public string GetTypeKey()
        {
            throw new NotImplementedException();
        }

        public object GetValue(string propName)
        {
            return "Ok";
        }

        public void SetValue(string propName, object value)
        {
            throw new NotImplementedException();
        }
    }
}
