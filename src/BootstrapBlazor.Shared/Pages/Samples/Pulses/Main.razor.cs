// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared.Common;
using System.Collections.Generic;
namespace BootstrapBlazor.Shared.Pages.Pulses
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Main
    {
        /// <summary>
        /// 获得属性方法
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
        {
            // TODO: 移动到数据库中
            new AttributeItem() {
                Name = "Color",
                Description = "颜色",
                Type = "Color",
                ValueList = "None / Active / Primary / Secondary / Success / Danger / Warning / Info / Light / Dark / Link",
                DefaultValue = "Primary"
            },
            new AttributeItem() {
                Name = "Icon",
                Description = "图标",
                Type = "string",
                ValueList = "",
                DefaultValue = ""
            },
            new AttributeItem() {
                Name = "LoadingIcon",
                Description = "异步加载时的动画图标",
                Type = "string",
                ValueList = "",
                DefaultValue = "fa fa-fw fa-spin fa-spinner"
            },
            new AttributeItem() {
                Name = "Text",
                Description = "显示文字",
                Type = "string",
                ValueList = "",
                DefaultValue = ""
            },
            new AttributeItem() {
                Name = "Size",
                Description = "尺寸",
                Type = "Size",
                ValueList = "None / ExtraSmall / Small / Medium / Large / ExtraLarge",
                DefaultValue = "None"
            },
            new AttributeItem() {
                Name = "Class",
                Description = "样式",
                Type = "string",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = "IsBlock",
                Description = "填充按钮",
                Type = "boolean",
                ValueList = " — ",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsDisabled",
                Description = "是否禁用",
                Type = "boolean",
                ValueList = " — ",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsOutline",
                Description = "是否边框",
                Type = "boolean",
                ValueList = " — ",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsAsync",
                Description = "是否为异步按钮",
                Type = "boolean",
                ValueList = " — ",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "ChildContent",
                Description = "内容",
                Type = "RenderFragment",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = "ButtonStyle",
                Description = "按钮风格",
                Type = "ButtonStyle",
                ValueList = "None / Circle / Round",
                DefaultValue = "None"
            },
            new AttributeItem() {
                Name = "ButtonType",
                Description = "按钮类型",
                Type = "ButtonType",
                ValueList = "Button / Submit / Reset",
                DefaultValue = "Button"
            }
        };
    }
}
