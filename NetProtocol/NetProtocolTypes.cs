namespace HamstarHelpers.NetProtocol {
	public enum NetProtocolTypes : byte {
		RequestModSettings,
		SendModSettings,

		RequestModData,
		SendModData,

		RequestPlayerPermaDeath,
		SendPlayerPermaDeath,

		UploadPlayerData,
		SendPlayerData
		
		//SendSetAdmin
	}
}
