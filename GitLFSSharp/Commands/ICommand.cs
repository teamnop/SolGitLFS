using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    interface ICommand
    {
        Task<int> Run(string[] args);
    }
}
