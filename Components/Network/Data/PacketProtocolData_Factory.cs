using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	[Obsolete]
	public class PacketProtocolDataConstructorLock {
		internal PacketProtocolDataConstructorLock() { }
	}




	public abstract partial class PacketProtocolData {
		[Obsolete("use constructor", true)]
		protected abstract class Factory<T> where T : PacketProtocolData {
			protected abstract void Initialize( T data );

			public T Create() {
				Type dataType = typeof( T );
				Type factoryType = this.GetType();
				Type factoryContainerType = factoryType.DeclaringType;
				
				if( dataType != factoryContainerType ) {
					throw new NotImplementedException( "Invalid PacketProtocolData factory for class " + dataType.Name + "; expected a " + factoryContainerType.Name );
				}

				T data;
				ConstructorInfo ctor = factoryContainerType.GetConstructor( BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] {}, null );

				if( ctor == null ) {
					ctor = factoryContainerType.GetConstructor( BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(PacketProtocolDataConstructorLock) }, null );
					data = (T)ctor.Invoke( new object[] { new PacketProtocolDataConstructorLock() } );
				} else {
					data = (T)ctor.Invoke( new object[] {} );
				}
				
				/*T data = (T)Activator.CreateInstance( factoryContainerType,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock() },
					null
				);*/
				this.Initialize( data );

				data.OnInitialize();

				return data;
			}
		}



		////////////////
		
		protected abstract void OnInitialize();

		internal void InternalOnInitialize() { this.OnInitialize(); }
	}
}
