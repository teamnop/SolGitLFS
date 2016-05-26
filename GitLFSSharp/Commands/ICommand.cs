using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    interface ICommand
    {
        int Run(string[] args);
    }
}
