using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.Collections;

namespace TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            FIM_Interface _fim_interface = new FIM_Interface();
            Console.WriteLine(_fim_interface.GetAnchor("users"));
            Console.WriteLine(_fim_interface.GetAnchor("groups"));

            var _attr = new Dictionary<string, string>();
            _attr.Add("name__familyName", "FFF");
            _attr.Add("name__givenName", "GGG");
            string json = _fim_interface.GetJSONObject("users", _attr);
            Console.WriteLine(json);
            //var _attr = new Dictionary<string, string>();
            //_attr.Add("primaryEmail", "hoge@hoge.com");
            //_attr.Add("familyName", "family");
            //_attr.Add("givenName", "given");
            //_attr.Add("suspended", "false");
            //string _json = _fim_interface.GetJSONObject("users", _attr);
            //Console.WriteLine(_json);
            Console.ReadKey();
        }
    }
}
