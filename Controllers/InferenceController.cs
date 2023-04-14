using ByuEgyptSite.MLModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json;

namespace ByuEgyptSite.Controllers
    //this is the controller to make the API call to the ONNX file work
    //the route to access just the api is /score. This works on localhost.
    //we also integrated swagger with this app for testing purposes. It can be accessed with /swagger
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

        //beneath is the process to access the onnx file and get the predicted value.
        //We then send the predicted value off to the researcher controller
        [HttpGet]
        public ActionResult Score(string json)
        {
            var data = JsonConvert.DeserializeObject<UserData>(json);

            var result = _session.Run(new List<NamedOnnxValue> //this is where we actually score our model
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });

            string score = result.First().AsTensor<string>().ToArray()[0];
            var prediction = new Prediction { PredictedValue = score };
            //Added to send prediction to view
            string json2 = JsonConvert.SerializeObject(prediction);

            return RedirectToAction("SendPrediction", "Researcher", new { json = json2 });
        }
    }
}