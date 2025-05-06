using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using GameFramework;
using MiniExcelLibs;
using ThunderFireUITool;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class EntityTool
    {
        private const string COMMON_ENTITY_XLSX = "../Design/Excel/Game/Datas/Entity.xlsx";
        private const string COMMON_ENTITY_ASSET_PATH = "Assets/Res/Entity";

        private const string NO_ADD_ENTITY_SHEET = "~未添加的实体";

        [MenuItem("Game/Entity Tool/Write Entity To Tables")]
        public static void WriteCommonEntityToTables()
        {
            MiniExcel.Insert(COMMON_ENTITY_XLSX, null, NO_ADD_ENTITY_SHEET, ExcelType.XLSX, overwriteSheet: true);
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[]{ COMMON_ENTITY_ASSET_PATH });
            if (guids.Length < 1)
                return;
            List<string> sheetNames = MiniExcel.GetSheetNames(COMMON_ENTITY_XLSX);
            if (sheetNames.Count < 1)
                return;
            List<string> entityAssetNames = new List<string>();
            int maxEntityId = 0;
            foreach (string sheetName in sheetNames)
            {
                if(sheetName.StartsWith('~'))
                    continue;
                var rows = MiniExcel.Query(COMMON_ENTITY_XLSX, sheetName: sheetName, excelType: ExcelType.XLSX,  startCell: "A4");
                foreach (var row in rows)
                {
                    ExpandoObject rowObj = (ExpandoObject)row;
                    dynamic assetName = rowObj.FirstOrDefault(item => item.Key == "E").Value;
                    if (!string.IsNullOrEmpty(assetName))
                    {
                        entityAssetNames.Add(assetName);
                    }
                    dynamic entityId = rowObj.FirstOrDefault(item => item.Key == "B").Value;
                    maxEntityId = Mathf.Max(maxEntityId, (int)entityId);
                }
            }
            List<object> insertList = new List<object>();
            foreach (var guid in guids)
            {
                string entityAsset = AssetDatabase.GUIDToAssetPath(guid);
                string entityAssetName = Utility.Path.GetRegularPath(entityAsset).Replace(COMMON_ENTITY_ASSET_PATH, "");
                if (entityAssetNames.Contains(entityAssetName))
                    continue;
                var value = new
                {
                    Id = ++maxEntityId,
                    CSName = Path.GetFileNameWithoutExtension(entityAsset),
                    Desc = "",
                    AssetName = entityAssetName,
                    EntityGroupName = "Default",
                    Priority = 0,
                };
                insertList.Add(value);
            }
            if (insertList.Count > 0)
            {
                MiniExcel.Insert(COMMON_ENTITY_XLSX, insertList.ToArray(), NO_ADD_ENTITY_SHEET, ExcelType.XLSX, overwriteSheet: true);
                Debug.Log($"未添加的实体写入：{ThunderFireUIToolConfig.TextTablePath}@{NO_ADD_ENTITY_SHEET}！");
            }
            else
            {
                Debug.Log("没有添加的实体！");
            }
        }
    }
}
