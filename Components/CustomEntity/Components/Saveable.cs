using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		protected class MyFactory : PacketProtocolData.Factory<SaveableEntityComponent> {
			private readonly bool AsJson;


			////////////////

			public MyFactory( bool as_json ) {
				this.AsJson = as_json;
			}

			////

			public override void Initialize( SaveableEntityComponent data ) {
				data.AsJson = this.AsJson;
			}
		}



		////////////////

		public static SaveableEntityComponent Create( bool as_json ) {
			var factory = new MyFactory( as_json );
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

		public bool AsJson = true;



		////////////////

		protected SaveableEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }
	}
}
