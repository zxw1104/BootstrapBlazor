// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityFrameworkCoreDataService
    {
        Task CancelAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CancelAsync(object model) { CancelAsync(); return Task.CompletedTask; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditAsync(object model);
    }
}
