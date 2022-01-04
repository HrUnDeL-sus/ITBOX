using System;
using System.Diagnostics;
using System.IO;
using OpenTK;
namespace ITBOX
{
    class Program
    {
       
        static void Main(string[] args)
        {
            using (var window =new ItBoxWindow())
            {
                window.Run(60, 60);
            }
        }
    }
}
