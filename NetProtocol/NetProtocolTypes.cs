namespace HamstarHelpers.NetProtocol {
	enum NetProtocolTypes : byte {
		RequestModSettings,
		SendModSettings,

		RequestModData,
		SendModData,

		RequestPlayerPermaDeath,
		SendPlayerPermaDeath,

		RequestPlayerData,
		SendPlayerData,
		UploadPlayerData
		
		//SendSetAdmin
	}
}
