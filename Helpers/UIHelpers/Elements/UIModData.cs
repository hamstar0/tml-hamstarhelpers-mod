using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.AnimatedColor;
using HamstarHelpers.WebRequests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.UIHelpers.Elements {
	public class UIModData : UIPanel {
		public Mod Mod { get; private set; }
		public string Author { get; private set; }
		public string HomepageUrl { get; private set; }
		public Version LatestAvailableVersion { get; private set; }

		public UIImage IconElem { get; private set; }
		public UIElement TitleElem { get; private set; }
		public UIElement AuthorElem { get; private set; }
		public UITextPanelButton ConfigButton { get; private set; }
		public UIElement VersionAlertElem { get; private set; }

		public bool HasIconLoaded { get; private set; }
		public bool WillDrawOwnHoverElements { get; private set; }



		////////////////

		public UIModData( UITheme theme, Mod mod, bool will_draw_own_hover_elements = true )
				: this( theme, null, mod, will_draw_own_hover_elements ) { }
		
		public UIModData( UITheme theme, int? idx, Mod mod, bool will_draw_own_hover_elements = true ) {
			var self = this;
			TmodFile modfile = mod.File;

			this.Mod = mod;
			this.WillDrawOwnHoverElements = will_draw_own_hover_elements;

			this.Author = null;
			this.HomepageUrl = null;
			this.HasIconLoaded = false;
			this.LatestAvailableVersion = default( Version );

			if( HamstarHelpersMod.Instance.Config.IsCheckingModVersions ) {
				BuildPropertiesEditor props = modfile != null ?
					BuildPropertiesEditor.GetBuildPropertiesForModFile( modfile ) :
					(BuildPropertiesEditor)null;
				if( props != null ) {
					this.Author = (string)props.GetField( "author" );
					this.HomepageUrl = (string)props.GetField( "homepage" );
				}
			}
			
			// Container

			this.SetPadding( 4f );
			this.Width.Set( 0f, 1f );
			this.Height.Set( 64, 0f );

			float title_offset = 72f;

			// Mod index

			if( idx != null ) {
				var mod_idx_elem = new UIText( (int)idx + "" );
				mod_idx_elem.Left.Set( title_offset, 0f );
				this.Append( (UIElement)mod_idx_elem );

				title_offset += 16f;
			}

			// Mod title

			string mod_title = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			
			if( !String.IsNullOrEmpty(this.HomepageUrl) ) {
				this.TitleElem = new UIWebUrl( theme, mod_title, this.HomepageUrl, false );
			} else {
				this.TitleElem = new UIText( mod_title );
			}
			this.TitleElem.Left.Set( 88f, 0f );
			this.Append( (UIElement)this.TitleElem );

			// Mod author

			if( this.Author != null ) {
				this.AuthorElem = new UIText( "By: "+this.Author, 0.7f );
				this.AuthorElem.Top.Set( 20f, 0f );
				this.AuthorElem.Left.Set( title_offset, 0f );
				this.Append( (UIElement)this.AuthorElem );
			}

			// Mod icon

			if( modfile != null && modfile.HasFile( "icon.png" ) ) {
				var stream = new MemoryStream( modfile.GetFile( "icon.png" ) );
				var icon_tex = Texture2D.FromStream( Main.graphics.GraphicsDevice, stream );

				if( icon_tex.Width == 80 && icon_tex.Height == 80 ) {
					this.IconElem = new UIImage( icon_tex );
					this.IconElem.Top.Set( -4f, 0f );
					this.IconElem.Left.Set( -4f, 0f );
					this.IconElem.MarginTop = -8f;
					this.IconElem.MarginLeft = -4f;
					this.IconElem.ImageScale = 0.7f;
					this.Append( this.IconElem );
				}
			}

			// Mod config button

			if( ModMetaDataManager.HasConfig(mod) ) {
				var config_button = new UITextPanelButton( theme, "Open Config File" );
				config_button.Width.Set( 160f, 0f );
				config_button.HAlign = 1f;
				config_button.VAlign = 1f;
				this.Append( config_button );

				this.ConfigButton = config_button;
					
				this.ConfigButton.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
					string path = ModMetaDataManager.GetConfigRelativePath( mod );
					string fullpath = Main.SavePath + Path.DirectorySeparatorChar + path;

					try {
						Process.Start( fullpath );
					} catch( Exception e ) {
						try {
							string dir = new FileInfo( fullpath ).Directory.FullName;
							Process.Start( dir );
						} catch( Exception ) { }

						Main.NewText( "Couldn't open config file " + path + ": " + e.Message, Color.Red );
					}
				};
			}
		}


		////////////////

		[System.Obsolete( "use UIModData.CheckForNewVersionAsync", true )]
		public void CheckForNewVersion() {
			this.CheckForNewVersionAsync();
		}

		public void CheckForNewVersionAsync() {
			Action<Version> on_success = delegate ( Version vers ) {
				this.LatestAvailableVersion = vers;
			};
			Action<Exception> on_fail = delegate ( Exception e ) {
				if( HamstarHelpersMod.Instance.Config.DebugModeNetInfo ) {
					LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName + "': " + e.ToString() );
				}
			};

			ModVersionGet.GetLatestKnownVersionAsync( this.Mod, on_success, on_fail );
		}


		////////////////

		public override int CompareTo( object obj ) {
			if( this.Mod.Name == HamstarHelpersMod.Instance.Name ) {
				return -1;
			}

			if( obj != null && obj is UIModData ) {
				var other = (UIModData)obj;

				try {
					if( ModMetaDataManager.HasGithub( this.Mod ) && !ModMetaDataManager.HasGithub( other.Mod ) ) {
						return -1;
					}
					if( ModMetaDataManager.HasConfig( this.Mod ) && !ModMetaDataManager.HasConfig( other.Mod ) ) {
						return -1;
					}
				} catch( Exception ) { }

				return string.Compare( this.Mod.Name, other.Mod.Name );
			}

			return -1;
		}

		////////////////


		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering && this.WillDrawOwnHoverElements ) {
				this.DrawHoverEffects( sb );
			}
		}

		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			if( this.LatestAvailableVersion > this.Mod.Version ) {
				Color color = AnimatedColors.Fire.CurrentColor;
				CalculatedStyle inner_dim = base.GetInnerDimensions();
				Vector2 pos = inner_dim.Position();
				pos.X += 128f;
			
				sb.DrawString( Main.fontDeathText, "Update Needed!", pos, color, 0f, default( Vector2 ), 1f, SpriteEffects.None, 1f );
			}
		}


		public void DrawHoverEffects( SpriteBatch sb ) {
			if( this.TitleElem.IsMouseHovering ) {
				if( this.TitleElem is UIWebUrl ) {
					var title_url = (UIWebUrl)this.TitleElem;

					if( !title_url.WillDrawOwnHoverUrl ) {
						title_url.DrawHoverEffects( sb );
					}
				}
			}
		}
	}
}
