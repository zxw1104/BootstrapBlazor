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

    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new AttributeItem("OnResult","签名结果回调方法",  "","EventCallback<string>") ,
        new AttributeItem("OnAlert","手写签名警告信息回调",  "","EventCallback<string>") ,
        new AttributeItem("OnError","错误回调方法",  "","Func<string, Task>") ,
        new AttributeItem("OnClose","手写签名关闭信息回调",  "","EventCallback") ,

        new AttributeItem("SignAboveLabel","在框内签名标签文本",  "在框内签名") ,
        new AttributeItem("ClearBtnTitle","清除按钮文本",  "清除") ,
        new AttributeItem("SignatureAlertText","请先签名提示文本",  "请先签名") ,
        new AttributeItem("ChangeColorBtnTitle","换颜色按钮文本",  "换颜色") ,
        new AttributeItem("UndoBtnTitle","撤消按钮文本",  "撤消") ,
        new AttributeItem("SaveBase64BtnTitle","保存为base64按钮文本",  "确定") ,
        new AttributeItem("SavePNGBtnTitle","保存为PNG按钮文本",  "PNG") ,
        new AttributeItem("SaveJPGBtnTitle","保存为JPG按钮文本",  "JPG") ,
        new AttributeItem("SaveSVGBtnTitle","保存为SVG按钮文本",  "SVG") ,

        new AttributeItem("EnableChangeColorBtn","启用换颜色按钮",  "true","bool") ,
        new AttributeItem("EnableAlertJS","启用JS错误弹窗",  "true","bool") ,
        new AttributeItem("EnableSaveBase64Btn","启用保存为base64按钮",  "true","bool") ,
        new AttributeItem("EnableSavePNGBtn","启用保存为PNG按钮文本",  "false","bool") ,
        new AttributeItem("EnableSaveJPGBtn","启用保存为JPG按钮文本",  "false","bool") ,
        new AttributeItem("EnableSaveSVGBtn","启用保存为SVG按钮文本",  "false","bool") ,

        new AttributeItem("CssClass","组件CSS式样",  "signature-pad-body") ,
        new AttributeItem("BtnCssClass","按钮CSS式样",  "btn btn-light") ,
        new AttributeItem("Responsive","响应式css界面,为所有用户设计最佳体验",  "false","bool") ,
        new AttributeItem("BackgroundColor","组件背景",  "rgb(255, 255, 255),设置 rgba(0,0,0,0)为透明") ,
    };

}
