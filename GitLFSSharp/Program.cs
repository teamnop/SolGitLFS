using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp
{
    class Program
    {
        static int Main(string[] args)
        {
            var commandHolder = new Commands.CommandHolder();

            Commands.ICommand command = null;
            if (args.Length == 0)
            {
                command = commandHolder.GetCommand();
            }
            else
            {
                command = commandHolder.GetCommand(args[0]);
            }

            var commandTask = command.Run(args.Skip(1).ToArray());
            commandTask.Wait();

            return commandTask.Result;
        }
    }
}
