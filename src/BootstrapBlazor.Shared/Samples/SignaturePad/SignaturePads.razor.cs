// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using BootstrapBlazor.Shared.Components;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// 
/// </summary>
public sealed partial class SignaturePads
{

    /// <summary>
    /// 签名Base64
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// 签名Base64
    /// </summary>
    public string? Result2 { get; set; }

    /// <summary>
    /// 签名Base64
    /// </summary>
    public string? Result3 { get; set; }

    private Task OnResult(string result)
    {
        Result = result;
        StateHasChanged();
        return Task.CompletedTask;
    } 

    private Task OnResult2(string result)
    {
        Result2 = result;
        StateHasChanged();
        return Task.CompletedTask;
    } 

    private Task OnResult3(string result)
    {
        Result3 = result;
        StateHasChanged();
        return Task.CompletedTask;
    } 

    ///// <summary>
    ///// 获得属性方法
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    //{
    //    new AttributeItem("EnableSaveBase64Btn","启用保存为base64按钮",  "true","bool") ,
    //    new AttributeItem("EnableSavePNGBtn","启用保存为PNG按钮文本",  "false","bool") ,
    //    new AttributeItem("EnableSaveJPGBtn","启用保存为JPG按钮文本",  "false","bool") ,
    //    new AttributeItem("EnableSaveSVGBtn","启用保存为SVG按钮文本",  "false","bool") ,

    //    new AttributeItem("CssClass","组件CSS式样",  "signature-pad-body") ,
    //    new AttributeItem("BtnCssClass","按钮CSS式样",  "btn btn-light") ,
    //    new AttributeItem("Responsive","响应式css界面,为所有用户设计最佳体验",  "false","bool") ,
    //    new AttributeItem("BackgroundColor","组件背景",  "rgb(255, 255, 255),设置 rgba(0,0,0,0)为透明") ,
    //};

    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        // TODO: 移动到数据库中
        new AttributeItem() {
            Name = "OnResult",
            Description = "签名结果回调方法",
            Type = "Func<string, Task>?",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new AttributeItem() {
            Name = "OnAlert",
            Description = "手写签名警告信息回调",
            Type = "Func<string, Task>?",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new AttributeItem() {
            Name = "OnError",
            Description = "错误回调方法",
            Type = "Func<string, Task>?",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new AttributeItem() {
            Name = "OnClose",
            Description = "手写签名关闭信息回调",
            Type = "Func<string, Task>?",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new AttributeItem() {
            Name = "SignAboveLabel",
            Description = "在框内签名标签文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "在框内签名"
        },
        new AttributeItem() {
            Name = "ClearBtnTitle",
            Description = "清除按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "清除"
        },
        new AttributeItem() {
            Name = "SignatureAlertText",
            Description = "请先签名提示文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "请先签名"
        },
        new AttributeItem() {
            Name = "ChangeColorBtnTitle",
            Description = "换颜色按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "换颜色"
        },
        new AttributeItem() {
            Name = "ChangeColorBtnTitle",
            Description = "换颜色按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "换颜色"
        },
        new AttributeItem() {
            Name = "UndoBtnTitle",
            Description = "撤消按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "撤消"
        },
        new AttributeItem() {
            Name = "SaveBase64BtnTitle",
            Description = "保存为 base64 按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "确定"
        },
        new AttributeItem() {
            Name = "SavePNGBtnTitle",
            Description = "保存为 PNG 按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "PNG"
        },
        new AttributeItem() {
            Name = "SaveJPGBtnTitle",
            Description = "保存为 JPG 按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "JPG"
        },
        new AttributeItem() {
            Name = "SaveSVGBtnTitle",
            Description = "保存为 SVG 按钮文本",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "SVG"
        },
        new AttributeItem() {
            Name = "EnableChangeColorBtn",
            Description = "启用换颜色按钮",
            Type = "bool",
            ValueList = " — ",
            DefaultValue = "true"
        },
        new AttributeItem() {
            Name = "EnableAlertJS",
            Description = "启用 JS 错误弹窗",
            Type = "bool",
            ValueList = " — ",
            DefaultValue = "true"
        }
    };
}
