namespace ExampleProject
{
    public static class Paths
    {
        /// <summary>
        /// Wherever your Assets/Tooltips folder is located (likely in your application directory, so this can likely be left alone.
        /// </summary>
        public const string TooltipFills = "Assets/Tooltips";

        /// <summary>
        /// Set this to the exported BLP BlizzardInterfaceArt folder (should be in your WoW folder)
        /// </summary>
        public const string BLP_BlizzardInterfaceArt = null;
        /// <summary>
        /// Set this to where you want your BLP files to be converted to PNGs, as well as where you want the PNGs to be pulled from.
        /// </summary>
        public const string BlizzardInterfaceArt = null;
        public const string Interface = BlizzardInterfaceArt + "/Interface";
        public const string Icons = Interface + "/ICONS";
        public const string Tooltips = Interface + "/Tooltips";

        /// <summary>
        /// Set this to where you want your data files to be stored (icon usage files and weapon types json)
        /// </summary>
        public const string Data = "export";
        public const string IconUsage = Data + "/Icon Usage";
        public const string WeaponTypes = Data + "/weapon-types.json";

        /// <summary>
        /// Set this to where your files exported from wow.tools are located.
        /// </summary>
        public const string DatabaseDirectory = null;
    }
}
