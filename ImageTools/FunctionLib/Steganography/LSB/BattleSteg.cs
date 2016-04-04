﻿using System;
using System.Linq;

namespace FunctionLib.Steganography.LSB
{
    public class BattleSteg : LsbWithRandomness
    {
        public override string Name { get { return "BattleSteg"; } }
        public override string Description { get
            {
                return "The algorithm filters the image using an edge-detection filter " +
                "like \"Laplace-Filter\". Hiding the secret information into random pixels with high filter-values.";
            }
        }
        protected override bool EncodingIteration(int lsbIndicator)
        {
            throw new NotImplementedException();
        }

        protected override bool DecodingIteration(int lsbIndicator)
        {
            throw new NotImplementedException();
        }
    }
}