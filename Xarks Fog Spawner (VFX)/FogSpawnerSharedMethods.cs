using UnityEngine;
using HoldfastSharedMethods;
using System;

public class FogSpawnerSharedMethods : IHoldfastSharedMethods
{
    private bool isServer;

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
        var vfxManager = GameObject.FindObjectOfType<VFXFogManager>();
        if (vfxManager == null)
        {
            Debug.LogWarning("FogSpawnerSharedMethods: No VFXFogManager instance found in scene.");
            return;
        }

        vfxManager.PassConfigVariables(value);
    }

    // Required IHoldfastSharedMethods interface stubs (unchanged)
    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attacker, FactionCountry defender, GameplayMode mode, GameType type) { }
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
    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry faction, PlayerClass playerClass, int uniformId, GameObject playerObject) { }
    public void OnPlayerStartCarry(int playerId, CarryableObjectType objType) { }
    public void OnPlayerWeaponSwitch(int playerId, string weapon) { }
    public void OnRCCommand(int playerId, string input, string output, bool success) { }
    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn) { }
    public void OnRoundEndFactionWinner(FactionCountry faction, FactionRoundWinnerReason reason) { }
    public void OnRoundEndPlayerWinner(int playerId) { }
    public void OnScorableAction(int playerId, int score, ScorableActionType reason) { }
    public void OnShipDamaged(int shipId, int oldHp, int newHp) { }
    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry faction, ShipType shipType, int shipName) { }
    public void OnShotInfo(int playerId, int count, Vector3[][] shotPoints, float[] distances, float[] offsets,
        float[] hAngles, float[] hMax, float[] velocities, float[] gravities, float[] baseDamages,
        float[] rangeModifiers, float[] buffedDamages, float[] totalDamages, Vector3[] hitPositions,
        Vector3[] hitDirections, int[] hitPlayerIds, int[] hitObjIds, int[] hitShipIds, int[] hitVehicleIds)
    { }
    public void OnTextMessage(int playerId, TextChatChannel channel, string text) { }
    public void OnUpdateSyncedTime(double time) { }
    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason) { }
    public void OnVehicleSpawned(int vehicleId, FactionCountry faction, PlayerClass playerClass, GameObject vehicleObject, int ownerPlayerId) { }
}
