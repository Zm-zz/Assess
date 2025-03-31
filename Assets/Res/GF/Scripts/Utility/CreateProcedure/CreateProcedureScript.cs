using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class CreateProcedureScript
{
    //�ű�ģ��·��
    private const string TemplateScriptPath = "Assets/Res/GF/Scripts/Utility/CreateProcedure/Procedure_.cs.txt";

    //�˵���
    [MenuItem("Assets/Create/Procedure/ProcedureScript", false, 1)]
    static void CreateScript()
    {
        string path = "Assets";
        foreach (UnityEngine.Object item in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(item);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateProcedureScriptAsset>(),
        path + "/Procedure_.cs",
        null, TemplateScriptPath);
    }
}


class CreateProcedureScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string newScriptPath, string templatePath)
    {
        UnityEngine.Object obj = CreateTemplateScriptAsset(newScriptPath, templatePath);
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }

    public static UnityEngine.Object CreateTemplateScriptAsset(string newScriptPath, string templatePath)
    {
        string fullPath = Path.GetFullPath(newScriptPath);
        StreamReader streamReader = new StreamReader(templatePath);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newScriptPath);
        //�滻ģ����ļ���
        text = Regex.Replace(text, "Procedure_", fileNameWithoutExtension);
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(newScriptPath);
        return AssetDatabase.LoadAssetAtPath(newScriptPath, typeof(UnityEngine.Object));
    }

}