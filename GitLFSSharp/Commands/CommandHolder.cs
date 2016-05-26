using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class CommandHolder
    {
        private Dictionary<string, ICommand> CommandDict = new Dictionary<string, ICommand>();
        private string DefaultCommand = "test";

        public CommandHolder()
        {
            Register();
        }

        private void Register()
        {
            CommandDict.Add("test", new TestCommand());
            CommandDict.Add("smudge", new SmudgeCommand());
        }

        public ICommand GetCommand(string command = null)
        {
            if (string.IsNullOrEmpty(command))
            {
                command = DefaultCommand;
            }

            return CommandDict[command];
        }
    }
}
