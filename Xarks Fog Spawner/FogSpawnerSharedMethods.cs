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
            Debug.Log("###- XARKS FOG SPAWNER IS RUNNING ON THE SERVER -###");
        }
    }
    public void PassConfigVariables(string[] value)
    {
        var mono = FogSpawnerMono.Instance;
        if (mono == null)
        {
            Debug.LogWarning("FogSpawnerSharedMethods: No FogSpawnerMono instance found.");
            return;
        }

        foreach (var entry in value)
        {
            var splitData = entry.Split(':');
            if (splitData.Length != 3 || splitData[0] != "XFS") continue;

            string key = splitData[1].Trim().ToLowerInvariant();
            string[] components = splitData[2].Split(',');

            switch (key)
            {
                case "fog position":
                    if (components.Length == 3 &&
                        float.TryParse(components[0], out float px) &&
                        float.TryParse(components[1], out float py) &&
                        float.TryParse(components[2], out float pz))
                    {
                        mono.fogPosition = new Vector3(px, py, pz);
                    }
                    break;

                case "fog scale":
                    if (components.Length == 3 &&
                        float.TryParse(components[0], out float sx) &&
                        float.TryParse(components[1], out float sy) &&
                        float.TryParse(components[2], out float sz))
                    {
                        mono.fogScale = new Vector3(sx, sy, sz);
                    }
                    break;

                case "shape scale":
                    if (components.Length == 3 &&
                        float.TryParse(components[0], out float ssx) &&
                        float.TryParse(components[1], out float ssy) &&
                        float.TryParse(components[2], out float ssz))
                    {
                        mono.shapeScale = new Vector3(ssx, ssy, ssz);
                    }
                    break;

                case "size":
                    if (float.TryParse(splitData[2], out float size))
                        mono.fogSize = size;
                    break;

                case "rate":
                    if (float.TryParse(splitData[2], out float rate))
                        mono.fogEmissionRate = rate;
                    break;

                case "speed":
                    if (float.TryParse(splitData[2], out float speed))
                        mono.startSpeed = speed;
                    break;

                case "color":
                    if (components.Length == 4 &&
                        float.TryParse(components[0], out float r) &&
                        float.TryParse(components[1], out float g) &&
                        float.TryParse(components[2], out float b) &&
                        float.TryParse(components[3], out float a))
                    {
                        mono.fogColor = new Color(r, g, b, a);
                    }
                    break;

                case "noise strength":
                    if (float.TryParse(splitData[2], out float noiseStrength))
                        mono.noiseStrength = noiseStrength;
                    break;

                case "noise scroll":
                    if (float.TryParse(splitData[2], out float scroll))
                        mono.noiseScroll = scroll;
                    break;
            }
        }

        Debug.Log("FogSpawnerSharedMethods: Config applied. Spawning fog...");
        mono.SpawnFog();
    }

    // Required IHoldfastSharedMethods stubs
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
