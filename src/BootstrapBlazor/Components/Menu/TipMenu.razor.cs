// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components.Routing;

namespace BootstrapBlazor.Components;

/// <summary>
/// 
/// </summary>
public partial class TipMenu : IDisposable
{

    private IEnumerable<MenuItem>? _items;
    /// <summary>
    /// 菜单是否初始化
    /// </summary>
    private bool _init;
    /// <summary>
    /// 获得/设置 菜单数据集合
    /// </summary>
    [Parameter]
    [NotNull]
    public IEnumerable<MenuItem>? Items
    {
        get => _items ?? Enumerable.Empty<MenuItem>();
        set
        {
            if (_items != value)
            {
                _items = value;
                _init = false;
            }
        }
    }

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
    /// 获得/设置 NavigationManager 实例
    /// </summary>
    [Inject]
    [NotNull]
    private NavigationManager? Navigator { get; set; }

    private List<MenuItem> BreadcrumbItems { get; set; } = new();

    /// <summary>
    /// OnInitializedAsync 方法
    /// </summary>
    /// <returns></returns>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!Parent.DisableNavigation && Navigator != null)
        {
            Navigator.LocationChanged += Navigation_LocationChanged;
        }
    }

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
    /// OnParametersSet 方法
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // 参数变化时重新整理菜单
        if (!_init && Items.Any())
        {
            var url = Navigator.ToBaseRelativePath(Navigator.Uri);
            InitBreadcrumbItems(url);
            _init = true;
        }
    }

    private async Task OnClickItem(MenuItem item)
    {
        if (OnClick != null)
        {
            await OnClick(item);
        }
    }

    private void InitBreadcrumbItems(string url)
    {
        BreadcrumbItems.Clear();
        MenuItem? item = null;
        foreach (var menu in Items)
        {
            item = MenuItemExtensions.CascadingFindChild(menu, (x) => x.Url?.TrimStart('/').Equals(url, StringComparison.OrdinalIgnoreCase) ?? false);
            if (item != null)
            {
                BreadcrumbItems.Add(item);
                var current = item;
                while (current.Parent != null)
                {
                    BreadcrumbItems.Add(current.Parent);
                    current = current.Parent;
                }
                return;
            }
        }
    }

    private void Navigation_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        var location = Navigator.ToBaseRelativePath(e.Location);
        InitBreadcrumbItems(location);
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void Dispose()
    {
        if (!Parent.DisableNavigation && Navigator != null)
        {
            Navigator.LocationChanged -= Navigation_LocationChanged;
        }
        GC.SuppressFinalize(this);
    }
}
