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
        /// 获得/设置 屏幕捕捉按钮文字 默认为 屏幕捕捉
        /// </summary>
        [Parameter]
        [NotNull]
        public string? StartCaptureButtonText { get; set; }

        /// <summary>
        /// 获得/设置 摄像头捕捉按钮文字 默认为 摄像头
        /// </summary>
        [Parameter]
        [NotNull]
        public string? StartCameraCaptureButtonText { get; set; }

        /// <summary>
        /// 获得/设置 停止按钮文字 默认为 停止
        /// </summary>
        [Parameter]
        [NotNull]
        public string? StopCaptureButtonText { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizer<ScreenCapture>? Localizer { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            StartCaptureButtonText ??= Localizer[nameof(StartCaptureButtonText)];
            StartCameraCaptureButtonText ??= Localizer[nameof(StartCameraCaptureButtonText)];
            StopCaptureButtonText ??= Localizer[nameof(StopCaptureButtonText)]; 
        }

        ///// <summary>
        ///// OnAfterRenderAsync 方法
        ///// </summary>
        ///// <param name="firstRender"></param>
        ///// <returns></returns>
        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender && JSRuntime != null)
        //    {
        //        await JSRuntime.InvokeVoidAsync(ScreenCaptureElement, "bb_screencapture");
        //    }
        //}

        private async Task StartCapture()
        {
            await JSRuntime.InvokeVoidAsync(ScreenCaptureElement, "bb_screencapture","start");
        }

        private async Task StartCameraCapture()
        {
            await JSRuntime.InvokeVoidAsync(ScreenCaptureElement, "bb_screencapture","start", "camera");
        }

        private async Task StopCapture()
        {
            await JSRuntime.InvokeVoidAsync(ScreenCaptureElement, "bb_screencapture","stop");
        }
    }
    }
