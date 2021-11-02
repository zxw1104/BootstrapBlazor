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
    /// <summary>
    /// 评论回复组件
    /// </summary>
    public partial class CommentReplay
    {
        private string Pt = "pt-2";
        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter]
        public CommentWrapper Param { get; set; }

        /// <summary>
        /// 是否是顶级回复，顶级回复与子级回复 绑定 Content字段不同,默认为false
        /// </summary>
        [Parameter]
        public bool IsTopLevelReplay { get; set; }

        /// <summary>
        /// 取消事件
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// 取消事件
        /// </summary>
        [Parameter]
        public Action OnConfirm { get; set; }
        /// <summary>
        /// 点击提交按钮，调用此方法
        /// </summary>
        /// <returns></returns>
        public Task OnClickConfirm()
        {
            if (Param != null)
            {
                OnConfirm?.Invoke();
                return Param.OnClickCreate();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 点击取消按钮，调用此方法
        /// </summary>
        /// <returns></returns>
        public Task OnClickCancel()
        {
            if (OnCancel.HasDelegate && Param != null)
            {
                if (IsTopLevelReplay)
                {
                    Param.TopContent = string.Empty;
                }
                else
                {
                    Param.ReplayContent = string.Empty;
                }
                return OnCancel.InvokeAsync();
            }
            return Task.CompletedTask;
        }
    }
}
