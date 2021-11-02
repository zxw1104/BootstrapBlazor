// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BootstrapBlazor.Components
{
    public partial class SwitchDisplay
    {
        /// <summary>
        /// 是否处于显示 第一个模板状态
        /// </summary>
        private bool isShowDefaultTemplate = true;

        /// <summary>
        /// 默认显示模板
        /// </summary>
        ///
        [Parameter]
        public RenderFragment? DefaultTemplate { get; set; }

        /// <summary>
        /// 切换后显示的模板
        /// </summary>
        ///
        [Parameter]
        public RenderFragment? SecondTemplate { get; set; }

        /// <summary>
        /// 切换显示模板
        /// </summary>
        public void SwitchTemplate()
        {
            isShowDefaultTemplate = !isShowDefaultTemplate;
            StateHasChanged();
        }
    }
}
