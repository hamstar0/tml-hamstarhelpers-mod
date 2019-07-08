using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the "rarity" attribute of items.
	/// </summary>
	public static partial class ItemRarityAttributeHelpers {
		/// <summary></summary>
		public const int HighestVanillaRarity = 11;
		/// <summary></summary>
		public const int JunkRarity = -1;
		/// <summary></summary>
		public const int QuestItemRarity = -11;


		////////////////
		
		/// <summary>
		/// The color values corresponding to each vanilla rarity type.
		/// </summary>
		public static readonly IReadOnlyDictionary<int, Color> RarityColor = new ReadOnlyDictionary<int, Color>(
			new Dictionary<int, Color> {
				{ -12, new Color( 255, 175, 0 ) },
				{ -11, new Color( 255, 175, 0 ) },
				{ -2, Colors.RarityAmber },
				{ -1, Colors.RarityTrash },
				{ 0, Colors.RarityNormal },
				{ 1, Colors.RarityBlue },
				{ 2, Colors.RarityGreen },
				{ 3, Colors.RarityOrange },
				{ 4, Colors.RarityRed },
				{ 5, Colors.RarityPink },
				{ 6, Colors.RarityPurple },
				{ 7, Colors.RarityLime },
				{ 8, Colors.RarityYellow },
				{ 9, Colors.RarityCyan },
				{ 10, new Color( 255, 40, 100 ) },
				{ 11, new Color( 180, 40, 255 ) }
			}
		);
		/// <summary>
		/// The color name of each vanilla rarity type.
		/// </summary>
		public static readonly IReadOnlyDictionary<int, string> RarityColorText = new ReadOnlyDictionary<int, string>(
			new Dictionary<int, string> {
				{ -12, "Rainbow" },
				{ -11, "Rainbow" },
				{ -2, "Amber" },
				{ -1, "Grey" },
				{ 0, "White" },
				{ 1, "Blue" },
				{ 2, "Green" },
				{ 3, "Orange" },
				{ 4, "Light Red" },
				{ 5, "Pink" },
				{ 6, "Light Purple" },
				{ 7, "Lime" },
				{ 8, "Yellow" },
				{ 9, "Cyan" },
				{ 10, "Red" },
				{ 11, "Purple" }
			}
		);
		/// <summary>
		/// The game context of each vanilla rarity.
		/// </summary>
		public static readonly IReadOnlyDictionary<int, string> RarityLabel = new ReadOnlyDictionary<int, string>(
			new Dictionary<int, string> {
				{ -12, "Expert" },
				{ -11, "Expert" },
				{ -2, "Quest" },
				{ -1, "Junk" },
				{ 0, "Common" },
				{ 1, "Uncommon" },
				{ 2, "Mid Pre-Hardmode" },
				{ 3, "Late Pre-Hardmode" },
				{ 4, "Early Hardmode" },
				{ 5, "Mid Hardmode" },
				{ 6, "Pre Plantera" },
				{ 7, "Post Plantera" },
				{ 8, "Late Hardmode" },
				{ 9, "Post Lunatic" },
				{ 10, "Post Moonlord" },
				{ 11, "End Game" }
			}
		);
	}
}
