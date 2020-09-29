using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class FaceLogin
    {
        private ElementReference FaceElement { get; set; }

#nullable disable
        private Modal ModalSetting { get; set; }
#nullable restore

        /// <summary>
        /// OnAfterRenderAsync 方法
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender) await JSRuntime.InvokeVoidAsync(FaceElement, "bb_face");
        }

        private async Task OnRegister()
        {
            await BaiduFaceHttpClient.Register();
        }
    }
}
