// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 引导页参数
/// </summary>
public class DriverOptions
{
    /// <summary>
    /// 弹出提示class,默认为"scoped-class"
    /// </summary>
    public string ClassName { get; set; } = "scoped-class";

    /// <summary>
    /// 是否启用动画
    /// </summary>
    public bool Animate { get; set; }  = true;

    /// <summary>
    /// 不透明度
    /// </summary>
    public float Opacity { get; set; } = 0.75f;

    /// <summary>
    /// 弹出提示框边距
    /// </summary>
    public int Padding { get; set; } = 10;

    /// <summary>
    /// 点击外部是否关闭
    /// </summary>
    public bool AllowClose { get; set; } = true;

    /// <summary>
    /// 点击外部是否执行下一步
    /// </summary>
    public bool OverlayClickNext { get; set; } = false;

    /// <summary>
    /// 是否支持键盘控制
    /// </summary>
    public bool KeyboardControl { get; set; } = true;

}
