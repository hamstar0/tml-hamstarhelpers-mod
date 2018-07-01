using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public static partial class ItemAttributeHelpers {
		public const int HighestVanillaRarity = 11;
		public const int JunkRarity = -1;
		public const int QuestItemRarity = -11;

		////////////////
		
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


		////////////////

		[Obsolete( "use ItemAttributeHelpers.RarityColor", false)]
		public static Color GetRarityColor( int rarity ) {
			switch( rarity ) {
			case -2:
				return Colors.RarityAmber;
				//return new Color( 255, 175, 0 );
			case -1:
				return Colors.RarityTrash;
				//return new Color( 130, 130, 130 );
			case 0:
				return Colors.RarityNormal;
				//return Colors.RarityNormal;
			case 1:
				return Colors.RarityBlue;
				//return new Color( 150, 150, 255 );
			case 2:
				return Colors.RarityGreen;
				//return new Color( 150, 255, 150 );
			case 3:
				return Colors.RarityOrange;
				//return new Color( 255, 200, 150 );
			case 4:
				return Colors.RarityRed;
				//return new Color( 255, 150, 150 );
			case 5:
				return Colors.RarityPink;
				//return new Color( 255, 150, 255 );
			case 6:
				return Colors.RarityPurple;
				//return new Color( 210, 160, 255 );
			case 7:
				return Colors.RarityLime;
				//return new Color( 150, 255, 10 );
			case 8:
				return Colors.RarityYellow;
				//return new Color( 255, 255, 10 );
			case 9:
				return Colors.RarityCyan;
				//return new Color( 5, 200, 255 );
			case 10:
				return new Color( 255, 40, 100 );
			case 11:
				return new Color( 180, 40, 255 );
			case -11:
			case -12:
				return new Color( 255, 175, 0 );
			default:
				return new Color( Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor );
			}
		}
	}
}
