using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GeneralUtility
{
    public static void ChangeState(this ProcedureBase procedure, string stateName, IFsm<IProcedureManager> procedureOwner, string assemblyName = "Assembly-CSharp")
    {
        // string className = $"Launch.{stateName}, {assemblyName}";
        string className = $"{stateName}, {assemblyName}";
        Type procedureType = Type.GetType(className);

        if (procedureType == null)
        {
            Debug.LogError($"未找到类: {className}");
            return;
        }

        // 获取当前类型的所有方法
        var currentType = procedure.GetType();
        // Debug.Log($"<color=green>当前类型:</color> {currentType.FullName}");

        var allMethods = currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        var changeStateMethods = allMethods.Where(m => m.Name == "ChangeState").ToList();

        // 精确查找目标泛型方法
        MethodInfo targetMethod = changeStateMethods.FirstOrDefault(m =>
        {
            if (!m.IsGenericMethod) return false;

            var parameters = m.GetParameters();
            if (parameters.Length != 1) return false;

            // 关键修改：检查参数类型是否为IFsm<T>（考虑泛型参数）
            var paramType = parameters[0].ParameterType;
            return paramType.IsGenericType &&
                   paramType.GetGenericTypeDefinition() == typeof(IFsm<>);
        });

        // 绑定泛型参数并调用
        try
        {
            MethodInfo boundMethod = targetMethod.MakeGenericMethod(procedureType);
            boundMethod.Invoke(procedure, new[] { procedureOwner });
            // Debug.Log($"<color=green>当前类型:</color> {procedureType.FullName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"调用失败: {e.GetType().Name} - {e.Message}");
            Debug.LogError($"StackTrace: {e.StackTrace}");
        }
    }
}
