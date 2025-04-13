// === FogSpawnerSharedMethods.cs ===

using UnityEngine;
using HoldfastSharedMethods;

public class FogSpawnerSharedMethods : IHoldfastSharedMethods
{
    private bool isServer;
    private GameObject fogManager;

    public void OnIsServer(bool server)
    {
        isServer = server;
        if (isServer)
        {
            Debug.Log("###- XARKS VFX FOG MANAGER IS RUNNING ON THE SERVER -###");
        }
    }

    public void PassConfigVariables(string[] value)
    {
        if (fogManager == null)
        {
            fogManager = GameObject.Find("FogManager");
            if (fogManager == null)
            {
                Debug.LogWarning("XarksFog: FogManager not found when applying config.");
                return;
            }
        }

        var fogController = fogManager.GetComponent<XarksFogDomeController>();
        if (fogController == null)
        {
            Debug.LogWarning("XarksFog: XarksFogDomeController component missing.");
            return;
        }

        float r = 0.8f, g = 0.85f, b = 0.9f, a = 0.3f;
        float density = 0.3f, min = 0.1f;

        foreach (string entry in value)
        {
            if (!entry.StartsWith("xarksfog:")) continue;

            string[] parts = entry.Split(':');
            if (parts.Length < 3) continue;

            string key = parts[1].ToLower();
            string val = parts[2];

            switch (key)
            {
                case "color":
                    string[] rgba = val.Split(',');
                    if (rgba.Length == 4 &&
                        float.TryParse(rgba[0], out r) &&
                        float.TryParse(rgba[1], out g) &&
                        float.TryParse(rgba[2], out b) &&
                        float.TryParse(rgba[3], out a))
                    {
                        // Parsed
                    }
                    break;

                case "density":
                    float.TryParse(val, out density);
                    break;

                case "min":
                    float.TryParse(val, out min);
                    break;
            }
        }

        fogController.IsClient = !isServer;
        fogController.SetFog(new Color(r, g, b, a), density, min);
        Debug.Log($"[XarksFog] Config applied: Color({r},{g},{b},{a}), Density: {density}, MinFog: {min}");
    }

    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attacker, FactionCountry defender, GameplayMode mode, GameType type)
    {
        fogManager = GameObject.Find("FogManager");
        if (fogManager == null)
        {
            Debug.LogWarning("XarksFog: FogManager not found in scene.");
            return;
        }

        var fogDomeController = fogManager.GetComponent<XarksFogDomeController>();
        if (fogDomeController == null)
        {
            Debug.LogWarning("XarksFog: XarksFogDomeController component missing on FogManager.");
            return;
        }

        fogDomeController.IsClient = !isServer;
    }

    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject)
    {
        Debug.Log("[XarksFog] OnPlayerSpawned triggered. Attempting fog dome setup.");

        var fogController = GameObject.Find("FogManager")?.GetComponent<XarksFogDomeController>();
        if (fogController != null)
        {
            fogController.StartCoroutine(fogController.WaitForRealLocalCamera());
        }
        else
        {
            Debug.LogWarning("[XarksFog] FogManager or XarksFogDomeController not found on player spawn.");
        }
    }

    public void OnPlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot) { }
    public void OnPlayerLeft(int playerId) { }
    public void OnUpdateElapsedTime(float time) { }
    public void OnUpdateTimeRemaining(float time) { }
    public void OnSyncValueState(int value) { }
    public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason) { }
    public void OnBuffStart(int playerId, BuffType buff) { }
    public void OnBuffStop(int playerId, BuffType buff) { }
    public void OnCapturePointCaptured(int pointId) { }
    public void OnCapturePointDataUpdated(int pointId, int defenders, int attackers) { }
    public void OnCapturePointOwnerChanged(int pointId, FactionCountry faction) { }
    public void OnConsoleCommand(string input, string output, bool success) { }
    public void OnDamageableObjectDamaged(GameObject obj, int objId, int shipId, int oldHp, int newHp) { }
    public void OnEmplacementConstructed(int itemId) { }
    public void OnEmplacementPlaced(int itemId, GameObject obj, EmplacementType type) { }
    public void OnInteractableObjectInteraction(int playerId, int objectId, GameObject obj, InteractionActivationType type, int transition) { }
    public void OnIsClient(bool isClient, ulong steamId) { }
    public void OnPlayerBlock(int attackerId, int defenderId) { }
    public void OnPlayerEndCarry(int playerId) { }
    public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason) { }
    public void OnPlayerKilledPlayer(int killerId, int victimId, EntityHealthChangedReason reason, string details) { }
    public void OnPlayerKilledVehicle(int killerId, int vehicleId, EntityHealthChangedReason reason, string details) { }
    public void OnPlayerMeleeStartSecondaryAttack(int playerId) { }
    public void OnPlayerShoot(int playerId, bool dryShot) { }
    public void OnPlayerShout(int playerId, CharacterVoicePhrase phrase) { }
    public void OnPlayerStartCarry(int playerId, CarryableObjectType objType) { }
    public void OnPlayerWeaponSwitch(int playerId, string weapon) { }
    public void OnRCCommand(int playerId, string input, string output, bool success) { }
    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn) { }
    public void OnRoundEndFactionWinner(FactionCountry faction, FactionRoundWinnerReason reason) { }
    public void OnRoundEndPlayerWinner(int playerId) { }
    public void OnScorableAction(int playerId, int score, ScorableActionType reason) { }
    public void OnShipDamaged(int shipId, int oldHp, int newHp) { }
    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry faction, ShipType shipType, int shipName) { }
    public void OnShotInfo(int playerId, int count, Vector3[][] shotPoints, float[] distances, float[] offsets, float[] hAngles, float[] hMax, float[] velocities, float[] gravities, float[] baseDamages, float[] rangeModifiers, float[] buffedDamages, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections, int[] hitPlayerIds, int[] hitObjIds, int[] hitShipIds, int[] hitVehicleIds) { }
    public void OnTextMessage(int playerId, TextChatChannel channel, string text) { }
    public void OnUpdateSyncedTime(double time) { }
    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason) { }
    public void OnVehicleSpawned(int vehicleId, FactionCountry faction, PlayerClass playerClass, GameObject vehicleObject, int ownerPlayerId) { }
}
