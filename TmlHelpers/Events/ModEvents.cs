using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Localization;
using Terraria.UI;


namespace HamstarHelpers.TmlHelpers.Events {
	/*public class ModEvents {
		public delegate void AddRecipeGroupsEvt();
		public delegate void AddRecipesEvt();
		public delegate void CallEvt( params object[] args );
		public delegate void HandlePacketEvt( BinaryReader reader, ref int player_who );
		public delegate void HijackGetDataEvt( ref byte messageType, ref BinaryReader reader, int playerNumber );
		public delegate void HijackSendDataEvt( ref int whoAmI, ref int msgType, ref int remoteClient, ref int ignoreClient, NetworkText text, ref int number, ref float number2, ref float number3, ref float number4, ref int number5, ref int number6, ref int number7 );
		public delegate void HotKeyPressedEvt( string name );
		public delegate void LoadEvt();
		public delegate void ModifyInterfaceLayersEvt( List<GameInterfaceLayer> layers );
		public delegate void ModifyLightingBrightnessEvt( ref float scale );
		public delegate void ModifySunLightColorEvt( ref Color tileColor, ref Color backgroundColor );
		public delegate Matrix ModifyTransformMatrixEvt( ref Matrix Transform );
		public delegate void PostAddRecipesEvt();
		public delegate void PostDrawFullscreenMapEvt( string mouseText );
		public delegate void PostDrawInterfaceEvt( SpriteBatch spriteBatch );
		public delegate void PostSetupContentEvt();
		public delegate void PostUpdateInputEvt();
		public delegate void PreSaveAndQuitEvt();
		public delegate void UnloadEvt();
		public delegate void UpdateMusicEvt( ref int music );



		////////////////

		private event AddRecipeGroupsEvt _AddRecipeGroups;
		private event AddRecipesEvt _AddRecipes;
		private event CallEvt _Call;
		private event HijackGetDataEvt _HijackGetData;
		private event HijackSendDataEvt _HijackSendData;
		private event HotKeyPressedEvt _HotKeyPressed;
		private event LoadEvt _Load;
		private event HandlePacketEvt _HandlePacket;
		private event ModifyInterfaceLayersEvt _ModifyInterfaceLayers;
		private event ModifyLightingBrightnessEvt _ModifyLightingBrightness;
		private event ModifySunLightColorEvt _ModifySunLightColor;
		private event ModifyTransformMatrixEvt _ModifyTransformMatrix;
		private event PostAddRecipesEvt _PostAddRecipes;
		private event PostDrawFullscreenMapEvt _PostDrawFullscreenMap;
		private event PostDrawInterfaceEvt _PostDrawInterface;
		private event PostSetupContentEvt _PostSetupContent;
		private event PostUpdateInputEvt _PostUpdateInput;
		private event PreSaveAndQuitEvt _PreSaveAndQuit;
		private event UnloadEvt _Unload;
		private event UpdateMusicEvt _UpdateMusic;

		////////////////

		public event AddRecipeGroupsEvt AddRecipeGroups {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception("Cannot add hooks after load."); }
				lock( this._AddRecipeGroups ) { this._AddRecipeGroups += value; }
			}
			remove { lock( this._AddRecipeGroups ) { this._AddRecipeGroups -= value; } }
		}
		public event AddRecipesEvt AddRecipes {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._AddRecipes ) { this._AddRecipes += value; }
			}
			remove { lock( this._AddRecipes ) { this._AddRecipes -= value; } }
		}
		public event CallEvt Call {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Call ) { this._Call += value; }
			}
			remove { lock( this._Call ) { this._Call -= value; } }
		}
		public event HandlePacketEvt HandlePacket {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._HandlePacket ) { this._HandlePacket += value; }
			}
			remove { lock( this._HandlePacket ) { this._HandlePacket -= value; } }
		}
		public event HijackGetDataEvt HijackGetData {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._HijackGetData ) { this._HijackGetData += value; }
			}
			remove { lock( this._HijackGetData ) { this._HijackGetData -= value; } }
		}
		public event HijackSendDataEvt HijackSendData {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._HijackSendData ) { this._HijackSendData += value; }
			}
			remove { lock( this._HijackSendData ) { this._HijackSendData -= value; } }
		}
		public event HotKeyPressedEvt HotKeyPressed {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._HotKeyPressed ) { this._HotKeyPressed += value; }
			}
			remove { lock( this._HotKeyPressed ) { this._HotKeyPressed -= value; } }
		}
		public event LoadEvt Load {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Load ) { this._Load += value; }
			}
			remove { lock( this._Load ) { this._Load -= value; } }
		}
		public event ModifyInterfaceLayersEvt ModifyInterfaceLayers {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifyInterfaceLayers ) { this._ModifyInterfaceLayers += value; }
			}
			remove { lock( this._ModifyInterfaceLayers ) { this._ModifyInterfaceLayers -= value; } }
		}
		public event ModifyLightingBrightnessEvt ModifyLightingBrightness {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifyLightingBrightness ) { this._ModifyLightingBrightness += value; }
			}
			remove { lock( this._ModifyLightingBrightness ) { this._ModifyLightingBrightness -= value; } }
		}
		public event ModifySunLightColorEvt ModifySunLightColor {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifySunLightColor ) { this._ModifySunLightColor += value; }
			}
			remove { lock( this._ModifySunLightColor ) { this._ModifySunLightColor -= value; } }
		}
		public event ModifyTransformMatrixEvt ModifyTransformMatrix {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._ModifyTransformMatrix ) { this._ModifyTransformMatrix += value; }
			}
			remove { lock( this._ModifyTransformMatrix ) { this._ModifyTransformMatrix -= value; } }
		}
		public event PostAddRecipesEvt PostAddRecipes {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostAddRecipes ) { this._PostAddRecipes += value; }
			}
			remove { lock( this._PostAddRecipes ) { this._PostAddRecipes -= value; } }
		}
		public event PostDrawFullscreenMapEvt PostDrawFullscreenMap {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostDrawFullscreenMap ) { this._PostDrawFullscreenMap += value; }
			}
			remove { lock( this._PostDrawFullscreenMap ) { this._PostDrawFullscreenMap -= value; } }
		}
		public event PostDrawInterfaceEvt PostDrawInterface {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostDrawInterface ) { this._PostDrawInterface += value; }
			}
			remove { lock( this._PostDrawInterface ) { this._PostDrawInterface -= value; } }
		}
		public event PostSetupContentEvt PostSetupContent {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostSetupContent ) { this._PostSetupContent += value; }
			}
			remove { lock( this._PostSetupContent ) { this._PostSetupContent -= value; } }
		}
		public event PostUpdateInputEvt PostUpdateInput {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PostUpdateInput ) { this._PostUpdateInput += value; }
			}
			remove { lock( this._PostUpdateInput ) { this._PostUpdateInput -= value; } }
		}
		public event PreSaveAndQuitEvt PreSaveAndQuit {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._PreSaveAndQuit ) { this._PreSaveAndQuit += value; }
			}
			remove { lock( this._PreSaveAndQuit ) { this._PreSaveAndQuit -= value; } }
		}
		public event UnloadEvt Unload {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._Unload ) { this._Unload += value; }
			}
			remove { lock( this._Unload ) { this._Unload -= value; } }
		}
		public event UpdateMusicEvt UpdateMusic {
			add {
				if( HamstarHelpersMod.Instance.HasSetupContent ) { throw new Exception( "Cannot add hooks after load." ); }
				lock( this._UpdateMusic ) { this._UpdateMusic += value; }
			}
			remove { lock( this._UpdateMusic ) { this._UpdateMusic -= value; } }
		}

		////////////////
		
		public void OnAddRecipeGroups() { this._AddRecipeGroups(); }
		public void OnAddRecipes() { this._AddRecipes(); }
		public void OnCall( params object[] args ) { this._Call( args ); }
		public void OnHandlePacket( BinaryReader reader, ref int player_who ) {
			this._HandlePacket( reader, ref player_who );
		}
		public void OnHijackGetData( ref byte messageType, ref BinaryReader reader, int playerNumber ) {
			this._HijackGetData( ref messageType, ref reader, playerNumber );
		}
		public void OnHijackSendData( ref int whoAmI, ref int msgType, ref int remoteClient, ref int ignoreClient, NetworkText text, ref int number, ref float number2, ref float number3, ref float number4, ref int number5, ref int number6, ref int number7 ) {
			this._HijackSendData( ref whoAmI, ref msgType, ref remoteClient, ref ignoreClient, text, ref number, ref number2, ref number3, ref number4, ref number5, ref number6, ref number7 );
		}
		public void OnHotKeyPressed( string name ) { this._HotKeyPressed( name ); }
		public void OnLoad() { this._Load(); }
		public void OnModifyInterfaceLayers( List<GameInterfaceLayer> layers ) { this._ModifyInterfaceLayers( layers ); }
		public void OnModifyLightingBrightness( ref float scale ) { this._ModifyLightingBrightness( ref scale ); }
		public void OnModifySunLightColor( ref Color tileColor, ref Color backgroundColor ) {
			this._ModifySunLightColor( ref tileColor, ref backgroundColor );
		}
		public void OnModifyTransformMatrix( ref Matrix transform ) { this._ModifyTransformMatrix( ref transform ); }
		public void OnPostDrawFullscreenMap( string mouseText ) { this._PostDrawFullscreenMap( mouseText ); }
		public void OnPostDrawInterface( SpriteBatch sb ) { this._PostDrawInterface( sb ); }
		public void OnPostSetupContent() { this._PostSetupContent(); }
		public void OnPostUpdateInput() { this._PostUpdateInput(); }
		public void OnPreSaveAndQuit() { this._PreSaveAndQuit(); }
		public void OnUnload() { this._Unload(); }
		public void OnUpdateMusic( ref int music ) { this._UpdateMusic( ref music ); }
	}*/
}
