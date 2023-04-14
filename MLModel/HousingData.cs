using Microsoft.ML.OnnxRuntime.Tensors;
//This holds all of the data that is fed into our supervised prediction model
public class HousingData 
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

    //prepare the object we'll use with the Onnx file
    public Tensor<float> AsTensor() 
    {
        float[] data = new float[]
        {
            depth, length, area_NE,
            area_NW, area_SE, area_SW, wrapping_B, wrapping_H, wrapping_W,
            samplescollected_false, samplescollected_true, ageatdeath_A,
            ageatdeath_C, ageatdeath_I, ageatdeath_N
        };
        int[] dimensions = new int[] { 1, 15 }; //***adjust this number to match the number of inputs we have (8 was here)
        return new DenseTensor<float>(data, dimensions);
    }
}