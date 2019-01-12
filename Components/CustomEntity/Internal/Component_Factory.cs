using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		protected abstract class CustomEntityComponentFactory<T> : Factory<T> where T : CustomEntityComponent {
			protected sealed override void Initialize( T data ) {
				this.InitializeComponent( data );
			}

			protected abstract void InitializeComponent( T data );
		}



		////////////////

		protected CustomEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		internal CustomEntityComponent InternalClone() {
			return this.Clone();
		}

		protected virtual CustomEntityComponent Clone() {
			var clone = (CustomEntityComponent)this.MemberwiseClone();
			//clone.InternalOnInitialize();
			return clone;
		}
	}
}
