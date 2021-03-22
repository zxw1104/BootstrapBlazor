// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.EntityFrameworkCore;

namespace BootstrapBlazor.Shared.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class FooContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public FooContext(DbContextOptions<FooContext> options) : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Foo>? Foos { get; set; }
    }
}
