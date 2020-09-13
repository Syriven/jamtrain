module Types

open PTHexGrid

type Tile =
    | Grass
    | Tree

type World =
    {
        Map: PTHexGrid<Tile>
    }