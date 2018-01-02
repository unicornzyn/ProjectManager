/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：Reflex.cs
// 文件功能描述：反射操作类
//----------------------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Common
{
    /// <summary>
    /// 反射操作类
    /// </summary>
    public class ReflexST
    {
        /// <summary>
        /// 作者：yjq
        /// 时间：2011-03-17
        /// 功能：用反射的方法执行
        /// 注意：此方法为简单版的 不支持加参数的反射执行
        /// </summary>
        /// <param name="functionFullName">方法名称（格式：命名空间名称.类名.方法名）</param>
        /// <returns></returns>
        public static string StartMethod(string functionFullName,object[] ob)
        {
            //命名空间名称            
            string assmblyName = functionFullName.Substring(0, functionFullName.IndexOf("."));

            //类名
            int lastPointPosition = functionFullName.LastIndexOf(".");
            string classFullName = functionFullName.Substring(0, lastPointPosition);

            //方法名称
            int fullNameLength = functionFullName.Length;
            string functionName = functionFullName.Substring(lastPointPosition + 1, fullNameLength - lastPointPosition - 1);
           
            //反射到类
            Type myclass = System.Type.GetType(classFullName + "," + assmblyName);

           
            string result = string.Empty;

            //执行方法
            MethodInfo mi = myclass.GetMethod(functionName);

            //执行结果
            result = mi.Invoke(null, ob).ToString();
            
            return result;
        }
    }



}
