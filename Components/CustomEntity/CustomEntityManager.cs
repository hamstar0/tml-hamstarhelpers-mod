using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public readonly static object MyLock = new object();



		////////////////

		internal CustomEntityTemplateManager TemplateMngr;

		private readonly IDictionary<int, CustomEntity> EntitiesByIndexes = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<Type, ISet<int>> EntitiesByComponentType = new Dictionary<Type, ISet<int>>();

		private Func<bool> OnTickGet;



		////////////////

		internal CustomEntityManager() {
			this.TemplateMngr = new CustomEntityTemplateManager();

			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += CustomEntityManager._Update;

			// Initialize components
			var component_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );

			foreach( var component_type in component_types ) {
				Type[] nested_types = component_type.GetNestedTypes( BindingFlags.Public | BindingFlags.NonPublic );

				foreach( var nested_type in nested_types ) {
					if( nested_type.IsSubclassOf( typeof( CustomEntityComponent.StaticInitializer ) ) ) {
						var static_init = (CustomEntityComponent.StaticInitializer)Activator.CreateInstance( nested_type );
						static_init.StaticInitializationWrapper();
					}
				}
			}

			if( !Main.dedServ ) {
				Overlays.Scene["CustomEntity"] = new CustomEntityOverlay( EffectPriority.VeryHigh, RenderLayers.TilesAndNPCs );

				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}

			Promises.AddPostWorldUnloadEachPromise( () => {
				lock( CustomEntityManager.MyLock ) {
					this.EntitiesByIndexes.Clear();
					this.EntitiesByComponentType.Clear();
				}
			} );

			DataDumper.SetDumpSource( "CustomEntityList", () => {
				lock( CustomEntityManager.MyLock ) {
					return string.Join( "\n  ", this.EntitiesByIndexes.OrderBy( kv => kv.Key )
						.Select( kv => kv.Key + ": " + kv.Value.ToString() )
					);
				}
			} );
		}

		~CustomEntityManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.CustomEntMngr == null ) { return; }

			if( mymod.CustomEntMngr.OnTickGet() ) {
				mymod.CustomEntMngr.Update();
			}
		}

		internal void Update() {
			if( !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			lock( CustomEntityManager.MyLock ) {
				foreach( CustomEntity ent in this.EntitiesByIndexes.Values.ToArray() ) {
					ent.Update();
				}
			}
		}


		////////////////

		private static void _PostDrawAll( GameTime _ ) {
			var sb = Main.spriteBatch;
			bool is_begun = XnaHelpers.IsMainSpriteBatchBegun();

			if( !is_begun ) {
				sb.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.BackgroundViewMatrix.TransformationMatrix );
			}
			ModHelpersMod.Instance?.CustomEntMngr?.PostDrawAll( sb );
			if( !is_begun ) {
				sb.End();
			}
		}


		internal void PreDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.PreDraw( Main.spriteBatch, ent );
				}
			}
		}

		internal void DrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.Draw( sb, ent );
				}
			}
		}


		private void PostDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.PostDraw( Main.spriteBatch, ent );
				}
			}
		}
	}
}
