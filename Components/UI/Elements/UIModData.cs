using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Internals.WebRequests;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Components.DataStructures;


namespace HamstarHelpers.Components.UI.Elements {
	public partial class UIModData : UIPanel {
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

		private ISet<string> ModTags = new HashSet<string>();



		////////////////

		public UIModData( UITheme theme, Mod mod, bool willDrawOwnHoverElements = true )
				: this( theme, null, mod, willDrawOwnHoverElements ) { }

		public UIModData( UITheme theme, int? idx, Mod mod, bool willDrawOwnHoverElements = true ) {
			this.InitializeMe( theme, idx, mod, willDrawOwnHoverElements );

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				ISet<string> modTags = args.ModTags?.GetOrDefault( mod.Name );
				this.ModTags = modTags ?? this.ModTags;

				return false;
			} );
		}


		////////////////

		public void CheckForNewVersionAsync() {
			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				if( args.Found && args.Info.ContainsKey(this.Mod.Name) ) {
					this.LatestAvailableVersion = args.Info[ this.Mod.Name ].Item2;
				} else {
					if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
						LogHelpers.Log( "Error retrieving version number of '" + this.Mod.DisplayName+"'" ); //+ "': " + reason );
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

		public override int CompareTo( object obj ) {
			var other = obj as UIModData;
			if( other == null ) {   // Other object types are always sorted less than UIModData
				return 1;
			}

			// Always sort own mod to top; this mod's configs should be available first
			if( other.Mod.Name == ModHelpersMod.Instance.Name ) {
				return 1;
			} else if( this.Mod.Name == ModHelpersMod.Instance.Name ) {
				return -1;
			}

			try {
				// Prioritize github'd mods
				if( ModMetaDataManager.HasGithub( this.Mod ) && !ModMetaDataManager.HasGithub( other.Mod ) ) {
					return -1;
				} else if( !ModMetaDataManager.HasGithub( this.Mod ) && ModMetaDataManager.HasGithub( other.Mod ) ) {
					return 1;
				}

				// Prioritize config'd mods
				if( ModMetaDataManager.HasConfig( this.Mod ) && !ModMetaDataManager.HasConfig( other.Mod ) ) {
					return -1;
				} else if( !ModMetaDataManager.HasConfig( this.Mod ) && ModMetaDataManager.HasConfig( other.Mod ) ) {
					return 1;
				}
			} catch { }

			return string.Compare( other.Mod.Name, this.Mod.Name );
		}
	}
}
