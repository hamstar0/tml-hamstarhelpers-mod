using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player's appearance.
	/// </summary>
	public class PlayerAppearanceHelpers {
		/// <summary>
		/// Indicates if player has something on their back.
		/// </summary>
		/// <param name="plr"></param>
		/// <returns></returns>
		public static bool IsPlayerBackSlotFilled( Player plr ) {
			if( plr.wings != 0 && plr.velocity.Y != 0f ) {
				return true;
			}
			switch( plr.HeldItem.type ) {
			//case ItemID.Flamethrower: ?
			case ItemID.Clentaminator:
			case ItemID.LeafBlower:
			case ItemID.HeatRay:
			case ItemID.EldMelter:
				return true;
			}

			return plr.turtleArmor || plr.body == 106 || plr.body == 170;
		}


		/*public static void GetSwingableHeldItemPosition(		// UNTESTED
					Vector2 holderPos,
					float holderOffsetHitbox,   //plr.mount.PlayerOffsetHitbox
					int holderWidth,
					int holderHeight,
					int holderDir,
					int gravDir,
					Texture2D itemTex,
					int itemAnim,
					int itemAnimMax,
					out Vector2 itemPos,
					out float itemRot ) {
			if( (double)itemAnim < (double)itemAnimMax * 0.333 ) {
				float itemFatnessOffset = 10f;
				if( itemTex.Width > 32 ) {
					itemFatnessOffset = 14f;
				} else if( itemTex.Width >= 52 ) {
					itemFatnessOffset = 24f;
				} else if( itemTex.Width >= 64 ) {
					itemFatnessOffset = 28f;
				} else if( itemTex.Width >= 92 ) {
					itemFatnessOffset = 38f;
				}

				itemPos.X = holderPos.X
					+ (float)holderWidth * 0.5f
					+ ( (float)itemTex.Width * 0.5f - itemFatnessOffset ) * (float)holderDir;
				itemPos.Y = holderPos.Y + 24f + holderOffsetHitbox;
			} else if( (double)itemAnim < (double)itemAnimMax * 0.666 ) {
				float itemFatnessOffset = 10f;
				if( itemTex.Width > 32 ) {
					itemFatnessOffset = 18f;
				} else if( itemTex.Width >= 52 ) {
					itemFatnessOffset = 24f;
				} else if( itemTex.Width >= 64 ) {
					itemFatnessOffset = 28f;
				} else if( itemTex.Width >= 92 ) {
					itemFatnessOffset = 38f;
				}
				itemPos.X = holderPos.X
					+ (float)holderWidth * 0.5f
					+ ( (float)itemTex.Width * 0.5f - itemFatnessOffset ) * (float)holderDir;

				itemFatnessOffset = 10f;
				if( itemTex.Height > 32 ) {
					itemFatnessOffset = 8f;
				} else if( itemTex.Height > 52 ) {
					itemFatnessOffset = 12f;
				} else if( itemTex.Height > 64 ) {
					itemFatnessOffset = 14f;
				}

				itemPos.Y = holderPos.Y + itemFatnessOffset + holderOffsetHitbox;
			} else {
				float itemFatnessOffset = 6f;
				if( itemTex.Width > 32 ) {
					itemFatnessOffset = 14f;
				} else if( itemTex.Width >= 48 ) {
					itemFatnessOffset = 18f;
				} else if( itemTex.Width >= 52 ) {
					itemFatnessOffset = 24f;
				} else if( itemTex.Width >= 64 ) {
					itemFatnessOffset = 28f;
				} else if( itemTex.Width >= 92 ) {
					itemFatnessOffset = 38f;
				}
				itemPos.X = holderPos.X
					+ ( (float)holderWidth * 0.5f )
					- ( (float)itemTex.Width * 0.5f - itemFatnessOffset ) * (float)holderDir;

				itemFatnessOffset = 10f;
				if( itemTex.Height > 32 ) {
					itemFatnessOffset = 10f;
				}
				if( itemTex.Height > 52 ) {
					itemFatnessOffset = 12f;
				}
				if( itemTex.Height > 64 ) {
					itemFatnessOffset = 14f;
				}

				itemPos.Y = holderPos.Y + itemFatnessOffset + holderOffsetHitbox;
			}

			itemRot = ( ( ( (float)itemAnim / (float)itemAnimMax ) - 0.5f ) * (float)holderDir * -3.5f )
				- ( (float)holderDir * 0.3f );

			if( gravDir == -1f ) {
				itemRot = -itemRot;
				itemPos.Y = holderPos.Y + (float)holderHeight + ( holderPos.Y - itemPos.Y );
			}
		}*/
	}
}
