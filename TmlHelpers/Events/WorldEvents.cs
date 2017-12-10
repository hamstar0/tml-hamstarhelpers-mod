using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;


namespace HamstarHelpers.TmlHelpers.Events {
	/*public class WorldEvents {
		public delegate void ChooseWaterStyleEvt( ref int style );
		public delegate void InitializeEvt();
		public delegate void LoadEvt( TagCompound tags );
		public delegate void LoadLegacyEvt( BinaryReader reader );
		public delegate void ModifyWorldGenTasksEvt( List<GenPass> tasks, ref float totalWeight );
		public delegate void NetReceiveEvt( BinaryReader reader );
		public delegate void NetSendEvt( BinaryWriter writer );
		public delegate void PostDrawTilesEvt();
		public delegate void PostUpdateEvt();
		public delegate void PostWorldGenEvt();
		public delegate void PreUpdateEvt();
		public delegate void PreWorldGenEvt();
		public delegate void ResetNearbyTileEffectsEvt();
		public delegate void SaveEvt( TagCompound tags );
		public delegate void TileCountsAvailableEvt( ref int[] tileCounts );



		////////////////

		private event ChooseWaterStyleEvt _ChooseWaterStyle;
		private event InitializeEvt _Initialize;
		private event LoadEvt _Load;
		private event LoadLegacyEvt _LoadLegacy;
		private event ModifyWorldGenTasksEvt _ModifyWorldGenTasks;
		private event NetReceiveEvt _NetReceive;
		private event NetSendEvt _NetSend;
		private event PostDrawTilesEvt _PostDrawTiles;
		private event PostUpdateEvt _PostUpdate;
		private event PostWorldGenEvt _PostWorldGen;
		private event PreUpdateEvt _PreUpdate;
		private event PreWorldGenEvt _PreWorldGen;
		private event ResetNearbyTileEffectsEvt _ResetNearbyTileEffects;
		private event SaveEvt _Save;
		private event TileCountsAvailableEvt _TileCountsAvailable;


		////////////////

		public event ChooseWaterStyleEvt ChooseWaterStyle {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ChooseWaterStyle ) { this._ChooseWaterStyle += value; }
			}
			remove { lock( this._ChooseWaterStyle ) { this._ChooseWaterStyle -= value; } }
		}
		public event InitializeEvt Initialize {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Initialize ) { this._Initialize += value; }
			}
			remove { lock( this._Initialize ) { this._Initialize -= value; } }
		}
		public event LoadEvt Load {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Load ) { this._Load += value; }
			}
			remove { lock( this._Load ) { this._Load -= value; } }
		}
		public event LoadLegacyEvt LoadLegacy {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._LoadLegacy ) { this._LoadLegacy += value; }
			}
			remove { lock( this._LoadLegacy ) { this._LoadLegacy -= value; } }
		}
		public event ModifyWorldGenTasksEvt ModifyWorldGenTasks {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifyWorldGenTasks ) { this._ModifyWorldGenTasks += value; }
			}
			remove { lock( this._ModifyWorldGenTasks ) { this._ModifyWorldGenTasks -= value; } }
		}
		public event NetReceiveEvt NetReceive {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._NetReceive ) { this._NetReceive += value; }
			}
			remove { lock( this._NetReceive ) { this._NetReceive -= value; } }
		}
		public event NetSendEvt NetSend {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._NetSend ) { this._NetSend += value; }
			}
			remove { lock( this._NetSend ) { this._NetSend -= value; } }
		}
		public event PostDrawTilesEvt PostDrawTiles {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostDrawTiles ) { this._PostDrawTiles += value; }
			}
			remove { lock( this._PostDrawTiles ) { this._PostDrawTiles -= value; } }
		}
		public event PostUpdateEvt PostUpdate {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostUpdate ) { this._PostUpdate += value; }
			}
			remove { lock( this._PostUpdate ) { this._PostUpdate -= value; } }
		}
		public event PostWorldGenEvt PostWorldGen {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostWorldGen ) { this._PostWorldGen += value; }
			}
			remove { lock( this._PostWorldGen ) { this._PostWorldGen -= value; } }
		}
		public event PreUpdateEvt PreUpdate {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PreUpdate ) { this._PreUpdate += value; }
			}
			remove { lock( this._PreUpdate ) { this._PreUpdate -= value; } }
		}
		public event PreWorldGenEvt PreWorldGen {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PreWorldGen ) { this._PreWorldGen += value; }
			}
			remove { lock( this._PreWorldGen ) { this._PreWorldGen -= value; } }
		}
		public event ResetNearbyTileEffectsEvt ResetNearbyTileEffects {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ResetNearbyTileEffects ) { this._ResetNearbyTileEffects += value; }
			}
			remove { lock( this._ResetNearbyTileEffects ) { this._ResetNearbyTileEffects -= value; } }
		}
		public event SaveEvt Save {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Save ) { this._Save += value; }
			}
			remove { lock( this._Save ) { this._Save -= value; } }
		}
		public event TileCountsAvailableEvt TileCountsAvailable {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._TileCountsAvailable ) { this._TileCountsAvailable += value; }
			}
			remove { lock( this._TileCountsAvailable ) { this._TileCountsAvailable -= value; } }
		}


		////////////////

		public void OnChooseWaterStyle( ref int style ) {
			this._ChooseWaterStyle( ref style );
		}
		public void OnInitialize() {
			this._Initialize();
		}
		public void OnLoad( TagCompound tags ) {
			this._Load( tags );
		}
		public void OnLoadLegacy( BinaryReader reader ) {
			this._LoadLegacy( reader );
		}
		public void OnModifyWorldGenTasks( List<GenPass> tasks, ref float totalWeight ) {
			this._ModifyWorldGenTasks( tasks, ref totalWeight );
		}
		public void OnNetReceive( BinaryReader reader ) {
			this._NetReceive( reader );
		}
		public void OnNetSend( BinaryWriter writer ) {
			this._NetSend( writer );
		}
		public void OnPostDrawTiles() {
			this._PostDrawTiles();
		}
		public void OnPostUpdate() {
			this._PostUpdate();
		}
		public void OnPostWorldGen() {
			this._PostWorldGen();
		}
		public void OnPreUpdate() {
			this._PreUpdate();
		}
		public void OnPreWorldGen() {
			this._PreWorldGen();
		}
		public void OnResetNearbyTileEffects() {
			this._ResetNearbyTileEffects();
		}
		public void OnSave( TagCompound tags ) {
			this._Save( tags );
		}
		public void OnTileCountsAvailable( ref int[] tileCounts ) {
			this._TileCountsAvailable( ref tileCounts );
		}
	}*/
}
