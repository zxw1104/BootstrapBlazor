// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// MenuItem 扩展操作类
/// </summary>
public static class MenuItemExtensions
{
    /// <summary>
    /// 级联设置 <see cref="MenuItem"/> Active 状态
    /// </summary>
    /// <param name="item"></param>
    /// <param name="active"></param>
    public static void CascadingSetActive(this MenuItem item, bool active = true)
    {
        item.IsActive = active;
        var current = item;
        while (current.Parent != null)
        {
            current.Parent.IsActive = active;
            current.Parent.IsCollapsed = false;
            current = current.Parent;
        }
    }

    /// <summary>
    /// 级联向下设置 <see cref="MenuItem"/> Active 状态
    /// </summary>
    /// <param name="item"></param>
    /// <param name="active"></param>
    public static void CascadingSetChildActive(MenuItem item, bool active = true)
    {
        if (item.IsActive != active)
        {
            item.IsActive = active;
        }
        foreach (var menu in item.Items)
        {
            CascadingSetChildActive(menu, active);
        }
    }

    /// <summary>
    /// 级联向下查找 <see cref="MenuItem"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="func">判定条件</param>
    /// <returns></returns>
    public static MenuItem? CascadingFindChild(MenuItem item, Func<MenuItem, bool> func)
    {
        if (func(item))
        {
            return item;
        }

        MenuItem? result = null;
        foreach (var menu in item.Items)
        {
            result = CascadingFindChild(menu, func);
            if (result != null)
            {
                break;
            }
        }
        return result;
    }
}
