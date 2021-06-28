// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// DataTable 动态数据上下文实现类 <see cref="DynamicObjectContext" />
    /// </summary>
    public class DataTableDynamicContext : DynamicObjectContext
    {
        /// <summary>
        /// 获得/设置 相关联的 DataTable 实例
        /// </summary>
        public DataTable? DataTable { get; set; }

        /// <summary>
        /// GetItems 方法
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IDynamicObject> GetItems()
        {
            var ret = new List<DataTableDynamicObject>();
            if (DataTable != null)
            {
                foreach (DataRow row in DataTable.Rows)
                {
                    ret.Add(new DataTableDynamicObject() { Row = row });
                }
            }
            return ret.Cast<IDynamicObject>();
        }

        /// <summary>
        /// 获得列信息方法
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ITableColumn> GetColumns()
        {
            var ret = new List<InternalTableColumn>();
            if (DataTable != null)
            {
                foreach (DataColumn col in DataTable.Columns)
                {
                    ret.Add(new InternalTableColumn(col.ColumnName, col.DataType, col.ColumnName));
                }
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<DynamicObject> AddAsync()
        {
            if (DataTable == null) throw new NullReferenceException();

            var row = DataTable.NewRow();
            SetDefaultValue(row);
            DynamicObject item = new DataTableDynamicObject()
            {
                Row = row
            };
            return Task.FromResult(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<bool> SaveAsync(DynamicObject item)
        {
            if (item is DataTableDynamicObject dynamicObject)
            {
                var row = dynamicObject.Row;
                if (row != null)
                {
                    DataTable?.Rows.InsertAt(row, 0);
                    DataTable?.AcceptChanges();
                }
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(IEnumerable<DynamicObject> items)
        {
            foreach (var item in items)
            {
                var row = ((DataTableDynamicObject)item).Row;
                if (row != null)
                {
                    DataTable?.Rows.Remove(row);
                }
            }
            return Task.FromResult(true);
        }

        private void SetDefaultValue(DataRow row)
        {
            foreach (DataColumn col in row.Table.Columns)
            {
                var defaultValue = col.DefaultValue;
                if (defaultValue == null || defaultValue == DBNull.Value)
                {
                    // TODO: 完善所有数据类型
                    defaultValue = col.DataType.Name switch
                    {
                        nameof(DateTime) => DateTime.Now,
                        nameof(Int32) => 0,
                        _ => ""
                    };
                    row[col] = defaultValue;
                }
            }
        }
    }
}
