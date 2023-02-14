using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Basically all the static ish data needed to manage the game
/// </summary>
public class GameState
{
    // TODO abstract this a bit more, to simplify planet loading

    /// <summary>
    /// Player level, as important a thing as their is
    /// </summary>
    public static int _level { get; set; } = 0;

    // collection targets for each level
    /// <summary>
    /// Collection targets for each level...level DIV 5...this covers 30 levels
    /// </summary>
    public static int[] _levelGoals { get; set; } = new[] { 5, 10, 15, 20, 25 };

    /// <summary>
    /// Contains the timer lists for levels...level DIV 5...this covers 30 levels
    /// </summary>
    public static int[] _levelTimers { get; set; } = new[] { 60, 45, 30, 20, 10 };

    /// <summary>
    /// Ordered list of planets, but drawn randomly
    /// </summary>
    public static string[] _prefabList { get; set; } = new[]
    {
        "Prefabs/Planets/Alien/Alien_Design1",
        "Prefabs/Planets/Alien/Alien_Design2",
        "Prefabs/Planets/Alien/Alien_Design3",
        "Prefabs/Planets/Alien/Alien_Design4",
        "Prefabs/Planets/Alien/Alien_Design5",
        "Prefabs/Planets/Alien/Alien_Design6",
        "Prefabs/Planets/Alien/Alien_Design7",
        "Prefabs/Planets/Alien/Alien_Design8",
        "Prefabs/Planets/Alien/Alien_Design9",
        "Prefabs/Planets/Alien/Alien_Design10",

        "Prefabs/Planets/Earth-Like/Earth-Like_Design1",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design2",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design3",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design4",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design5",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design6",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design7",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design8",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design9",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design10",

        "Prefabs/Planets/Tundra/Tundra_Design1",
        "Prefabs/Planets/Tundra/Tundra_Design2",
        "Prefabs/Planets/Tundra/Tundra_Design3",
        "Prefabs/Planets/Tundra/Tundra_Design4",
        "Prefabs/Planets/Tundra/Tundra_Design5",

        "Prefabs/Planets/Desert/Desert_Design1",
        "Prefabs/Planets/Desert/Desert_Design2",
        "Prefabs/Planets/Desert/Desert_Design3",
        "Prefabs/Planets/Desert/Desert_Design4",
        "Prefabs/Planets/Desert/Desert_Design5",

        "Prefabs/Planets/Frozen/Frozen_Design1",
        "Prefabs/Planets/Frozen/Frozen_Design2",
        "Prefabs/Planets/Frozen/Frozen_Design3",
        "Prefabs/Planets/Frozen/Frozen_Design4",
        "Prefabs/Planets/Frozen/Frozen_Design5",

        "Prefabs/Planets/Temperate/Temperate_Design1",
        "Prefabs/Planets/Temperate/Temperate_Design2",
        "Prefabs/Planets/Temperate/Temperate_Design3",
        "Prefabs/Planets/Temperate/Temperate_Design4",
        "Prefabs/Planets/Temperate/Temperate_Design5",
    };

    /// <summary>
    /// Background color for each planet listed above, in the same order
    /// </summary>
    public static Color[] _skyboxList { get; set; } = new[]
    {
        // Alien
        new Color(0, 160f/255f, 240f/255f, 1),
        new Color(255f/255f, 157f/255f, 113f/255f, 1),
        new Color(0f/255f, 183f/255f, 253f/255f, 1),
        new Color(0f/255f, 165f/255f, 240f/255f, 1),
        new Color(0f/255f, 165f/255f, 240f/255f, 1),
        new Color(0f/255f, 182f/255f, 253f/255f, 1),
        new Color(0f/255f, 176f/255f, 252f/255f, 1),
        new Color(35f/255f, 151f/255f, 247f/255f, 1),   // option 41 168 109
        new Color(0f/255f, 90f/255f, 22f/255f, 1),      // option 94 123 223
        new Color(138f/255f, 175f/255f, 65f/255f, 1),   // option 0 172 252

        // Earth
        new Color(65f/255f, 71f/255f, 86f/255f, 0),     //
        new Color(0f/255f, 91f/255f, 157f/255f, 0),     // option 52 74 83
        new Color(52f/255f, 74f/255f, 83f/255f, 0),     //
        new Color(65f/255f, 71f/255f, 86f/255f, 0),     
        new Color(0f/255f, 91f/255f, 157f/255f, 0),     
        new Color(52f/255f, 74f/255f, 83f/255f, 0),     
        new Color(65f/255f, 71f/255f, 86f/255f, 0),     
        new Color(0f/255f, 91f/255f, 157f/255f, 0),     
        new Color(52f/255f, 74f/255f, 83f/255f, 0),     
        new Color(65f/255f, 71f/255f, 86f/255f, 0),

        // Tundra
        new Color(0f/255f, 165f/255f, 240f/255f, 0),
        new Color(0f/255f, 165f/255f, 240f/255f, 0),
        new Color(0f/255f, 165f/255f, 240f/255f, 0),
        new Color(0f/255f, 165f/255f, 240f/255f, 0),
        new Color(0f/255f, 165f/255f, 240f/255f, 0),

        // Desert
        new Color(105f/255f, 162f/255f, 244f/255f, 0),  // option 33 182 123
        new Color(145f/255f, 139f/255f, 192f/255f, 0),  // option 0 187 223, 3 166 144
        new Color(133f/255f, 124f/255f, 231f/255f, 0),  // option 148 151 236, 77 177 130, 0 161 107
        new Color(0f/255f, 181f/255f, 213f/255f, 0),    //
        new Color(0f/255f, 194f/255f, 252f/255f, 0),    //

        // Frozen
        new Color(63f/255f, 74f/255f, 60f/255f, 0),     //
        new Color(63f/255f, 74f/255f, 60f/255f, 0),     //
        new Color(63f/255f, 74f/255f, 60f/255f, 0),     //
        new Color(249f/255f, 248f/255f, 123f/255f, 0),  //
        new Color(152f/255f, 21f/255f, 229f/255f, 0),   //

        // Temperate
        new Color(245f/255f, 160f/255f, 77f/255f, 0),   //
        new Color(245f/255f, 160f/255f, 77f/255f, 0),
        new Color(245f/255f, 160f/255f, 77f/255f, 0),
        new Color(66f/255f, 70f/255f, 86f/255f, 0),
        new Color(66f/255f, 70f/255f, 86f/255f, 0)      //

    };

    /// <summary>
    /// Not working yet - song list
    /// </summary>
    public static string[] _songList { get; set; } = new[]
    {
        "Music/gypsy1",
        "Music/gypsy2",
        "Music/gypsy3"
    };

    /// <summary>
    /// Contains the level type...level mod 6
    /// </summary>
    public static string[] _levelType { get; set; } = new[]
    {
        "Collection",
        "Collection TT",
        "Find",
        "Find TT",
        "Clearcut",
        "Clearcut TT"
    };


}
