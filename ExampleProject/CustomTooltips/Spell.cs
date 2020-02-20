using Awv.Games.Graphics;

namespace ExampleProject.CustomTooltips
{
    public class Spell : ISpell
    {
        #region Properties
        public IGraphic Icon { get; set; }
        public string Name { get; set; }
        public int ResourceCost { get; set; }
        public string Resource { get; set; }
        /// <summary>
        /// In yards
        /// </summary>
        public int Range { get; set; }
        /// <summary>
        ///  In seconds
        /// </summary>
        public decimal CastTime { get; set; }
        public string Description { get; set; }
        #endregion
        #region ISpell Accessors
        public IGraphic GetIcon() => Icon;
        public string GetName() => Name;
        public int GetResourceCost() => ResourceCost;
        public string GetResource() => Resource;
        public int GetRange() => Range;
        public decimal GetCastTime() => CastTime;
        public string GetDescription() => Description;
        #endregion
    }
}
