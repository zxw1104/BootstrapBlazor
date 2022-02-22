// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 引导页
/// </summary>
public partial class Driver: BootstrapComponentBase, IDisposable
{

    private ElementReference DriverElement { get; set; }

    private bool _needInit = false;

    /// <summary>
    /// 引导页相关选项
    /// </summary>
    [Parameter]
    [NotNull]
    public DriverOptions? DriverOptions { get; set; }

    /// <summary>
    /// 步骤相关选项
    /// </summary>
    [Parameter]
    public IEnumerable<StepOptions>? StepOptions { get; set; }

    /// <summary>
    /// 开始引导
    /// </summary>
    public async Task StartAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_start");
    }

    /// <summary>
    /// 重置引导
    /// </summary>
    public async Task ResetAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_reset");
    }

    /// <summary>
    /// 刷新引导
    /// </summary>
    public async Task RefreshAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_refresh");
    }

    /// <summary>
    /// 移动到下一个引导
    /// </summary>
    public async Task MoveNextAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_moveNext");
    }

    /// <summary>
    /// 移动到上一个引导
    /// </summary>
    public async Task MovePreviousAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_movePrevious");
    }

    /// <summary>
    /// 移动到上一个引导
    /// </summary>
    public async Task HidePopoverAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_hidePopover");
    }

    /// <summary>
    /// 移动到上一个引导
    /// </summary>
    public async Task ShowPopoverAsync()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_showPopover");
    }

    /// <summary>
    /// 是否有下一步
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasNextStepAsync()
    {
        return await JSRuntime.InvokeAsync<bool>(DriverElement, "bb_driver_hasNextStep");
    }

    /// <summary>
    /// 是否有上一步
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasPreviousStepAsync()
    {
        return await JSRuntime.InvokeAsync<bool>(DriverElement, "bb_driver_hasPreviousStep");
    }

    /// <summary>
    /// 是否是激活状态
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsActivatedAsync()
    {
        return await JSRuntime.InvokeAsync<bool>(DriverElement, "bb_driver_isActivated");
    }

    /// <summary>
    /// 是否有高亮元素
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasHighlightedElementAsync()
    {
        return await JSRuntime.InvokeAsync<bool>(DriverElement, "bb_driver_hasHighlightedElement");
    }

    /// <summary>
    /// OnParametersSet
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _needInit = true;
    }

    /// <summary>
    /// OnAfterRenderAsync
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender || _needInit)
        {
            await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_init", DriverOptions, StepOptions);

        }
    }

    /// <summary>
    ///
    /// </summary>
    public void Dispose()
    {

    }
}
