﻿using ByuEgyptSite.MLModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json;

namespace ByuEgyptSite.Controllers
{
    [ApiController]
    [Route("/score")]
    public class InferenceController : Controller
    {
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        //Call the .onnx file with our trained model and retreive prediction based on user data
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