using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_work
{
    internal class Generator
    {
        public Generator(List<int> codeIn, List<int> consts, List<string> ids, List<string> strings)
        {
            this.codeIn = codeIn;
            this.consts = consts;
            this.ids = ids;
            this.strings = strings;
        }
        private List<int> codeIn;
        private List<int> consts;
        private List<string> ids;
        private List<string> strings;
        Dictionary<int, string> tokenCode = new Dictionary<int, string>()
        {
            [1] = ";",
            [2] = ":",
            [3] = ",",
            [4] = ":=",
            [5] = "-",
            [6] = "/",
            [7] = "+",
            [8] = "*",
            [9] = "(",
            [10] = ")",
            [11] = ".",
            [101] = "program",
            [102] = "var",
            [103] = "Integer",
            [104] = "Double",
            [105] = "begin",
            [106] = "Readln",
            [107] = "for",
            [108] = "to",
            [109] = "do",
            [110] = "write",
            [111] = "writeln",
            [112] = "end",
            [113] = "ID",
            [114] = "const",
            [115] = "string",
            [1150] = "ID_d"
        };
        private List<string> specialTokens = new List<string>()
        {
            ";","begin","end"
        };
        private List<string> specialTokens2 = new List<string>()
        {
            ":",",","-","+","*","/","(",")",".","ID","const","string","Integer","Double","Readln",":=","to","writeln"
        };
        private List<string> codeOut = new List<string>();
        private void Generate()
        {
            string token = "";

            for (int i = 0; i < codeIn.Count; i++)
            {
                if (tokenCode.TryGetValue(codeIn[i], out token))
                {
                    codeOut.Add(token);
                    if (codeIn[i] == 101)
                    {
                        codeOut.Add(" ");
                        codeOut.Add("main");
                        continue;
                    }
                }
                if (specialTokens.Contains(token))
                {
                    codeOut.Add("\n");
                }
                if ((!specialTokens2.Contains(token)))
                {
                    codeOut.Add(" ");
                }
            }
        }
        private void FillWithIdsAndConstsAndStrings()
        {
            for (int i = 0, id = 0, con = 0, str = 0; i < codeOut.Count; i++)
            {
                if (codeOut[i] == "ID")
                {
                    codeOut[i] = ids[id];
                    id++;
                }
                else if (codeOut[i] == "const")
                {
                    codeOut[i] = consts[con].ToString();
                    con++;
                }
                else if (codeOut[i] == "string")
                {
                    codeOut[i] = strings[str];
                    str++;
                }
                else if (codeOut[i] == "ID_d")
                {
                    codeOut.Remove("ID_d");
                    id++;
                }
            }
        }
        public string GenerateAndGetCodeAsString()
        {
            if (codeIn != null)
            {
                string codeOutString = "";
                Generate();
                FillWithIdsAndConstsAndStrings();
                for (int i = 0; i < codeOut.Count; i++)
                {
                    codeOutString += codeOut[i];
                }
                return codeOutString;
            }
            else { return null; }
        }
    }
}
