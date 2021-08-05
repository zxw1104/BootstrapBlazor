// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace BootstrapBlazor.Shared.Pages.Table
{
    /// <summary>
    /// 表格动态添加列示例组件
    /// </summary>
    public partial class TablesDynamicColumn
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        public MyDataService DataService { get; set; } = new MyDataService();

        static int propIndex = 1;

        private List<string> properties = new List<string>();
        /// <summary>
        /// 添加列
        /// </summary>
        public Task AddColumn(IEnumerable<DynamicUser> items)
        {
            //动态属性名称
            var newPropertyName = $"动态属性-{propIndex}";
            int orderIndex = 1;
            //把属性名保存起来，便于后面动态移除属性
            properties.Add(newPropertyName);
            if (propIndex % 2 == 0)
            {
                DynamicUser.dynamicObjectBuilder.AddProperty(orderIndex++, newPropertyName,
                    typeof(string),
                    new Attribute[] {
                        //必填
                        new RequiredAttribute(),
                        new AutoGenerateColumnAttribute()
                        { Order = propIndex + 10,Text = $"动态属性{propIndex}-字符串类型" }});

                propIndex++;
            }
            else
            {
                //将动态属性注册全局动态属性注册中心
                DynamicUser.dynamicObjectBuilder.AddProperty(orderIndex++, newPropertyName, typeof(int),
                    new Attribute[] { new AutoGenerateColumnAttribute() {
                        Order = propIndex + 10,
                        Text = $"动态属性{propIndex}-int类型" }});
                propIndex++;
            }


            //手动通知Table，更新列信息
            DynamicUser.dynamicObjectBuilder.RefreshTableColumn();

            //重新渲染组件
            StateHasChanged();
            return Task.CompletedTask;

        }
        /// <summary>
        /// 删除最后一列
        /// </summary>
        public Task DeleteLastColumn(IEnumerable<DynamicUser> items)
        {
            if (properties.Count > 0)
            {
                var propName = properties.Last();
                properties.Remove(propName);

                DynamicUser.dynamicObjectBuilder.RemoveProperty(propName);
                //将动态属性注册全局动态属性注册中心
            }

            //手动通知Table，更新列信息
            DynamicUser.dynamicObjectBuilder.RefreshTableColumn();

            //重新渲染组件
            StateHasChanged();
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 示例Model，动态属性的User
    /// </summary>
    public class DynamicUser : DynamicBase
    {
        /// <summary>
        /// 动态对象建造者
        /// </summary>
        public static DynamicObjectBuilder dynamicObjectBuilder { get; set; }

        static DynamicUser()
        {
            int orderIndex = 1;
            //定义动态对象拥有的属性,只有在这里注册的属性，才会在Table中显示出来
            dynamicObjectBuilder = new DynamicObjectBuilder(typeof(DynamicUser));
            dynamicObjectBuilder.AddClassAttribute(new AutoGenerateClassAttribute
            {
                Filterable = true,
                Searchable = true
            })
                .AddProperty(orderIndex++,"Address.Name", typeof(string), new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                        Order = 3,
                        Editable=false,
                        Searchable = false,
                        Text = "地址信息" }
                })
                .AddProperty(orderIndex++, "Id", typeof(int), new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                        Order = 1, 
                        Text = "Id" ,Editable=false},
                })
                .AddProperty(orderIndex++, "Name", typeof(string), new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                        Order = 1,
                        Text = "名称" },
                    new RequiredAttribute(),
                    new StringLengthAttribute(5)
                })
                .AddProperty(orderIndex++, "Age", typeof(int), new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                        Order = 2,
                        Searchable = false,
                        Text = "年龄" }
                });
        }

        static int userId = 1;

        /// <summary>
        /// 获取Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetId(IDynamicType obj)
        {
            var user = (DynamicUser)obj;
            return user.Id;
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetAddressName(IDynamicType obj)
        {
            var user = (DynamicUser)obj;
            if (user.Address==null)
            {
                user.Address = new DynamicAddress();
                user.Address.SetValue("Name", "默认地址");
            }
            return user.Address.GetValue("Name");
        }

        /// <summary>
        /// 设置地址
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="v"></param>
        private static void SetAddressName(IDynamicType obj, object v)
        {
            ((DynamicUser)obj).Address.SetValue("Name", v);
        }
        /// <summary>
        /// 生成一个默认动态用户，用户点击添加按钮时，使用此构造函数，创建默认对象
        /// </summary>
        public DynamicUser()
        {
            Id = userId++;
            //注册复杂属性的访问器
            AddCustomGetSet("Address.Name", GetAddressName, SetAddressName);

            //注册Id静态属性的访问器
            AddCustomGetSet("Id", GetId, null);
        }

        public DynamicAddress Address { get; set; }
        /// <summary>
        /// 静态Id属性
        /// </summary>
        public int Id { get; set; }

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
            SetValue("Name", n);
            return this;
        }


        public override DynamicObjectBuilder GetBuilder()
        {
            return dynamicObjectBuilder;
        }

        /// <summary>
        /// 构造新的User对象，在点击编辑时，会使用此方法，创建一个当前对象的副本
        /// </summary>
        /// <returns></returns>
        public override IDynamicType New()
        {
            return new DynamicUser();
        }
    }

    public class DynamicAddress : DynamicBase
    {
        public static DynamicObjectBuilder dynamicObjectBuilder { get; set; }

        static DynamicAddress()
        {
            int orderIndex = 1;
            //定义动态对象拥有的属性
            dynamicObjectBuilder = new DynamicObjectBuilder(typeof(DynamicAddress));
            dynamicObjectBuilder.AddClassAttribute(new AutoGenerateClassAttribute
            {
                Filterable = true,
                Searchable = true
            })
                .AddProperty(orderIndex, "Name", typeof(string), new Attribute[] {
                    new AutoGenerateColumnAttribute() {
                        Order = 1,
                        Text = "名称" },
                    new RequiredAttribute(),
                    new StringLengthAttribute(5)
                });
        }

        public string Name
        {
            get { return GetValue<string>("Name"); }
            set
            {
                SetValue("Name", value);
            }
        }
        public override DynamicObjectBuilder GetBuilder()
        {
            return dynamicObjectBuilder;
        }

        public override IDynamicType New()
        {
            return new DynamicAddress();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyDataService : IDataService<DynamicUser>
    {
        static MyDataService()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = new DynamicUser();
                user.SetName($"user {i}");
                AllUser.Add(user);
            }
        }

        public static List<DynamicUser> AllUser = new List<DynamicUser>();
        /// <summary>
        /// 点击新建按钮时触发
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<bool> AddAsync(DynamicUser model)
        {
            //设置对象为临时对象，在保存时，会使用此标记判断对象是新增还是更新
            model.IsTemp = true;
            return Task.FromResult(true);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(IEnumerable<DynamicUser> models)
        {
            foreach (var item in models)
            {
                AllUser.Remove(item);
            }
            return Task.FromResult(true);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public Task<QueryData<DynamicUser>> QueryAsync(QueryPageOptions option)
        {
            return Task.FromResult(new QueryData<DynamicUser> { Items = AllUser });
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                //编辑
                var oldModel = AllUser.First(p => p.Id == model.Id);

                //获取动态类型所有的属性
                var props = TypeInfoHelper.GetProperties(oldModel);
                foreach (var p in props)
                {
                    oldModel.SetValue(p.Name, model.GetValue(p.Name));
                }
            }
            return Task.FromResult(true);
        }
    }
}
