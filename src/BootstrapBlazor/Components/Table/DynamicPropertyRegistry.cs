// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 动态属性注册中心
    /// </summary>
    internal class DynamicPropertyRegistry
    {
        private Dictionary<string, PropertyInfo> props = new();
        private HashSet<Attribute> classAttrs = new();
        public Type TypeInfo { get; set; }
        #region 注册
        /// <summary>
        /// 添加动态属性
        /// </summary>
        /// <param name="typeKey"></param>
        /// <param name="info"></param>
        public void AddProperty(PropertyInfo info)
        {
            props.Add(info.Name, info);
        }

        /// <summary>
        /// 根据属性名，判断属性是否存在
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool IsPropertyExist(string propName)
        {
           return props.ContainsKey(propName);
        }

        /// <summary>
        /// 删除动态属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        public bool RemoveProperty(PropertyInfo info)
        {
            if (props.ContainsKey(info.Name))
            {
                return props.Remove(info.Name);
            }
            throw new NotFoundPropertyException(TypeInfo.FullName, info.Name);
        }
        /// <summary>
        /// 注册 AutoGenerateClassAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        public void AddClassAttribute(Attribute info)
        {
            classAttrs.Add(info);
        }

        /// <summary>
        /// 移除 AutoGenerateClassAttribute
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool RemoveClassAttribute(Attribute info)
        {
            return classAttrs.Remove(info);
        }

        #endregion

        #region 使用
        /// <summary>
        /// 获取指定了类型的所有属性信息
        /// </summary>
        /// <returns></returns>
        public PropertyInfo[] GetProperties()
        {
            return props.Values.ToArray();
        }
        /// <summary>
        /// 获取指定类型的指定属性
        /// </summary>
        /// <param name="typeKey">类型key</param>
        /// <param name="propName">属性名称</param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string propName)
        {
            if (IsPropertyExist(propName))
            {
                return props[propName];
            }
            throw new NotFoundPropertyException(TypeInfo.FullName, propName);
        }
        /// <summary>
        /// 获取类型上面的 所有特性
        /// </summary>
        /// <param name="type">类型key</param>
        /// <returns></returns>
        public List<Attribute> GetClasseAttributes()
        {
            return classAttrs.ToList();
        }
        #endregion
    }

    /// <summary>
    /// 在动态类型上，未找到指定名称的属性
    /// </summary>
    public class NotFoundPropertyException : Exception
    {
        /// <summary>
        /// 构造异常
        /// </summary>
        /// <param name="typeKey"></param>
        /// <param name="propName"></param>
        public NotFoundPropertyException(string typeKey, string propName) : base($"在动态类型{typeKey}上，未找到{propName}属性")
        {
        }
    }

    /// <summary>
    /// 动态对象基础接口
    /// </summary>
    public interface IDynamicType : ICloneable
    {
        /// <summary>
        /// 根据属性名称，获取属性值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        object? GetValue(string propName);
        /// <summary>
        /// 根据属性名称，设置属性值
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        void SetValue(string propName, object value);

        /// <summary>
        /// 获取动态Builder对象
        /// </summary>
        DynamicObjectBuilder GetBuilder();
    }

    /// <summary>
    /// 类型信息查询辅助类
    ///
    /// 此类支持对动态类型和静态类型的属性和类特性查询
    /// </summary>
    public class TypeInfoHelper
    {
        public static T? GetTypeAttribute<T>(Type type)
            where T : Attribute
        {
            if (IsDynamicType(type))
            {
                throw new Exception("逻辑有bug，动态类型不应该通过类型获取属性信息");
            }
            else
            {
                return type.GetCustomAttribute<T>();
            }
        }
        /// <summary>
        /// 获取当前对象的属性定义
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(object model)
        {
            if (model is IDynamicType dType)
            {
                return dType.GetBuilder().GetProperties();
            }
            else
            {
                return model.GetType().GetProperties();
            }
        }

        /// <summary>
        /// 动态类型，不提供根据类型，查询属性定义的方法，因为可能同一个类型，对应多个 不同的属性
        ///
        /// 之前所有需要通过类型获取属性定义的 地方，都要改为 根据 DynamicObjectBuilder获取 属性定义
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type type)
        {
            if (IsDynamicType(type))
            {
                throw new Exception("逻辑有bug，动态类型不应该通过类型获取属性信息");
            }
            return type.GetProperties();
        }

        public static bool IsDynamicType(Type type)
        {
            var isCustomType = type.IsAssignableTo(typeof(IDynamicType));
            return isCustomType;
        }
    }

    /// <summary>
    /// 动态属性信息
    /// </summary>
    class DynamicPropertyInfo : PropertyInfo
    {
        /// <summary>
        /// 构造动态属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propType"></param>
        /// <param name="attributes"></param>
        public DynamicPropertyInfo(string name, Type propType, Attribute[] attributes)
        {
            this.name = name;
            this.propertyType = propType;
            if (attributes==null)
            {
                this.attributes = Array.Empty<Attribute>();
            }
            else
            {
                this.attributes = attributes;
            }
        }
        public override PropertyAttributes Attributes => PropertyAttributes.None;

        public override bool CanRead => true;

        public override bool CanWrite => true;

        private Type propertyType;
        public override Type PropertyType => propertyType;

        public override Type? DeclaringType => throw new NotImplementedException();


        private string name;
        public override string Name => name;

        public override Type? ReflectedType => throw new NotImplementedException();

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return null;
        }

        private Attribute[] attributes;
        public override object[] GetCustomAttributes(bool inherit)
        {
            return attributes;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return attributes.Where(a => a.GetType() == attributeType).ToArray();
        }

        public override MethodInfo? GetGetMethod(bool nonPublic)
        {
            return null;
        }

        public static ParameterInfo[] parameterInfos = Array.Empty<ParameterInfo>();


        public override ParameterInfo[] GetIndexParameters()
        {
            return parameterInfos;
        }

        public override MethodInfo? GetSetMethod(bool nonPublic)
        {
            return null;
        }

        public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        {
            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        {

        }
    }

    /// <summary>
    /// 动态类型建造者
    /// </summary>
    public class DynamicObjectBuilder
    {
        //private string typeKey;
        //private Type type;
        private DynamicPropertyRegistry dynamicPropertyRegistry = new DynamicPropertyRegistry();

        /// <summary>
        /// 用户手动触发更新属性定义会执行此委托
        /// 对于Table组件，应该将此委托触发重新生成列
        /// </summary>
        internal Action? RefreshProperty;

        /// <summary>
        /// 刷新表格列
        /// </summary>
        public void RefreshTableColumn()
        {
            RefreshProperty?.Invoke();
        }
        public Type ObjectType
        {
            get
            {
                return dynamicPropertyRegistry.TypeInfo;
            }
        }
        /// <summary>
        /// 创建构造器实例
        /// </summary>
        /// <param name="type">动态类型的Type</param>
        /// <param name="typeKey">动态类型的字符串唯一标识</param>
        public DynamicObjectBuilder(Type type)
        {
            dynamicPropertyRegistry.TypeInfo = type;
        }

        /// <summary>
        /// 添加动态属性定义
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="propType">属性类型</param>
        /// <param name="attributes">属性的Attribute</param>
        public DynamicObjectBuilder AddProperty(string name, Type propType, Attribute[] attributes)
        {
            dynamicPropertyRegistry.AddProperty(new DynamicPropertyInfo(name, propType, attributes));
            return this;
        }

        /// <summary>
        /// 移除指定属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public DynamicObjectBuilder RemoveProperty(string name)
        {
            var property = dynamicPropertyRegistry.GetProperty(name);
            dynamicPropertyRegistry.RemoveProperty(property);
            return this;
        }

        /// <summary>
        /// 给类型添加Attribute
        /// </summary>
        /// <param name="attribute">类型的Attribute</param>
        /// <returns></returns>
        public DynamicObjectBuilder AddClassAttribute(Attribute attribute)
        {
            dynamicPropertyRegistry.AddClassAttribute(attribute);
            return this;
        }

        /// <summary>
        /// 将动态类型的动态属性设置为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void SetDefaultValues<T>(T obj) where T : IDynamicType, new()
        {
            var props = dynamicPropertyRegistry.GetProperties();
            foreach (DynamicPropertyInfo p in props)
            {
                var targetType = p.PropertyType;
                var defaultValue = GetDefaultValue(targetType);
                obj.SetValue(p.Name, defaultValue);
            }
        }

        /// <summary>
        /// 获取指定属性的默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object? GetPropertyDefaultValue(string propName)
        {
            var targetType = dynamicPropertyRegistry.GetProperty(propName).PropertyType;
            return GetDefaultValue(targetType);
        }

        private static object? GetDefaultValue(Type targetType)
        {
            var defaultValue = targetType.IsValueType ? Activator.CreateInstance(targetType) : string.Empty;
            return defaultValue;
        }

        /// <summary>
        /// 获取指定了类型的所有属性信息
        /// </summary>
        /// <returns></returns>
        public PropertyInfo[] GetProperties()
        {
            return dynamicPropertyRegistry.GetProperties();
        }

        /// <summary>
        /// 获取指定类型的指定属性
        /// </summary>
        /// <param name="typeKey">类型key</param>
        /// <param name="propName">属性名称</param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string propName)
        {
            return dynamicPropertyRegistry.GetProperty(propName);
        }

        /// <summary>
        /// 获取类型上面的 所有特性
        /// </summary>
        /// <param name="type">类型key</param>
        /// <returns></returns>
        public List<Attribute> GetClasseAttributes()
        {
            return dynamicPropertyRegistry.GetClasseAttributes();
        }

        /// <summary>
        /// 判断指定属性是否存在
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool IsPropertyExist(string propName)
        {
            return dynamicPropertyRegistry.IsPropertyExist(propName);
        }
    }
}
