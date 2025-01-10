using UnityEngine;
using HoldfastSharedMethods;
using HoldfastBridge;
using System;
using System.Collections.Generic;

public class XarksNaturePresetRandomizer : IHoldfastSharedMethods, IHoldfastGame 
{
    private bool isServer;
    private bool executeCommandAfterDelay = false;
    private float delayTimer = 0f;
    private const float delayTime = 30f; // Delay for sending the command.
    private readonly System.Random random = new System.Random();
    private string[] commands;

    public static IHoldfastGameMethods GameMethods { get; set; } // Static access to game methods

    public void OnIsServer(bool server)
    {
        isServer = server;
        if (isServer)
        {
            Debug.Log("Server mode detected. Ready to execute commands.");
        }
    }

    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
    {
        if (isServer)
        {
            Debug.Log($"Round ended. Preparing to execute a random command after a delay. Round: {roundId}, Map: {mapName}");
            executeCommandAfterDelay = true;
            delayTimer = 0f; // Reset the timer
        }
    }

    public void OnUpdateElapsedTime(float time)
    {
        if (isServer && executeCommandAfterDelay)
        {
            delayTimer += time;

            if (delayTimer >= delayTime)
            {
                Debug.Log("Delay timer reached. Executing random command.");
                delayTimer = 0f;
                executeCommandAfterDelay = false; // Reset the delay trigger
                ExecuteRandomCommand();
            }
        }
    }

    private void ExecuteRandomCommand()
    {
        if (GameMethods == null)
        {
            Debug.LogError("GameMethods is not set. Cannot execute commands.");
            return;
        }

        // Pick a random command
        string randomCommand = commands[random.Next(commands.Length)];

        // Log the command
        Debug.Log($"Executed random command after round end: {randomCommand}");

        // Execute the command
        string output;
        Exception exception;
        GameMethods.ExecuteConsoleCommand(randomCommand, out output, out exception, 0);

        if (exception != null)
        {
            Debug.LogError($"Error executing command: {exception.Message}");
        }
        else
        {
            Debug.Log($"Command executed successfully: {output}");
        }
    }

    public void PassConfigVariables(string[] value)
    {
        // Default list of nature presets
        string[] defaultCommands = {
        "Forest_Clear_Day",
        "Forest_Clear_Dawn",
        "Forest_Clear_Night",
        "Forest_Rain_Day",
        "Forest_Rain_Dawn",
        "Forest_Rain_Night",
        "Forest_Storm",
        "Forest_Dark_Night",
        "Snow_Snowing_Day",
        "Snow_Snowing_Dawn",
        "Snow_Snowing_Night",
        "Snow_Snowstorm",
        "Snow_Dark_Night",
        "Sand_Clear_Day",
        "Sand_Clear_Dawn",
        "Sand_Clear_Night",
        "Sand_Sandstorm",
        "City_Clear_Day",
        "City_Clear_Dawn",
        "City_Clear_Night",
        "City_Rain_Day",
        "City_Rain_Dawn",
        "City_Rain_Night",
        "City_Storm",
        "Frontlines_Forest_Clear_Day",
        "Frontlines_Forest_Clear_Night",
        "Frontlines_Forest_Clear_Dawn",
        "Frontlines_Forest_Rain_Day",
        "Frontlines_Forest_Rain_Night",
        "Frontlines_Forest_Rain_Dawn",
        "Frontlines_Forest_Storm",
        "Frontlines_Snow_Clear_Day",
        "Frontlines_Snow_Clear_Night",
        "Frontlines_Snow_Clear_Dawn",
        "Frontlines_Snow_Snowing_Day",
        "Frontlines_Snow_Snowing_Night",
        "Frontlines_Snow_Snowing_Dawn",
        "Frontlines_Snow_Snowstorm",
        "Frontlines_Ocean_Clear_Dawn",
        "Frontlines_Ocean_Clear_Day",
        "Frontlines_Ocean_Clear_Night",
        "Frontlines_Ocean_Rain_Dawn",
        "Frontlines_Ocean_Rain_Day",
        "Frontlines_Ocean_Rain_Night",
        "Frontlines_Ocean_Storm",
        "Frontlines_Sand_Clear_Dawn",
        "Frontlines_Sand_Clear_Day",
        "Frontlines_Sand_Clear_Night",
        "Frontlines_Sand_Sandstorm",
        "Frontlines_Forest_Dark_Night",
        "Frontlines_Snow_Dark_Night",
        "Frontlines_Ocean_Dark_Night",
        "Frontlines_City_Clear_Day",
        "Frontlines_City_Clear_Night",
        "Frontlines_City_Clear_Dawn",
        "Frontlines_City_Rain_Day",
        "Frontlines_City_Rain_Night",
        "Frontlines_City_Rain_Dawn",
        "Frontlines_City_Storm",
        "Frontlines_Trench_Clear_Day",
        "Frontlines_Trench_Clear_Night",
        "Frontlines_Trench_Clear_Dawn",
        "Frontlines_Trench_Rain_Day",
        "Frontlines_Trench_Rain_Night",
        "Frontlines_Trench_Rain_Dawn",
        "Frontlines_Trench_Storm",
        "Frontlines_Trench_Dark_Night"
    };

        // Define preset groups
        Dictionary<string, string[]> presetGroups = new Dictionary<string, string[]>
    {
        { "Forest_All", new string[] { "Forest_Clear_Day", "Forest_Clear_Dawn", "Forest_Clear_Night", "Forest_Rain_Day", "Forest_Rain_Dawn", "Forest_Rain_Night", "Forest_Storm", "Forest_Dark_Night" } },
        { "Snow_All", new string[] { "Snow_Snowing_Day", "Snow_Snowing_Dawn", "Snow_Snowing_Night", "Snow_Snowstorm", "Snow_Dark_Night" } },
        { "Sand_All", new string[] { "Sand_Clear_Day", "Sand_Clear_Dawn", "Sand_Clear_Night", "Sand_Sandstorm" } },
        { "City_All", new string[] { "City_Clear_Day", "City_Clear_Dawn", "City_Clear_Night", "City_Rain_Day", "City_Rain_Dawn", "City_Rain_Night", "City_Storm" } },
        { "Frontlines_Forest_All", new string[] { "Frontlines_Forest_Clear_Day", "Frontlines_Forest_Clear_Night", "Frontlines_Forest_Clear_Dawn", "Frontlines_Forest_Rain_Day", "Frontlines_Forest_Rain_Night", "Frontlines_Forest_Rain_Dawn", "Frontlines_Forest_Storm", "Frontlines_Forest_Dark_Night" } },
        { "Frontlines_Snow_All", new string[] { "Frontlines_Snow_Clear_Day", "Frontlines_Snow_Clear_Night", "Frontlines_Snow_Clear_Dawn", "Frontlines_Snow_Snowing_Day", "Frontlines_Snow_Snowing_Night", "Frontlines_Snow_Snowing_Dawn", "Frontlines_Snow_Snowstorm", "Frontlines_Snow_Dark_Night" } },
        { "Frontlines_Ocean_All", new string[] { "Frontlines_Ocean_Clear_Dawn", "Frontlines_Ocean_Clear_Day", "Frontlines_Ocean_Clear_Night", "Frontlines_Ocean_Rain_Dawn", "Frontlines_Ocean_Rain_Day", "Frontlines_Ocean_Rain_Night", "Frontlines_Ocean_Storm", "Frontlines_Ocean_Dark_Night" } },
        { "Frontlines_Sand_All", new string[] { "Frontlines_Sand_Clear_Dawn", "Frontlines_Sand_Clear_Day", "Frontlines_Sand_Clear_Night", "Frontlines_Sand_Sandstorm" } },
        { "Frontlines_City_All", new string[] { "Frontlines_City_Clear_Day", "Frontlines_City_Clear_Night", "Frontlines_City_Clear_Dawn", "Frontlines_City_Rain_Day", "Frontlines_City_Rain_Night", "Frontlines_City_Rain_Dawn", "Frontlines_City_Storm" } },
        { "Frontlines_Trench_All", new string[] { "Frontlines_Trench_Clear_Day", "Frontlines_Trench_Clear_Night", "Frontlines_Trench_Clear_Dawn", "Frontlines_Trench_Rain_Day", "Frontlines_Trench_Rain_Night", "Frontlines_Trench_Rain_Dawn", "Frontlines_Trench_Storm", "Frontlines_Trench_Dark_Night" } }
    };

        // Initialize the commands array with default values
        List<string> commandsList = new List<string>();

        // Loop through the passed values
        bool userDefinedPresets = false;
        for (int i = 0; i < value.Length; i++)
        {
            // Split the current value into parts
            var splitData = value[i].Split(':');
            if (splitData.Length != 3)
            {
                continue;
            }

            // Check if the first part of the split data matches the mod id
            if (splitData[0] == "XNPR")
            {
                // Check the variable type and parse the value accordingly
                if (splitData[1] == "preset")
                {
                    string presetName = splitData[2];
                    if (presetGroups.ContainsKey(presetName))
                    {
                        // Add all commands from the preset group
                        foreach (var command in presetGroups[presetName])
                        {
                            commandsList.Add($"nature preset {command}");
                        }
                    }
                    else
                    {
                        // Prepend "nature preset" to the user-defined preset
                        commandsList.Add($"nature preset {presetName}");
                    }
                    userDefinedPresets = true;
                }
            }
        }

        // If no user-defined presets, use default presets
        if (!userDefinedPresets)
        {
            foreach (var command in defaultCommands)
            {
                commandsList.Add($"nature preset {command}");
            }
        }

        // Update the commands array with the user-defined presets
        commands = commandsList.ToArray();
    }


    public void OnGameMethodsInitialized(IHoldfastGameMethods gameMethods)
    {
        GameMethods = gameMethods;
        Debug.Log("Game methods initialized.");
    }

    public void OnPlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot) { }
    public void OnPlayerLeft(int playerId) { }
    public void OnUpdateTimeRemaining(float time) { }
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