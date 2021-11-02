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
    /// 展示一条树状结构的评论
    /// </summary>
    public partial class CommentDisplay
    {
        /// <summary>
        /// 当前要显示的评论对象
        /// </summary>
        [Parameter]
        public CommentItem? CommentItem { get; set; }

        /// <summary>
        /// 缩进级别,组件内部使用，外部无需设置,默认值为0，即顶级评论缩进级别为0
        /// </summary>
        [Parameter]
        public int IndentLevel { get; set; }

        /// <summary>
        /// 评论组件参数
        /// </summary>
        [CascadingParameter]
        public CommentWrapper? Param { get; set; }

        private SwitchDisplay? switchControl;

        /// <summary>
        /// 展开和收缩切换
        /// </summary>
        /// <returns></returns>
        private Task OnSwitchDisplay()
        {
            if (switchControl!=null)
            {
                switchControl.SwitchTemplate();
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 当前是否在回复状态
        /// </summary>
        private bool isInReplyStatus;

        /// <summary>
        /// 点击回复，设置父任务和状态
        /// </summary>
        /// <returns></returns>
        public Task OnClickReplay()
        {
            if (Param!=null)
            {
                Param.CurReplayComment = this.CommentItem;
                isInReplyStatus = true;
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 点击取消，设置父任务为null，进去取消状态
        /// </summary>
        /// <returns></returns>
        private Task OnClickCancel()
        {
            if (Param!=null)
            {
                Param.CurReplayComment = null;
                isInReplyStatus = false;
            }
            return Task.CompletedTask;
        }

        private void OnConfirm()
        {
            isInReplyStatus = false;
            //StateHasChanged();
        }

        /// <summary>
        /// 自定义缩进
        /// </summary>
        private string IndentPx => $"{(IndentLevel * 30)}px";
    }
}
