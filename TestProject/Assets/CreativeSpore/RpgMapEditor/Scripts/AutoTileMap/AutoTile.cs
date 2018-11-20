using UnityEngine;
using System.Collections;
using System;


namespace CreativeSpore.RpgMapEditor
{

    public enum eLayerType
    {
        Ground, // tiles with predefined collisions
        Overlay, // tiles no collision
        Objects, // objects like triggers and actors
        FogOfWar, // used for fog of war
    };

    /// <summary>
    /// An auto-tile has 4 parts that change according to neighbors. These are the different types for each part.
    /// </summary>
    public enum eTilePartType
    {
        INT_CORNER,
        EXT_CORNER,
        INTERIOR,
        H_SIDE, // horizontal sides
        V_SIDE // vertical sides
    }

    [Obsolete("This has been deprecated after adding multiple layer support!")]
    /// <summary>
    /// Each type of tile layer in the map
    /// </summary>
    public enum eTileLayer
    {
        /// <summary>
        /// mostly for tiles with no alpha
        /// </summary>
        GROUND,
        /// <summary>
        /// mostly for tiles with alpha
        /// </summary>
        GROUND_OVERLAY,
        /// <summary>
        /// for tiles that should be drawn over everything else
        /// </summary>
        OVERLAY
    }

    /// <summary>
    /// Each type of tile of the map
    /// </summary>
    public enum eTileType
    {
        /// <summary>
        /// Animated auto-tiles with 3 frames of animation, usually named with _A1 suffix in the texture
        /// </summary>
        ANIMATED,
        /// <summary>
        /// Ground auto-Tiles, usually named with _A2 suffix in the texture
        /// </summary>
        GROUND,
        /// <summary>
        /// Building auto-Tiles, usually named with _A3 suffix in the texture
        /// </summary>
        BUILDINGS,
        /// <summary>
        /// Wall auto-Tiles, usually named with _A4 suffix in the texture
        /// </summary>
        WALLS,
        /// <summary>
        /// Normal tiles, usually named with _A5 suffix in the texture. Same as Objects tiles, but included as part of an auto-tileset
        /// </summary>
        NORMAL,
        /// <summary>
        /// Normal tiles, usually named with _B, _C, _D and _E suffix in the texture
        /// </summary>
        OBJECTS
    };

    /// <summary>
    /// Type map collision according to tile on certain map position
    /// </summary>
    public enum eTileCollisionType
    {
        /// <summary>
        /// Used to indicate the empty tile with no type
        /// </summary>
        EMPTY = -1,
        /// <summary>
        /// A PASSABLE tile over a BLOC, WALL, or FENCE allow walking over it.
        /// </summary>
        PASSABLE,
        /// <summary>
        /// Not passable
        /// </summary>
        BLOCK,
        /// <summary>
        /// Partially not passable, depending of autotiling
        /// </summary>
        WALL,
        /// <summary>
        /// Partially not passable, depending of autotiling
        /// </summary>
        FENCE,
        /// <summary>
        /// A passable tile. Used to check when a tile should be placed in overlay layer
        /// </summary>
        OVERLAY,
        /// <summary>
        /// The size of this enum
        /// </summary>
        _SIZE
    }

    /// <summary>
    /// Define a tile of the map
    /// </summary>        
    public class AutoTile
    {
        /// <summary>
        /// Sub-Tileset index of the tileset. A sub-tileset is the slot area of tileset when editing it.
        /// </summary>
        public int TilesetIdx;
        /// <summary>
        /// Tile id ( unique for each tile of all sub-tilesets )
        /// </summary>
        public int Id = -1;
        /// <summary>
        /// This is the mapped idx, used internally to manage animates tiles ( 3 different tiles grouped as one )
        /// </summary>
        public int MappedIdx;
        /// <summary>
        /// The type of tile
        /// </summary>
        public eTileType Type;

        /// <summary>
        /// The x coordinate in tiles of this tile in the map
        /// </summary>
        public int TileX;
        /// <summary>
        /// The y coordinate in tiles of this tile in the map
        /// </summary>
        public int TileY;
        /// <summary>
        /// Layer index of this tile ( see eTileLayer )
        /// </summary>
        public int Layer;
        /// <summary>
        /// An auto-tile has 4 parts that change according to neighbors. A normal tile only one.
        /// </summary>
        public int[] TilePartsIdx;
        /// <summary>
        /// The type of each part of the tile
        /// </summary>
        public eTilePartType[] TilePartsType;

        /// <summary>
        /// Added to specify the length of TileParts. Usually 4, but only 1 for OBJECT and NORMAL tiles
        /// </summary>
        public int TilePartsLength;

        public bool IsWaterTile()
        {
            return Id != -1 && Type == eTileType.ANIMATED; // TODO: temporary fix: if it's an animated tileset, it's considered as water
        }
    }
}
