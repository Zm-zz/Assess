namespace Launch
{
    /// <summary>
    /// ÓÎÏ·Èë¿Ú¡£
    /// </summary>
    public partial class Main
    {
        public static GlobalComponent Global
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Global = UnityGameFramework.Runtime.GameEntry.GetComponent<GlobalComponent>();
        }
    }
}
