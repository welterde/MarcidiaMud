using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Sessions;
using System.IO;

namespace Marcidia.Game.Admin.SessionStates
{
    class AdminSplashScreenState : SessionState
    {
        public const string SplashScreenFile = @"Data\AdminSplashScreen.txt";

        // Monostate string in this case to avoid loading it more than once
        private static string SplashScreenText;
        
        protected override void OnInputRecieved(string input)
        {
            
        }

        public override void Start()
        {
            LoadSplashScreen();

            Output.WriteLine(SplashScreenText);

            Session.PushState(new LoginState());
        }

        public override void Stop()
        {        
        }

        public override void Resume()
        {
            Start();
        }

        public override void Pause()
        {         
        }

        private void LoadSplashScreen()
        {
            if (SplashScreenText != null)
                return;

            using (StreamReader streamReader = new StreamReader(SplashScreenFile))
            {
                SplashScreenText = streamReader.ReadToEnd();
            }
        }
    }
}
