using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Helpers.Audio {
	/// <summary>
	/// Assorted static "helper" functions pertaining to sounds.
	/// </summary>
	public class SoundHelpers : ILoadable {
		/// <summary>
		/// Gets volume and pan data for a sound that would play at a given point.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <returns></returns>
		public static (float Volume, float Pan) GetSoundDataFromSource( int worldX, int worldY ) {
			var maxRange = new Rectangle(
				(int)( Main.screenPosition.X - (Main.screenWidth * 2) ),
				(int)( Main.screenPosition.Y - (Main.screenHeight * 2) ),
				Main.screenWidth * 5,
				Main.screenHeight * 5 );
			var source = new Rectangle( worldX, worldY, 1, 1 );

			Vector2 screenCenter = new Vector2(
				Main.screenPosition.X + (float)Main.screenWidth * 0.5f,
				Main.screenPosition.Y + (float)Main.screenHeight * 0.5f );

			if( !source.Intersects(maxRange) ) {
				return (0, 0);
			}

			float pan = (float)(worldX - screenCenter.X) / (float)(Main.screenWidth / 2);
			float distX = (float)worldX - screenCenter.X;
			float distY = (float)worldY - screenCenter.Y;
			float dist = (float)Math.Sqrt( (distX * distX) + (distY * distY) );
			float vol = 1f - (dist / ((float)Main.screenWidth * 1.5f));

			pan = MathHelper.Clamp( pan, -1f, 1f );
			vol = Math.Max( vol, 0f );

			return (vol, pan);
		}


		////////////////

		/// <summary>
		/// A more flexible variant of `Main.PlaySound` to allow adjusting volume and position, in particular for
		/// currently-playing sounds.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="soundPath"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		public static void PlaySound( Mod mod, string soundPath, Vector2 position, float volume = 1f ) {
			if( Main.netMode == NetmodeID.Server ) { return; }

			LegacySoundStyle sound;
			var sndHelp = ModContent.GetInstance<SoundHelpers>();

			if( sndHelp.Sounds.ContainsKey( soundPath ) ) {
				sound = sndHelp.Sounds[soundPath];
				sndHelp.Sounds[soundPath] = sound.WithVolume( volume );
			} else {
				try {
					sound = mod.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/" + soundPath
					).WithVolume( volume );

					sndHelp.Sounds[soundPath] = sound;
				} catch( Exception e ) {
					throw new ModHelpersException( "Sound load issue.", e );
				}
			}

			Main.PlaySound( sound, position );
		}

		/// <summary>
		/// A more flexible variant of `Main.PlaySound` to allow adjusting volume and position.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="sound"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		public static void PlaySound( string name, LegacySoundStyle sound, Vector2 position, float volume = 1f ) {
			if( Main.netMode == NetmodeID.Server ) { return; }

			var sndHelp = ModContent.GetInstance<SoundHelpers>();

			if( sndHelp.Sounds.ContainsKey(name) ) {
				sound = sndHelp.Sounds[name];
			}

			try {
				sound = sound.WithVolume( volume );
				sndHelp.Sounds[ name ] = sound;
			} catch( Exception e ) {
				throw new ModHelpersException( "Sound load issue.", e );
			}

			Main.PlaySound( sound, position );
		}



		////////////////

		private IDictionary<string, LegacySoundStyle> Sounds = new Dictionary<string, LegacySoundStyle>();



		////////////////

		void ILoadable.OnModsLoad() { }
		void ILoadable.OnModsUnload() { }
		void ILoadable.OnPostModsLoad() { }
	}
}
