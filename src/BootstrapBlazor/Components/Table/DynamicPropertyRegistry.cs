// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 动态属性注册中心
    /// </summary>
    public class DynamicPropertyRegistry
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
        T GetValue<T>(string propName);

        object GetValue(string propName);

        void SetValue(string propName, object value);

        /// <summary>
        /// 获取动态Builder对象
        /// </summary>
        DynamicObjectBuilder GetBuilder();

        /// <summary>
        /// 根据属性名，判断当前属性 是否是动态属性
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        bool IsDynamicProperty(string propName);
    }

    public abstract class DynamicBase : IDynamicType
    {
        private ConcurrentDictionary<string, object?> propDic = new();

        private ConcurrentDictionary<string, (Func<IDynamicType,object> Get, Action<IDynamicType,object> Set)> longPathPropDic = new();
        /// <summary>
        /// 根据属性名，获取属性值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object? GetValue(string propName)
        {
            if (longPathPropDic.ContainsKey(propName))
            {
                return longPathPropDic[propName].Get(this);
            }
            //如果属性不存在，则添加属性默认值
            //当在运行时动态添加属性时，就会出现属性不存在的情况
            if (!propDic.ContainsKey(propName))
            {
                propDic[propName] = GetBuilder().GetPropertyDefaultValue(propName);
            }
            return propDic[propName];
        }

        /// <summary>
        /// 添加自定义GetSet
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="getSet"></param>
        /// <returns></returns>
        protected bool AddCustomGetSet(string propName, Func<IDynamicType,object> Get, Action<IDynamicType,object> Set)
        {
            return longPathPropDic.TryAdd(propName, (Get,Set));
        }
        /// <summary>
        /// 设置指定属性为指定值
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public void SetValue(string propName, object value)
        {
            if (longPathPropDic.ContainsKey(propName))
            {
                longPathPropDic[propName].Set(this,value);
            }
            //这里需要类型转换
            propDic[propName] = value;
        }

        public abstract DynamicObjectBuilder GetBuilder();


        public virtual bool IsDynamicProperty(string propName)
        {
            return true;
        }

        /// <summary>
        /// 此方法只会克隆已注册的动态属性，动态属性是指在
        /// DynamicObjectBuider中注册的属性
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            var obj = New();
            var props = TypeInfoHelper.GetProperties(this);
            foreach (var p in props)
            {
                obj.SetValue(p.Name, GetValue(p.Name));
            }
            return obj;
        }

        public abstract IDynamicType New();

        public T GetValue<T>(string propName)
        {
            return (T)GetValue(propName);
        }
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
        /// 根据属性名，获取属性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static PropertyInfo? GetProperty(object model, string propName)
        {
            if (model is IDynamicType dType)
            {
                return dType.GetBuilder().GetProperty(propName);
            }
            else
            {
                return model.GetType().GetProperties().FirstOrDefault(v => v.Name == propName);
            }
        }

        public static object? GetPropertyValue(object model, string propName)
        {
            if (model is IDynamicType dType)
            {
                //if (dType.GetBuilder())
                //{

                //}
                return dType.GetValue(propName);
            }
            else
            {
                return model.GetType().GetProperties().FirstOrDefault(v => v.Name == propName).GetValue(model);
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

        /// <summary>
        /// 解析表达式 中的 属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessor"></param>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        public static void ParsePropertyName(LambdaExpression exp, out string fieldName, out string fieldFullPath, out Type propType)
        {
            var accessorBody = exp.Body;
            fieldName = null;
            propType = null;
            fieldFullPath = null;
            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                //if (accessorBody is MethodCallExpression methodCall)
                //{
                //    var constExp = (methodCall.Arguments[0] as ConstantExpression);
                //    var arg = constExp.Value.ToString();
                //    fieldName = arg;
                //    propType = constExp.Type;
                //}
                //else
                //{
                //    throw new NotSupportedException($"不支持这种表达式，{accessorBody.GetType().FullName}");
                //}
                throw new NotSupportedException($"不支持这种表达式，只支持简单的属性访问表达式，如A.B.C");
            }
            else
            {
                //全路径字段名
                fieldFullPath = memberExpression.ToString();
                var pIndex = fieldFullPath.IndexOf('.');
                fieldFullPath = fieldFullPath.Substring(pIndex + 1);

                //最后一段路径
                fieldName = memberExpression.Member.Name;
                if (memberExpression.Member is PropertyInfo info)
                {
                    propType = info.PropertyType;
                }
                else if (memberExpression.Member is FieldInfo info2)
                {
                    propType = info2.FieldType;
                }

            }
        }

        public static void ParseModelAndProperty(LambdaExpression exp, out object model, out string fieldName)
        {
            var accessorBody = exp.Body;
            model = null;
            fieldName = null;
            // Unwrap casts to object
            if (accessorBody is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                if (accessorBody is MethodCallExpression methodCall)
                {
                    var arg = (methodCall.Arguments[0] as ConstantExpression).Value.ToString();
                    var obj = methodCall.Object;
                    model = GetModelFromExp(obj);
                    fieldName = arg;
                }
            }
            else
            {
                fieldName = memberExpression.Member.Name;

                if (memberExpression.Expression is ConstantExpression constantExpression)
                {
                    if (constantExpression.Value is null)
                    {
                        throw new ArgumentException("The provided expression must evaluate to a non-null value.");
                    }
                    model = constantExpression.Value;
                }
                else if (memberExpression.Expression != null)
                {
                    model = GetModelFromExp(memberExpression.Expression);
                }
                else
                {
                    throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. FieldIdentifier only supports simple member accessors (fields, properties) of an object.");
                }
            }
        }

        public static object GetModelFromExp(Expression exp)
        {
            var modelLambda = Expression.Lambda(exp);
            var modelLambdaCompiled = (Func<object?>)modelLambda.Compile();
            var result = modelLambdaCompiled();
            if (result is null)
            {
                throw new ArgumentException("The provided expression must evaluate to a non-null value.");
            }
            return result;
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
            if (attributes == null)
            {
                this.attributes = Array.Empty<Attribute>();
            }
            else
            {
                this.attributes = attributes;
            }
        }

        public LambdaExpression GetValueExpression { get; set; }

        public LambdaExpression SetValueExpression { get; set; }
        public DynamicPropertyInfo(LambdaExpression exp, Attribute[] attributes)
        {
            TypeInfoHelper.ParsePropertyName(exp, out string fieldName, out string fullPath, out Type type);
            this.name = fullPath;
            this.propertyType = type;
            if (attributes == null)
            {
                this.attributes = Array.Empty<Attribute>();
            }
            else
            {
                this.attributes = attributes;
            }
            GetValueExpression = exp;
            //ValueExpression.Compile();
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
        public void SetDefaultValues<TType>(TType obj) where TType : IDynamicType, new()
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

    /// <summary>
    /// 属性建造器
    /// </summary>
    //public class PropertyBuilder
    //{
    //    private readonly DynamicPropertyRegistry dynamicPropertyRegistry;

    //    public PropertyBuilder(DynamicPropertyRegistry dynamicPropertyRegistry)
    //    {
    //        this.dynamicPropertyRegistry = dynamicPropertyRegistry;
    //    }
    //    /// <summary>
    //    /// 添加动态属性定义
    //    /// </summary>
    //    /// <param name="name">属性名称</param>
    //    /// <param name="propType">属性类型</param>
    //    /// <param name="attributes">属性的Attribute</param>
    //    public PropertyBuilder AddProperty(string name, Type propType, Attribute[] attributes)
    //    {
    //        dynamicPropertyRegistry.AddProperty(new DynamicPropertyInfo(name, propType, attributes));
    //        return this;
    //    }

    //    public PropertyBuilder AddProperty(LambdaExpression exp, Attribute[] attributes)
    //    {
    //        dynamicPropertyRegistry.AddProperty(new DynamicPropertyInfo(exp, attributes));
    //        return this;
    //    }
    //}

    //public class PropertyBuilder<TModel> : PropertyBuilder
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="objectBuilder"></param>
    //    public PropertyBuilder(DynamicPropertyRegistry registry) : base(registry)
    //    {

    //    }
    //    /// <summary>
    //    /// 添加动态属性,表达式的写法为 u=>u.Address.Name这种 属性表达式，不能调用方法
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="exp"></param>
    //    /// <param name="attributes"></param>
    //    /// <returns></returns>
    //    public PropertyBuilder<TModel> AddProperty<TProperty>(Expression<Func<TModel, TProperty>> exp, Attribute[] attributes)
    //    {

    //        base.AddProperty(exp, attributes);
    //        return this;
    //    }
    //}
}
