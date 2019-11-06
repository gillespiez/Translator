using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using Newtonsoft.Json;
using System.Net.Http;

namespace Understand
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

       //setup for synthesis
        private const string key_var = "db9259dc904344cea17ea079c0f6e1b8";
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable(key_var);

        //setup for translator
        //private const string endpoint_var = "https://understand-translate.cognitiveservices.azure.com/sts/v1.0/issuetoken";
        //private static readonly string endpoint = Environment.GetEnvironmentVariable(endpoint_var);

        async void Translate_Button_Clicked(string subscriptionKey, string endpoint, string route, string userInput)
        {
            //object[] body = new object[] { new { Text = userInput } };
            //var requestBody = JsonConvert.SerializeObject(body);

            //using (var client = new HttpClient())
            //using (var request = new HttpRequestMessage())
            //{

            //    // Set the method to Post.
            //    request.Method = HttpMethod.Post;

            //    // Construct the URI and add headers.
            //    request.RequestUri = new Uri(endpoint + route);
            //    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            //    request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            //    // Send the request and get response.
            //    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            //    // Read response as a string.
            //    string result = await response.Content.ReadAsStringAsync();
            //    // Deserialize the response using the classes created earlier.
            //    TranslationClass[] deserializedOutput = JsonConvert.DeserializeObject<TranslationClass[]>(result);
            //}
        }

        async void Speak_Button_Clicked(object sender, EventArgs e, string result)
        {
            var config = SpeechConfig.FromSubscription("7de39c59c0774086b8e4bb22bff0df45", "westus");

            string text = result;

            var synthesizer = new SpeechSynthesizer(config);
            using (var result1 = await synthesizer.SpeakTextAsync(text))
            {
                if (result1.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                }
                else if (result1.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result1);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
        }

        //private void Microphone_Button_Clicked(object sender, EventArgs e)
        //{
        //            var config1 = SpeechTranslationConfig.FromSubscription("7de39c59c0774086b8e4bb22bff0df45", "westus");

        //            string fromLanguage = "en-US";
        //            config1.SpeechRecognitionLanguage = fromLanguage;
        //            config1.AddTargetLanguage("de");

        //            // Sets voice name of synthesis output.
        //            const string GermanVoice = "de-DE-Hedda";
        //            config1.VoiceName = GermanVoice;
        //            // Creates a translation recognizer using microphone as audio input.
        //            using (var recognizer = new TranslationRecognizer(config1))
        //            {
        //                // Subscribes to events.
        //                recognizer.Recognizing += (s, c) =>
        //                {
        //                    Console.WriteLine($"RECOGNIZING in '{fromLanguage}': Text={c.Result.Text}");
        //                    foreach (var element in c.Result.Translations)
        //                    {
        //                        Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
        //                    }
        //                };

        //                recognizer.Recognized += (s, c) =>
        //                {
        //                    if (c.Result.Reason == ResultReason.TranslatedSpeech)
        //                    {
        //                        Console.WriteLine($"\nFinal result: Reason: {c.Result.Reason.ToString()}, recognized text in {fromLanguage}: {c.Result.Text}.");
        //                        foreach (var element in c.Result.Translations)
        //                        {
        //                           Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
        //                        }
        //                    }
        //                };

        //                recognizer.Synthesizing += (s, c) =>
        //                {
        //                    var audio = c.Result.GetAudio();
        //                    Console.WriteLine(audio.Length != 0
        //                        ? $"AudioSize: {audio.Length}"
        //                        : $"AudioSize: {audio.Length} (end of synthesis data)");
        //                };

        //                recognizer.Canceled += (s, c) =>
        //                {
        //                    Console.WriteLine($"\nRecognition canceled. Reason: {c.Reason}; ErrorDetails: {c.ErrorDetails}");
        //                };

        //                recognizer.SessionStarted += (s, c) =>
        //                {
        //                    Console.WriteLine("\nSession started event.");
        //                };

        //                recognizer.SessionStopped += (s, c) =>
        //                {
        //                   Console.WriteLine("\nSession stopped event.");
        //               };

        //                // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
        //                Console.WriteLine("Say something...");
        //                recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

        //                do
        //               {
        //                    Console.WriteLine("Press Enter to stop");
        //                } while (Console.ReadKey().Key != ConsoleKey.Enter);

        //                // Stops continuous recognition.
        //                recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        //            }
        //       }

        //    }
    }
}

