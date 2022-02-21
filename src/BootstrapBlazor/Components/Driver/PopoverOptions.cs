// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 弹出框选项
/// </summary>
public class PopoverOptions
{
    /// <summary>
    /// 弹出提示class,默认为"popover-class"
    /// </summary>
    public string ClassName { get; set; } = "popover-class";

    /// <summary>
    /// 弹出框标题，支持html
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 弹出框内容，支持html
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    /// 完成按钮文本
    /// </summary>
    public string DoneBtnText { get; set; } = "完成";

    /// <summary>
    /// 关闭按钮文本
    /// </summary>
    public string CloseBtnText { get; set; } = "关闭";

    /// <summary>
    /// 下一步按钮文本
    /// </summary>
    public string NextBtnText { get; set; } = "下一步";

    /// <summary>
    /// 上一步按钮文本
    /// </summary>
    public string PrevBtnText { get; set; } = "上一步";

    /// <summary>
    /// 是否显示控制按钮
    /// </summary>
    public bool ShowButtons { get; set; } = true;
}
