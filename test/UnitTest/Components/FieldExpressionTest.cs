// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Forms;
using Natasha.CSharp;
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
            TypeInfoHelper.ParseModelAndProperty(exp, out object child, out string field);

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


            TypeInfoHelper.ParseModelAndProperty(exp1, out object model1, out string field1);


            Assert.Equal(apple, model1);
            Assert.Equal(nameof(Apple.Name), field1);
            Assert.Equal(name, exp1.Compile().Invoke());

   

            Expression<Func<User, string>> exp3 = (user) => user.DynamicApple.Name;
            TypeInfoHelper.ParsePropertyName(exp3, out string field3, out string fullPath, out Type type);
            Assert.Equal(nameof(DynamicApple.Name), field3);
            Assert.Equal(typeof(string), type);
            Assert.Equal("DynamicApple.Name", fullPath);


            //放弃这种方式
            //Expression<Func<string>> exp2 = () => user.DynamicApple.GetValue<string>(nameof(DynamicApple.Name));
            //TypeInfoHelper.ParseModelAndProperty(exp2, out object model2, out string field2);
            //Assert.Equal(apple, model2);
            //Assert.Equal(nameof(Apple.Name), field2);
            //Assert.Equal("ok", exp2.Compile().Invoke());

            //Expression<Func<User, string>> exp4 = (u) => u.DynamicApple.GetValue<string>(nameof(DynamicApple.Name));
            //TypeInfoHelper.ParsePropertyName(exp4, out string field4, out Type type2);
            //Assert.Equal(nameof(DynamicApple.Name), field4);
            //Assert.Equal(typeof(string), type2);
        }

        /// <summary>
        /// 根据Get表达式，生成Set表达式
        /// </summary>
        [Fact]
        public void GetSetExpression_Test()
        {
            //NatashaInitializer.InitializeAndPreheating();
            //Expression<Func<User, object>> exp3 = (user) => user.DynamicApple.GetValue("Name");
            //Expression<Action<User, string>> exp4 = (user,s) => user.DynamicApple.SetValue("Name",s);

            Expression<Func<User, string>> exp3 = (user) => user.DynamicApple.Name;
            Expression<Action<User, string>> exp4 = (user, s) => user.DynamicApple.SetValue("Name", s);

            //通过传递Get表达式，只是一种可选的 方式，因为如果 动态信息 是从DB中 拿出来的，则只能是 多级的字符串表示
            //多级属性 Address.Name
            //单级     Name

            //先解决这种 只有属性 的Get转Set，                  然后再解决 这种字符串多级的问题！！


            TypeInfoHelper.ParsePropertyName(exp3, out string field3, out string fullPath, out Type type);
            MemberExpression memberExpression;

            var p1 = exp3.Parameters.First();
            var p2 = Expression.Parameter(type);
            var member = exp3.Body as MemberExpression;
            var instanceExp = member.Expression;
            var propExp= Expression.Property(instanceExp, field3);

            var hwFunc = FastMethodOperator
                .RandomDomain()
                .Param(p1.Type, "user")
                .Param(type, "v")
                .Body($"{exp3}=v;")
                .Compile();

         
            //下面这个方法 报错，可能多级表达式 还得一级一级的 拼接
            var model = TypeInfoHelper.GetModelFromExp(instanceExp);
            //SetValue方法表达式
            //var dType = instanceExp.Type;
            //var setMethod= dType.GetMethod(nameof(IDynamicType.SetValue));
            //var callExp= Expression.Call(instanceExp, setMethod);
            //Expression<Action<User, string>> exp = Expression.Lambda<Action<User, string>>(callExp, p1, p2);

            User user = new User();
            user.DynamicApple = new DynamicApple();


            var name = "123";
            hwFunc.DynamicInvoke(user,name);
            //exp.Compile().Invoke(user, name);
            Assert.Equal(name, user.DynamicApple.GetValue(nameof(DynamicApple.Name)));
            Assert.Equal(name, user.DynamicApple.Name);

            //Expression<Action<User, string>> exp = (u, s) => u.DynamicApple.SetValue("Name",s);

            //var param_p1 = Expression.Parameter(typeof(User));
            //var param_p2 = Expression.Parameter(typeof(string));

            //var p= typeof(DynamicApple).GetProperty("Name");

            ////获取设置属性的值的方法
            //var mi = p.GetSetMethod(true);
            //var body = Expression.Call(param_p1, mi, param_p2);
            //var action= Expression.Lambda<Action<User, string>>(body, param_p1, param_p2);
            //action.Compile().Invoke()
        }

        public static void ParseModelAndProperty(LambdaExpression exp, out object model, out string fieldName)
        {
            var accessorBody = exp.Body;
            var p = exp.Parameters.First();
            model = null;
            fieldName = null;

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new Exception("支持持成员访问表达式");
            }
            else
            {
                fieldName = memberExpression.Member.Name;
                
                

            }
        }

        /// <summary>
        /// 查询非泛型Method
        /// </summary>
        [Fact]
        public void GetMethod_Test()
        {
            var method = typeof(DynamicApple).GetMethods().Where(m=>m.Name==nameof(IDynamicType.GetValue) && !m.ContainsGenericParameters);
            Assert.NotNull(method);
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

        public T GetValue<T>(string propName)
        {
            return (T)GetValue(propName);
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

        public T GetValue<T>(string propName)
        {
            return (T)GetValue(propName);
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
