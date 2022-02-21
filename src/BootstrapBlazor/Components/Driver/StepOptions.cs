// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 步骤选项
/// </summary>
public class StepOptions
{
    /// <summary>
    /// css选择器
    /// </summary>
    [NotNull]
    public string? Element { get; set; }

    /// <summary>
    /// 弹窗背景色，默认为#ffffff
    /// </summary>
    public string StageBackground { get; set; } = "#ffffff";

    /// <summary>
    /// 弹出框选项
    /// </summary>
    public PopoverOptions? Popover { get; set; }
}
