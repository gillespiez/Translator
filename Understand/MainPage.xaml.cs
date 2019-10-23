using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

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

        
       private async void Translate_Button_Clicked(object sender, EventArgs e)
       {
           var config = SpeechConfig.FromSubscription("7de39c59c0774086b8e4bb22bff0df45", "westus");
           string text = userInput.Text;
           var synthesizer = new SpeechSynthesizer(config);
            using (var result = await synthesizer.SpeakTextAsync(text))
           {
               if (result.Reason == ResultReason.SynthesizingAudioCompleted)
               {
                  Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
              }
               else if (result.Reason == ResultReason.Canceled)
               {
                   var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                   Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                   if (cancellation.Reason == CancellationReason.Error)
                   {
                       Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                       Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                       Console.WriteLine($"CANCELED: Did you update the subscription info?");
                   }
               }
           }

            string fromLanguage = "en-US";
            config.SpeechRecognitionLanguage = fromLanguage;
            config.AddTargetLanguage("de");

            // Sets voice name of synthesis output.
            const string GermanVoice = "de-DE-Hedda";
            config.VoiceName = GermanVoice;
            // Creates a translation recognizer using microphone as audio input.
            using (var recognizer = new TranslationRecognizer(config))
            {
                // Subscribes to events.
                recognizer.Recognizing += (s, e) =>
                {
                    Console.WriteLine($"RECOGNIZING in '{fromLanguage}': Text={e.Result.Text}");
                    foreach (var element in e.Result.Translations)
                    {
                        Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
                    }
                };

                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.TranslatedSpeech)
                    {
                        Console.WriteLine($"\nFinal result: Reason: {e.Result.Reason.ToString()}, recognized text in {fromLanguage}: {e.Result.Text}.");
                        foreach (var element in e.Result.Translations)
                        {
                            Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
                        }
                    }
                };

                recognizer.Synthesizing += (s, e) =>
                {
                    var audio = e.Result.GetAudio();
                    Console.WriteLine(audio.Length != 0
                        ? $"AudioSize: {audio.Length}"
                        : $"AudioSize: {audio.Length} (end of synthesis data)");
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"\nRecognition canceled. Reason: {e.Reason}; ErrorDetails: {e.ErrorDetails}");
                };

                recognizer.SessionStarted += (s, e) =>
                {
                    Console.WriteLine("\nSession started event.");
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("\nSession stopped event.");
                };

                // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                Console.WriteLine("Say something...");
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                do
                {
                    Console.WriteLine("Press Enter to stop");
                } while (Console.ReadKey().Key != ConsoleKey.Enter);

                // Stops continuous recognition.
                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }

    }

    ////public static async Task SynthesisToSpeakerAsync()
    ////{
    ////    // Creates an instance of a speech config with specified subscription key and service region.
    ////    // Replace with your own subscription key and service region (e.g., "westus").
    ////    /*var config = SpeechConfig.FromSubscription("7de39c59c0774086b8e4bb22bff0df45", "westus");*/

    ////    // Creates a speech synthesizer using the default speaker as audio output.
    ////    using (var synthesizer = new SpeechSynthesizer(config))
    ////    {
    ////        // Receive a text from console input and synthesize it to speaker.
    ////        Console.WriteLine("Type some text that you want to speak...");
    ////        Console.Write("> ");
    ////        string text = text;

    ////        using (var result = await synthesizer.SpeakTextAsync(text))
    ////        {
    ////            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
    ////            {
    ////                Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
    ////            }
    ////            else if (result.Reason == ResultReason.Canceled)
    ////            {
    ////                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
    ////                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

    ////                if (cancellation.Reason == CancellationReason.Error)
    ////                {
    ////                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
    ////                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
    ////                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
    ////                }
    ////            }
    ////        }
    ////    }
    ////}

    //static void Main()
    //{

    //   SynthesisToSpeakerAsync().Wait();
    //    Console.WriteLine("Press any key to exit...");
    //    Console.ReadKey();
    //}


}
}
