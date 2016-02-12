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

            var ret = _fim_interface.ParseResponse("{'error': {'errors': [{'domain': 'global','reason': 'duplicate','message': 'Entity already exists.'}],'code': 409,'message': 'Entity already exists.'}}");
            Console.WriteLine(ret["RESULT"]);
            Console.WriteLine(ret["REASON"]);

            //Console.WriteLine(_fim_interface.GetAnchor("users"));
            //Console.WriteLine(_fim_interface.GetAnchor("groups"));

            //var _attr = new Dictionary<string, string>();
            //_attr.Add("primaryEmail", "hoge@hoge.com");
            //_attr.Add("emails__address", "mail@hoge.com");
            //_attr.Add("emails__type", "hoge");
            //_attr.Add("emails__primary", "true");
            //string json = _fim_interface.GetJSONObject("users", _attr);
            //Console.WriteLine(json);
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
