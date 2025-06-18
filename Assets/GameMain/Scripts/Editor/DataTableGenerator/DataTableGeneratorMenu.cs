//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using NPOI.SS.UserModel;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace StarForce.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Game Framework/DataTable/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (string dataTableName in ProcedurePreload.DataTableNames)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        #region Excel To Txt
        public static readonly string[] DataTableNames = new string[]
       {

            "Entity",       // 实体
            "Scene",        // 场景
            "Sound",        // 声音
            "UIForm",       // 界面
            "UISound",      // UI声音
       };

        [MenuItem("Game Framework/DataTable/Excel To Txt", false, 0)]
        private static void GenerateExcelToTxt()
        {
            foreach (string dataTableName in DataTableNames)
            {
                ExportExcel(dataTableName);
            }

            AssetDatabase.Refresh();
        }


        private const string ExcelPath = "GameMain/DataTables/Excels/";
        private const string TxtPath = "GameMain/DataTables/";
        private const string Extension = ".xlsx";

        public static void ExportExcel(string fileName)
        {
            string filePath = "Assets/" + ExcelPath + fileName + Extension;
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //npoi读取并解析excel的sheet数据
                var workbook = WorkbookFactory.Create(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                //遍历sheet数据，转换成项目需要的txt格式
                var txt = ReadDataTable2Txt(sheet);
                //写入到工程目录
                fileName = fileName.Replace("Config", "");
                var savePath = Path.Combine(Application.dataPath, TxtPath, fileName + ".txt");
                if (!File.Exists(savePath))
                {
                    File.Create(savePath).Close(); // 创建文件并关闭
                }
                File.WriteAllText(savePath, txt);
            }
            AssetDatabase.Refresh();
        }

        //第一行为表头，第二行为字段名 第三行为类型
        static string ReadDataTable2Txt(ISheet sheet)
        {
            // 获取表格有多少列 
            int columns = sheet.GetRow(3).LastCellNum;
            // 获取表格有多少行 
            int rows = sheet.LastRowNum + 1;

            StringBuilder sb = new StringBuilder();
            sb.Append("#"); //读取TableName
            for (int j = 0; j < columns; j++)
            {
                var cell = sheet.GetRow(0)?.GetCell(j);
                if (cell != null)
                    sb.Append("\t" + cell);
            }

            sb.Append("\n#"); //读取FiledName
            for (int j = 0; j < columns; j++)
            {
                var cell = sheet.GetRow(2)?.GetCell(j);
                if (cell != null)
                    sb.Append("\t" + cell);
            }

            sb.Append("\n#"); //读取FiledType
            for (int j = 0; j < columns; j++)
            {
                sb.Append("\t");
                var cell = sheet.GetRow(3)?.GetCell(j);
                if (cell != null)
                    sb.Append(cell);
            }

            sb.Append("\n#"); //读取FiledDesc
            for (int j = 0; j < columns; j++)
            {
                sb.Append("\t");
                var cell = sheet.GetRow(1)?.GetCell(j);
                if (cell != null)
                {
                    var desc = cell.ToString();
                    // desc = ConfigEditorUtility.ClearDesc(desc);
                    sb.Append(desc);
                }
            }

            //读取第4行后的表数据
            for (int i = 4; i < rows; i++)
            {
                var first = sheet.GetRow(i)?.GetCell(0);
                if (first == null) continue;

                sb.Append("\n");
                for (int j = 0; j < columns; j++)
                {
                    sb.Append("\t");
                    var cell = sheet.GetRow(i)?.GetCell(j);
                    if (cell != null)
                    {
                        sb.Append(cell);
                    }
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
