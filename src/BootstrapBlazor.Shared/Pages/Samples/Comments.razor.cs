// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BootstrapBlazor.Shared.Pages
{
    public partial class Comments
    {
        private string headerUrl = "_content/BootstrapBlazor.Shared/images/Argo-C.png";
        /// <summary>
        /// 
        /// </summary>
        public Comments()
        {
            CommentList = Enumerable.Range(1, 2).Select(i => new CommentItem
            {
                AvatorUrl = headerUrl,
                Content = $"评论内容{i}",
                CreateTime = DateTime.Now,
                UserName = $"用户{i}",
                Children = Enumerable.Range(1, 2).Select(i => new CommentItem
                {
                    AvatorUrl = headerUrl,
                    Content = $"回复的评论内容{i}",
                    CreateTime = DateTime.Now,
                    UserName = $"回复用户{i}"
                }).ToList()
            }).ToList();

            MarkdowmCommentList = Enumerable.Range(1, 2).Select(i => new CommentItem
            {
                AvatorUrl = headerUrl,
                Content = $"<h4>Markdown评论内容{i}</h4>",
                CreateTime = DateTime.Now,
                UserName = $"用户{i}",
                Children = Enumerable.Range(1, 2).Select(i => new CommentItem
                {
                    AvatorUrl = headerUrl,
                    Content = $"<h5>Markdown回复的评论内容{i}</h5>",
                    CreateTime = DateTime.Now,
                    UserName = $"回复用户{i}"
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// 实例评论数据集合
        /// </summary>
        public List<CommentItem> CommentList { get; set; }

        /// <summary>
        /// 使用Markdown评论编辑器的评论集合
        /// </summary>
        public List<CommentItem> MarkdowmCommentList { get; set; }

        /// <summary>
        /// 创建默认评论
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Task OnCreateComment(CreateCommentInfo info)
        {
            AddToList(info, CommentList);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Task OnCreateMarkdownComment(CreateCommentInfo info)
        {
            AddToList(info, MarkdowmCommentList);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 将新增的评论，添加到指定的评论集合中
        /// </summary>
        /// <param name="info"></param>
        /// <param name="list"></param>
        private void AddToList(in CreateCommentInfo info, List<CommentItem> list)
        {
            var newComment = new CommentItem
            {
                AvatorUrl = headerUrl,
                Content = info.Content,
                CreateTime = DateTime.Now,
                UserName = $"当前用户",
            };
            // 回复评论
            if (info.Parent != null)
            {
                if (info.Parent.Children == null)
                {
                    info.Parent.Children = new List<CommentItem>();
                }
                // 添加评论
                info.Parent.Children.Add(newComment);
            }
            // 顶级评论
            else
            {
                list.Add(newComment);
            }
        }

        private IEnumerable<EventItem> GetCommentWrapperEvents()
        {
            return new EventItem[]
              {
                     new EventItem()
                    {
                        Name = "OnCreateComment",
                        Description = "点击提交时触发此事件",
                        Type ="EventCallback<CreateCommentInfo>"
                    },
              };
        }

        private IEnumerable<AttributeItem> GetCommentDisplayAttributes()
        {
            return new AttributeItem[]
             {
                    new AttributeItem(){
                        Name = "CommentItem",
                        Description = "评论对象",
                        Type = "CommentItem",
                        ValueList = " — ",
                        DefaultValue = " — "
                    },
             };
        }

        private IEnumerable<AttributeItem> GetCommentReplayAttributes()
        {
            return new AttributeItem[]
             {
                    new AttributeItem(){
                        Name = "IsTopLevelReplay",
                        Description = "是否是顶级回复组件",
                        Type = "boolean",
                        ValueList = " — ",
                        DefaultValue = " false "
                    },
             };
        }

    }
}
