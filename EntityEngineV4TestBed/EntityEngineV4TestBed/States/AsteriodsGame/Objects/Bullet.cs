﻿using EntityEngineV4.CollisionEngine;
using EntityEngineV4.CollisionEngine.Shapes;
using EntityEngineV4.Components;
using EntityEngineV4.Components.Rendering;
using EntityEngineV4.Engine;
using EntityEngineV4.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EntityEngineV4TestBed.States.AsteriodsGame.Objects
{
    public class Bullet : BaseNode
    {
        public override bool IsObject
        {
            get { return true; }
        }

        public ImageRender Render;
        public Circle Shape;
        public Timer DeathTimer;

        public Bullet(Node parent, string name)
            : base(parent, name)
        {

            Render = new ImageRender(this, "Render");
            Render.SetTexture(GetRoot<State>().GetService<AssetCollector>().GetAsset<Texture2D>("bullet"));
            Render.Layer = .1f;
            Render.Scale = new Vector2(.1f);
            Render.Color = Color.White;
            Render.LinkDependency(ImageRender.DEPENDENCY_BODY, Body);

            //Make our collision rectangles the size of the rendered sprite.
            Body.Bounds = Render.Bounds;
            Body.Origin = new Vector2(Render.Texture.Width / 2f, Render.Texture.Height / 2f);

            Shape = new Circle(this, "Shape", Body.Width / 2);
            Shape.Offset = new Vector2(Body.Width / 2, Body.Height / 2);
            Shape.LinkDependency(Circle.DEPENDENCY_BODY, Body);

            Collision.Group.AddMask(1);
            Collision.Pair.AddMask(2);
            Collision.Immovable = true;
            Collision.CollideEvent += collision => Recycle();
            Collision.LinkDependency(Collision.DEPENDENCY_SHAPE, Shape);
            Shape.LinkDependency(Circle.DEPENDENCY_COLLISION, Collision);

            DeathTimer = new Timer(this, "DeathTimer");
            DeathTimer.Milliseconds = 2000;
            DeathTimer.LastEvent += Recycle;
            DeathTimer.LastEvent += DeathTimer.Stop;
        }

        public override void Initialize()
        {
            base.Initialize();
            DeathTimer.Start();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            Physics.FaceVelocity();

            Render.Color = Name == "BulletRecycled" ? Color.Red : Color.Blue;
        }

        public override void Reuse(Node parent, string name)
        {
            base.Reuse(parent, name);
            Body.Origin = new Vector2(Render.Texture.Width / 2f, Render.Texture.Height / 2f);

            DeathTimer.Milliseconds = 2000;
            DeathTimer.LastEvent += Recycle;
            DeathTimer.Start();

            Render.Layer = .1f;
            Render.Scale = new Vector2(.1f);
            Render.Color = Color.White;
        }
    }
}
