﻿using System;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Services.Server {
	[Obsolete("use Services/Network/Server", true)]
	public class Server {
		[Obsolete( "use Services/Network/Server", true )]
		public int AveragePing {
			get {
				return ModHelpersMod.Instance.Server.AveragePing;
			}
			internal set {
				ModHelpersMod.Instance.Server.AveragePing = value;
			}
		}
	}
}
