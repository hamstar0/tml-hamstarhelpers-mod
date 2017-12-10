using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.TmlHelpers.Events {
	/*public class TileEvents {
		public delegate int[] AdjTilesEvt( int type );
		public delegate void AnimateTileEvt();
		public delegate void AutoSelectEvt( int i, int j, int type, Item item );
		public delegate void CanExplodeEvt( int i, int j, int type );
		public delegate void CanKillTileEvt( int i, int j, int type, ref bool blockDamaged );
		public delegate void CanPlaceEvt( int i, int j, int type );
		public delegate void ChangeWaterfallStyleEvt( int type, ref int style );
		public delegate void CreateDustEvt( int i, int j, int type, ref int dustType );
		public delegate void DangersenseEvt( int i, int j, int type, Player player );
		public delegate void DrawEffectsEvt( int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex );
		public delegate void DropEvt( int i, int j, int type );
		public delegate void DropCritterChanceEvt( int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance );
		public delegate void FloorVisualsEvt( int type, Player player );
		public delegate void HitWireEvt( int i, int j, int type );
		public delegate void KillSoundEvt( int i, int j, int type );
		public delegate void KillTileEvt( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem );
		public delegate void ModifyLightEvt( int i, int j, int type, ref float r, ref float g, ref float b );
		public delegate void MouseOverEvt( int i, int j, int type );
		public delegate void MouseOverFarEvt( int i, int j, int type );
		public delegate void NearbyEffectsEvt( int i, int j, int type, bool closer );
		public delegate void NumDustEvt( int i, int j, int type, bool fail, ref int num );
		public delegate void PlaceInWorldEvt( int i, int j, Item item );
		public delegate void PostDrawEvt( int i, int j, int type, SpriteBatch spriteBatch );
		public delegate void PreDrawEvt( int i, int j, int type, SpriteBatch spriteBatch );
		public delegate void PreHitWireEvt( int i, int j, int type );
		public delegate void RandomUpdateEvt( int i, int j, int type );
		public delegate void RightClickEvt( int i, int j, int type );
		public delegate int SaplingGrowthTypeEvt( int type, ref int style );
		public delegate void SetDefaultsEvt();
		public delegate void SetSpriteEffectsEvt( int i, int j, int type, ref SpriteEffects spriteEffects );
		public delegate void SlopeEvt( int i, int j, int type );
		public delegate void SpecialDrawEvt( int i, int j, int type, SpriteBatch spriteBatch );
		public delegate void TileFrameEvt( int i, int j, int type, ref bool resetFrame, ref bool noBreak );


		////////////////

		private event AdjTilesEvt _AdjTiles;
		private event AnimateTileEvt _AnimateTile;
		private event AutoSelectEvt _AutoSelect;
		private event CanExplodeEvt _CanExplode;
		private event CanKillTileEvt _CanKillTile;
		private event CanPlaceEvt _CanPlace;
		private event ChangeWaterfallStyleEvt _ChangeWaterfallStyle;
		private event CreateDustEvt _CreateDust;
		private event DangersenseEvt _Dangersense;
		private event DrawEffectsEvt _DrawEffects;
		private event DropEvt _Drop;
		private event DropCritterChanceEvt _DropCritterChance;
		private event FloorVisualsEvt _FloorVisuals;
		private event HitWireEvt _HitWire;
		private event KillSoundEvt _KillSound;
		private event KillTileEvt _KillTile;
		private event ModifyLightEvt _ModifyLight;
		private event MouseOverEvt _MouseOver;
		private event MouseOverFarEvt _MouseOverFar;
		private event NearbyEffectsEvt _NearbyEffects;
		private event NumDustEvt _NumDust;
		private event PlaceInWorldEvt _PlaceInWorld;
		private event PostDrawEvt _PostDraw;
		private event PreDrawEvt _PreDraw;
		private event PreHitWireEvt _PreHitWire;
		private event RandomUpdateEvt _RandomUpdate;
		private event RightClickEvt _RightClick;
		private event SaplingGrowthTypeEvt _SaplingGrowthType;
		private event SetDefaultsEvt _SetDefaults;
		private event SetSpriteEffectsEvt _SetSpriteEffects;
		private event SlopeEvt _Slope;
		private event SpecialDrawEvt _SpecialDraw;
		private event TileFrameEvt _TileFrame;


		////////////////

		public event AdjTilesEvt AdjTiles {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._AdjTiles ) { this._AdjTiles += value; }
			}
			remove { lock( this._AdjTiles ) { this._AdjTiles -= value; } }
		}
		public event AnimateTileEvt AnimateTile {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._AnimateTile ) { this._AnimateTile += value; }
			}
			remove { lock( this._AnimateTile ) { this._AnimateTile -= value; } }
		}
		public event AutoSelectEvt AutoSelect {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._AutoSelect ) { this._AutoSelect += value; }
			}
			remove { lock( this._AutoSelect ) { this._AutoSelect -= value; } }
		}
		public event CanExplodeEvt CanExplode {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._CanExplode ) { this._CanExplode += value; }
			}
			remove { lock( this._CanExplode ) { this._CanExplode -= value; } }
		}
		public event CanKillTileEvt CanKillTile {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._CanKillTile ) { this._CanKillTile += value; }
			}
			remove { lock( this._CanKillTile ) { this._CanKillTile -= value; } }
		}
		public event CanPlaceEvt CanPlace {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._CanPlace ) { this._CanPlace += value; }
			}
			remove { lock( this._CanPlace ) { this._CanPlace -= value; } }
		}
		public event ChangeWaterfallStyleEvt ChangeWaterfallStyle {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ChangeWaterfallStyle ) { this._ChangeWaterfallStyle += value; }
			}
			remove { lock( this._ChangeWaterfallStyle ) { this._ChangeWaterfallStyle -= value; } }
		}
		public event CreateDustEvt CreateDust {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._CreateDust ) { this._CreateDust += value; }
			}
			remove { lock( this._CreateDust ) { this._CreateDust -= value; } }
		}
		public event DangersenseEvt Dangersense {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Dangersense ) { this._Dangersense += value; }
			}
			remove { lock( this._Dangersense ) { this._Dangersense -= value; } }
		}
		public event DrawEffectsEvt DrawEffects {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._DrawEffects ) { this._DrawEffects += value; }
			}
			remove { lock( this._DrawEffects ) { this._DrawEffects -= value; } }
		}
		public event DropEvt Drop {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Drop ) { this._Drop += value; }
			}
			remove { lock( this._Drop ) { this._Drop -= value; } }
		}
		public event DropCritterChanceEvt DropCritterChance {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._DropCritterChance ) { this._DropCritterChance += value; }
			}
			remove { lock( this._DropCritterChance ) { this._DropCritterChance -= value; } }
		}
		public event FloorVisualsEvt FloorVisuals {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._FloorVisuals ) { this._FloorVisuals += value; }
			}
			remove { lock( this._FloorVisuals ) { this._FloorVisuals -= value; } }
		}
		public event HitWireEvt HitWire {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._HitWire ) { this._HitWire += value; }
			}
			remove { lock( this._HitWire ) { this._HitWire -= value; } }
		}
		public event KillSoundEvt KillSound {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._KillSound ) { this._KillSound += value; }
			}
			remove { lock( this._KillSound ) { this._KillSound -= value; } }
		}
		public event KillTileEvt KillTile {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._KillTile ) { this._KillTile += value; }
			}
			remove { lock( this._KillTile ) { this._KillTile -= value; } }
		}
		public event ModifyLightEvt ModifyLight {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifyLight ) { this._ModifyLight += value; }
			}
			remove { lock( this._ModifyLight ) { this._ModifyLight -= value; } }
		}
		public event MouseOverEvt MouseOver {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._MouseOver ) { this._MouseOver += value; }
			}
			remove { lock( this._MouseOver ) { this._MouseOver -= value; } }
		}
		public event MouseOverFarEvt MouseOverFar {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._MouseOverFar ) { this._MouseOverFar += value; }
			}
			remove { lock( this._MouseOverFar ) { this._MouseOverFar -= value; } }
		}
		public event NearbyEffectsEvt NearbyEffects {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._NearbyEffects ) { this._NearbyEffects += value; }
			}
			remove { lock( this._NearbyEffects ) { this._NearbyEffects -= value; } }
		}
		public event NumDustEvt NumDust {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._NumDust ) { this._NumDust += value; }
			}
			remove { lock( this._NumDust ) { this._NumDust -= value; } }
		}
		public event PlaceInWorldEvt PlaceInWorld {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PlaceInWorld ) { this._PlaceInWorld += value; }
			}
			remove { lock( this._PlaceInWorld ) { this._PlaceInWorld -= value; } }
		}
		public event PostDrawEvt PostDraw {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostDraw ) { this._PostDraw += value; }
			}
			remove { lock( this._PostDraw ) { this._PostDraw -= value; } }
		}
		public event PreDrawEvt PreDraw {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PreDraw ) { this._PreDraw += value; }
			}
			remove { lock( this._PreDraw ) { this._PreDraw -= value; } }
		}
		public event PreHitWireEvt PreHitWire {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PreHitWire ) { this._PreHitWire += value; }
			}
			remove { lock( this._PreHitWire ) { this._PreHitWire -= value; } }
		}
		public event RandomUpdateEvt RandomUpdate {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._RandomUpdate ) { this._RandomUpdate += value; }
			}
			remove { lock( this._RandomUpdate ) { this._RandomUpdate -= value; } }
		}
		public event RightClickEvt RightClick {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._RightClick ) { this._RightClick += value; }
			}
			remove { lock( this._RightClick ) { this._RightClick -= value; } }
		}
		public event SaplingGrowthTypeEvt SaplingGrowthType {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._SaplingGrowthType ) { this._SaplingGrowthType += value; }
			}
			remove { lock( this._SaplingGrowthType ) { this._SaplingGrowthType -= value; } }
		}
		public event SetDefaultsEvt SetDefaults {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._SetDefaults ) { this._SetDefaults += value; }
			}
			remove { lock( this._SetDefaults ) { this._SetDefaults -= value; } }
		}
		public event SetSpriteEffectsEvt SetSpriteEffects {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._SetSpriteEffects ) { this._SetSpriteEffects += value; }
			}
			remove { lock( this._SetSpriteEffects ) { this._SetSpriteEffects -= value; } }
		}
		public event SlopeEvt Slope {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Slope ) { this._Slope += value; }
			}
			remove { lock( this._Slope ) { this._Slope -= value; } }
		}
		public event SpecialDrawEvt SpecialDraw {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._SpecialDraw ) { this._SpecialDraw += value; }
			}
			remove { lock( this._SpecialDraw ) { this._SpecialDraw -= value; } }
		}
		public event TileFrameEvt TileFrame {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._TileFrame ) { this._TileFrame += value; }
			}
			remove { lock( this._TileFrame ) { this._TileFrame -= value; } }
		}


		////////////////

		public void OnAdjTiles( int type ) {
			this._AdjTiles( type );
		}
		public void OnAnimateTile() {
			this._AnimateTile();
		}
		public void OnAutoSelect( int i, int j, int type, Item item ) {
			this._AutoSelect( i, j, type, item );
		}
		public void OnCanExplode( int i, int j, int type ) {
			this._CanExplode( i, j, type );
		}
		public void OnCanKillTile( int i, int j, int type, ref bool blockDamaged ) {
			this._CanKillTile( i, j, type, ref blockDamaged );
		}
		public void OnCanPlace( int i, int j, int type ) {
			this._CanPlace( i, j, type );
		}
		public void OnChangeWaterfallStyle( int type, ref int style ) {
			this._ChangeWaterfallStyle( type, ref style );
		}
		public void OnCreateDust( int i, int j, int type, ref int dustType ) {
			this._CreateDust( i, j, type, ref dustType );
		}
		public void OnDangersense( int i, int j, int type, Player player ) {
			this._Dangersense( i, j, type, player );
		}
		public void OnDrawEffects( int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex ) {
			this._DrawEffects( i, j, type, spriteBatch, ref drawColor, ref nextSpecialDrawIndex );
		}
		public void OnDrop( int i, int j, int type ) {
			this._Drop( i, j, type );
		}
		public void OnDropCritterChance( int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance ) {
			this._DropCritterChance( i, j, type, ref wormChance, ref grassHopperChance, ref jungleGrubChance );
		}
		public void OnFloorVisuals( int type, Player player ) {
			this._FloorVisuals( type, player );
		}
		public void OnHitWire( int i, int j, int type ) {
			this._HitWire( i, j, type );
		}
		public void OnKillSound( int i, int j, int type ) {
			this._KillSound( i, j, type );
		}
		public void OnKillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			this._KillTile( i, j, type, ref fail, ref effectOnly, ref noItem );
		}
		public void OnModifyLight( int i, int j, int type, ref float r, ref float g, ref float b ) {
			this._ModifyLight( i, j, type, ref r, ref g, ref b );
		}
		public void OnMouseOver( int i, int j, int type ) {
			this._MouseOver( i, j, type );
		}
		public void OnMouseOverFar( int i, int j, int type ) {
			this._MouseOverFar( i, j, type );
		}
		public void OnNearbyEffects( int i, int j, int type, bool closer ) {
			this._NearbyEffects( i, j, type, closer );
		}
		public void OnNumDust( int i, int j, int type, bool fail, ref int num ) {
			this._NumDust( i, j, type, fail, ref num );
		}
		public void OnPlaceInWorld( int i, int j, Item item ) {
			this._PlaceInWorld( i, j, item );
		}
		public void OnPostDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			this._PostDraw( i, j, type, spriteBatch );
		}
		public void OnPreDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			this._PreDraw( i, j, type, spriteBatch );
		}
		public void OnPreHitWire( int i, int j, int type ) {
			this._PreHitWire( i, j, type );
		}
		public void OnRandomUpdate( int i, int j, int type ) {
			this._RandomUpdate( i, j, type );
		}
		public void OnRightClick( int i, int j, int type ) {
			this._RightClick( i, j, type );
		}
		public void OnSaplingGrowthType( int type, ref int style ) {
			this._SaplingGrowthType( type, ref style );
		}
		public void OnSetDefaults() {
			this._SetDefaults();
		}
		public void OnSetSpriteEffects( int i, int j, int type, ref SpriteEffects spriteEffects ) {
			this._SetSpriteEffects( i, j, type, ref spriteEffects );
		}
		public void OnSlope( int i, int j, int type ) {
			this._Slope( i, j, type );
		}
		public void OnSpecialDraw( int i, int j, int type, SpriteBatch spriteBatch ) {
			this._SpecialDraw( i, j, type, spriteBatch );
		}
		public void OnTileFrame( int i, int j, int type, ref bool resetFrame, ref bool noBreak ) {
			this._TileFrame( i, j, type, ref resetFrame, ref noBreak );
		}
	}*/
}
