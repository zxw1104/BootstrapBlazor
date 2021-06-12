// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace BootstrapBlazor.Shared.Pages.Table
{
    /// <summary>
    /// 表格动态添加列示例组件
    /// </summary>
    public partial class TablesDynamicColumn
    {
        public MyDataService DataService { get; set; } = new MyDataService();

        private Table<DynamicUser> table;
        static int propIndex = 1;
        /// <summary>
        /// 添加列
        /// </summary>
        public void AddColumn()
        {
            if (propIndex % 2 == 0)
            {
                //动态属性名称
                var newPropertyName = $"DynamicProperty-{propIndex}";
                var newPropertyDefaultValue = $"默认值{propIndex}";

                //为当前表格中的所有对象添加动态属性
                MyDataService.AllUser.ForEach(user => user.AddProperty(newPropertyName, newPropertyDefaultValue));

                //将动态属性注册全局动态属性注册中心
                DynamicPropertyRegistry.AddProperty(typeof(DynamicUser), new DynamicPropertyInfo(newPropertyName, typeof(string), new Attribute[] { new AutoGenerateColumnAttribute() { Order = propIndex + 10, Filterable = true, Searchable = true, Text = $"动态属性{propIndex}-字符串类型" } }));
                propIndex++;
            }
            else
            {
                //动态属性名称
                var newPropertyName = $"DynamicProperty-{propIndex}";
                var newPropertyDefaultValue = 1;

                //为当前表格中的所有对象添加动态属性
                MyDataService.AllUser.ForEach(user => user.AddProperty(newPropertyName, newPropertyDefaultValue));

                //将动态属性注册全局动态属性注册中心
                DynamicPropertyRegistry.AddProperty(typeof(DynamicUser), new DynamicPropertyInfo(newPropertyName, typeof(int), new Attribute[] { new AutoGenerateColumnAttribute() { Order = propIndex + 10, Filterable = true, Searchable = true, Text = $"动态属性{propIndex}-int类型" } }));
                propIndex++;
            }
            //手动通知Table，更新列信息
            table.ReGenerateColumn();

            //重新渲染组件
            StateHasChanged();
        }
        /// <summary>
        /// 删除最后一列
        /// </summary>
        public void DeleteLastColumn()
        {
            var props = DynamicPropertyRegistry.GetProperties(typeof(DynamicUser));

            if (props.Length>1)
            {
                var lastProperty = props.Last();
                var propName = lastProperty.Name;

                //为当前表格中的所有对象添加动态属性
                MyDataService.AllUser.ForEach(user => user.RemoveProperty(propName));

                //将动态属性注册全局动态属性注册中心
                DynamicPropertyRegistry.RemoveProperty(typeof(DynamicUser), lastProperty);
            }
           

            //手动通知Table，更新列信息
            table.ReGenerateColumn();

            //重新渲染组件
            StateHasChanged();
        }
    }

    /// <summary>
    /// 示例Model，动态属性的User
    /// </summary>
    public class DynamicUser : IDynamicType
    {
        static DynamicUser()
        {
            var type = typeof(DynamicUser);
            //添加类型Attribute定义
            DynamicPropertyRegistry.AddAutoGenerateClassAttribute(type, new AutoGenerateClassAttribute());

            //添加属性Attribute定义
            DynamicPropertyRegistry.AddProperty(type, new DynamicPropertyInfo("Name", typeof(string), new Attribute[] { new AutoGenerateColumnAttribute() { Order = 1, Filterable = true, Searchable = true, Text = "名称" } }));
            DynamicPropertyRegistry.AddProperty(type, new DynamicPropertyInfo("Age", typeof(int), new Attribute[] { new AutoGenerateColumnAttribute() { Order = 2, Filterable = true, Searchable = true, Text = "年龄" } }));
        }
        private Dictionary<string, object?> propDic = new Dictionary<string, object?>();

        /// <summary>
        /// 动态类型实体
        /// </summary>
        /// <param name="propDic"></param>
        public DynamicUser(Dictionary<string, object?> propDic)
        {
            this.propDic = propDic;
        }

        static int i = 1;

        /// <summary>
        /// 生成一个默认动态用户，用户点击添加按钮时，使用此构造函数，创建默认对象
        /// </summary>
        public DynamicUser()
        {
            //从注册中心获取当前对象有哪些动态属性
            var props = DynamicPropertyRegistry.GetProperties(typeof(DynamicUser));
            propDic.Add("Name", $"张三--{i++}");
            propDic.Add("Age", i + 10);
            Id = i++;
            foreach (DynamicPropertyInfo p in props)
            {
                var targetType= p.PropertyType;
                var defaultValue= targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
                propDic[p.Name] = defaultValue;
            }
        }

        /// <summary>
        /// 给当前对象添加一个动态属性
        /// </summary>
        public void AddProperty(string propName, object defaultValue)
        {
            propDic.Add(propName, defaultValue);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="propName"></param>
        public void RemoveProperty(string propName)
        {
            if (propDic.ContainsKey(propName))
            {
                propDic.Remove(propName);
            }
        }

        /// <summary>
        /// Id属性
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 根据属性名，获取属性值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object? GetValue(string propName)
        {
            if (propDic.ContainsKey(propName))
            {
                return propDic[propName];
            }
            return $"属性不存在,{propName}";
        }
        /// <summary>
        /// 当前对象是否是创建时的临时对象
        /// </summary>
        public bool IsTemp { get; set; } = false;
        /// <summary>
        /// 设置名称属性
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DynamicUser SetName(string n)
        {
            propDic["Name"] = n;
            return this;
        }
        /// <summary>
        /// 设置指定属性为指定值
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public void SetValue(string propName, object value)
        {
            //这里需要类型转换
            propDic[propName] = value;
        }

        /// <summary>
        /// 编辑时 调用Clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Dictionary<string, object?> newPropDic = new Dictionary<string, object?>();
            foreach (var item in propDic)
            {
                newPropDic[item.Key] = item.Value;
            }
            return new DynamicUser(newPropDic) { Id = this.Id };
        }

        public void CopyFrom(IDynamicType other)
        {
            foreach (var item in propDic)
            {
                propDic[item.Key] = other.GetValue(item.Key);
            }
        }
    }


    public class MyDataService : IDataService<DynamicUser>
    {
        static MyDataService()
        {
            for (int i = 0; i < 10; i++)
            {
                AllUser.Add(new DynamicUser().SetName(DateTime.Now.ToString()));
            }
        }
        public static List<DynamicUser> AllUser = new List<DynamicUser>();
        public Task<bool> AddAsync(DynamicUser model)
        {
            model.IsTemp = true;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(IEnumerable<DynamicUser> models)
        {
            foreach (var item in models)
            {
                AllUser.Remove(item);
            }
            return Task.FromResult(true);
        }

        public Task<QueryData<DynamicUser>> QueryAsync(QueryPageOptions option)
        {
            return Task.FromResult(new QueryData<DynamicUser> { Items = AllUser });
        }

        public Task<bool> SaveAsync(DynamicUser model)
        {
            //临时对象 保存 就是新增操作
            if (model.IsTemp)
            {
                //新增
                model.IsTemp = false;
                AllUser.Add(model);
            }
            else
            {
                //编辑 直接替换对象
                var index = AllUser.FindIndex(p => p.Id == model.Id);
                AllUser[index] = model;
            }
            return Task.FromResult(true);
        }
    }
}
