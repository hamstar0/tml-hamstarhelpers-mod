using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		protected abstract class CustomEntityComponentFactory<T> : Factory<T> where T : CustomEntityComponent {
			protected override void Initialize( T data ) {
				this.InitializeComponent( data );
				data.PostInitialize();
			}

			protected abstract void InitializeComponent( T data );
		}



		////////////////

		protected CustomEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) {
			Type baseFactoryType = typeof( CustomEntityComponentFactory<> );

			if( !DotNetHelpers.IsSubclassOfRawGeneric( typeof(CustomEntityComponentFactory<>), ctorLock.FactoryType) ) {
				if( ctorLock.FactoryType != typeof( PacketProtocolData ) ) {
					throw new NotImplementedException( "CustomEntityComponent " + this.GetType().Name + " uses invalid factory " + ctorLock.FactoryType.Name );
				}
			}
		}

		////

		internal CustomEntityComponent InternalClone() {
			return this.Clone();
		}

		protected virtual CustomEntityComponent Clone() {
			return (CustomEntityComponent)this.MemberwiseClone();
		}
	}
}
