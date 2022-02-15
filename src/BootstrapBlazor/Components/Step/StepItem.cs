// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// Step 组件
/// </summary>
public class StepItem : ComponentBase
{
    /// <summary>
    /// 获得/设置 步骤显示文字
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// 获得/设置 步骤显示图标
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = "fa fa-check";

    /// <summary>
    /// 获得/设置 步骤状态
    /// </summary>
    [Parameter]
    public StepStatus Status { get; set; }

    /// <summary>
    /// 获得/设置 描述信息
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// 获得/设置 step 的间距不填写将自适应间距支持百分比
    /// </summary>
    [Parameter]
    public string? Space { get; set; }

    /// <summary>
    /// 获得/设置 是否为图标
    /// </summary>
    [Parameter]
    public bool IsIcon { get; set; }

    /// <summary>
    /// 获得/设置 是否为最后一个 Step
    /// </summary>
    [Parameter]
    public bool IsLast { get; set; }

    /// <summary>
    /// 获得/设置 是否居中对齐
    /// </summary>
    [Parameter]
    public bool IsCenter { get; set; }

    /// <summary>
    /// 获得/设置 Step 顺序
    /// </summary>
    internal int StepIndex { get; set; }

    /// <summary>
    /// 获得/设置 父级组件 Steps 实例
    /// </summary>
    [CascadingParameter]
    private Step? Steps { get; set; }

    /// <summary>
    /// 获得/设置 步骤组件状态改变时回调委托
    /// </summary>
    [Parameter]
    public Action<StepStatus>? OnStatusChanged { get; set; }

    /// <summary>
    /// 获得/设置 每个 step 的模板
    /// </summary>
    [Parameter]
    public RenderFragment? DescriptionTemplate { get; set; }

    /// <summary>
    /// 获得/设置 每个 step 的内容
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Steps != null)
        {
            Steps.AddItem(this);
            StepIndex = Steps.Items.Count;
        }
    }
}
