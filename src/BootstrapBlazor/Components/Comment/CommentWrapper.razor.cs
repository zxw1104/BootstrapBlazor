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
    /// 评论包装器,此类主要用于给所有评论提供模板，和统一的事件绑定
    /// </summary>
    public partial class CommentWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public CommentWrapper()
        {
            GetIndent = i => $"{i*30}px";
        }
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 作为顶级回复组件的PlaceHolder
        /// </summary>
        [Parameter]
        public string TopLevelPlaceHolder { get; set; }

        /// <summary>
        /// 作为非顶级回复组件的PlaceHolder
        /// </summary>
        [Parameter]
        public string PlaceHolder { get; set; }
        /// <summary>
        /// 当前回复的评论对象
        /// </summary>
        public CommentItem? CurReplayComment { get; set; }

        /// <summary>
        /// 子级回复的内容
        /// </summary>
        public string? ReplayContent { get; set; }

        /// <summary>
        /// 顶级回复内容
        /// </summary>
        public string? TopContent { get; set; }
        /// <summary>
        /// 当点击评论时，触发此委托
        /// </summary>
        [Parameter]
        public EventCallback<CreateCommentInfo> OnCreateComment { get; set; }


        /// <summary>
        /// 触发创建评论事件
        /// </summary>
        /// <returns></returns>
        public Task OnClickCreate()
        {
            if (OnCreateComment.HasDelegate)
            {
                var info = new CreateCommentInfo
                {
                    Parent = CurReplayComment,
                };
               
                // 子级回复
                if (CurReplayComment != null)
                {
                    info.Content = ReplayContent;
                    ReplayContent = string.Empty;
                    CurReplayComment = null;
                }
                else
                {
                    // 顶级回复
                    info.Content = TopContent;
                    TopContent = string.Empty;
                }
                return OnCreateComment.InvokeAsync(info);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取缩进，默认是 Indent * 30 px
        /// </summary>
        [Parameter]
        public Func<int,string> GetIndent { get; set; }
        #region 模板
        /// <summary>
        /// 
        /// </summary>
        ///
        [Parameter]
        public RenderFragment<CommentDisplay>? CommentDisplayHeaderTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ///
        [Parameter]
        public RenderFragment<CommentDisplay>? CommentDisplayBodyTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<CommentDisplay>? CommentDisplayFooterTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///
        [Parameter]
        public RenderFragment<CommentReplay?>? CommentReplayHeaderTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ///
        [Parameter]
        public RenderFragment<CommentReplay?>? CommentReplayBodyTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ///
        [Parameter]
        public RenderFragment<CommentReplay?>? CommentReplayFooterTemplate { get; set; }
        #endregion

    }

    /// <summary>
    /// 用户点击提交评论时，回调方法接受的参数
    /// </summary>
    public struct CreateCommentInfo
    {
        /// <summary>
        /// 当前评论的父对象，如果是顶级评论，则此对象为null
        /// </summary>
        public CommentItem? Parent { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string? Content { get; set; }
    }
}
