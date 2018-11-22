﻿using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		protected abstract class CustomEntityComponentFactory<T> : Factory<T> where T : CustomEntityComponent {
			public override void Initialize( T data ) {
				this.InitializeComponent( data );
				data.PostInitialize();
			}

			public abstract void InitializeComponent( T data );
		}



		////////////////

		protected CustomEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			Type base_factory_type = typeof( CustomEntityComponentFactory<> );

			if( !DotNetHelpers.IsSubclassOfRawGeneric( typeof(CustomEntityComponentFactory<>), ctor_lock.FactoryType) ) {
				if( ctor_lock.FactoryType != typeof( PacketProtocolData ) ) {
					throw new NotImplementedException( "CustomEntityComponent " + this.GetType().Name + " uses invalid factory " + ctor_lock.FactoryType.Name );
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
