using System;
using Puffin.Core.Drawing;
using Puffin.Core.Ecs.Components;
using Puffin.Core.Tiles;

namespace Puffin.Core.Ecs.Systems
{
    class DrawingSystem : ISystem
    {
        private readonly IDrawingSurface drawingSurface;
        
        public DrawingSystem(IDrawingSurface drawingSurface)
        {
            this.drawingSurface = drawingSurface;
        }

        // Has no references but used in unit tests (by Moq)
        internal DrawingSystem() { }

        public virtual void OnAddEntity(Entity entity)
        {
            if (entity.IsUiElement)
            {
                this.drawingSurface.AddUiEntity(entity);
            }
            else if (
                entity.Get<SpriteComponent>() != null ||
                entity.Get<TextLabelComponent>() != null ||
                entity.Get<ColourComponent>() != null ||
                entity.Get<CameraComponent>() != null)
            {
                this.drawingSurface.AddEntity(entity);
            }
        }

        public void OnRemoveEntity(Entity entity)
        {
            this.drawingSurface.RemoveEntity(entity);
        }

        public virtual void OnAddTileMap(TileMap tileMap)
        {
            this.drawingSurface.AddTileMap(tileMap);
        }

        public virtual void OnRemoveTileMap(TileMap tileMap)
        {
            this.drawingSurface.RemoveTileMap(tileMap);
        }

        public virtual void OnUpdate(TimeSpan elapsed)
        {
            
        }
        
        public virtual void OnDraw(TimeSpan elapsed, int backgroundColour, string backgroundImage, bool clearDisplay = true)
        {
            this.drawingSurface.DrawAll(backgroundColour, backgroundImage, clearDisplay);
        }

        internal void FlushToScreen()
        {
            this.drawingSurface.FlushToScreen();
        }
    }
}