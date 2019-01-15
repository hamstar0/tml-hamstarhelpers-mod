using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal PacketProtocolDataConstructorLock() { }
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			protected abstract void Initialize( T data );

			public T Create() {
				Type dataType = typeof( T );
				Type factoryType = this.GetType();
				Type factoryContainerType = factoryType.DeclaringType;
				
				if( dataType != factoryContainerType ) {
					throw new NotImplementedException( "Invalid PacketProtocolData factory for class " + dataType.Name + "; expected a " + factoryContainerType.Name );
				}

				T data = (T)Activator.CreateInstance( factoryContainerType,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock() },
					null
				);
				this.Initialize( data );

				data.OnInitialize();

				return data;
			}
		}



		////////////////

		/*rotected static T CreateDefault<T>( Factory<T> factory ) where T : PacketProtocolData {
			Type factoryType = factory.GetType();

			if( typeof(T) != factoryType.DeclaringType || factoryType.IsAbstract ) {
				throw new HamstarException( "Invalid factory "+factoryType.ToString() );
			}

			return factory.Create();
		}*/

		////////////////

		internal static PacketProtocolData CreateRawUninitialized( Type dataType ) {
			if( !dataType.IsSubclassOf(typeof(PacketProtocolData)) ) {
				throw new HamstarException("Not a PacketProtocolData subclass.");
			}

			var data = (PacketProtocolData)Activator.CreateInstance( dataType,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock() },
				null
			);

			return data;
		}
	}
}
