using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public class ModIdentityHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();



		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( ModIdentityHelpers.ModIds.ContainsKey( mod ) ) {
				return ModIdentityHelpers.ModIds[mod];
			}
			ModIdentityHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return ModIdentityHelpers.ModIds[mod];
		}


		public static IDictionary<Mod, Version> FindDependencyModMajorVersionMismatches( Mod mod ) {
			Services.Tml.BuildPropertiesEditor buildEditor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			IDictionary<string, Version> modRefs = buildEditor.ModReferences;
			var badModDeps = new Dictionary<Mod, Version>();

			foreach( var kv in modRefs ) {
				Mod depMod = ModLoader.GetMod( kv.Key );
				if( depMod == null ) { continue; }

				if( depMod.Version.Major != kv.Value.Major ) {
					badModDeps[depMod] = kv.Value;
				}
			}

			return badModDeps;
		}

		////////////////

		public static string FormatBadDependencyModList( Mod mod ) {
			IDictionary<Mod, Version> badDepMods = ModIdentityHelpers.FindDependencyModMajorVersionMismatches( mod );

			if( badDepMods.Count != 0 ) {
				IEnumerable<string> badDepModsList = badDepMods.SafeSelect(
					kv => kv.Key.DisplayName + " (needs " + kv.Value.ToString() + ", is " + kv.Key.Version.ToString() + ")"
				);
				return mod.DisplayName + " (" + mod.Name + ") is out of date with its dependency mod(s): " + string.Join( ", \n", badDepModsList );
			}
			return null;
		}


		////////////////

		public static bool IsLoadedModProperlyPresented( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				LogHelpers.Alert( "Invalid mod "+modName );
				return false;
			}

			IDictionary<string, Services.Tml.BuildPropertiesEditor> modInfos = ModListHelpers.GetLoadedModNamesWithBuildProps();
			if( !modInfos.ContainsKey(modName) ) {
				LogHelpers.Alert( "Missing mod "+modName );
				return false;
			}

			string author = modInfos[modName].Author;
			string description = modInfos[modName].Description;
			string homepage = modInfos[modName].Homepage;

			return ModIdentityHelpers.IsProperlyPresented( mod.DisplayName, author, description, homepage );
		}

		public static void IsListModProperlyPresented( string modName, Action<bool> callback ) {
			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				if( args.Found && args.Info.ContainsKey( modName ) ) {
					var modInfo = args.Info[ modName ];

					bool isProper = ModIdentityHelpers.IsProperlyPresented( modInfo.;

					callback( isProper );
				} else {
					if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
						LogHelpers.Log( "Error retrieving mod data for '" + modName + "'" ); //+ "': " + reason );
					}
				}
				return false;
			} );
		}

		public static bool IsProperlyPresented( string displayName, string author, string description, string homepage ) {
			if( description.Length < 16 ) { return false; }
			if( homepage.Length < 10 ) { return false; }

			if( description.Contains("Learn how to mod with tModLoader by exploring the source code for this mod.") ) { return false; }
			if( description.Contains("Modify this file with a description of your mod.") ) { return false; }

			homepage = homepage.ToLower();

			//if( homepage.Contains( "discord.gg/" ) ) { return false; }

			// Go away, url shorteners
			if( homepage.Contains( "bit.ly/" ) ) { return false; }
			if( homepage.Contains( "goo.gl/" ) ) { return false; }
			if( homepage.Contains( "tinyurl.com/" ) ) { return false; }
			if( homepage.Contains( "is.gd/" ) ) { return false; }
			if( homepage.Contains( "cli.gs/" ) ) { return false; }
			if( homepage.Contains( "pic.gd/" ) ) { return false; }
			if( homepage.Contains( "dwarfurl.com/" ) ) { return false; }
			if( homepage.Contains( "ow.ly/" ) ) { return false; }
			if( homepage.Contains( "yfrog.com/" ) ) { return false; }
			if( homepage.Contains( "migre.me/" ) ) { return false; }
			if( homepage.Contains( "ff.im/" ) ) { return false; }
			if( homepage.Contains( "tiny.cc/" ) ) { return false; }
			if( homepage.Contains( "url4.eu/" ) ) { return false; }
			if( homepage.Contains( "tr.im/" ) ) { return false; }
			if( homepage.Contains( "twit.ac/" ) ) { return false; }
			if( homepage.Contains( "su.pr/" ) ) { return false; }
			if( homepage.Contains( "twurl.nl/" ) ) { return false; }
			if( homepage.Contains( "snipurl.com/" ) ) { return false; }
			if( homepage.Contains( "budurl.com/" ) ) { return false; }
			if( homepage.Contains( "short.to/" ) ) { return false; }
			if( homepage.Contains( "ping.fm/" ) ) { return false; }
			if( homepage.Contains( "digg.com/" ) ) { return false; }
			if( homepage.Contains( "post.ly/" ) ) { return false; }
			if( homepage.Contains( "just.as/" ) ) { return false; }
			if( homepage.Contains( "/tk/" ) ) { return false; }
			if( homepage.Contains( "bkite.com/" ) ) { return false; }
			if( homepage.Contains( "snipr.com/" ) ) { return false; }
			if( homepage.Contains( "flic.kr/" ) ) { return false; }
			if( homepage.Contains( "loopt.us/" ) ) { return false; }
			if( homepage.Contains( "doiop.com/" ) ) { return false; }
			if( homepage.Contains( "twitthis.com/" ) ) { return false; }
			if( homepage.Contains( "htxt.it/" ) ) { return false; }
			if( homepage.Contains( "alturl.com/" ) ) { return false; }
			if( homepage.Contains( "redirx.com/" ) ) { return false; }
			if( homepage.Contains( "digbig.com/" ) ) { return false; }
			if( homepage.Contains( "short.ie/" ) ) { return false; }
			if( homepage.Contains( "u.mavrev.com/" ) ) { return false; }
			if( homepage.Contains( "kl.am/" ) ) { return false; }
			if( homepage.Contains( "wp.me/" ) ) { return false; }
			if( homepage.Contains( "u.nu/" ) ) { return false; }
			if( homepage.Contains( "rubyurl.com/" ) ) { return false; }
			if( homepage.Contains( "om.ly/" ) ) { return false; }
			if( homepage.Contains( "linkbee.com/" ) ) { return false; }
			if( homepage.Contains( "yep.it/" ) ) { return false; }
			if( homepage.Contains( "posted.at/" ) ) { return false; }
			if( homepage.Contains( "xrl.us/" ) ) { return false; }
			if( homepage.Contains( "metamark.net/" ) ) { return false; }
			if( homepage.Contains( "sn.im/" ) ) { return false; }
			if( homepage.Contains( "hurl.ws/" ) ) { return false; }
			if( homepage.Contains( "eepurl.com/" ) ) { return false; }
			if( homepage.Contains( "idek.net/" ) ) { return false; }
			if( homepage.Contains( "urlpire.com/" ) ) { return false; }
			if( homepage.Contains( "chilp.it/" ) ) { return false; }
			if( homepage.Contains( "moourl.com/" ) ) { return false; }
			if( homepage.Contains( "snurl.com/" ) ) { return false; }
			if( homepage.Contains( "xr.com/" ) ) { return false; }
			if( homepage.Contains( "lin.cr/" ) ) { return false; }
			if( homepage.Contains( "easyuri.com/" ) ) { return false; }
			if( homepage.Contains( "zz.gd/" ) ) { return false; }
			if( homepage.Contains( "ur1.ca/" ) ) { return false; }
			if( homepage.Contains( "url.ie/" ) ) { return false; }
			if( homepage.Contains( "adjix.com/" ) ) { return false; }
			if( homepage.Contains( "twurl.cc/" ) ) { return false; }
			if( homepage.Contains( "s7y.us/" ) ) { return false; }
			if( homepage.Contains( "easyurl.net/" ) ) { return false; }
			if( homepage.Contains( "atu.ca/" ) ) { return false; }
			if( homepage.Contains( "sp2.ro/" ) ) { return false; }
			if( homepage.Contains( "profile.to/" ) ) { return false; }
			if( homepage.Contains( "ub0.cc/" ) ) { return false; }
			if( homepage.Contains( "minurl.fr/" ) ) { return false; }
			if( homepage.Contains( "cort.as/" ) ) { return false; }
			if( homepage.Contains( "fire.to/" ) ) { return false; }
			if( homepage.Contains( "2tu.us/" ) ) { return false; }
			if( homepage.Contains( "twiturl.de/" ) ) { return false; }
			if( homepage.Contains( "to.ly/" ) ) { return false; }
			if( homepage.Contains( "burnurl.com/" ) ) { return false; }
			if( homepage.Contains( "nn.nf/" ) ) { return false; }
			if( homepage.Contains( "clck.ru/" ) ) { return false; }
			if( homepage.Contains( "notlong.com/" ) ) { return false; }
			if( homepage.Contains( "thrdl.es/" ) ) { return false; }
			if( homepage.Contains( "spedr.com/" ) ) { return false; }
			if( homepage.Contains( "vl.am/" ) ) { return false; }
			if( homepage.Contains( "miniurl.com/" ) ) { return false; }
			if( homepage.Contains( "virl.com/" ) ) { return false; }
			if( homepage.Contains( "piurl.com/" ) ) { return false; }
			if( homepage.Contains( "1url.com/" ) ) { return false; }
			if( homepage.Contains( "gri.ms/" ) ) { return false; }
			if( homepage.Contains( "tr.my/" ) ) { return false; }
			if( homepage.Contains( "sharein.com/" ) ) { return false; }
			if( homepage.Contains( "urlzen.com/" ) ) { return false; }
			if( homepage.Contains( "fon.gs/" ) ) { return false; }
			if( homepage.Contains( "shrinkify.com/" ) ) { return false; }
			if( homepage.Contains( "ri.ms/" ) ) { return false; }
			if( homepage.Contains( "b23.ru/" ) ) { return false; }
			if( homepage.Contains( "fly2.ws/" ) ) { return false; }
			if( homepage.Contains( "xrl.in/" ) ) { return false; }
			if( homepage.Contains( "fhurl.com/" ) ) { return false; }
			if( homepage.Contains( "wipi.es/" ) ) { return false; }
			if( homepage.Contains( "korta.nu/" ) ) { return false; }
			if( homepage.Contains( "shortna.me/" ) ) { return false; }
			if( homepage.Contains( "fa.b/" ) ) { return false; }
			if( homepage.Contains( "wapurl.co.uk/" ) ) { return false; }
			if( homepage.Contains( "urlcut.com/" ) ) { return false; }
			if( homepage.Contains( "6url.com/" ) ) { return false; }
			if( homepage.Contains( "abbrr.com/" ) ) { return false; }
			if( homepage.Contains( "simurl.com/" ) ) { return false; }
			if( homepage.Contains( "klck.me/" ) ) { return false; }
			if( homepage.Contains( "x.se/" ) ) { return false; }
			if( homepage.Contains( "2big.at/" ) ) { return false; }
			if( homepage.Contains( "url.co.uk/" ) ) { return false; }
			if( homepage.Contains( "ewerl.com/" ) ) { return false; }
			if( homepage.Contains( "inreply.to/" ) ) { return false; }
			if( homepage.Contains( "tighturl.com/" ) ) { return false; }
			if( homepage.Contains( "a.gg/" ) ) { return false; }
			if( homepage.Contains( "tinytw.it/" ) ) { return false; }
			if( homepage.Contains( "zi.pe/" ) ) { return false; }
			if( homepage.Contains( "riz.gd/" ) ) { return false; }
			if( homepage.Contains( "hex.io/" ) ) { return false; }
			if( homepage.Contains( "fwd4.me/" ) ) { return false; }
			if( homepage.Contains( "bacn.me/" ) ) { return false; }
			if( homepage.Contains( "shrt.st/" ) ) { return false; }
			if( homepage.Contains( "ln-s.ru/" ) ) { return false; }
			if( homepage.Contains( "tiny.pl/" ) ) { return false; }
			if( homepage.Contains( "o-x.fr/" ) ) { return false; }
			if( homepage.Contains( "starturl.com/" ) ) { return false; }
			if( homepage.Contains( "jijr.com/" ) ) { return false; }
			if( homepage.Contains( "shorl.com/" ) ) { return false; }
			if( homepage.Contains( "icanhaz.com/" ) ) { return false; }
			if( homepage.Contains( "updating.me/" ) ) { return false; }
			if( homepage.Contains( "kissa.be/" ) ) { return false; }
			if( homepage.Contains( "hellotxt.com/" ) ) { return false; }
			if( homepage.Contains( "pnt.me/" ) ) { return false; }
			if( homepage.Contains( "nsfw.in/" ) ) { return false; }
			if( homepage.Contains( "xurl.jp/" ) ) { return false; }
			if( homepage.Contains( "yweb.com/" ) ) { return false; }
			if( homepage.Contains( "urlkiss.com/" ) ) { return false; }
			if( homepage.Contains( "qlnk.net/" ) ) { return false; }
			if( homepage.Contains( "w3t.org/" ) ) { return false; }
			if( homepage.Contains( "lt.tl/" ) ) { return false; }
			if( homepage.Contains( "twirl.at/" ) ) { return false; }
			if( homepage.Contains( "zipmyurl.com/" ) ) { return false; }
			if( homepage.Contains( "urlot.com/" ) ) { return false; }
			if( homepage.Contains( "a.nf/" ) ) { return false; }
			if( homepage.Contains( "hurl.me/" ) ) { return false; }
			if( homepage.Contains( "urlhawk.com/" ) ) { return false; }
			if( homepage.Contains( "tnij.org/" ) ) { return false; }
			if( homepage.Contains( "4url.cc/" ) ) { return false; }
			if( homepage.Contains( "firsturl.de/" ) ) { return false; }
			if( homepage.Contains( "hurl.it/" ) ) { return false; }
			if( homepage.Contains( "sturly.com/" ) ) { return false; }
			if( homepage.Contains( "shrinkster.com/" ) ) { return false; }
			if( homepage.Contains( "ln-s.net/" ) ) { return false; }
			if( homepage.Contains( "go2cut.com/" ) ) { return false; }
			if( homepage.Contains( "liip.to/" ) ) { return false; }
			if( homepage.Contains( "shw.me/" ) ) { return false; }
			if( homepage.Contains( "xeeurl.com/" ) ) { return false; }
			if( homepage.Contains( "liltext.com/" ) ) { return false; }
			if( homepage.Contains( "lnk.gd/" ) ) { return false; }
			if( homepage.Contains( "xzb.cc/" ) ) { return false; }
			if( homepage.Contains( "linkbun.ch/" ) ) { return false; }
			if( homepage.Contains( "href.in/" ) ) { return false; }
			if( homepage.Contains( "urlbrief.com/" ) ) { return false; }
			if( homepage.Contains( "2ya.com/" ) ) { return false; }
			if( homepage.Contains( "safe.mn/" ) ) { return false; }
			if( homepage.Contains( "shrunkin.com/" ) ) { return false; }
			if( homepage.Contains( "bloat.me/" ) ) { return false; }
			if( homepage.Contains( "krunchd.com/" ) ) { return false; }
			if( homepage.Contains( "minilien.com/" ) ) { return false; }
			if( homepage.Contains( "shortlinks.co.uk/" ) ) { return false; }
			if( homepage.Contains( "qicute.com/" ) ) { return false; }
			if( homepage.Contains( "rb6.me/" ) ) { return false; }
			if( homepage.Contains( "urlx.ie/" ) ) { return false; }
			if( homepage.Contains( "pd.am/" ) ) { return false; }
			if( homepage.Contains( "go2.me/" ) ) { return false; }
			if( homepage.Contains( "tinyarro.ws/" ) ) { return false; }
			if( homepage.Contains( "tinyvid.io/" ) ) { return false; }
			if( homepage.Contains( "lurl.no/" ) ) { return false; }
			if( homepage.Contains( "ru.ly/" ) ) { return false; }
			if( homepage.Contains( "lru.jp/" ) ) { return false; }
			if( homepage.Contains( "rickroll.it/" ) ) { return false; }
			if( homepage.Contains( "togoto.us/" ) ) { return false; }
			if( homepage.Contains( "clickmeter.com/" ) ) { return false; }
			if( homepage.Contains( "hugeurl.com/" ) ) { return false; }
			if( homepage.Contains( "tinyuri.ca/" ) ) { return false; }
			if( homepage.Contains( "shrten.com/" ) ) { return false; }
			if( homepage.Contains( "shorturl.com/" ) ) { return false; }
			if( homepage.Contains( "quip-art.com/" ) ) { return false; }
			if( homepage.Contains( "urlao.com/" ) ) { return false; }
			if( homepage.Contains( "a2a.me/" ) ) { return false; }
			if( homepage.Contains( "tcrn.ch/" ) ) { return false; }
			if( homepage.Contains( "goshrink.com/" ) ) { return false; }
			if( homepage.Contains( "decenturl.com/" ) ) { return false; }
			if( homepage.Contains( "decenturl.com/" ) ) { return false; }
			if( homepage.Contains( "zi.ma/" ) ) { return false; }
			if( homepage.Contains( "1link.in/" ) ) { return false; }
			if( homepage.Contains( "sharetabs.com/" ) ) { return false; }
			if( homepage.Contains( "shoturl.us/" ) ) { return false; }
			if( homepage.Contains( "fff.to/" ) ) { return false; }
			if( homepage.Contains( "hover.com/" ) ) { return false; }
			if( homepage.Contains( "lnk.in/" ) ) { return false; }
			if( homepage.Contains( "jmp2.net/" ) ) { return false; }
			if( homepage.Contains( "dy.fi/" ) ) { return false; }
			if( homepage.Contains( "urlcover.com/" ) ) { return false; }
			if( homepage.Contains( "2pl.us/" ) ) { return false; }
			if( homepage.Contains( "tweetburner.com/" ) ) { return false; }
			if( homepage.Contains( "u6e.de/" ) ) { return false; }
			if( homepage.Contains( "xaddr.com/" ) ) { return false; }
			if( homepage.Contains( "gl.am/" ) ) { return false; }
			if( homepage.Contains( "dfl8.me/" ) ) { return false; }
			if( homepage.Contains( "go.9nl.com/" ) ) { return false; }
			if( homepage.Contains( "gurl.es/" ) ) { return false; }
			if( homepage.Contains( "c-o.in/" ) ) { return false; }
			if( homepage.Contains( "traceurl.com/" ) ) { return false; }
			if( homepage.Contains( "liurl.cn/" ) ) { return false; }
			if( homepage.Contains( "myurl.in/" ) ) { return false; }
			if( homepage.Contains( "urlenco.de/" ) ) { return false; }
			if( homepage.Contains( "ne1.net/" ) ) { return false; }
			if( homepage.Contains( "buk.me/" ) ) { return false; }
			if( homepage.Contains( "rsmonkey.com/" ) ) { return false; }
			if( homepage.Contains( "cuturl.com/" ) ) { return false; }
			if( homepage.Contains( "turo.us/" ) ) { return false; }
			if( homepage.Contains( "sqrl.it/" ) ) { return false; }
			if( homepage.Contains( "iterasi.net/" ) ) { return false; }
			if( homepage.Contains( "tiny123.com/" ) ) { return false; }
			if( homepage.Contains( "esyurl.com/" ) ) { return false; }
			if( homepage.Contains( "urlx.org/" ) ) { return false; }
			if( homepage.Contains( "iscool.net/" ) ) { return false; }
			if( homepage.Contains( "twitterpan.com/" ) ) { return false; }
			if( homepage.Contains( "gowat.ch/" ) ) { return false; }
			if( homepage.Contains( "poprl.com/" ) ) { return false; }
			if( homepage.Contains( "njx.me/" ) ) { return false; }
			
			return true;
		}
	}
}
