using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Messages.Inbox;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public static ModHelpersMod Instance { get; private set; }

		////

		//internal static ModHelpersPrivilegedUserConfig PrivConfig => ModContent.GetInstance<ModHelpersPrivilegedUserConfig>();
		//public static ModHelpersConfig Config => ModContent.GetInstance<ModHelpersConfig>();



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogLibraries.Log( "UNHANDLED crash? " + e.IsTerminating
				+ " \nSender: " + sender.ToString()
				+ " \nMessage: " + e.ExceptionObject.ToString() );
		}



		////////////////

		private int LastSeenCPScreenWidth = -1;
		private int LastSeenCPScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////
		
		public bool MouseInterface { get; private set; }



		////////////////

		public ModHelpersMod() {
			ModHelpersMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.InitializeModules();
		}


		public override void Load() {
			//ErrorLogger.Log( "Loading Mod Helpers. Ensure you have .NET Framework v4.6+ installed, if you're having problems." );
			//if( Environment.Version < new Version( 4, 0, 30319, 42000 ) ) {
			//	SystemHelpers.OpenUrl( "https://dotnet.microsoft.com/download/dotnet-framework-runtime" );
			//	throw new FileNotFoundException( "Mod Helpers "+this.Version+" requires .NET Framework v4.6+ to work." );
			//}

			this.LoadFull();

			InboxMessages.SetMessage( "ModHelpers:ControlPanelTags",
				"Mod tag lists have now been added to the Control Panel. Mod tags can be modified in the Mod Info menu page via. the main menu.",
				false
			);
		}

		////

		public override void Unload() {
			try {
				LogLibraries.Alert( "Unloading mod..." );
			} catch { }

			this.UnloadFull();
			
			ModHelpersMod.Instance = null;
		}


		////////////////

		public override void PostSetupContent() {
			this.PostSetupContentFull();

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			this.AddRecipesFull();
		}
		
		public override void AddRecipeGroups() {
			this.AddRecipeGroupsFull();

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}
		
		public override void PostAddRecipes() {
			this.PostAddRecipesFull();

			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }
			
			Services.Timers.Timers.SetTimer( "ModHelpersLoadFinish", 1, true, () => {
				this.PostLoadFull();
				return false;
			} );
/*DataDumper.SetDumpSource( "DEBUG", () => {
	var data = Services.DataStore.DataStore.GetAll();
	string str = "";

	foreach( var kv in data ) {
		string key = kv.Key as string;
		if( key == null ) { continue; }

		if( key[key.Length-1] != 'A' ) {
			continue;
		}

		string keyB = key.Substring( 0, key.Length - 1 ) + "B";
		if( !data.ContainsKey(keyB) ) { continue; }

		double valA = (double)kv.Value;
		double valB = (double)data[keyB];
		if( valA != valB ) {
			str += key + " " + valA + " vs " + valB+",\n";
		}
	}

	return str;
} );*/
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.LoadHooks.PreSaveAndExit();
		}


		////////////////

		public override void PostUpdateEverything() {
			this.MouseInterface = Main.LocalPlayer.mouseInterface;
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
