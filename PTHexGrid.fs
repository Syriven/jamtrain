module PTHexGrid

open HexGrid

type PTHexGrid<'Tile>(width,height,scale:float,initFunc: int -> int -> 'Tile) =
    member this.HexGrid = HexGrid(width,height,scale,initFunc)