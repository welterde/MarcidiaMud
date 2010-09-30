using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Sessions;
using Marcidia.Game.Admin.Services;

namespace Marcidia.Game.Admin.SessionStates
{
    class LoginState : SessionState
    {
        internal enum State
        {
            AskForUsername,
            EnterUsername,
            AskForPassword,
            EnterPassword
        }

        IAdminUserService adminUserService;
        State state;
        string username;
        string password;

        public LoginState()
        {
            state = State.AskForUsername;
        }

        protected override void OnInputRecieved(string input)
        {
            switch (state)
            {
                case State.EnterUsername:
                    username = input;
                    break;
                case State.EnterPassword:
                    password = input;
                    break;
            }

            Run();
        }

        public override void Start()
        {
            adminUserService = Services.GetService(typeof(IAdminUserService)) as IAdminUserService;

            Run();
        }

        public override void Stop()
        {
            
        }

        public override void Resume()
        {
            // Pop ourselves from the state
            Session.PopState();
        }

        public override void Pause()
        {
            
        }

        private void Run()
        {
            switch (state)
            {
                case State.AskForUsername:
                    Output.Write("`wUsername: ");
                    state = State.EnterUsername;
                    break;                
                case State.EnterUsername:
                    Output.Write("`wPassword: ");
                    state = State.EnterPassword;
                    break;
                case State.EnterPassword:
                    if (AttemptLogin())
                        state = (State)10;
                    else
                        state = State.AskForUsername;
                    
                    Run();
                    break;
            }
        }

        private bool AttemptLogin()
        {
            AdminUser adminUser = adminUserService.Login(username, password);

            username = string.Empty;
            password = string.Empty;

            if (adminUser == null)
            {
                Output.WriteLine("`rInvalid Credentials`w");
                return false;
            }

            Output.WriteLine("Login Successful");
            // just here to cause the connection to hang
            return true;
        }
    }
}
