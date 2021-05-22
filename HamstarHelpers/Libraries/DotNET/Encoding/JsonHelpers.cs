using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;


namespace HamstarHelpers.Libraries.DotNET.Encoding {
    /// <summary>
    /// Assorted static "helper" functions pertaining to hash codes.
    /// </summary>
    public class JsonLibraries {
        /// <summary>
        /// Removes incompatible entries from the input JSON object.
        /// </summary>
        /// <typeparam name="CastToObject"></typeparam>
        /// <param name="jobj"></param>
        /// <param name="flags"></param>
        /// <returns>Map of JSON entries (by entry name) skimmed from the given input.</returns>
        public static IDictionary<string, string> SkimIncompatibleEntries<CastToObject>(
                    JObject jobj,
                    BindingFlags flags = BindingFlags.Public | BindingFlags.Instance ) {
            var removed = new Dictionary<string, string>();
            
            IEnumerable<string> props = typeof( CastToObject )
                    .GetProperties( flags )
                    .Select( p => p.Name );
            IEnumerable<string> fields = typeof( CastToObject )
                    .GetFields( flags )
                    .Select( p => p.Name );

            var members = new HashSet<string>( props );
            members.UnionWith( fields );

            IEnumerable<JProperty> filter = jobj.Properties()
                .Where( jp =>
                    jp.Value.Type != JTokenType.Object
                    || !jp.Value.Children<JProperty>()
                        .Any( cp => members.Contains(cp.Name) )
                );

            foreach( JProperty prop in filter.ToArray() ) {
                removed[prop.Name] = prop.ToString();
                prop.Remove();
            }

            return removed;
        }
    }
}
