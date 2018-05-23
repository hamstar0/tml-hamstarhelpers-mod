using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	abstract public class EntityGroupDefinition<T> where T : Entity {
		public IDictionary<string, bool> BoolFields { get; private set; }
		public IDictionary<string, IDictionary<double, double>> NumFields { get; private set; }
		public IDictionary<string, Regex> StringFields { get; private set; }
		public EntityGroupDefinition<T> AnyOf { get; private set; }

		protected Func<T, bool> CustomMatcher = null;


		////////////////

		public EntityGroupDefinition() {
			this.BoolFields = new Dictionary<string, bool>();
			this.NumFields = new Dictionary<string, IDictionary<double, double>>();
			this.StringFields = new Dictionary<string, Regex>();
			this.AnyOf = null;
		}

		public EntityGroupDefinition( Func<T, bool> matcher ) : this() {
			this.CustomMatcher = matcher;
		}


		////////////////

		abstract public T[] GetPool();

		abstract public void ClearPool();


		////////////////

		private object GetTypeNameAndValue( string member_name, T entity, out string type_name ) {
			Type mytype = typeof( T );
			FieldInfo field = mytype.GetField( member_name );
			object value = null;
			
			if( field != null ) {
				type_name = field.GetType().Name;
				value = field.GetValue( entity );
			} else {
				PropertyInfo prop = mytype.GetProperty( member_name );
				if( prop == null ) {
					throw new Exception( "Invalid field or property " + member_name );
				}

				type_name = prop.GetType().Name;
				value = prop.GetValue( entity );
			}

			return value;
		}


		////////////////

		public bool ValidateBoolFields( T entity, bool all_only ) {
			Type mytype = typeof( T );

			foreach( var kv in this.BoolFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );

				if( field_type_name != "Boolean" ) {
					throw new InvalidCastException();
				}

				if( (bool)raw_field_value == kv.Value ) {
					if( !all_only ) {
						return true;
					}
				} else {
					if( all_only ) {
						return false;
					}
				}
			}

			return all_only;
		}

		public bool ValidateNumFields( T entity, bool all_only ) {
			Type mytype = typeof( T );

			foreach( var kv in this.NumFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );
				double field_val;

				switch( field_type_name ) {
				case "Byte":
					field_val = (double)( (byte)raw_field_value );
					break;
				case "SByte":
					field_val = (double)( (sbyte)raw_field_value );
					break;
				case "Int16":
					field_val = (double)( (short)raw_field_value );
					break;
				case "UInt16":
					field_val = (double)( (ushort)raw_field_value );
					break;
				case "Int32":
					field_val = (double)( (int)raw_field_value );
					break;
				case "UInt32":
					field_val = (double)( (uint)raw_field_value );
					break;
				case "Int64":
					field_val = (double)( (long)raw_field_value );
					break;
				case "UInt64":
					field_val = (double)( (ulong)raw_field_value );
					break;
				case "Single":
					field_val = (double)( (float)raw_field_value );
					break;
				case "Double":
					field_val = (double)raw_field_value;
					break;
				default:
					throw new InvalidCastException();
				}

				foreach( KeyValuePair<double, double> match_val in kv.Value ) {
					if( field_val < match_val.Key || field_val >= match_val.Value ) {
						if( all_only ) {
							return false;
						}
					} else {
						if( !all_only ) {
							return true;
						}
					}
				}
			}

			return all_only;
		}

		public bool ValidateStringFields( T entity, bool all_only ) {
			Type mytype = typeof( T );

			foreach( var kv in this.StringFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );

				if( field_type_name != "String" ) {
					throw new InvalidCastException();
				}

				if( kv.Value.IsMatch( (string)raw_field_value ) ) {
					if( !all_only ) {
						return true;
					}
				} else {
					if( all_only ) {
						return false;
					}
				}
			}

			return all_only;
		}

		////////////////

		public bool Validate( T entity ) {
			if( !this.ValidateBoolFields( entity, false ) ) { return false; }
			if( !this.ValidateNumFields( entity, false ) ) { return false; }
			if( !this.ValidateStringFields( entity, false ) ) { return false; }
			if( this.AnyOf != null && !this.AnyOf.ValidateAny( entity ) ) { return false; }
			if( this.CustomMatcher != null && !this.CustomMatcher( entity ) ) { return false; }

			return true;
		}

		protected virtual bool ValidateAny( T entity ) {
			if( this.ValidateBoolFields( entity, false ) ) { return true; }
			if( this.ValidateNumFields( entity, false ) ) { return true; }
			if( this.ValidateStringFields( entity, false ) ) { return true; }
			if( this.CustomMatcher != null && this.CustomMatcher( entity ) ) { return true; }

			return false;
		}


		////////////////

		public virtual ISet<T> GetGroup() {
			Type mytype = typeof( T );
			T[] pool = this.GetPool();
			ISet<T> group = new HashSet<T>();

			foreach( T ent in pool ) {
				if( this.Validate(ent) ) {
					group.Add( ent );
				}
			}

			return group;
		}
	}
}
