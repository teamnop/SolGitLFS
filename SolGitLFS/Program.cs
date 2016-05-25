using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> inputParams = new Dictionary<string, string>();
            bool isFirst = true;
            while (true)
            {
                var rawData = Console.ReadLine();

                if (string.IsNullOrEmpty(rawData))
                {
                    break;
                }

                if (rawData.Last() == '\n')
                {
                    rawData = rawData.Remove(rawData.Length - 1);
                }

                var splitIndex = rawData.IndexOf(' ');
                if (splitIndex < 0)
                {
                    // TODO: Input 그대로 redirect
                    Console.WriteLine("r");
                    return;
                }

                var key = rawData.Substring(0, splitIndex);
                var value = rawData.Substring(splitIndex + 1);

                if (isFirst == true && key.Equals("version", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    // TODO: Input 그대로 redirect
                    Console.WriteLine(key);
                    return;
                }
                isFirst = false;
                inputParams[key] = value;
            }
            /*
            foreach (var arg in args)
            {
                Console.Error.WriteLine(arg);
            }

            foreach (var input in inputParams)
            {
                Console.Error.WriteLine(input.Key + " " + input.Value);
            }*/
           
        }
    }
}
