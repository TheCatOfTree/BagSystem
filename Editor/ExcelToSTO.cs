using System.IO;
using Codice.Client.BaseCommands.Differences;
using Common;
using NUnit.Framework.Internal.Filters;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ExcelToSTO : EditorWindow
    {
        private static string _folderPath;
        private static string _excelPath;

        private static ItemDatabase _itemDB;
        private static ItemData _item;
        private static string _result;
        private static int _fileCount = 0;

        ExcelToSTO()
        {
            this.titleContent = new GUIContent("Excel to ScripTableObject");
        }

        [MenuItem("Tools/ExcelToSTO")]
        static void showWindow()
        {
            EditorWindow.GetWindow(typeof(ExcelToSTO));
            
            _excelPath = $@"{Application.streamingAssetsPath}\Excel\Item.xlsx";
            _itemDB = Resources.Load<ItemDatabase>("ItemDatabase");
        }

        void Init()
        {
            _result = " ";
            _fileCount = 0;
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Excel to ScripTableObject");
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            _excelPath = EditorGUILayout.TextField("Excel File Path", _excelPath);
            GUILayout.Space(5);
            _itemDB = (ItemDatabase)EditorGUILayout.ObjectField("Item Database", _itemDB, typeof(ItemDatabase), false);
            GUILayout.Space(5);
            GUILayout.Label("Result: " + _result);
            GUILayout.Space(5);
            GUILayout.Label("Switch File Count : " + _fileCount);
            if (GUILayout.Button("确定"))
            {
                Init();
                LoadItemsFromExcel();
            }
        }


        private void LoadItemsFromExcel()
        {
            if (_itemDB == null)
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(_excelPath);
            if (!fileInfo.Exists)
            {
                return;
            }

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;

                _itemDB.items.Clear();
                for (int row = 2; row <= rowCount; row++) // 从第二行开始读
                {
                    int itemID = int.Parse(worksheet.Cells[row, 1].Text);
                    string name = worksheet.Cells[row, 2].Text;
                    int maxCount = int.Parse(worksheet.Cells[row, 3].Text);
                    string canCreate = worksheet.Cells[row, 4].Text;
                    string text = worksheet.Cells[row, 5].Text;
                    string getMethod = worksheet.Cells[row, 6].Text;
                    string recipe = worksheet.Cells[row, 7].Text;
                    string type = worksheet.Cells[row, 8].Text;
                    string imagePath = worksheet.Cells[row, 9].Text;
                    
                    ItemData newItem = CreateInstance<ItemData>();//创建保存
                    newItem.itemID = itemID;
                    newItem.itemName = name;
                    newItem.maxCount = maxCount;
                    newItem.canCreate = canCreate == "Yes" ? true : false;
                    newItem.text = text;
                    newItem.getMethod = getMethod == "制造台" ? Method.Create : Method.Enemy;
                    newItem.recipe = recipe;
                    newItem.type = type == "Weapon" ? Type.Weapon : Type.Materials;
                    newItem.imagePath = imagePath;
                    
                    // 将物品添加到STO中
                    _itemDB.items.Add(newItem);

                    // 保存为独立的STO资源
                    string assetPath = $"Assets/Resources/Items/{name}.asset";
                    AssetDatabase.CreateAsset(newItem, assetPath);
                }
            }

            SaveItemDatabase();
            Debug.Log("Items loaded successfully!");
        }

        private void SaveItemDatabase()
        {
            EditorUtility.SetDirty(_itemDB);
            AssetDatabase.SaveAssets();
        }
    }
}