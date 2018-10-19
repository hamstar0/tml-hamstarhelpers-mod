using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Internals.WebRequests;
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


namespace HamstarHelpers.Components.UI.Elements {
	public class UIModData : UIPanel {
		public Mod Mod { get; private set; }
		public string Author { get; private set; }
		public string HomepageUrl { get; private set; }
		public Version LatestAvailableVersion { get; private set; }

		public UIImage IconElem { get; private set; }
		public UIElement TitleElem { get; private set; }
		public UIElement AuthorElem { get; private set; }
		public UITextPanelButton ConfigResetButton { get; private set; }
		public UITextPanelButton ConfigOpenButton { get; private set; }
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

			if( ModHelpersMod.Instance.Config.IsCheckingModVersions ) {
				Services.Tml.BuildPropertiesEditor props = modfile != null ?
					Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( modfile ) :
					(Services.Tml.BuildPropertiesEditor)null;
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
				if( !Main.dedServ ) {   //...?
					try {
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
					} catch( Exception e ) {
						LogHelpers.Log( "!ModHelpers.UIModData.CTor - " + e.ToString() );
					}
				}
			}

			// Mod config button

			if( ModMetaDataManager.HasConfig(mod) ) {
				if( Main.netMode == 0 ) {
					if( ModMetaDataManager.HasConfigDefaultsReset( mod ) ) {
						this.ConfigResetButton = new UITextPanelButton( theme, "Reset Config File" );
						this.ConfigResetButton.Width.Set( 160f, 0f );
						this.ConfigResetButton.Left.Set( -320f, 1f );
						this.ConfigResetButton.VAlign = 1f;
						this.Append( this.ConfigResetButton );

						this.ConfigResetButton.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
							string msg = mod.DisplayName + " config file reset to defaults.";

							ModMetaDataManager.ResetDefaultsConfig( mod );
							Main.NewText( msg, Color.Lime );
							LogHelpers.Log( msg );
						};
					}

					this.ConfigOpenButton = new UITextPanelButton( theme, "Open Config File" );
					this.ConfigOpenButton.Width.Set( 160f, 0f );
					this.ConfigOpenButton.HAlign = 1f;
					this.ConfigOpenButton.VAlign = 1f;
					this.Append( this.ConfigOpenButton );
					
					this.ConfigOpenButton.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
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
		}


		////////////////

		public void CheckForNewVersionAsync() {
			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				if( args.Found && args.Info.ContainsKey(this.Mod.Name) ) {
					this.LatestAvailableVersion = args.Info[ this.Mod.Name ].Item2;
				} else {
					if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
						LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName ); //+ "': " + reason );
					}
				}
				return false;
			} );

			/*Action<Version> on_success = delegate ( Version vers ) {
				this.LatestAvailableVersion = vers;
			};
			Action<string> on_fail = delegate ( string reason ) {
				if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
					LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName + "': " + reason );
				}
			};

			GetModVersion.GetLatestKnownVersionAsync( this.Mod, on_success, on_fail );*/
		}


		////////////////

		public override int CompareTo( object obj ) {
			if( this.Mod.Name == ModHelpersMod.Instance.Name ) {
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
			
				sb.DrawString( Main.fontDeathText, this.LatestAvailableVersion.ToString()+" Available", pos, color, 0f, default( Vector2 ), 1f, SpriteEffects.None, 1f );
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
