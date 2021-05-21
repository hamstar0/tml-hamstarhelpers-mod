using System;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Classes.DataStructures;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to item identification.
	/// </summary>
	public partial class ItemGroupIdentityHelpers {
		/// <summary>
		/// Gets all vanilla item types of a given "container context".
		/// </summary>
		/// <param name="context">Contexts include: bossBag, crate, herbBag, goodieBag, lockBox, present</param>
		/// <returns></returns>
		public static int[] GetVanillaContainerItemTypes( string context ) {
			if( context == "bossBag" ) {
				return new int[] { 3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327, 3328, 3329, 3330, 3331,
					3332, 3860, 3862, 3861 };
			}
			if( context == "crate" ) {
				return new int[] { 2334, 2335, 2336,
					3203, 3204, 3205, 3206, 3207, 3208 };
			}
			if( context == "herbBag" ) {
				return new int[] { ItemID.HerbBag };
			}
			if( context == "goodieBag" ) {
				return new int[] { ItemID.GoodieBag };
			}
			if( context == "lockBox" ) {
				return new int[] { ItemID.LockBox };
			}
			if( context == "present" ) {
				return new int[] { ItemID.Present };
			}
			return new int[] { };
		}
	}
}
