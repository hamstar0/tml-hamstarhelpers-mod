using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		protected class SaveableEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : SaveableEntityComponent {
			public readonly bool AsJson;


			////////////////

			public SaveableEntityComponentFactory( bool asJson ) {
				this.AsJson = asJson;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.AsJson = this.AsJson;

				this.InitializeSaveableEntityComponent( data );
			}

			protected virtual void InitializeSaveableEntityComponent( T data ) { }
		}



		////////////////

		public static SaveableEntityComponent CreateSaveableEntityComponent( bool asJson ) {
			var factory = new SaveableEntityComponentFactory<SaveableEntityComponent>( asJson );
			return factory.Create();
		}



		////////////////

		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator LoadAllValidator;

		internal readonly static object LoadAllDataKey = new object();



		////////////////

		static SaveableEntityComponent() {
			SaveableEntityComponent.LoadAllDataKey = new object();
			SaveableEntityComponent.MyValidatorKey = new object();
			SaveableEntityComponent.LoadAllValidator = new PromiseValidator( SaveableEntityComponent.MyValidatorKey );
		}


		////////////////

		public static string GetFileNameBase() {
			return "world_" + WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		[PacketProtocolIgnore]
		public bool AsJson = true;



		////////////////

		protected SaveableEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		public override void OnInitialize() { }
	}
}
