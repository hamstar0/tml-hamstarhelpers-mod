using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/*class HamstarHelpersTile : GlobalTile {
		public override int[] AdjTiles( int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnAdjTiles( type );
			return base.AdjTiles( type );
		}
		public override void AnimateTile() {
			HamstarHelpersMod.Instance.TileEvents.OnAnimateTile();
			base.AnimateTile();
		}
		public override bool AutoSelect( int i, int j, int type, Item item ) {
			HamstarHelpersMod.Instance.TileEvents.OnAutoSelect( i, j, type, item );
			return base.AutoSelect( i, j, type, item );
		}
		public override bool CanExplode( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnCanExplode( i, j, type );
			return base.CanExplode( i, j, type );
		}
		public override bool CanKillTile( int i, int j, int type, ref bool blockDamaged ) {
			HamstarHelpersMod.Instance.TileEvents.OnCanKillTile( i, j, type, ref blockDamaged );
			return base.CanKillTile( i, j, type, ref blockDamaged );
		}
		public override bool CanPlace( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnCanPlace( i, j, type );
			return base.CanPlace( i, j, type );
		}
		public override void ChangeWaterfallStyle( int type, ref int style ) {
			HamstarHelpersMod.Instance.TileEvents.OnChangeWaterfallStyle( type, ref style );
			base.ChangeWaterfallStyle( type, ref style );
		}
		public override bool CreateDust( int i, int j, int type, ref int dustType ) {
			HamstarHelpersMod.Instance.TileEvents.OnCreateDust( i, j, type, ref dustType );
			return base.CreateDust( i, j, type, ref dustType );
		}
		public override bool Dangersense( int i, int j, int type, Player player ) {
			HamstarHelpersMod.Instance.TileEvents.OnDangersense( i, j, type, player );
			return base.Dangersense( i, j, type, player );
		}
		public override void DrawEffects( int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex ) {
			HamstarHelpersMod.Instance.TileEvents.OnDrawEffects( i, j, type, spriteBatch, ref drawColor, ref nextSpecialDrawIndex );
			base.DrawEffects( i, j, type, spriteBatch, ref drawColor, ref nextSpecialDrawIndex );
		}
		public override bool Drop( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnDrop( i, j, type );
			return base.Drop( i, j, type );
		}
		public override void DropCritterChance( int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance ) {
			HamstarHelpersMod.Instance.TileEvents.OnDropCritterChance( i, j, type, ref wormChance, ref grassHopperChance, ref jungleGrubChance );
			base.DropCritterChance( i, j, type, ref wormChance, ref grassHopperChance, ref jungleGrubChance );
		}
		public override void FloorVisuals( int type, Player player ) {
			HamstarHelpersMod.Instance.TileEvents.OnFloorVisuals( type, player );
			base.FloorVisuals( type, player );
		}
		public override void HitWire( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnHitWire( i, j, type );
			base.HitWire( i, j, type );
		}
		public override bool KillSound( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnKillSound( i, j, type );
			return base.KillSound( i, j, type );
		}
		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			HamstarHelpersMod.Instance.TileEvents.OnKillTile( i, j, type, ref fail, ref effectOnly, ref noItem );
			base.KillTile( i, j, type, ref fail, ref effectOnly, ref noItem );
		}
		public override void ModifyLight( int i, int j, int type, ref float r, ref float g, ref float b ) {
			HamstarHelpersMod.Instance.TileEvents.OnModifyLight( i, j, type, ref r, ref g, ref b );
			base.ModifyLight( i, j, type, ref r, ref g, ref b );
		}
		public override void MouseOver( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnMouseOver( i, j, type );
			base.MouseOver( i, j, type );
		}
		public override void MouseOverFar( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnMouseOverFar( i, j, type );
			base.MouseOverFar( i, j, type );
		}
		public override void NearbyEffects( int i, int j, int type, bool closer ) {
			HamstarHelpersMod.Instance.TileEvents.OnNearbyEffects( i, j, type, closer );
			base.NearbyEffects( i, j, type, closer );
		}
		public override void NumDust( int i, int j, int type, bool fail, ref int num ) {
			HamstarHelpersMod.Instance.TileEvents.OnNumDust( i, j, type, fail, ref num );
			base.NumDust( i, j, type, fail, ref num );
		}
		public override void PlaceInWorld( int i, int j, Item item ) {
			HamstarHelpersMod.Instance.TileEvents.OnPlaceInWorld( i, j, item );
			base.PlaceInWorld( i, j, item );
		}
		public override void PostDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			HamstarHelpersMod.Instance.TileEvents.OnPostDraw( i, j, type, spriteBatch );
			base.PostDraw( i, j, type, spriteBatch );
		}
		public override bool PreDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			HamstarHelpersMod.Instance.TileEvents.OnPreDraw( i, j, type, spriteBatch );
			return base.PreDraw( i, j, type, spriteBatch );
		}
		public override bool PreHitWire( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnPreHitWire( i, j, type );
			return base.PreHitWire( i, j, type );
		}
		public override void RandomUpdate( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnRandomUpdate( i, j, type );
			base.RandomUpdate( i, j, type );
		}
		public override void RightClick( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnRightClick( i, j, type );
			base.RightClick( i, j, type );
		}
		public override int SaplingGrowthType( int type, ref int style ) {
			HamstarHelpersMod.Instance.TileEvents.OnSaplingGrowthType( type, ref style );
			return base.SaplingGrowthType( type, ref style );
		}
		public override void SetSpriteEffects( int i, int j, int type, ref SpriteEffects spriteEffects ) {
			HamstarHelpersMod.Instance.TileEvents.OnSetSpriteEffects( i, j, type, ref spriteEffects );
			base.SetSpriteEffects( i, j, type, ref spriteEffects );
		}
		public override void SetDefaults() {
			HamstarHelpersMod.Instance.TileEvents.OnSetDefaults();
			base.SetDefaults();
		}
		public override bool Slope( int i, int j, int type ) {
			HamstarHelpersMod.Instance.TileEvents.OnSlope( i, j, type );
			return base.Slope( i, j, type );
		}
		public override void SpecialDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			HamstarHelpersMod.Instance.TileEvents.OnSpecialDraw( i, j, type, spriteBatch );
			base.SpecialDraw( i, j, type, spriteBatch );
		}
		public override bool TileFrame( int i, int j, int type, ref bool resetFrame, ref bool noBreak ) {
			HamstarHelpersMod.Instance.TileEvents.OnTileFrame( i, j, type, ref resetFrame, ref noBreak );
			return base.TileFrame( i, j, type, ref resetFrame, ref noBreak );
		}
	}*/
}
