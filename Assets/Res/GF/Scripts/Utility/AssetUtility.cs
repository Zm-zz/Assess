using GameFramework;

public static class AssetUtility
{
    public static string GetUIFormAsset(string assetName)
    {
        return Utility.Text.Format("Assets/Res/GF/Prefabs/UIForms/{0}.prefab", assetName);
    }
}
