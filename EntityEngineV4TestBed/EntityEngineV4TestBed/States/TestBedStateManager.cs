﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityEngineV4.Engine;
using EntityEngineV4.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityEngineV4TestBed.States
{
    public class TestBedStateManager : Entity
    {
        private DoubleInput _backkey;

        public bool GoBack { get; private set; }
        public TestBedStateManager(EntityState stateref, string name) : base(stateref, name)
        {
            _backkey = new DoubleInput(this, "BackKey", Keys.Back, Buttons.Back, PlayerIndex.One);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            GoBack = _backkey.Released();
        }
    }
}