// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// 
/// </summary>
public partial class TipSelect
{
    private ElementReference MenuElemenet { get; set; }

    private JSInterop<TipSelect>? Interop { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private string? LinkClassString => CssBuilder.Default("nav-link")
        .AddClass("py-3 fs-4 m-0 justify-content-center")
        .AddClass("disabled", Item.IsDisabled)
        .AddClass("active", Item.IsActive)
        .Build();

    /// <summary>
    ///
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    protected static string GetIconString(string? icon) => string.IsNullOrWhiteSpace(icon)
        ? "fa-fw fa-bars"
        : icon.Contains("fa-fw", StringComparison.OrdinalIgnoreCase) ? icon : $"{icon} fa-fw";

    /// <summary>
    /// 获得/设置 组件数据源
    /// </summary>
    [Parameter]
    [NotNull]
    public MenuItem? Item { get; set; }

    /// <summary>
    /// 获得/设置 菜单项点击回调委托
    /// </summary>
    [Parameter]
    public Func<MenuItem, Task>? OnClick { get; set; }

    [CascadingParameter]
    [NotNull]
    private Menu? Parent { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Menu>? Localizer { get; set; }

    /// <summary>
    /// SetParametersAsync 方法
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (Parent == null)
        {
            throw new InvalidOperationException(Localizer["InvalidOperationExceptionMessage"]);
        }

        // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            Interop = new JSInterop<TipSelect>(JSRuntime);
            await Interop.InvokeVoidAsync(this, MenuElemenet, "bb_tip_menu"
                , nameof(ClickAsync));
        }
    }

    /// <summary>
    /// 客户端点击菜单时回调方法
    /// 因为是克隆的节点，无法绑定到原始节点上，必须通过key识别
    /// </summary>
    /// <param name="key">菜单的key</param>
    /// <returns></returns>
    [JSInvokable]
    public Task ClickAsync(string key)
    {
        var menu = MenuItemExtensions.CascadingFindChild(Item, (item) => item.Key == key);
        if (menu != null)
        {
            //激活当前菜单及其上级菜单
            menu.CascadingSetActive(true);
            //传入当前菜单的顶层菜单，取消其他菜单的激活状态
            Parent?.RemoveActiveWithoutKey(Item.Key);
        }
        return Task.CompletedTask;
    }

    private async Task OnClickItem(MenuItem item)
    {
        if (OnClick != null)
        {
            await OnClick(item);
        }
    }
}
