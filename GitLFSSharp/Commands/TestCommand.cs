using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class TestCommand : ICommand
    {
        public Task<int> Run(string[] args)
        {
            return Task.FromResult<int>(0);
        }
    }
}
