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
            Debug.LogError($"δ�ҵ���: {className}");
            return;
        }

        // ��ȡ��ǰ���͵����з���
        var currentType = procedure.GetType();
        // Debug.Log($"<color=green>��ǰ����:</color> {currentType.FullName}");

        var allMethods = currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        var changeStateMethods = allMethods.Where(m => m.Name == "ChangeState").ToList();

        // ��ȷ����Ŀ�귺�ͷ���
        MethodInfo targetMethod = changeStateMethods.FirstOrDefault(m =>
        {
            if (!m.IsGenericMethod) return false;

            var parameters = m.GetParameters();
            if (parameters.Length != 1) return false;

            // �ؼ��޸ģ������������Ƿ�ΪIFsm<T>�����Ƿ��Ͳ�����
            var paramType = parameters[0].ParameterType;
            return paramType.IsGenericType &&
                   paramType.GetGenericTypeDefinition() == typeof(IFsm<>);
        });

        // �󶨷��Ͳ���������
        try
        {
            MethodInfo boundMethod = targetMethod.MakeGenericMethod(procedureType);
            boundMethod.Invoke(procedure, new[] { procedureOwner });
            // Debug.Log($"<color=green>��ǰ����:</color> {procedureType.FullName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"����ʧ��: {e.GetType().Name} - {e.Message}");
            Debug.LogError($"StackTrace: {e.StackTrace}");
        }
    }
}
