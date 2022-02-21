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

    private JSInterop<Driver>? Interop { get; set; }

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
    public async Task Start()
    {
        await JSRuntime.InvokeVoidAsync(DriverElement, "bb_driver_start");
    }

    /// <summary>
    /// 重置引导
    /// </summary>
    public void Reset()
    {

    }

    /// <summary>
    /// 刷新引导
    /// </summary>
    public void Refresh()
    {

    }

    /// <summary>
    /// 移动到下一个引导
    /// </summary>
    public void MoveNext()
    {

    }

    /// <summary>
    /// 移动到上一个引导
    /// </summary>
    public void MovePrevious()
    {

    }

    /// <summary>
    /// OnAfterRenderAsync
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
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
