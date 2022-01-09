using System;
using System.Diagnostics;
using System.IO;
using OpenTK;
namespace ITBOX
{
  public sealed class Program
    {
       
        static void Main(string[] args)
        {
            var window = new ItBoxWindow();
            window.Run(60, 60);
        }
    }
}
