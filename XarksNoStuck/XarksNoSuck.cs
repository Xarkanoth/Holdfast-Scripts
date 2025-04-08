// Importing necessary libraries
using System.Collections.Generic;
using UnityEngine;
using HoldfastSharedMethods;
using UnityEngine.UI;

// Defining a public class XarksNoStuck that implements the interface IHoldfastSharedMethods
public class XarksNoStuck : IHoldfastSharedMethods
{
    // Declaring private variables
    private bool isServer;
    private Dictionary<int, bool> playersInServer;
    private int timeElapsedToCheck;
    private bool didWeCheck;
    private int playerCountRequired;
    private int mapRotationRotate;
    private InputField f1MenuInput;
    private float currentTime;

    // Method that gets called when server status is checked
    public void OnIsServer(bool server)
    {
        // Setting server status and initializing variables
        isServer = server;
        playersInServer = new Dictionary<int, bool>();
        didWeCheck = false;

        // Finding all Canvas objects
        var canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            // Checking if the current canvas is the "Game Console Panel"
            if (string.Compare(canvases[i].name, "Game Console Panel", true) == 0)
            {
                // Finding the input field where the player types messages
                f1MenuInput = canvases[i].GetComponentInChildren<InputField>(true);
                if (f1MenuInput != null)
                {
                    Debug.Log("ModInterface::OnIsServer() Found the Game Console Panel");
                }
                else
                {
                    Debug.Log("ModInterface::OnIsServer() We did Not find the Game Console Panel");
                }
                break;
            }
        }
    }

    // Method that gets called when a player joins the game
    public void OnPlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
    {
        // Adding the player to the playersInServer dictionary if they are not a bot
        if (!isBot)
        {
            playersInServer.Add(playerId, true);
        }
    }

    // Method that gets called when a player leaves the game
    public void OnPlayerLeft(int playerId)
    {
        // Removing the player from the playersInServer dictionary
        playersInServer.Remove(playerId);
    }

    // Method that updates the remaining time
    public void OnUpdateTimeRemaining(float time)
    {
        currentTime = (float)time;
    }

    // Method that updates the elapsed time
    public void OnUpdateElapsedTime(float time)
    {
        // Checking if the elapsed time is greater than or equal to the timeElapsedToCheck
        if (!didWeCheck && time >= timeElapsedToCheck)
        {
            didWeCheck = true;
            // Checking if the number of players is less than the required count and if the server is active
            if (playersInServer.Count < playerCountRequired && isServer)
            {
                // Broadcasting a message and rotating the map
                f1MenuInput.onEndEdit.Invoke("broadcast Swapping Map, not enough people on server.");
                var msg = "delayed " + Mathf.FloorToInt(currentTime - 5f) + " mapRotation " + mapRotationRotate;
                f1MenuInput.onEndEdit.Invoke(msg);
            }
        }
    }

    // Method to pass configuration variables
    public void PassConfigVariables(string[] value)
    {
        // Setting default values for configuration variables
        playerCountRequired = 2;
        mapRotationRotate = 1;
        timeElapsedToCheck = 120;

        // Looping through the passed values
        for (int i = 0; i < value.Length; i++)
        {
            // Splitting the current value into parts
            var splitData = value[i].Split(':');
            if (splitData.Length != 3)
            {
                continue;
            }

            // Checking if the first part of the split data matches the mod id
            if (splitData[0] == "3347655136")
            {
                // Checking the variable type and parsing the value accordingly
                if (splitData[1] == "playerCount")
                {
                    if (!int.TryParse(splitData[2], out playerCountRequired))
                    {
                        Debug.Log("Tried parsing playerCount but invalid format was found.");
                    }
                }
                else if (splitData[1] == "mapRotation")
                {
                    if (!int.TryParse(splitData[2], out mapRotationRotate))
                    {
                        Debug.Log("Tried parsing mapRotation but invalid format was found.");
                    }
                }
                else if (splitData[1] == "timeElapsed")
                {
                    if (!int.TryParse(splitData[2], out timeElapsedToCheck))
                    {
                        Debug.Log("Tried parsing timeElapsed but invalid format was found.");
                    }
                }
            }
        }
    }

    // Empty methods that need to be implemented due to the IHoldfastSharedMethods interface
    public void OnSyncValueState(int value) { }
    public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason) { }
    public void OnBuffStart(int playerId, BuffType buff) { }
    public void OnBuffStop(int playerId, BuffType buff) { }
    public void OnCapturePointCaptured(int capturePoint) { }
    public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount) { }
    public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry) { }
    public void OnConsoleCommand(string input, string output, bool success) { }
    public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp) { }
    public void OnEmplacementConstructed(int itemId) { }
    public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType) { }
    public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject, InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex) { }
    public void OnIsClient(bool client, ulong steamId) { }
    public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId) { }
    public void OnPlayerEndCarry(int playerId) { }
    public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason) { }
    public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string details) { }
    public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details) { }
    public void OnPlayerMeleeStartSecondaryAttack(int playerId) { }
    public void OnPlayerShoot(int playerId, bool dryShot) { }
    public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase) { }
    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject) { }
    public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject) { }
    public void OnPlayerWeaponSwitch(int playerId, string weapon) { }
    public void OnRCCommand(int playerId, string input, string output, bool success) { }
    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn) { }
    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType) { }
    public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason) { }
    public void OnRoundEndPlayerWinner(int playerId) { }
    public void OnScorableAction(int playerId, int score, ScorableActionType reason) { }
    public void OnShipDamaged(int shipId, int oldHp, int newHp) { }
    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipName) { }
    public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances, float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles, float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues, float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections, int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds) { }
    public void OnTextMessage(int playerId, TextChatChannel channel, string text) { }
    public void OnUpdateSyncedTime(double time) { }
    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason) { }
    public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject, int ownerPlayerId) { }
}