// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace BootstrapBlazor.Shared.Pages.Table
{
    public partial class TablesBindDataTable
    {
        private DataTable dataTable1;
        private DataTable dataTable2;
        private CustomDataTableAdapter customDataTableAdapter;

        protected override Task OnInitializedAsync()
        {
            dataTable1 = new DataTable();
            dataTable1.Columns.Add(new DataColumn("Id", typeof(int)));
            dataTable1.Columns.Add(new DataColumn("int类型列", typeof(int)));
            dataTable1.Columns.Add(new DataColumn("string类型列", typeof(string)));
            dataTable1.Columns.Add(new DataColumn("DateTime类型列", typeof(DateTime)));

            dataTable2 = new DataTable();
            dataTable2.Columns.Add(new DataColumn("Id", typeof(int)));
            dataTable2.Columns.Add(new DataColumn("int类型列", typeof(int)));
            dataTable2.Columns.Add(new DataColumn("string类型列", typeof(string)));
            dataTable2.Columns.Add(new DataColumn("DateTime类型列", typeof(DateTime)));


            for (int i = 0; i < 10; i++)
            {
                dataTable1.LoadDataRow(new object?[] { i, i * 10, $"string{i}", DateTime.Now.AddDays(i) }, true);
                dataTable2.LoadDataRow(new object?[] { i, i * 10, $"string{i}", DateTime.Now.AddDays(i) }, true);
            }

            customDataTableAdapter = new CustomDataTableAdapter(dataTable2);

            return base.OnInitializedAsync();
        }

        int propIndex = 1;
        private List<string> properties = new List<string>();
        /// <summary>
        /// 添加列
        /// </summary>
        public Task AddColumn(IEnumerable<DataRowAdapter> items)
        {
            //动态属性名称
            var newPropertyName = $"动态列-{propIndex}";

            //把属性名保存起来，便于后面动态移除属性
            properties.Add(newPropertyName);
            if (propIndex % 2 == 0)
            {
                customDataTableAdapter.AddColumn(newPropertyName,
                    typeof(string),
                    new Attribute[] {
                        //必填
                        new RequiredAttribute(),
                        new AutoGenerateColumnAttribute()
                        { Order = propIndex + 10,Text = $"动态列{propIndex}-字符串类型" }});

                propIndex++;
            }
            else
            {
                //将动态属性注册全局动态属性注册中心
                customDataTableAdapter.AddColumn(newPropertyName, typeof(int),
                    new Attribute[] { new AutoGenerateColumnAttribute() {
                        Order = propIndex + 10,
                        Text = $"动态列{propIndex}-int类型" }});
                propIndex++;
            }
            //手动通知Table，更新列信息
            customDataTableAdapter.RefreshColumn();

            //重新渲染组件
            StateHasChanged();
            return Task.CompletedTask;
        }
        /// <summary>
        /// 删除最后一列
        /// </summary>
        public Task DeleteLastColumn(IEnumerable<DataRowAdapter> items)
        {
            if (properties.Count > 0)
            {
                var propName = properties.Last();
                properties.Remove(propName);

                customDataTableAdapter.RemoveColumn(propName);
                //将动态属性注册全局动态属性注册中心
            }

            //手动通知Table，更新列信息
            customDataTableAdapter.RefreshColumn();

            //重新渲染组件
            StateHasChanged();
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 自定义DataTable适配器
    ///
    /// 此类用于配置DataTable中的Column的一些额外的高级特性，如数据验证，列是否显示
    /// </summary>
    public class CustomDataTableAdapter : DataTableAdapter
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="table"></param>
        public CustomDataTableAdapter(DataTable table) : base(table)
        {

        }
        public override void RegistDynamicProperty(DynamicObjectBuilder builder)
        {
            int orderIndex = 1;
            foreach (DataColumn item in Table.Columns)
            {
                //Id列不显示
                if (item.ColumnName == "Id")
                {
                    continue;
                }
                else if (item.ColumnName == "int类型列")
                {
                    builder.AddProperty(item.ColumnName, item.DataType, new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                    Order = orderIndex++,
                    Text = item.ColumnName
                    },new RequiredAttribute()
                });
                }
                else if (item.ColumnName == "string类型列")
                {
                    builder.AddProperty(item.ColumnName, item.DataType, new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                    Order = orderIndex++,
                    Text = item.ColumnName
                    },
                    new RequiredAttribute(),
                    new StringLengthAttribute(10)
                });
                }

                else
                {
                    builder.AddProperty(item.ColumnName, item.DataType, new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                    Order = orderIndex++,
                    Text = item.ColumnName
                    } });
                }
            }
        }
    }
}
