using GameFramework;

namespace Launch
{
    public static class UgionAssetUtility
    {
        public static string GetAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Ugion/Res/Prefabs/UI/{0}.prefab", assetName);
        }
    }
}
