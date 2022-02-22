// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Shared.Samples;
public partial class Drivers
{
    [NotNull]
    private Driver? Driver { get; set; }

    private DriverOptions DriverOptions { get; set; } = new DriverOptions();

    private List<StepOptions>? StepOptionsList { get; set; }

    /// <summary>
    /// OnInitialized
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        StepOptionsList = new List<StepOptions>()
        {
            new StepOptions()
            {
                Element = "#a",
                Popover = new PopoverOptions() { Title = "这里是整个<code>Card</code>", Description = "这里支持<b>html</b>标签", }
            },
            new StepOptions()
            {
                Element = "#b",
                Popover = new PopoverOptions() { Title = "这里是<code>CardHeader</code>", Description = "这里也支持<b>html</b>标签", }
            },
            new StepOptions()
            {
                Element = "#c",
                Popover = new PopoverOptions() { Title = "这里是<code>CardBody</code>", Description = "这里也支持<b>html</b>标签", }
            },
            new StepOptions()
            {
                Element = "#d",
                Popover = new PopoverOptions() { Title = "这里是<code>CardFooter</code>", Description = "这里也支持<b>html</b>标签", }
            }
        };
    }

    private async Task DriverCardAsync()
    {
        await Driver.StartAsync();

    }

    private async Task AutoDriverCardAsync()
    {
        await Driver.StartAsync();
        await Task.Delay(1000);
        while (await Driver.HasNextStepAsync())
        {
            await Driver.MoveNextAsync();
            await Task.Delay(1000);
        }
        await Driver.ResetAsync();
    }

    private bool _popoverState = true;

    private async Task SwitchPopover()
    {
        if (_popoverState)
        {
            await Driver.HidePopoverAsync();
        }
        else
        {
            await Driver.ShowPopoverAsync();
        }
        _popoverState = !_popoverState;
    }

    private void ChangeParameter()
    {
        DriverOptions.Opacity = 0f;
    }
}
