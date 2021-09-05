// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// ScreenCapture 组件部分类
    /// </summary>
    public sealed partial class ScreenCapture
    {
        private ElementReference ScreenCaptureElement { get; set; }

        /// <summary>
        /// 获得/设置 全屏按钮文字 默认为 全屏
        /// </summary>
        [Parameter]
        [NotNull]
        public string? ScreenCaptureButtonText { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizer<ScreenCapture>? Localizer { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ScreenCaptureButtonText ??= Localizer[nameof(ScreenCaptureButtonText)]; 
        }  

        private async Task ToggleScreenCapture()
        {
            await JSRuntime.InvokeVoidAsync(ScreenCaptureElement, "bb_toggleScreenCapture");
        }
    }
    }
