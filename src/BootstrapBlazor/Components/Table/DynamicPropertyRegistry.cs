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
        private static Dictionary<string, Dictionary<string, PropertyInfo>> typePropDic = new();
        private static Dictionary<string, List<Attribute>> classAttrDic = new();
        private static Dictionary<Type, string> typeKeyDic = new();

        #region 注册
        /// <summary>
        /// 给指定类型，添加动态属性
        /// </summary>
        /// <param name="typeKey"></param>
        /// <param name="info"></param>

        public static void AddProperty(string typeKey, PropertyInfo info)
        {
            if (!typePropDic.ContainsKey(typeKey))
            {
                typePropDic[typeKey] = new();
            }
            typePropDic[typeKey][info.Name] = info;
        }

        /// <summary>
        /// 给指定类型，删除动态属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        public static void RemoveProperty(string typeKey, PropertyInfo info)
        {
            if (!typePropDic.ContainsKey(typeKey))
            {
                return;
            }
            if (typePropDic[typeKey].ContainsKey(info.Name))
            {
                typePropDic[typeKey].Remove(info.Name);
            }
            else
            {
                throw new NotFoundPropertyException(typeKey, info.Name);
            }

        }
        /// <summary>
        /// 注册 AutoGenerateClassAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        public static void AddClassAttribute(string typeKey, Attribute info)
        {
            if (!classAttrDic.ContainsKey(typeKey))
            {
                classAttrDic[typeKey] = new List<Attribute>();
            }
            classAttrDic[typeKey].Add(info);
        }

        public static void RemoveClassAttribute(string typeKey, Attribute info)
        {
            if (!classAttrDic.ContainsKey(typeKey))
            {
                return;
            }
            classAttrDic[typeKey].Remove(info);
        }

        #endregion

        #region 使用
        /// <summary>
        /// 获取指定了类型的所有属性信息
        /// </summary>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(string typeKey)
        {
            return typePropDic[typeKey].Values.ToArray();
        }

        /// <summary>
        /// 获取指定类型的指定属性
        /// </summary>
        /// <param name="typeKey">类型key</param>
        /// <param name="propName">属性名称</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(string typeKey, string propName)
        {
            return typePropDic[typeKey][propName];
        }

        /// <summary>
        /// 获取类型上面的 所有特性
        /// </summary>
        /// <param name="type">类型key</param>
        /// <returns></returns>
        public static List<Attribute> GetClasseAttributes(string typeKey)
        {
            return classAttrDic[typeKey];
        }

        /// <summary>
        /// 注册类型的 字符串唯一标识
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeKey"></param>
        public static void RegistTypeKey(Type type, string typeKey)
        {
            typeKeyDic[type] = typeKey;
        }
        /// <summary>
        /// 获取类型对应的字符串Key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeKey(Type type)
        {
            return typeKeyDic[type];
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

    public abstract class BaseDynamicType : IDynamicType
    {
        /// <summary>
        /// 创建当前对象的新实例，Clone方法会调用此方法
        /// </summary>
        /// <returns></returns>
        public abstract BaseDynamicType New();

        /// <summary>
        /// 创建当前对象的副本
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var obj = New();
            var props = TypeInfoHelper.GetProperties(this);

            foreach (var p in props)
            {
                obj.SetValue(p.Name, this.GetValue(p.Name));
            }
            return obj;
            
        }

        public abstract void Configuration(DynamicObjectBuilder builder);
        
        /// <summary>
        /// 获取类型Key
        /// </summary>
        /// <returns></returns>
        public string GetTypeKey()
        {
            return $"{this.GetType().FullName}_{this.GetHashCode()}";
        }

        /// <summary>
        /// 根据属性名，获取属性值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>

        public abstract object? GetValue(string propName);

        /// <summary>
        /// 根据属性名，设置属性名
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public abstract void SetValue(string propName, object value);
       
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
        /// 获取类型对应的Key
        /// </summary>
        /// <returns></returns>
        string GetTypeKey();

        /// <summary>
        /// 配置动态属性
        /// </summary>
        //void Configuration(DynamicObjectBuilder builder);
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
                var typeKey = DynamicPropertyRegistry.GetTypeKey(type);
                return DynamicPropertyRegistry.GetClasseAttributes(typeKey).OfType<T>().FirstOrDefault();
            }
            else
            {
                return type.GetCustomAttribute<T>();
            }
        }
        public static PropertyInfo[] GetProperties(object model)
        {
            if (model is IDynamicType dType)
            {
                return DynamicPropertyRegistry.GetProperties(dType.GetTypeKey());
            }
            else
            {
                return model.GetType().GetProperties();
            }
        }

        public static PropertyInfo[] GetProperties(Type type)
        {
            if (IsDynamicType(type))
            {
                var typeKey = DynamicPropertyRegistry.GetTypeKey(type);
                return DynamicPropertyRegistry.GetProperties(typeKey);
            }
            else
            {
                return type.GetProperties();
            }
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
            this.attributes = attributes;
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
        private string typeKey;
        private Type type;

        /// <summary>
        /// 创建构造器实例
        /// </summary>
        /// <param name="type">动态类型的Type</param>
        /// <param name="typeKey">动态类型的字符串唯一标识</param>
        public DynamicObjectBuilder(Type type, string typeKey)
        {
            this.typeKey = typeKey;
            this.type = type;
            DynamicPropertyRegistry.RegistTypeKey(type, typeKey);
        }

        /// <summary>
        /// 添加动态属性定义
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="propType">属性类型</param>
        /// <param name="attributes">属性的Attribute</param>
        public DynamicObjectBuilder AddProperty(string name, Type propType, Attribute[] attributes)
        {
            DynamicPropertyRegistry.AddProperty(typeKey, new DynamicPropertyInfo(name, propType, attributes));
            return this;
        }

        /// <summary>
        /// 移除指定属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public DynamicObjectBuilder RemoveProperty(string name)
        {
            var property = DynamicPropertyRegistry.GetProperty(typeKey, name);
            DynamicPropertyRegistry.RemoveProperty(typeKey, property);
            return this;
        }

        /// <summary>
        /// 给类型添加Attribute
        /// </summary>
        /// <param name="attribute">类型的Attribute</param>
        /// <returns></returns>
        public DynamicObjectBuilder AddClassAttribute(Attribute attribute)
        {
            DynamicPropertyRegistry.AddClassAttribute(typeKey, attribute);
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
            var props = DynamicPropertyRegistry.GetProperties(typeKey);
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
            var targetType = DynamicPropertyRegistry.GetProperty(typeKey, propName).PropertyType;
            return GetDefaultValue(targetType);
        }

        private static object? GetDefaultValue(Type targetType)
        {
            var defaultValue = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            return defaultValue;
        }
    }
}
