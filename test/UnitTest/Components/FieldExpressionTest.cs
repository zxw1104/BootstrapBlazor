// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        public void GetPropertyValue_From_DynamicType_Test()
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

        /// <summary>
        /// 测试 静态对象 嵌套的表达式 解析是否正确
        /// </summary>
        [Fact]
        public void ParseComplexExp_Test()
        {
            string appName = "App";
            var apple = new Apple { Name = appName };
            User user = new User { Age = 1, Apple = apple };

            Expression<Func<string>> exp = () => user.Apple.Name;
            ParseAccessor(exp, out object child, out string field);

            Assert.Equal(child, apple);
            Assert.Equal(nameof(Apple.Name), field);
        }

        /// <summary>
        /// 测试动静结合 属性路径 表达式 解析是否正确
        /// </summary>
        [Fact]
        public void PropertyPath_Test()
        {
            var apple = new DynamicApple();
            string name = "name";
            apple.Name = name;
            User user = new User { Age = 1, DynamicApple = apple };

            Expression<Func<object>> exp1 = () => user.DynamicApple.Name;
            Expression<Func<object>> exp2 = () => user.DynamicApple.GetValue(nameof(Apple.Name));

            ParseAccessor(exp1, out object model1, out string field1);


            Assert.Equal(apple, model1);
            Assert.Equal(nameof(Apple.Name), field1);
            Assert.Equal(name, exp1.Compile().Invoke());


            ParseAccessor(exp2, out object model2, out string field2);
            Assert.Equal(apple, model2);
            Assert.Equal(nameof(Apple.Name), field2);
            Assert.Equal("ok", exp2.Compile().Invoke());
        }
        private static void ParseAccessor<T>(Expression<Func<T>> accessor, out object model, out string fieldName)
        {
            var accessorBody = accessor.Body;
            model = null;
            fieldName = null;
            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                //throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");
                if (accessorBody is MethodCallExpression methodCall)
                {
                    var arg = (methodCall.Arguments[0] as ConstantExpression).Value.ToString();
                    var obj = methodCall.Object;
                    model = GetModelFromExp(obj);
                    fieldName = arg;
                }
            }
            else
            {
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
                    model= GetModelFromExp(memberExpression.Expression);
                }
                else
                {
                    throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(FieldIdentifier)} only supports simple member accessors (fields, properties) of an object.");
                }
            }
        }

        private static object GetModelFromExp(Expression exp)
        {
            var modelLambda = Expression.Lambda(exp);
            var modelLambdaCompiled = (Func<object?>)modelLambda.Compile();
            var result = modelLambdaCompiled();
            if (result is null)
            {
                throw new ArgumentException("The provided expression must evaluate to a non-null value.");
            }
            return result;
        }
    }


    public class Apple
    {
        public string Name { get; set; }
    }

    public class DynamicApple : IDynamicType
    {
        public string Name { get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public DynamicObjectBuilder GetBuilder()
        {
            throw new NotImplementedException();
        }

        public object GetValue(string propName)
        {
            return "ok";
        }

        public bool IsDynamicProperty(string propName)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string propName, object value)
        {
            throw new NotImplementedException();
        }
    }

    public class User : IDynamicType
    {
        public int Age { get; set; }

        public Apple Apple { get; set; }

        public DynamicApple DynamicApple { get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public DynamicObjectBuilder GetBuilder()
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

        public bool IsDynamicProperty(string propName)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string propName, object value)
        {
            throw new NotImplementedException();
        }
    }
}
