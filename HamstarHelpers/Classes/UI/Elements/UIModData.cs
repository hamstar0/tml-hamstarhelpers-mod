using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Internals.WebRequests;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI panel element specialized for rendering and displaying a mod's data (especially as a list item).
	/// </summary>
	public partial class UIModData : UIThemedPanel {
		/// <summary>
		/// Mod represented by this element.
		/// </summary>
		public Mod Mod { get; private set; }
		/// <summary>
		/// Mod's author.
		/// </summary>
		public string Author { get; private set; }
		/// <summary>
		/// Mod's homepage.
		/// </summary>
		public string HomepageUrl { get; private set; }
		/// <summary>
		/// Latest version of the mod on the mod browser.
		/// </summary>
		public Version LatestAvailableVersion { get; private set; }

		/// <summary>
		/// Mod's icon.
		/// </summary>
		public UIImage IconElem { get; private set; }
		/// <summary>
		/// Mod title text.
		/// </summary>
		public UIElement TitleElem { get; private set; }
		/// <summary>
		/// Mod author text.
		/// </summary>
		public UIElement AuthorElem { get; private set; }
		/// <summary>
		/// Config file reset button for the mod.
		/// </summary>
		public UITextPanelButton ConfigResetButton { get; private set; }
		/// <summary>
		/// Config file open button.
		/// </summary>
		public UITextPanelButton ConfigOpenButton { get; private set; }
		/// <summary>
		/// Version update alert element.
		/// </summary>
		public UIElement VersionAlertElem { get; private set; }

		/// <summary>
		/// Displayed index within a display list.
		/// </summary>
		public UIText DisplayIndex { get; private set; }

		/// <summary>
		/// Indicates if mod's icon is loaded.
		/// </summary>
		public bool HasIconLoaded { get; private set; }
		/// <summary>
		/// Indicates if this element draws its own mouse-hover elements.
		/// </summary>
		public bool WillDrawOwnHoverElements { get; private set; }

		private ISet<string> ModTags = new HashSet<string>();



		////////////////

		/// <param name="theme">Visual appearance.</param>
		/// <param name="mod">Mod represented by this element.</param>
		/// <param name="willDrawOwnHoverElements">Indicates if this element draws its own mouse-hover elements.</param>
		public UIModData( UITheme theme, Mod mod, bool willDrawOwnHoverElements = true )
				: this( theme, null, mod, willDrawOwnHoverElements ) { }

		/// <param name="theme">Visual appearance.</param>
		/// <param name="idx">ID number assigned to this element in its list.</param>
		/// <param name="mod">Mod represented by this element.</param>
		/// <param name="willDrawOwnHoverElements">Indicates if this element draws its own mouse-hover elements.</param>
		public UIModData( UITheme theme, int? idx, Mod mod, bool willDrawOwnHoverElements = true )
				: base( theme, true ) {
			this.InitializeMe( idx, mod, willDrawOwnHoverElements );

			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				ISet<string> modTags = args.ModTags?.GetOrDefault( mod.Name );
				this.ModTags = modTags ?? this.ModTags;

				return false;
			} );

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Checks if the mod has new versions available from the mod browser, then updates it's overlay accordingly.
		/// </summary>
		public void CheckForNewVersionAsync() {
			CustomLoadHooks.AddHook( GetModInfo.ModInfoListLoadHookValidator, ( args ) => {
				BasicModInfoDatabase modDb = args.ModInfo;

				if( args.Found && modDb.ContainsKey(this.Mod.Name) ) {
					this.LatestAvailableVersion = modDb[ this.Mod.Name ].Version;
				} else {
					if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
						LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName+"' from mod browser" ); //+ "': " + reason );
					}
				}
				return false;
			} );

			/*Action<Version> onSuccess = delegate ( Version vers ) {
				this.LatestAvailableVersion = vers;
			};
			Action<string> onFail = delegate ( string reason ) {
				if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
					LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName + "': " + reason );
				}
			};

			GetModVersion.GetLatestKnownVersionAsync( this.Mod, onSuccess, onFail );*/
		}


		////////////////

		/// <summary>
		/// Defines the sort order position of this element relative to another object (typically another UIModData element).
		/// </summary>
		/// <param name="obj">Other object to compare rank against.</param>
		/// <returns>Greater than (+1), less than (-1), or equal to (0) comparison status.</returns>
		public override int CompareTo( object obj ) {
			var other = obj as UIModData;
			if( other == null ) {   // Other object types are always sorted less than UIModData
				return 1;
			}

			// Always sort Mod Helpers mod to top; this mod's configs should be available first
			if( other.Mod.Name == ModHelpersMod.Instance.Name ) {
				return 1;
			} else if( this.Mod.Name == ModHelpersMod.Instance.Name ) {
				return -1;
			}

			try {
				// Prioritize github'd mods
				if( ModFeaturesHelpers.HasGithub( this.Mod ) && !ModFeaturesHelpers.HasGithub( other.Mod ) ) {
					return -1;
				} else if( !ModFeaturesHelpers.HasGithub( this.Mod ) && ModFeaturesHelpers.HasGithub( other.Mod ) ) {
					return 1;
				}
			} catch { }

			return string.Compare( other.Mod.Name, this.Mod.Name );
		}
	}
}
