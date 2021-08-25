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
    /// 外部资源提供器
    /// </summary>
    public interface IExternalResourceProvider
    {
        /// <summary>
        /// 尝试从外部获取资源
        /// </summary>
        /// <param name="cultureInfoName">UI文化</param>
        /// <param name="resourceKey">资源Key，目前使用对象全名+属性名的方式，如 BootstrapBlazor.Components.Button.Text</param>
        /// <param name="resourceValue">对应的资源值</param>
        /// <returns></returns>
        bool TryGetResource(string cultureInfoName, string resourceKey, out string resourceValue);
    }
}
