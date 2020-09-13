namespace Jamtrain

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Sprite =
    {Texture: Texture2D; Size: Point; Offset: Point}
    member this.Draw(spriteBatch: SpriteBatch, position: Point) =
        let sourceRect = Rectangle(this.Offset, this.Size)
        let destRect = Rectangle(position, this.Size)
        spriteBatch.Draw(this.Texture, destRect, Nullable.op_Implicit sourceRect, Color.White)

type Game1 () as this =
    inherit Game()
 
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>

    let mutable testSpriteSheet = Unchecked.defaultof<Texture2D>
    let mutable testSprite = Unchecked.defaultof<Sprite>

    do
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

    override this.Initialize() =
        // TODO: Add your initialization logic here
        
        base.Initialize()

    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)

        testSpriteSheet <- this.Content.Load<Texture2D>("sprites")

        testSprite <-
            {
                Texture = testSpriteSheet
                Offset = Point(0,0)
                Size = Point(100,100)
            }

        // TODO: use this.Content to load your game content here
 
    override this.Update (gameTime) =
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        then this.Exit();

        // TODO: Add your update logic here
    
        base.Update(gameTime)
 
    override this.Draw (gameTime) =
        this.GraphicsDevice.Clear Color.CornflowerBlue

        // TODO: Add your drawing code here
        spriteBatch.Begin()
        testSprite.Draw(spriteBatch,Point(0,0))
        spriteBatch.End()

        base.Draw(gameTime)

