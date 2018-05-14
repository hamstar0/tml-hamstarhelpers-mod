using HamstarHelpers.Helpers;
using System;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		[Obsolete( "use ItemAttributeHelpers.HighestVanillaRarity", true )]
		public const int HighestVanillaRarity = 11;
		[Obsolete( "use ItemAttributeHelpers.JunkRarity", true )]
		public const int JunkRarity = -1;
		[Obsolete( "use ItemAttributeHelpers.QuestItemRarity", true )]
		public const int QuestItemRarity = -11;


		////////////////
		
		[Obsolete( "use ItemAttributeHelpers.IsPenetrator", true)]
		public static bool IsPenetrator( Item item ) {
			return ItemAttributeHelpers.IsPenetrator( item );
		}
		
		[Obsolete( "use ItemAttributeHelpers.IsTool", true )]
		public static bool IsTool( Item item ) {
			return ItemAttributeHelpers.IsTool( item );
		}
		
		[Obsolete( "use ItemAttributeHelpers.IsArmor", true )]
		public static bool IsArmor( Item item ) {
			return ItemAttributeHelpers.IsArmor( item );
		}

		[Obsolete( "use ItemAttributeHelpers.IsGrapple", true )]
		public static bool IsGrapple( Item item ) {
			return ItemAttributeHelpers.IsGrapple( item );
		}

		[Obsolete( "use ItemAttributeHelpers.IsYoyo", true )]
		public static bool IsYoyo( Item item ) {
			return ItemAttributeHelpers.IsYoyo( item );
		}

		[Obsolete( "use ItemAttributeHelpers.IsYoyo", true )]
		public static bool IsGameplayRelevant( Item item, bool toys_relevant=false, bool junk_relevant=false ) {
			return ItemAttributeHelpers.IsGameplayRelevant( item, toys_relevant, junk_relevant );
		}

		[Obsolete( "use ItemAttributeHelpers.LooselyAppraise", true )]
		public static float LooselyAppraise( Item item ) {
			return ItemAttributeHelpers.LooselyAppraise( item );
		}
	}
}
