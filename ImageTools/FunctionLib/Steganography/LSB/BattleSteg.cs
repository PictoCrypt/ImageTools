using System;

namespace FunctionLib.Steganography.LSB
{
    public class BattleSteg : LsbWithRandomness
    {
        public override string Name
        {
            get { return "BattleSteg"; }
        }

        public override string Description
        {
            get
            {
                return "The algorithm filters the image using an edge-detection filter " +
                       "like \"Laplace-Filter\". Hiding the secret information into random pixels with high filter-values.";
            }
        }

        protected override bool EncodingIteration()
        {
            throw new NotImplementedException();
        }

        protected override bool DecodingIteration()
        {
            throw new NotImplementedException();
        }
    }
}