﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class Mountains
    {
        public Dictionary<int[], Mountain> Tiles = new Dictionary<int[], Mountain>(new IntArrayComparer());
    }
}
