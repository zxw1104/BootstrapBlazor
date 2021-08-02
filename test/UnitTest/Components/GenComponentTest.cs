// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Natasha;
using Natasha.CSharp;

namespace UnitTest.Components
{
    /// <summary>
    /// 借助动态编译动态生成组件
    /// </summary>
    public class GenComponentTest
    {
        public void GenCode_Test()
        {
            //NatashaInitializer.InitializeAndPreheating();


            //var hwFunc = FastMethodOperator
            //    .RandomDomain()
            //    .Param(typeof(string), "str1")
            //    .Param<string>("str2")
            //    .Body("return str1+str2;")
            //    .Return<string>()
            //    .Compile<Func<string, string, string>>();


            //var buildAction = NDelegate.RandomDomain().Action<RenderTreeBuilder,Type>();

            //RenderTreeBuilder builder = new RenderTreeBuilder();
            //buildAction.Invoke(builder);
        }

        //public static void CreateDisplayByFieldType(this RenderTreeBuilder builder, ComponentBase component, IEditorItem item, object model, bool? showLabel = null)
        //{
        //    var fieldType = item.PropertyType;
        //    var fieldName = item.GetFieldName();
        //    var displayName = item.GetDisplayName();

        //    var typeParam = "type";
          


        //    var code = $@"
        //    builder.OpenComponent(0, typeof(Display<>).MakeGenericType({typeParam})));
        //    builder.AddAttribute(1, ""DisplayText"", displayName);
        //    builder.AddAttribute(2, ""Value"", fieldValue);
        //    builder.AddAttribute(3, ""ValueChanged"", fieldValueChanged);
        //    builder.AddAttribute(4, ""ValueExpression"", item.GetValueExpression());
        //    builder.AddAttribute(5, ""ShowLabel"", showLabel ?? true);
        //    builder.CloseComponent();
        //    ";

        //    var hwFunc = FastMethodOperator
        //     .RandomDomain()
        //     .Param(typeof(Type), typeParam)
        //     .Param<string>("str2")
        //     .Body("return str1+str2;")
        //     .Return<string>()
        //     .Compile<Func<string, string, string>>();
        //    //Console.WriteLine(hwFunc("Hello", " World!"));
        //    //var fieldValue = GenerateValue(model, fieldName);
        //    //var fieldValueChanged = GenerateValueChanged(component, model, fieldName, fieldType);
        //    //var valueExpression = GenerateValueExpression(model, fieldName, fieldType);
        //    //var fieldValue = model.fieldName;
        //    //var fieldValueChanged = GenerateValueChanged(component, model, fieldName, fieldType);
        //    //var valueExpression = GenerateValueExpression(model, fieldName, fieldType);


        //    //builder.OpenComponent(0, typeof(Display<>).MakeGenericType(fieldType));
        //    //builder.AddAttribute(1, "DisplayText", displayName);
        //    //builder.AddAttribute(2, "Value", fieldValue);
        //    //builder.AddAttribute(3, "ValueChanged", fieldValueChanged);
        //    //builder.AddAttribute(4, "ValueExpression", item.GetValueExpression());
        //    ////SetValueExpressionOrFieldIdentifierInfo(builder, 4, model, fieldName);

        //    //builder.AddAttribute(5, "ShowLabel", showLabel ?? true);
        //    //builder.CloseComponent();
        //}
    }
}
