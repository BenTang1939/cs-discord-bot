using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Utilities.GetAlert("TEST")); 
            Console.WriteLine("Test: " + config.bot.token);//Get data via key from .json file
            Console.ReadLine(); 
        }
    }
}
