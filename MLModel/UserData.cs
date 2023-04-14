using Microsoft.ML.OnnxRuntime.Tensors;

namespace ByuEgyptSite.MLModel
{
    public class UserData
    {
        public float depth { get; set; }
        public float length { get; set; }
        public float area_NE { get; set; }
        public float area_NW { get; set; }
        public float area_SE { get; set; }
        public float area_SW { get; set; }
        public float wrapping_B { get; set; }
        public float wrapping_H { get; set; }
        public float wrapping_W { get; set; }
        public float samplescollected_false { get; set; }
        public float samplescollected_true { get; set; }
        public float ageatdeath_A { get; set; }
        public float ageatdeath_C { get; set; }
        public float ageatdeath_I { get; set; }
        public float ageatdeath_N { get; set; }

        public Tensor<float> AsTensor() //prepare the object we'll use with the Onnx file
        {
            float[] data = new float[]
            {
            depth, length, area_NE,
            area_NW, area_SE, area_SW, wrapping_B, wrapping_H, wrapping_W,
            samplescollected_false, samplescollected_true, ageatdeath_A,
            ageatdeath_C, ageatdeath_I, ageatdeath_N
            };
            int[] dimensions = new int[] { 1, 15 }; //adjusted this number to match the number of inputs we have
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
