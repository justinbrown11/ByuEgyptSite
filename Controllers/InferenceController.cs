using ByuEgyptSite.MLModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json;

namespace ByuEgyptSite.Controllers
{
    [ApiController]
    [Route("/score")] //type in /score to make requests to the endpoint
    public class InferenceController : Controller
    {
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        [HttpGet]
        public ActionResult Score()
        {
            var json = TempData["UserInput"] as string;
            var data = JsonConvert.DeserializeObject<UserData>(json);
            //var data = TempData["UserInput"] as UserData;

            var result = _session.Run(new List<NamedOnnxValue> //this is where we actually score our model
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            //Tensor<string> score = result.First().AsTensor<string>(); //changed these two from float to string

            Tensor<string> score = result.First().AsTensor<string>();
            var categories = new[] { "W", "E" };
            int predictionIndex = Array.IndexOf(score.ToArray(), score.Max());

            var prediction = new Prediction { PredictedValue = categories[predictionIndex] };
            return Ok(prediction);
        }
    }
}