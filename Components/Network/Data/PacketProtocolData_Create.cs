using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal Type Context { get; private set; }


		internal PacketProtocolDataConstructorLock( Type context ) {
			this.Context = context;
		}
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			public abstract void Initialize( T data );

			public T Create() {
				Type init_type = this.GetType();
				Type my_type = init_type.DeclaringType;

				T data = (T)Activator.CreateInstance( my_type,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock( my_type ) },
					null
				);
				this.Initialize( data );

				return data;
			}
		}



		////////////////

		/*private Type GetMainFactoryType<T>() where T : PacketProtocolData {
			Type factory_type = typeof( Factory<T> );
			Type[] nesteds = this.GetType().GetNestedTypes( BindingFlags.Static | BindingFlags.NonPublic );

			foreach( var nested in nesteds ) {
				if( nested.IsSubclassOf( factory_type ) && !nested.IsAbstract ) {
					return nested;
				}
			}

			return null;
		}

		protected Type GetMainFactoryType() {
			return this.GetMainFactoryType<PacketProtocolData>();
		}*/


		////////////////
		
		internal static PacketProtocolData CreateRaw( Type data_type ) {
			return (PacketProtocolData)Activator.CreateInstance( data_type,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( PacketProtocolData ) ) },
				null
			);
		}


		////////////////
		
		protected PacketProtocolData( PacketProtocolDataConstructorLock ctor_lock ) {
			if( ctor_lock == null ) {
				throw new NotImplementedException( "Invalid " + this.GetType().Name + ": Must be factory generated or cloned." );
			}
			if( ctor_lock.Context.Name != this.GetType().Name ) {
				if( ctor_lock.Context.Name != "PacketProtocolData" ) {	// Allows cloning
					throw new NotImplementedException( "Invalid " + this.GetType().Name + ": Mismatched factory context." );
				}
			}
		}
	}
}
