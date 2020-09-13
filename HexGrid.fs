module HexGrid

open Utils
open System
open Microsoft.Xna.Framework

let hexUnitVertices: Vector2 [] =
    [| 0 .. 6 |]
    |> Array.map
        ((fun i -> (float32 (i) / 6.0f) * (2.0f * float32 (Math.PI)))
         >> (fun angle -> Vector2(cos (angle), sin (angle))))

type AxialCoord =
    {
        Q : int
        R : int
    }
    static member (+) (c1, c2) =
        {
            Q = c1.Q + c2.Q
            R = c1.R + c2.R
        }
    static member (-) (c1, c2) =
        {
            Q = c1.Q - c2.Q
            R = c1.R - c2.R
        }
    static member (*) ((c : AxialCoord), i) =
        {
            Q = c.Q * i
            R = c.R * i
        }

let axialToVec axial : Vector2 =
    Vector2(
        (3.0f/2.0f) * float32(axial.Q),
        (-(sqrt(3.0f)/2.0f * float32(axial.Q)) - (sqrt(3.0f) * float32(axial.R)))
    )

type CubeCoord =
    {
        X : int
        Y : int
        Z : int
    }

let cubeDistance a b =
    (abs <| a.X - b.X)
    |> max (abs <| a.Y - b.Y)
    |> max (abs <| a.Z - b.Z)

let axialToCube axial : CubeCoord =
    {
        X = axial.Q
        Z = axial.R
        Y = 0 - axial.Q - axial.R
    }

let hexDistance a b =
    let ac = axialToCube a
    let bc = axialToCube b

    cubeDistance ac bc

type Vector2 with
    member this.Scaled (s: float) : Vector2 =
        Vector2(
            this.X*(float32(s)),
            this.Y*(float32(s))
        )

type HexGrid<'Tile>(width, height, scale : float, initFunc: int -> int -> 'Tile) =
    member this.Tiles =
        Array2D.init width height initFunc
    member this.Scale : float = scale
    member this.DoForTiles (func : AxialCoord -> 'Tile -> unit) : unit =
        for q in 0 .. this.Tiles.GetLength(0) - 1 do
            for r in 0 .. this.Tiles.GetLength(1) - 1 do
                func {Q = q; R = r} this.Tiles.[q,r]
    member this.AxialToReal2D axial : Vector2 =
        (axialToVec axial).Scaled(this.Scale)
    member this.AxialToReal3D axial =
        let v2 = this.AxialToReal2D axial

        Vector3(v2, 0.0f)

type Direction =
    | UpRight
    | Up
    | UpLeft
    | DownLeft
    | Down
    | DownRight

type RotateDirection =
    | Clockwise
    | CounterClockwise

let rotated (rotateDir : RotateDirection) (dir: Direction) : Direction =
    match rotateDir with
    | Clockwise ->
        match dir with
            | UpRight -> DownRight
            | Up -> UpRight
            | UpLeft -> Up
            | DownLeft -> UpLeft
            | Down -> DownLeft
            | DownRight -> Down
    | CounterClockwise ->
        match dir with
            | UpRight -> Up
            | Up -> UpLeft
            | UpLeft -> DownLeft
            | DownLeft -> Down
            | Down -> DownRight
            | DownRight -> UpRight

let randDir =
    randInst<Direction>

let getPosAngle (vec : Vector2) : float =
    atan2 (float(vec.Y)) (float(vec.X))
        |> fun angle ->
            if angle < 0.0 then
                angle + 2.0 * Math.PI
            else
                angle

let piOver3 = Math.PI / 3.0

let vecToDir vec : Direction =
    let angle = getPosAngle vec

    let dirIndex : int = angle / piOver3 |> int

    numberedInst<Direction>(dirIndex)

let dirToAngle dir : float =
    Math.PI / 6.0 + (
        piOver3 * (float
            (match dir with
                | UpRight -> 0
                | Up -> 1
                | UpLeft -> 2
                | DownLeft -> 3
                | Down -> 4
                | DownRight -> 5
            )
        )
    )

let angleToUnitVec (angle: float) : Vector2 =
    Vector2(
        cos(float32(angle)),
        sin(float32(angle))
    )        

let axialToDir : AxialCoord -> Direction =
    axialToVec >> vecToDir