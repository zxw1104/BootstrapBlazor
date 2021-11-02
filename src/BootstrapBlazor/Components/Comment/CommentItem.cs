// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 一条评论数据
    /// </summary>
    public class CommentItem
    {
        /// <summary>
        /// 头像Url路径
        /// </summary>
        public string? AvatorUrl { get; set; }
        /// <summary>
        /// 发表此评论的用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 此评论的回复集合
        /// </summary>
        public List<CommentItem>? Children { get; set; } = new List<CommentItem>();
    }
}
